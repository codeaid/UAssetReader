using UAssetReader.Runtime.Core.Misc;

namespace UAssetReader.Runtime.CoreUObject.UObject;

public class FStructProperty : FProperty
{
    /// <summary>
    /// List of properties.
    /// </summary>
    public List<FProperty> Properties { get; init; } = new();

    /// <summary>
    /// Structure's GUID identifier.
    /// </summary>
    public FGuid StructGuid { get; init; }

    /// <summary>
    /// Name of the structure's type.
    /// </summary>
    public FName StructType { get; init; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="guid">Optional property GUID identifier.</param>
    public FStructProperty(FPropertyTag tag, FGuid? guid) : base(tag, guid)
    {
    }
}
