using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public class FFloatProperty : FProperty
{
    /// <summary>
    /// Property IEEE 32-bit floating point value.
    /// </summary>
    public float Value { get; init; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="guid">Optional property GUID identifier.</param>
    public FFloatProperty(FPropertyTag tag, FGuid? guid) : base(tag, guid)
    {
    }
}
