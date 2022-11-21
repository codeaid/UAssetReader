using UAssetReader.Core.Utils;

namespace UAssetReader.Core.IO;

public class UCoreReader
{
    /// <summary>
    /// Stream reader used to read contents of asset files.
    /// </summary>
    public StreamReader StreamReader { get; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="streamReader">Asset file stream reader.</param>
    public UCoreReader(StreamReader streamReader)
    {
        StreamReader = streamReader;
    }

    /// <summary>
    /// Reads a 32-bit boolean value.
    /// </summary>
    /// <returns>Boolean value read from the current stream.</returns>
    public bool ReadFBool() => StreamReader.ReadInt32() != 0;

    /// <summary>
    /// Read a list of objects from the current stream.
    /// </summary>
    /// <param name="factory">Callback to execute on each read.</param>
    /// <typeparam name="T">Type of objects being read.</typeparam>
    /// <returns>List of instantiated objects.</returns>
    public List<T> ReadList<T>(Func<T> factory)
    {
        // Determine the number of items to read from the stream.
        int size = StreamReader.ReadInt32();

        return ListUtils.CreateWithGenerator(size, factory);
    }

    /// <summary>
    /// Read an Unreal Engine string value from the current stream.
    /// </summary>
    /// <returns>String read from the stream.</returns>
    public string ReadFString()
    {
        // Determine length of the string to read.
        int length = StreamReader.ReadInt32();

        // Read value depending on the retrieved length.
        string value = length switch
        {
            // String does not contain any characters.
            0 => "",

            // String is a UTF-16 string.
            < 0 => StreamReader.ReadStringUtf16(length * -2),

            // String is the standard UTF-8 string.
            _ => StreamReader.ReadStringAscii(length)
        };

        return value;
    }
}
