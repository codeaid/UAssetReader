using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public abstract class FProperty
{
    /// <summary>
    /// Optional property GUID.
    /// </summary>
    public FGuid? Guid { get; }

    /// <summary>
    /// Property tag containing information about the current property.
    /// </summary>
    public FPropertyTag Tag { get; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="guid">Optional property GUID identifier.</param>
    public FProperty(FPropertyTag tag, FGuid? guid)
    {
        Guid = guid;
        Tag = tag;
    }

    /// <summary>
    /// Converts current property instance to string.
    /// </summary>
    /// <returns>Property name.</returns>
    public override string ToString() => Tag.Name.ToString();
}
