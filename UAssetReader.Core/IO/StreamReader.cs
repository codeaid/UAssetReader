using System.Text;
using Serilog;

namespace UAssetReader.Core.IO;

public class StreamReader
{
    /// <summary>
    /// Debug output logger.
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    /// Pointer position within the current stream.
    /// </summary>
    public long Position => Stream.Position;

    /// <summary>
    /// Source stream to process.
    /// </summary>
    private Stream Stream { get; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="stream">Source stream to process.</param>
    /// <param name="logger">Debug output logger.</param>
    protected StreamReader(Stream stream, ILogger? logger = null)
    {
        // Ensure a valid stream is being passed in.
        Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        Logger = logger;
    }

    /// <summary>
    /// Read the specified number of bytes from the current source stream.
    /// </summary>
    /// <param name="count">Number of bytes to read.</param>
    /// <returns>An array of bytes read from the stream.</returns>
    public byte[] ReadBytes(int count)
    {
        Logger?.Verbose("Reading {0} bytes at offset {1}", count, Position);

        // Read the specified number of bytes from the source stream.
        var buffer = new byte[count];
        int bytesRead = Stream.Read(buffer, 0, count);

        // Log information about having reached the end of the file while reading the stream.
        if (bytesRead == 0)
        {
            Logger?.Verbose("End of file reached, zero bytes read");
        }

        // Ensure number of bytes read matches the number of requested bytes.
        if (bytesRead == count)
        {
            return buffer;
        }

        Logger?.Error("Requested {0} bytes but read {1} at offset {2}", count, bytesRead, Position);
        throw new Exception($"Unable to read the required number of bytes ({count})");
    }

    /// <summary>
    /// Read a single byte boolean value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public bool ReadBool()
    {
        Logger?.Verbose($"Reading boolean value at {Position}");

        return ReadByte() != 0;
    }

    /// <summary>
    /// Read a single byte from the stream from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public byte ReadByte()
    {
        Logger?.Verbose($"Reading byte value at {Position}");

        byte[] bytes = ReadBytes(1);
        return bytes[0];
    }

    /// <summary>
    /// Read a 32-bit floating point value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public float ReadFloat32()
    {
        Logger?.Verbose($"Reading 32-bit floating point value at {Position}");

        byte[] bytes = ReadBytes(4);
        return BitConverter.ToSingle(bytes);
    }

    /// <summary>
    /// Read a 64-bit floating point value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public double ReadFloat64()
    {
        Logger?.Verbose($"Reading 64-bit floating point value at {Position}");

        byte[] bytes = ReadBytes(8);
        return BitConverter.ToDouble(bytes);
    }

    /// <summary>
    /// Read a 128-bit floating point value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public decimal ReadFloat128()
    {
        Logger?.Verbose($"Reading 128-bit floating point value at {Position}");

        byte[] bytes = ReadBytes(16);
        return Convert.ToDecimal(BitConverter.ToDouble(bytes, 0));
    }

    /// <summary>
    /// Read a GUID value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public Guid ReadGuid()
    {
        Logger?.Verbose($"Reading GUID value at {Position}");

        byte[] bytes = ReadBytes(16);
        return new Guid(bytes);
    }

    /// <summary>
    /// Read a 16-bit integer value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public short ReadInt16()
    {
        Logger?.Verbose($"Reading 16-bit integer value at {Position}");

        byte[] bytes = ReadBytes(2);
        return BitConverter.ToInt16(bytes);
    }

    /// <summary>
    /// Read a 32-bit integer value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public int ReadInt32()
    {
        Logger?.Verbose($"Reading 32-bit integer value at {Position}");

        byte[] bytes = ReadBytes(4);
        return BitConverter.ToInt32(bytes);
    }

    /// <summary>
    /// Read a 64-bit integer value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public long ReadInt64()
    {
        Logger?.Verbose($"Reading 64-bit integer value at {Position}");

        byte[] bytes = ReadBytes(8);
        return BitConverter.ToInt64(bytes);
    }

    /// <summary>
    /// Read a 16-bit signed integer value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public ushort ReadUInt16()
    {
        Logger?.Verbose($"Reading 16-bit unsigned integer value at {Position}");

        byte[] bytes = ReadBytes(2);
        return BitConverter.ToUInt16(bytes);
    }

    /// <summary>
    /// Read a 32-bit signed integer value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public uint ReadUInt32()
    {
        Logger?.Verbose($"Reading 32-bit unsigned integer value at {Position}");

        byte[] bytes = ReadBytes(4);
        return BitConverter.ToUInt32(bytes);
    }

    /// <summary>
    /// Read a 64-bit signed integer value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public ulong ReadUInt64()
    {
        Logger?.Verbose($"Reading 64-bit unsigned integer value at {Position}");

        byte[] bytes = ReadBytes(8);
        return BitConverter.ToUInt64(bytes);
    }

    /// <summary>
    /// Read an ASCII string value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public string ReadStringAscii(int length)
    {
        Logger?.Verbose($"Reading ASCII string value at {Position}");

        byte[] bytes = ReadBytes(length - 1);

        // Skip the null terminator.
        Skip(1);

        return Encoding.ASCII.GetString(bytes);
    }

    /// <summary>
    /// Read a UTF-16 string value from the current stream.
    /// </summary>
    /// <returns>Value converted from the byte span.</returns>
    public string ReadStringUtf16(int length)
    {
        Logger?.Verbose($"Reading UTF-16 string value at {Position}");

        byte[] bytes = ReadBytes(length - 1);

        // Skip the null terminator.
        Skip(1);

        return Encoding.Unicode.GetString(bytes);
    }

    /// <summary>
    /// Sets the pointer position within the current stream.
    /// </summary>
    /// <param name="offset">A byte offset relative to the beginning of the stream.</param>
    /// <returns>The new position within the current stream.</returns>
    public long Seek(long offset) => Stream.Seek(offset, SeekOrigin.Begin);

    /// <summary>
    /// Skip the specified number of bytes within the current stream.
    /// </summary>
    /// <param name="count">Number of bytes to skip.</param>
    /// <returns>The new position within the current stream.</returns>
    public long Skip(long count) => Stream.Seek(count, SeekOrigin.Current);
}
