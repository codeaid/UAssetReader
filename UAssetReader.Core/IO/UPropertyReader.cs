using Serilog;
using UAssetReader.Runtime.Core.Math;
using UAssetReader.Runtime.Core.Misc;
using UAssetReader.Runtime.CoreUObject.UObject;

namespace UAssetReader.Core.IO;

public class UPropertyReader
{
    /// <summary>
    /// Debug output logger.
    /// </summary>
    private ILogger? Logger { get; }

    /// <summary>
    /// Unreal Engine object reader used to process the current stream.
    /// </summary>
    private UObjectReader ObjectReader { get; }

    /// <summary>
    /// Stream reader used to process the current stream.
    /// </summary>
    private UStreamReader StreamReader { get; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="stream">Source stream to process.</param>
    /// <param name="logger">Debug output logger.</param>
    public UPropertyReader(Stream stream, ILogger? logger = null)
    {
        Logger = logger;
        ObjectReader = new UObjectReader(stream, logger);
        StreamReader = new UStreamReader(stream, logger);
    }

    /// <summary>
    /// Reads the list of properties from the current position until reaching the None entry.
    /// </summary>
    /// <returns>List of properties read from the current stream.</returns>
    private List<FProperty> ReadProperties()
    {
        Logger?.Debug($"Reading properties at {StreamReader.Position}");

        // Target container for all read properties.
        var properties = new List<FProperty>();

        while (true)
        {
            // Remember current stream position.
            long returnPosition = StreamReader.Position;

            // Skip ahead and read the name block.
            FName name = ObjectReader.ReadFName();

            // Stop processing properties if last one reached.
            if (name == "None")
            {
                break;
            }

            // Return back to the original position if name is not "None".
            StreamReader.Seek(returnPosition);

            // Read the property header to find out the type of the property.
            FPropertyTag propertyTag = ObjectReader.ReadFPropertyTag();

            // Read property object in its entirety.
            FProperty property = ReadProperty(propertyTag.Type, propertyTag, false);
            properties.Add(property);
        }

        return properties;
    }

    /// <summary>
    /// Reads the list of properties for the specified export until reaching the None entry.
    /// </summary>
    /// <param name="summary">Package file summary object.</param>
    /// <param name="export">Target export entry properties of which to read.</param>
    /// <returns>List of properties read from the current stream.</returns>
    public List<FProperty> ReadProperties(FPackageFileSummary summary, FObjectExport export)
    {
        // Remember the current stream position.
        long originalPosition = StreamReader.Position;

        // Data resides in a separate .uexp file (offset of the export is larger than the header file itself).
        if (export.SerialOffset >= summary.TotalHeaderSize)
        {
            // Calculate offset of the specified export in relation to the header summary.
            long offset = export.SerialOffset - summary.TotalHeaderSize;
            StreamReader.Seek(offset);
        }
        else
        {
            // Data resides in the same file.
            StreamReader.Seek(export.SerialOffset);
        }

        // Read list of properties from the current position.
        List<FProperty> properties = ReadProperties();

        // Reset stream position to its original position.
        StreamReader.Seek(originalPosition);

        return properties;
    }

    /// <summary>
    /// Reads a property at the current stream position.
    /// </summary>
    /// <param name="type">Target property type.</param>
    /// <param name="tag">Property tag.</param>
    /// <param name="skipGuid">Flag indicating whether reading GUID should be skipped.</param>
    /// <returns>Instance of the property object.</returns>
    /// <exception cref="Exception">Thrown if the specified property type is not supported.</exception>
    private FProperty ReadProperty(FName type, FPropertyTag tag, bool skipGuid)
    {
        Logger?.Debug($"Reading {type} at {StreamReader.Position}");

        return type.ToString() switch
        {
            "ArrayProperty" => ReadUArrayProperty(tag, skipGuid),
            "BoolProperty" => ReadUBoolProperty(tag, skipGuid),
            "FloatProperty" => ReadUFloatProperty(tag, skipGuid),
            "IntProperty" => ReadUIntProperty(tag, skipGuid),
            "ObjectProperty" => ReadUObjectProperty(tag, skipGuid),
            "StrProperty" => ReadUStrProperty(tag, skipGuid),
            "StructProperty" => ReadUStructProperty(tag, skipGuid),
            _ => throw new Exception($"Unknown property type '{tag.Type}' encountered"),
        };
    }

    /// <summary>
    /// Reads a UArrayProperty from the current stream.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="skipGuid">Flag indicating whether reading GUID should be skipped.</param>
    /// <returns>Instance of the property object.</returns>
    private FArrayProperty ReadUArrayProperty(FPropertyTag tag, bool skipGuid)
    {
        // Read information about the array type and size.
        FName arrayType = ObjectReader.ReadFName();
        FGuid? guid = skipGuid ? null : ObjectReader.ReadFPropertyGuid();
        int arraySize = StreamReader.ReadInt32();

        var items = new List<FProperty>();

        if (arrayType == "StructProperty")
        {
            // Handle struct properties differently.
            FPropertyTag structTag = ObjectReader.ReadFPropertyTag();
            IEnumerable<FStructProperty> properties = ReadUArrayPropertyStruct(structTag, arraySize);
            items.AddRange(properties);
        }
        else
        {
            // Loop through the specified number of properties and read them.
            for (var i = 0; i < arraySize; i++)
            {
                FProperty property = ReadProperty(arrayType, tag, true);
                items.Add(property);
            }
        }

        return new FArrayProperty(tag, guid)
        {
            ArrayType = arrayType,
            Items = items,
            Size = arraySize,
        };
    }

    /// <summary>
    /// Reads list of properties of a FStructProperty array.
    /// </summary>
    /// <param name="tag">Parent property tag.</param>
    /// <param name="size">Number of items to read from the stream.</param>
    /// <returns>List of FStructProperty instances.</returns>
    private IEnumerable<FStructProperty> ReadUArrayPropertyStruct(FPropertyTag tag, int size)
    {
        // Read information about the array.
        FName structType = ObjectReader.ReadFName();
        FGuid structGuid = ObjectReader.ReadFGuid();
        FGuid? guid = ObjectReader.ReadFPropertyGuid();

        int chunkSize = tag.Size / size;
        var structs = new List<FStructProperty>();

        for (var i = 0; i < size; i++)
        {
            switch (structType.ToString())
            {
                case "Color":
                case "Guid":
                case "Vector":
                case "Rotator":
                    FProperty property = ReadUStructPropertyChild(tag, structType);
                    structs.Add(new FStructProperty(tag, guid)
                    {
                        Properties = new List<FProperty> {property},
                        StructGuid = structGuid,
                        StructType = structType
                    });
                    break;
                default:
                    // Read bytes containing the structure.
                    byte[] bytes = StreamReader.ReadBytes(chunkSize);

                    // Instantiate a new property reader using the struct stream.
                    var stream = new MemoryStream(bytes);
                    var propertyReader = new UPropertyReader(stream);

                    structs.Add(new FStructProperty(tag, guid)
                    {
                        Properties = propertyReader.ReadProperties(),
                        StructGuid = structGuid,
                        StructType = structType
                    });
                    break;
            }
        }

        return structs;
    }

    /// <summary>
    /// Reads a UBoolProperty from the current stream.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="skipGuid">Flag indicating whether reading GUID should be skipped.</param>
    /// <returns>Instance of the property object.</returns>
    private FBoolProperty ReadUBoolProperty(FPropertyTag tag, bool skipGuid)
    {
        bool value = StreamReader.ReadBool();
        FGuid? guid = skipGuid ? null : ObjectReader.ReadFPropertyGuid();

        return new FBoolProperty(tag, guid)
        {
            Value = value
        };
    }

    /// <summary>
    /// Reads a UFloatProperty from the current stream.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="skipGuid">Flag indicating whether reading GUID should be skipped.</param>
    /// <returns>Instance of the property object.</returns>
    private FFloatProperty ReadUFloatProperty(FPropertyTag tag, bool skipGuid)
    {
        FGuid? guid = skipGuid ? null : ObjectReader.ReadFPropertyGuid();
        float value = StreamReader.ReadFloat32();

        return new FFloatProperty(tag, guid)
        {
            Value = value
        };
    }

    /// <summary>
    /// Reads a UIntProperty from the current stream.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="skipGuid">Flag indicating whether reading GUID should be skipped.</param>
    /// <returns>Instance of the property object.</returns>
    private FIntProperty ReadUIntProperty(FPropertyTag tag, bool skipGuid)
    {
        FGuid? guid = skipGuid ? null : ObjectReader.ReadFPropertyGuid();
        int value = StreamReader.ReadInt32();

        return new FIntProperty(tag, guid)
        {
            Value = value
        };
    }

    /// <summary>
    /// Reads a UObjectProperty from the current stream.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="skipGuid">Flag indicating whether reading GUID should be skipped.</param>
    /// <returns>Instance of the property object.</returns>
    private FObjectProperty ReadUObjectProperty(FPropertyTag tag, bool skipGuid)
    {
        FGuid? guid = skipGuid ? null : ObjectReader.ReadFPropertyGuid();
        FPackageIndex packageIndex = ObjectReader.ReadFPackageIndex();

        return new FObjectProperty(tag, guid)
        {
            Index = packageIndex
        };
    }

    /// <summary>
    /// Reads a UStrProperty from the current stream.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="skipGuid">Flag indicating whether reading GUID should be skipped.</param>
    /// <returns>Instance of the property object.</returns>
    private FStrProperty ReadUStrProperty(FPropertyTag tag, bool skipGuid)
    {
        FGuid? guid = skipGuid ? null : ObjectReader.ReadFPropertyGuid();
        string value = StreamReader.ReadFString();

        return new FStrProperty(tag, guid)
        {
            Value = value
        };
    }

    /// <summary>
    /// Reads a UStructProperty from the current stream.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="skipGuid">Flag indicating whether reading GUID should be skipped.</param>
    /// <returns>Instance of the property object.</returns>
    private FStructProperty ReadUStructProperty(FPropertyTag tag, bool skipGuid)
    {
        FName structType = ObjectReader.ReadFName();
        FGuid structGuid = ObjectReader.ReadFGuid();
        FGuid? guid = skipGuid ? null : ObjectReader.ReadFPropertyGuid();

        var properties = new List<FProperty>();
        FProperty property = ReadUStructPropertyChild(tag, structType);
        properties.Add(property);

        return new FStructProperty(tag, guid)
        {
            Properties = properties,
            StructGuid = structGuid,
            StructType = structType
        };
    }

    /// <summary>
    /// Reads StructProperty child object of the specified type.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="structType">Structure type name.</param>
    /// <see href="https://github.com/EpicGames/UnrealEngine/blob/master/Engine/Source/Runtime/CoreUObject/Public/UObject/NoExportTypes.h" />
    /// <returns>An instance of the child object.</returns>
    private FProperty ReadUStructPropertyChild(FPropertyTag tag, FName structType)
    {
        switch (structType.ToString())
        {
            case "Color":
                byte blue = StreamReader.ReadByte();
                byte green = StreamReader.ReadByte();
                byte red = StreamReader.ReadByte();
                byte alpha = StreamReader.ReadByte();

                var color = new FColor
                {
                    Red = red,
                    Green = green,
                    Blue = blue,
                    Alpha = alpha
                };
                return new FCustomProperty<FColor>(tag, color);

            case "Guid":
                Guid guid = StreamReader.ReadGuid();
                return new FCustomProperty<Guid>(tag, guid);

            case "Rotator":
                var rotator = new FRotator
                {
                    Pitch = StreamReader.ReadFloat32(),
                    Yaw = StreamReader.ReadFloat32(),
                    Roll = StreamReader.ReadFloat32(),
                };
                return new FCustomProperty<FRotator>(tag, rotator);

            case "Vector":
                var vector = new FVector
                {
                    X = StreamReader.ReadFloat32(),
                    Y = StreamReader.ReadFloat32(),
                    Z = StreamReader.ReadFloat32(),
                };
                return new FCustomProperty<FVector>(tag, vector);

            case "Vector2D":
                var vector2D = new FVector2D
                {
                    X = StreamReader.ReadFloat32(),
                    Y = StreamReader.ReadFloat32(),
                };
                return new FCustomProperty<FVector2D>(tag, vector2D);

            default:
                Logger?.Error(
                    $"Unknown struct property child type {structType} encountered at {StreamReader.Position} ({tag.Size} bytes)");

                byte[] bytes = StreamReader.ReadBytes(tag.Size);
                return new FCustomProperty<byte[]>(tag, bytes);
        }
    }
}
