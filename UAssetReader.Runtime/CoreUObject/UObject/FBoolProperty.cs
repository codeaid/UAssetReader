using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public class FBoolProperty : FProperty
{
    /// <summary>
    /// Property boolean value.
    /// </summary>
    public bool Value { get; init; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="guid">Optional property GUID identifier.</param>
    public FBoolProperty(FPropertyTag tag, FGuid? guid) : base(tag, guid)
    {
    }
}
