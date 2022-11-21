using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public readonly struct FObjectImport
{
    /// <summary>
    /// The name of the class for the UObject represented by this resource.
    /// </summary>
    public FName ClassName { get; init; }

    /// <summary>
    /// The name of the package that contains the class of the UObject represented by this resource.
    /// </summary>
    public FName ClassPackage { get; init; }

    /// <summary>
    /// Reference to the name of the current import.
    /// </summary>
    public FName ObjectName { get; init; }

    /// <summary>
    /// Index into SourceLinker's ExportMap for the export associated with this import's UObject.
    /// </summary>
    public FPackageIndex SourceIndex { get; init; }

    /// <summary>
    /// Converts current import to string.
    /// </summary>
    /// <returns>Name of the current import object.</returns>
    public override string ToString() => ObjectName.ToString();
}
