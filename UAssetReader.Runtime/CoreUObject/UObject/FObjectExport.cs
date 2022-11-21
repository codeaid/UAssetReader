using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public readonly struct FObjectExport
{
    /// <summary>
    /// Location of the resource for this export's class (if non-zero).
    /// </summary>
    public FPackageIndex ClassIndex { get; init; }

    /// <summary>
    /// Indicates whether the export should be created before creation dependencies.
    /// </summary>
    public bool CreateBeforeCreateDependencies { get; init; }

    /// <summary>
    /// Indicates whether the export should be created before serialization dependencies.
    /// </summary>
    public bool CreateBeforeSerializationDependencies { get; init; }

    /// <summary>
    /// The export table must serialize as a fixed size, this is use to index into a long list,
    /// which is later loaded into the array.
    /// </summary>
    public int FirstExportDependency { get; init; }

    /// <summary>
    /// Whether the export was forced into the export table via OBJECTMARK_ForceTagExp.
    /// </summary>
    public bool ForcedExport { get; init; }

    /// <summary>
    /// TRUE if this export is an asset object.
    /// </summary>
    public bool IsAsset { get; init; }

    /// <summary>
    /// Whether the export should be always loaded in editor game. TRUE doesn't mean that the object won't be loaded.
    /// </summary>
    public bool NotAlwaysLoadedForEditorGame { get; init; }

    /// <summary>
    /// Whether the export should be loaded on clients.
    /// </summary>
    public bool NotForClient { get; init; }

    /// <summary>
    /// Whether the export should be loaded on servers.
    /// </summary>
    public bool NotForServer { get; init; }

    /// <summary>
    /// The object flags for the UObject represented by this resource.
    /// </summary>
    public EObjectFlags ObjectFlags { get; init; }

    /// <summary>
    /// Reference to the name of the current export.
    /// </summary>
    public FName ObjectName { get; init; }

    /// <summary>
    /// If this object is a top level package (which must have been forced into the export table via
    /// OBJECTMARK_ForceTagExp) this is the package flags for the original package file.
    /// </summary>
    public uint PackageFlags { get; init; }

    /// <summary>
    /// If this object is a top level package (which must have been forced into the export table via
    /// OBJECTMARK_ForceTagExp) this is the GUID for the original package file.
    /// </summary>
    public FGuid PackageGuid { get; init; }

    /// <summary>
    /// Indicates whether the export should be serialized before creation dependencies.
    /// </summary>
    public bool SerializationBeforeCreateDependencies { get; init; }

    /// <summary>
    /// Indicates whether the export should be serialized before serialization dependencies.
    /// </summary>
    public bool SerializationBeforeSerializationDependencies { get; init; }

    /// <summary>
    /// The location (into the FLinker's underlying file reader archive) of the beginning of
    /// the data for this export's UObject.
    /// </summary>
    public long SerialOffset { get; init; }

    /// <summary>
    /// The number of bytes to serialize when saving/loading this export's UObject.
    /// </summary>
    public long SerialSize { get; init; }

    /// <summary>
    /// Location of the resource for this export's SuperField (parent).
    /// </summary>
    public FPackageIndex SuperIndex { get; init; }

    /// <summary>
    /// Location of the resource for this export's template/archetypes.
    /// </summary>
    public FPackageIndex TemplateIndex { get; init; }

    /// <summary>
    /// 	Location of this resource in export map.
    /// </summary>
    public FPackageIndex ThisIndex { get; init; }

    /// <summary>
    /// Converts current export to string.
    /// </summary>
    /// <returns>Name of the current export object.</returns>
    public override string ToString() => ObjectName.ToString();
}
