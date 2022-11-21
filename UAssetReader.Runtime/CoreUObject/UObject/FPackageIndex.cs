namespace UAssetReader.Runtime.CoreUObject.UObject;

public readonly struct FPackageIndex
{
    /// <summary>
    /// Index value.
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// TRUE if this is an index into the export map.
    /// </summary>
    public bool IsExport => Index > 0;

    /// <summary>
    /// TRUE if this is an index into the import map.
    /// </summary>
    public bool IsImport => Index < 0;

    /// <summary>
    /// TRUE if this is an not an index into any of the maps.
    /// </summary>
    public bool IsNull => Index == 0;
}
