using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.Core.Serialization;

public readonly struct FCustomVersion
{
    /// <summary>
    /// Unique custom key.
    /// </summary>
    public FGuid Key { get; init; }
}
