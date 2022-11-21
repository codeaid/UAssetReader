using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public class FObjectProperty : FProperty
{
    /// <summary>
    /// Index of the referenced object.
    /// </summary>
    public FPackageIndex Index { get; init; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="guid">Optional property GUID identifier.</param>
    public FObjectProperty(FPropertyTag tag, FGuid? guid) : base(tag, guid)
    {
    }
}
