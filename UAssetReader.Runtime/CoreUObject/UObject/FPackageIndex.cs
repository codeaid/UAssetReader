namespace UAssetReader.Runtime.CoreUObject.UObject;

public readonly struct FPackageIndex
{
    /// <summary>
    /// List of available export objects.
    /// </summary>
    public static List<FObjectExport> Exports { get; set; } = new();

    /// <summary>
    /// List of available import objects.
    /// </summary>
    public static List<FObjectImport> Imports { get; set; } = new();

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

    /// <summary>
    /// Returns the export object associated with the current index.
    /// </summary>
    /// <exception cref="Exception">Thrown if current index does not reference an export index.</exception>
    public FObjectExport ToExport
    {
        get
        {
            // Ensure current package index points to an export index.
            if (!IsExport)
            {
                throw new Exception($"Current index ({Index}) is not an export index entry");
            }

            int index = Index - 1;
            return Exports.ElementAt(index);
        }
    }

    /// <summary>
    /// Returns the import object associated with the current index.
    /// </summary>
    /// <exception cref="Exception">Thrown if current index does not reference an import index.</exception>
    public FObjectImport ToImport
    {
        get
        {
            // Ensure current package index points to an import index.
            if (!IsImport)
            {
                throw new Exception($"Current index ({Index}) is not an import index entry");
            }

            int index = -Index - 1;
            return Imports.ElementAt(index);
        }
    }

    /// <summary>
    /// Converts current index to the name of the referenced object.
    /// </summary>
    public string Value => IsImport
        ? ToImport.ObjectName.ToString()
        : IsExport
            ? ToExport.ObjectName.ToString()
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
    public static bool operator ==(FPackageIndex packageIndex, string s) => packageIndex.Value == s;

    /// <summary>
    /// Allows comparing name of an FPackageIndex object to a string for inequality.
    /// </summary>
    /// <param name="packageIndex">Source package index instance.</param>
    /// <param name="s">Target string to compare the object name to.</param>
    /// <returns>Boolean value indicating if FPackageIndex object's name does not match the target string.</returns>
    public static bool operator !=(FPackageIndex packageIndex, string s) => !(packageIndex == s);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is FPackageIndex other && other.Index == Index;

    /// <inheritdoc />
    public override int GetHashCode() => Index;
}
