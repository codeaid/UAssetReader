using Serilog;
using UAssetReader.Core.Utils;
using UAssetReader.Runtime.Core.Misc;
using UAssetReader.Runtime.Core.Serialization;
using UAssetReader.Runtime.Core.UObject;
using UAssetReader.Runtime.CoreUObject.UObject;

namespace UAssetReader.Core.IO;

public class UHeaderReader
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
    public UHeaderReader(Stream stream, ILogger? logger = null)
    {
        Logger = logger;
        ObjectReader = new UObjectReader(stream, logger);
        StreamReader = new UStreamReader(stream, logger);
    }

    /// <summary>
    /// Read an instance of FCustomVersion class from the current stream.
    /// </summary>
    /// <returns>An instance of FCustomVersion.</returns>
    private FCustomVersion ReadFCustomVersion()
    {
        Logger?.Debug($"Reading FCustomVersion at {StreamReader.Position}");

        return new FCustomVersion
        {
            Key = ObjectReader.ReadFGuid()
        };
    }

    /// <summary>
    /// Read an instance of FEngineVersion class from the current stream.
    /// </summary>
    /// <returns>An instance of FEngineVersion.</returns>
    private FEngineVersion ReadFEngineVersion()
    {
        Logger?.Debug($"Reading FEngineVersion at {StreamReader.Position}");

        return new FEngineVersion
        {
            Major = StreamReader.ReadUInt32(),
            Minor = StreamReader.ReadUInt32(),
            Patch = StreamReader.ReadUInt32(),
            Changelist = StreamReader.ReadUInt32(),
        };
    }

    /// <summary>
    /// Read an instance of FGenerationInfo class from the current stream.
    /// </summary>
    /// <returns>An instance of FGenerationInfo.</returns>
    private FGenerationInfo ReadFGenerationInfo()
    {
        Logger?.Debug($"Reading FGenerationInfo at {StreamReader.Position}");

        return new FGenerationInfo
        {
            ExportCount = StreamReader.ReadInt32(),
            NameCount = StreamReader.ReadInt32(),
        };
    }

    /// <summary>
    /// Read an instance of FNameEntry class from the current stream.
    /// </summary>
    /// <returns>An instance of FNameEntry.</returns>
    private FNameEntry ReadFNameEntry()
    {
        Logger?.Debug($"Reading FNameEntry at {StreamReader.Position}");

        return new FNameEntry
        {
            Value = StreamReader.ReadFString(),
            NonCasePreservingHash = StreamReader.ReadUInt16(),
            CasePreservingHash = StreamReader.ReadUInt16(),
        };
    }

    /// <summary>
    /// Read list of FNameEntry class instances from the current stream.
    /// </summary>
    /// <param name="summary">Source package file summary.</param>
    /// <returns>List of names.</returns>
    public List<FNameEntry> ReadFNameEntryList(FPackageFileSummary summary)
    {
        Logger?.Debug($"Reading list of FNameEntry objects at {StreamReader.Position}");

        // Remember the current stream position.
        long originalPosition = StreamReader.Position;

        // Read list of entries from the stream.
        StreamReader.Seek(summary.NameOffset);
        List<FNameEntry> list = ListUtils.CreateWithGenerator(summary.NameCount, ReadFNameEntry);

        // Reset stream position to its original position.
        StreamReader.Seek(originalPosition);

        return list;
    }

    /// <summary>
    /// Read an instance of FObjectExport class from the current stream.
    /// </summary>
    /// <returns>An instance of FObjectExport.</returns>
    private FObjectExport ReadFObjectExport()
    {
        Logger?.Debug($"Reading FObjectExport at {StreamReader.Position}");

        return new FObjectExport
        {
            ClassIndex = ObjectReader.ReadFPackageIndex(),
            SuperIndex = ObjectReader.ReadFPackageIndex(),
            TemplateIndex = ObjectReader.ReadFPackageIndex(),
            ThisIndex = ObjectReader.ReadFPackageIndex(),
            ObjectName = ObjectReader.ReadFName(),
            ObjectFlags = ObjectReader.ReadEObjectFlags(),
            SerialSize = StreamReader.ReadInt64(),
            SerialOffset = StreamReader.ReadInt64(),
            ForcedExport = StreamReader.ReadFBool(),
            NotForClient = StreamReader.ReadFBool(),
            NotForServer = StreamReader.ReadFBool(),
            PackageGuid = ObjectReader.ReadFGuid(),
            PackageFlags = StreamReader.ReadUInt32(),
            NotAlwaysLoadedForEditorGame = StreamReader.ReadFBool(),
            IsAsset = StreamReader.ReadFBool(),
            FirstExportDependency = StreamReader.ReadInt32(),
            SerializationBeforeSerializationDependencies = StreamReader.ReadFBool(),
            CreateBeforeSerializationDependencies = StreamReader.ReadFBool(),
            SerializationBeforeCreateDependencies = StreamReader.ReadFBool(),
            CreateBeforeCreateDependencies = StreamReader.ReadFBool(),
        };
    }

    /// <summary>
    /// Read list of FObjectExport class instances from the current stream.
    /// </summary>
    /// <param name="summary">Source package file summary.</param>
    /// <returns>List of exports.</returns>
    public List<FObjectExport> ReadFObjectExportList(FPackageFileSummary summary)
    {
        Logger?.Debug($"Reading list of FObjectExport objects at {StreamReader.Position}");

        // Remember the current stream position.
        long originalPosition = StreamReader.Position;

        // Read list of entries from the stream.
        StreamReader.Seek(summary.ExportOffset);
        List<FObjectExport> list = ListUtils.CreateWithGenerator(summary.ExportCount, ReadFObjectExport);

        // Reset stream position to its original position.
        StreamReader.Seek(originalPosition);

        return list;
    }

    /// <summary>
    /// Read an instance of FObjectImport class from the current stream.
    /// </summary>
    /// <returns>An instance of FObjectImport.</returns>
    private FObjectImport ReadFObjectImport()
    {
        Logger?.Debug($"Reading FObjectImport at {StreamReader.Position}");

        return new FObjectImport
        {
            ClassPackage = ObjectReader.ReadFName(),
            ClassName = ObjectReader.ReadFName(),
            SourceIndex = ObjectReader.ReadFPackageIndex(),
            ObjectName = ObjectReader.ReadFName(),
        };
    }

    /// <summary>
    /// Read list of FObjectImport class instances from the current stream.
    /// </summary>
    /// <param name="summary">Source package file summary.</param>
    /// <returns>List of imports.</returns>
    public List<FObjectImport> ReadFObjectImportList(FPackageFileSummary summary)
    {
        Logger?.Debug($"Reading list of FObjectImport objects at {StreamReader.Position}");

        // Remember the current stream position.
        long originalPosition = StreamReader.Position;

        // Read list of entries from the stream.
        StreamReader.Seek(summary.ImportOffset);
        List<FObjectImport> list = ListUtils.CreateWithGenerator(summary.ImportCount, ReadFObjectImport);

        // Reset stream position to its original position.
        StreamReader.Seek(originalPosition);

        return list;
    }

    /// <summary>
    /// Read an instance of FPackageFileSummary class from the current stream.
    /// </summary>
    /// <returns>An instance of FPackageFileSummary.</returns>
    public FPackageFileSummary ReadFPackageFileSummary()
    {
        Logger?.Debug($"Reading FPackageFileSummary at {StreamReader.Position}");

        return new FPackageFileSummary()
        {
            Tag = StreamReader.ReadInt32(),
            FileVersionUE4 = StreamReader.ReadInt32(),
            FileVersionLicenseeUE4 = StreamReader.ReadInt32(),
            FileVersionUE3 = StreamReader.ReadInt32(),
            FileVersionLicenseeUE3 = StreamReader.ReadInt32(),
            CustomVersion = StreamReader.ReadFList(ReadFCustomVersion),
            TotalHeaderSize = StreamReader.ReadInt32(),
            FolderName = StreamReader.ReadFString(),
            PackageFlags = StreamReader.ReadUInt32(),
            NameCount = StreamReader.ReadInt32(),
            NameOffset = StreamReader.ReadInt32(),
            GatherableTextDataCount = StreamReader.ReadInt32(),
            GatherableTextDataOffset = StreamReader.ReadInt32(),
            ExportCount = StreamReader.ReadInt32(),
            ExportOffset = StreamReader.ReadInt32(),
            ImportCount = StreamReader.ReadInt32(),
            ImportOffset = StreamReader.ReadInt32(),
            DependsOffset = StreamReader.ReadInt32(),
            SoftPackageReferencesCount = StreamReader.ReadInt32(),
            SoftPackageReferencesOffset = StreamReader.ReadInt32(),
            SearchableNamesOffset = StreamReader.ReadInt32(),
            ThumbnailTableOffset = StreamReader.ReadInt32(),
            Guid = ObjectReader.ReadFGuid(),
            Generations = StreamReader.ReadFList(ReadFGenerationInfo),
            SavedByEngineVersion = ReadFEngineVersion(),
            CompatibleWithEngineVersion = ReadFEngineVersion(),
            CompressionFlags = StreamReader.ReadUInt32(),
            PackageSource = StreamReader.ReadUInt32(),
            Unversioned = StreamReader.ReadFBool(),
            AssetRegistryDataOffset = StreamReader.ReadInt32(),
            BulkDataStartOffset = StreamReader.ReadInt64(),
            WorldTitleInfoDataOffset = StreamReader.ReadInt32(),
            ChunkIdentifiers = StreamReader.ReadFList(StreamReader.ReadInt32),
            PreloadDependencyCount = StreamReader.ReadInt32(),
            PreloadDependencyOffset = StreamReader.ReadInt32(),
        };
    }
}
