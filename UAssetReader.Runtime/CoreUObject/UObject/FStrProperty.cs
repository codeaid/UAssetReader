using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public class FStrProperty : FProperty
{
    /// <summary>
    /// Property string value.
    /// </summary>
    public string Value { get; init; } = "";

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="guid">Optional property GUID identifier.</param>
    public FStrProperty(FPropertyTag tag, FGuid? guid) : base(tag, guid)
    {
    }
}
