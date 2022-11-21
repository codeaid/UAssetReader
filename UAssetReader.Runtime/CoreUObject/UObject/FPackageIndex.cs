using UAssetReader.Runtime.Linkers;

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
    /// Check that this is an export and return the index into the export map.
    /// </summary>
    public int ToExport =>
        IsExport ? Index - 1 : throw new Exception($"Current index '{Index}' is not an export index");

    /// <summary>
    /// Check that this is an import and return the index into the import map.
    /// </summary>
    public int ToImport =>
        IsImport ? -Index - 1 : throw new Exception($"Current index '{Index}' is not an import index");

    /// <summary>
    /// Converts current index to the name of the referenced object.
    /// </summary>
    public string Value =>
        IsImport
            ? UObjectLinker.ReadObjectImport(this).ToString()
            : IsExport
                ? UObjectLinker.ReadObjectExport(this).ToString()
                : "[unknown]";

    /// <summary>
    /// Converts current index to string.
    /// </summary>
    /// <returns>Name of the referenced object.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Allows comparing name of an FPackageIndex object to a string for equality.
    /// </summary>
    /// <param name="packageIndex">Source package index instance.</param>
    /// <param name="s">Target string to compare the object name to.</param>
    /// <returns>Boolean value indicating if FPackageIndex object's name matches the target string.</returns>
    public static bool operator ==(FPackageIndex packageIndex, string s) => packageIndex.ToString() == s;

    /// <summary>
    /// Allows comparing name of an FPackageIndex object to a string for inequality.
    /// </summary>
    /// <param name="packageIndex">Source package index instance.</param>
    /// <param name="s">Target string to compare the object name to.</param>
    /// <returns>Boolean value indicating if FPackageIndex object's name does not match the target string.</returns>
    public static bool operator !=(FPackageIndex packageIndex, string s) => !(packageIndex == s);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is FPackageIndex packageIndex && packageIndex.Index == Index;

    /// <inheritdoc />
    public override int GetHashCode() => Index;
}
