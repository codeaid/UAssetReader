namespace UAssetReader.Runtime.Core.UObject;

public readonly struct FNameEntry
{
    /// <summary>
    /// Case preserving value hash.
    /// </summary>
    public ushort CasePreservingHash { get; init; }

    /// <summary>
    /// Non-case preserving value hash.
    /// </summary>
    public ushort NonCasePreservingHash { get; init; }

    /// <summary>
    /// Name entry value.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Converts current name entry to string.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value;
}
