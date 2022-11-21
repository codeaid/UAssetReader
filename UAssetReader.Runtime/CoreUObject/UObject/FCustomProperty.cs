namespace UAssetReader.Runtime.CoreUObject.UObject;

public class FCustomProperty<T> : FProperty
{
    /// <summary>
    /// Custom property value.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="tag">Property tag.</param>
    /// <param name="value">Property value.</param>
    public FCustomProperty(FPropertyTag tag, T value) : base(tag, null)
    {
        Value = value;
    }
}
