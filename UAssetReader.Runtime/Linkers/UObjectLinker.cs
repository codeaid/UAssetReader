using UAssetReader.Runtime.CoreUObject.UObject;

namespace UAssetReader.Runtime.Linkers;

public static class UObjectLinker
{
    /// <summary>
    /// List of available export objects.
    /// </summary>
    public static List<FObjectExport> ExportList { private get; set; } = new();

    /// <summary>
    /// List of available import objects.
    /// </summary>
    public static List<FObjectImport> ImportList { private get; set; } = new();

    /// <summary>
    /// Retrieve export object associated with the specified index.
    /// </summary>
    /// <param name="index">Source package index.</param>
    /// <returns>Export object at the specified index.</returns>
    /// <exception cref="Exception">Thrown if index is not an export index.</exception>
    public static FObjectExport ReadObjectExport(FPackageIndex index) =>
        ExportList.ElementAt(index.ToExport);

    /// <summary>
    /// Retrieve import object associated with the specified index.
    /// </summary>
    /// <param name="index">Source package index.</param>
    /// <returns>Import object at the specified index.</returns>
    /// <exception cref="Exception">Thrown if index is not an import index.</exception>
    public static FObjectImport ReadObjectImport(FPackageIndex index) =>
        ImportList.ElementAt(index.ToImport);
}
