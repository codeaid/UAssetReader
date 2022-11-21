using UAssetReader.Runtime.Core.Misc;
using UAssetReader.Runtime.Core.Serialization;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public readonly struct FPackageFileSummary
{
    /// <summary>
    /// Location into the file on disk for the asset registry tag data.
    /// </summary>
    public int AssetRegistryDataOffset { get; init; }

    /// <summary>
    /// Offset to the location in the file where the bulk data starts.
    /// </summary>
    public long BulkDataStartOffset { get; init; }

    /// <summary>
    /// Streaming install ChunkIDs
    /// </summary>
    public List<int> ChunkIdentifiers { get; init; }

    /// <summary>
    /// Engine version this package is compatible with.
    /// </summary>
    public FEngineVersion CompatibleWithEngineVersion { get; init; }

    /// <summary>
    /// Flags used to compress the file on save and decompress on load.
    /// </summary>
    public uint CompressionFlags { get; init; }

    /// <summary>
    /// Custom package version.
    /// </summary>
    public List<FCustomVersion> CustomVersion { get; init; }

    /// <summary>
    /// Location into the file on disk for the DependsMap data.
    /// </summary>
    public int DependsOffset { get; init; }

    /// <summary>
    /// Number of exports contained in this package.
    /// </summary>
    public int ExportCount { get; init; }

    /// <summary>
    /// Location into the file on disk for the ExportMap data.
    /// </summary>
    public int ExportOffset { get; init; }

    /// <summary>
    /// Licensee's Unreal Engine 3 file version.
    /// </summary>
    public int FileVersionLicenseeUE3 { get; init; }

    /// <summary>
    /// Licensee's Unreal Engine 4 file version.
    /// </summary>
    public int FileVersionLicenseeUE4 { get; init; }

    /// <summary>
    /// Unreal Engine 3 file version.
    /// </summary>
    public int FileVersionUE3 { get; init; }

    /// <summary>
    /// Unreal Engine 4 file version.
    /// </summary>
    public int FileVersionUE4 { get; init; }

    /// <summary>
    /// The Generic Browser folder name that this package lives in.
    /// </summary>
    public string FolderName { get; init; }

    /// <summary>
    /// Number of gatherable text data items in this package.
    /// </summary>
    public int GatherableTextDataCount { get; init; }

    /// <summary>
    /// Location into the file on disk for the gatherable text data items.
    /// </summary>
    public int GatherableTextDataOffset { get; init; }

    /// <summary>
    /// Data about previous versions of this package.
    /// </summary>
    public List<FGenerationInfo> Generations { get; init; }

    /// <summary>
    /// Current id for this package.
    /// </summary>
    public FGuid Guid { get; init; }

    /// <summary>
    /// Number of imports contained in this package.
    /// </summary>
    public int ImportCount { get; init; }

    /// <summary>
    /// Location into the file on disk for the ImportMap data.
    /// </summary>
    public int ImportOffset { get; init; }

    /// <summary>
    /// Number of names used in this package.
    /// </summary>
    public int NameCount { get; init; }

    /// <summary>
    /// Location into the file on disk for the name data.
    /// </summary>
    public int NameOffset { get; init; }

    /// <summary>
    /// The flags for the package.
    /// </summary>
    public uint PackageFlags { get; init; }

    /// <summary>
    /// Value that is used to determine if the package was saved by Epic (or licensee) or by a modder, etc.
    /// </summary>
    public uint PackageSource { get; init; }

    /// <summary>
    /// Number of preload dependencies used in this package.
    /// </summary>
    public int PreloadDependencyCount { get; init; }

    /// <summary>
    /// Location into the file on disk for the preload dependency data.
    /// </summary>
    public int PreloadDependencyOffset { get; init; }

    /// <summary>
    /// Engine version this package was saved with.
    /// </summary>
    public FEngineVersion SavedByEngineVersion { get; init; }

    /// <summary>
    /// Engine version this package was saved with.
    /// </summary>
    public int SearchableNamesOffset { get; init; }

    /// <summary>
    /// Number of soft package references contained in this package.
    /// </summary>
    public int SoftPackageReferencesCount { get; init; }

    /// <summary>
    /// Location into the file on disk for the soft package reference list.
    /// </summary>
    public int SoftPackageReferencesOffset { get; init; }

    /// <summary>
    /// Magic tag compared against PACKAGE_FILE_TAG to ensure that package is an Unreal package.
    /// </summary>
    public int Tag { get; init; }

    /// <summary>
    /// Thumbnail table offset.
    /// </summary>
    public int ThumbnailTableOffset { get; init; }

    /// <summary>
    /// Total size of all information that needs to be read in to create a FLinkerLoad.
    /// </summary>
    public int TotalHeaderSize { get; init; }

    /// <summary>
    /// If TRUE, this file will not be saved with version numbers or was saved without version numbers.
    /// </summary>
    public bool Unversioned { get; init; }

    /// <summary>
    /// Offset to the location in the file where the FWorldTileInfo data starts.
    /// </summary>
    public int WorldTitleInfoDataOffset { get; init; }
}
