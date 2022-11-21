namespace UAssetReader.Runtime.CoreUObject.UObject;

public readonly struct FGenerationInfo
{
    /// <summary>
    /// Number of exports in the linker's ExportMap for this generation.
    /// </summary>
    public int ExportCount { get; init; }

    /// <summary>
    /// Number of names in the linker's NameMap for this generation.
    /// </summary>
    public int NameCount { get; init; }
}
