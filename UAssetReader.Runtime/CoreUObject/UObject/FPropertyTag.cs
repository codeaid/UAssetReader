using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public readonly struct FPropertyTag
{
    /// <summary>
    /// Property index.
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// Property name.
    /// </summary>
    public FName Name { get; init; }

    /// <summary>
    /// Property size in bytes.
    /// </summary>
    public int Size { get; init; }

    /// <summary>
    /// Property type name.
    /// </summary>
    public FName Type { get; init; }
}