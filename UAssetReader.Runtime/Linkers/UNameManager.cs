using UAssetReader.Runtime.Core.Misc;
using UAssetReader.Runtime.Core.UObject;

namespace UAssetReader.Runtime.Linkers;

public static class UNameManager
{
    /// <summary>
    /// List of available name entries to use during the lookup.
    /// </summary>
    public static List<FNameEntry> NameList { private get; set; } = new();

    /// <summary>
    /// Retrieve a name entry at the specified index.
    /// </summary>
    /// <param name="name">Source name index entry.</param>
    /// <returns>Name entry.</returns>
    /// <exception cref="Exception">Throw if the specified index is out of bounds.</exception>
    public static FNameEntry ReadNameEntry(FName name)
    {
        // Ensure name list has been populated.
        if (NameList.Count == 0)
        {
            throw new Exception("Name list has not been populated");
        }

        // Ensure name index is a positive value.
        if (name.Index < 0)
        {
            throw new Exception($"Name index must be a positive value: {name.Index}");
        }

        // Ensure index is not larger than the number of available names.
        if (name.Index >= NameList.Count)
        {
            throw new Exception($"Name index is outside available bounds: {name.Index} (out of {NameList.Count})");
        }

        return NameList.ElementAt(name.Index);
    }
}
