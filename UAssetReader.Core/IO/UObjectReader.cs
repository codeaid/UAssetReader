using Serilog;
using UAssetReader.Runtime.Core.Misc;
using UAssetReader.Runtime.CoreUObject.UObject;

namespace UAssetReader.Core.IO;

public class UObjectReader
{
    /// <summary>
    /// Debug output logger.
    /// </summary>
    private ILogger? Logger { get; }

    /// <summary>
    /// Stream reader used to process the current stream.
    /// </summary>
    private UStreamReader StreamReader { get; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="stream">Source stream to process.</param>
    /// <param name="logger">Debug output logger.</param>
    public UObjectReader(Stream stream, ILogger? logger = null)
    {
        StreamReader = new UStreamReader(stream, logger);
        Logger = logger;
    }

    /// <summary>
    /// Read value of the EObjectFlags enum from the current stream.
    /// </summary>
    /// <returns>Value of EObjectFlags enum.</returns>
    public EObjectFlags ReadEObjectFlags()
    {
        Logger?.Debug($"Reading EObjectFlags at {StreamReader.Position}");

        return (EObjectFlags) StreamReader.ReadUInt32();
    }

    /// <summary>
    /// Read an instance of FGuid class from the current stream.
    /// </summary>
    /// <returns>An instance of FGuid.</returns>
    public FGuid ReadFGuid()
    {
        Logger?.Debug($"Reading FGuid at {StreamReader.Position}");

        return new FGuid
        {
            Value = StreamReader.ReadGuid(),
        };
    }

    /// <summary>
    /// Read an instance of FName class from the current stream.
    /// </summary>
    /// <returns>An instance of FName.</returns>
    public FName ReadFName()
    {
        Logger?.Debug($"Reading FName at {StreamReader.Position}");

        // Retrieve index of the name entry.
        int index = StreamReader.ReadInt32();

        // Read and discard (skip) additional bytes (unknown).
        // Potentially https://docs.unrealengine.com/5.0/en-US/API/Runtime/Core/UObject/EFindName/.
        StreamReader.Skip(sizeof(int));

        return new FName
        {
            Index = index
        };
    }

    /// <summary>
    /// Read an instance of FPackageIndex class from the current stream.
    /// </summary>
    /// <returns>An instance of FPackageIndex.</returns>
    public FPackageIndex ReadFPackageIndex()
    {
        Logger?.Debug($"Reading FPackageIndex at {StreamReader.Position}");

        return new FPackageIndex
        {
            Index = StreamReader.ReadInt32()
        };
    }

    /// <summary>
    /// Read an instance of FPropertyGuid class from the current stream.
    /// </summary>
    /// <returns>An instance of FPropertyGuid.</returns>
    public FGuid? ReadFPropertyGuid()
    {
        Logger?.Debug($"Reading property FGuid at {StreamReader.Position}");

        // Determine if GUID value is present in the stream.
        bool hasGuid = StreamReader.ReadBool();

        // Read the existing GUID if present or use NULL if not.
        return hasGuid ? ReadFGuid() : null;
    }

    /// <summary>
    /// Read an instance of FPropertyTag class from the current stream.
    /// </summary>
    /// <returns>An instance of FPropertyTag.</returns>
    public FPropertyTag ReadFPropertyTag()
    {
        Logger?.Debug($"Reading FPropertyTag at {StreamReader.Position}");

        return new FPropertyTag
        {
            Name = ReadFName(),
            Type = ReadFName(),
            Size = StreamReader.ReadInt32(),
            Index = StreamReader.ReadInt32(),
        };
    }
}
