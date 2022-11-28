using Serilog;
using UAssetReader.Core.Utils;

namespace UAssetReader.Core.IO;

public class UStreamReader : StreamReader
{
    /// <inheritdoc />
    public UStreamReader(Stream stream, ILogger? logger = null): base(stream, logger)
    {
    }

    /// <summary>
    /// Read a 4 byte boolean value.
    /// </summary>
    /// <returns>Boolean value read from the stream.</returns>
    public bool ReadFBool()
    {
        Logger?.Verbose("Reading Unreal Engine boolean value at {Position}", Position);

        return ReadInt32() != 0;
    }

    /// <summary>
    /// Read a list of values from the current stream using a callback.
    /// </summary>
    /// <param name="generator">Callback to invoke for each value.</param>
    /// <typeparam name="T">Type of the values being read.</typeparam>
    /// <returns>List of values read from the stream.</returns>
    public List<T> ReadFList<T>(Func<T> generator)
    {
        Logger?.Verbose("Reading Unreal Engine list using a generator at {Position}", Position);

        // Determine the number of items to read from the stream.
        int count = ReadInt32();

        return ListUtils.CreateWithGenerator(count, generator);
    }

    /// <summary>
    /// Read an Unreal Engine string value from the current stream.
    /// </summary>
    /// <returns>String value read from the stream.</returns>
    public string ReadFString()
    {
        Logger?.Verbose("Reading Unreal Engine string value at {Position}", Position);

        // Determine length of the string to read.
        int length = ReadInt32();

        // Read value depending on the retrieved length.
        string value = length switch
        {
            // String does not contain any characters.
            0 => "",

            // String is a UTF-16 string.
            < 0 => ReadStringUtf16(length * -2),

            // String is the standard UTF-8 string.
            _ => ReadStringAscii(length)
        };

        return value;
    }
}
