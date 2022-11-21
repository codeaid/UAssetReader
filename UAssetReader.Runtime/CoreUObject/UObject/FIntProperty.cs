using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public class FIntProperty : FProperty
{
    /// <summary>
    /// Property integer value.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="guid">Optional property GUID identifier.</param>
    public FIntProperty(FPropertyTag tag, FGuid? guid) : base(tag, guid)
    {
    }
}
