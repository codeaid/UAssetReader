using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public class FArrayProperty : FProperty
{
    /// <summary>
    /// Name of the array item type.
    /// </summary>
    public FName ArrayType { get; init; }

    /// <summary>
    /// List of array properties.
    /// </summary>
    public List<FProperty> Items { get; init; } = new();

    /// <summary>
    /// Number of properties in the array.
    /// </summary>
    public int Size { get; init; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="guid">Optional property GUID identifier.</param>
    public FArrayProperty(FPropertyTag tag, FGuid? guid) : base(tag, guid)
    {
    }
}
