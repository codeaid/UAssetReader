namespace UAssetReader.Runtime.Core.Misc;

public readonly struct FEngineVersion
{
    /// <summary>
    /// Version's changelist value.
    /// </summary>
    public uint Changelist { get; init; }

    /// <summary>
    /// Engine's major version.
    /// </summary>
    public uint Major { get; init; }

    /// <summary>
    /// Engine's minor version.
    /// </summary>
    public uint Minor { get; init; }

    /// <summary>
    /// Engine's patch version.
    /// </summary>
    public uint Patch { get; init; }

    /// <summary>
    /// Converts current engine version to string.
    /// </summary>
    /// <returns>Current version formatted as a semantic version string.</returns>
    public override string ToString() => $"{Major}.{Minor}.{Patch}.{Changelist}";
}
