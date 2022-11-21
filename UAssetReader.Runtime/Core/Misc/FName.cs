using UAssetReader.Runtime.Core.UObject;
using UAssetReader.Runtime.Linkers;

namespace UAssetReader.Runtime.Core.Misc;

public readonly struct FName
{
    /// <summary>
    /// Target index of the name reference.
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// Retrieves the name entry associated with the current index.
    /// </summary>
    /// <exception cref="Exception">Thrown if an invalid index is encountered.</exception>
    public FNameEntry Name => UNameManager.ReadNameEntry(this);

    /// <summary>
    /// Converts current name entry to string.
    /// </summary>
    /// <returns>Value of the associated name entry.</returns>
    public override string ToString() => Name.Value;

    /// <summary>
    /// Allows comparing value of an FName instance with a string for equality.
    /// </summary>
    /// <param name="name">Source FName instance.</param>
    /// <param name="s">Target string to compare the instance to.</param>
    /// <returns>Boolean value indicating if FName entry's value matches the target string.</returns>
    public static bool operator ==(FName name, string s) => name.ToString() == s;

    /// <summary>
    /// Allows comparing value of an FName instance with a string for inequality.
    /// </summary>
    /// <param name="name">Source FName instance.</param>
    /// <param name="s">Target string to compare the instance to.</param>
    /// <returns>Boolean value indicating if FName entry's value does not match the target string.</returns>
    public static bool operator !=(FName name, string s) => !(name == s);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is FName other && other.Index == Index;

    /// <inheritdoc />
    public override int GetHashCode() => Index;
}
