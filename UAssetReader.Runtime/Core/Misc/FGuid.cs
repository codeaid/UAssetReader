namespace UAssetReader.Runtime.Core.Misc;

public readonly struct FGuid
{
    /// <summary>
    /// Object's GUID value.
    /// </summary>
    public Guid Value { get; init; }

    /// <summary>
    /// Converts current object to string.
    /// </summary>
    /// <returns>The formatted GUID string.</returns>
    public override string ToString() => ToString(EGuidFormats.DigitsWithHyphens);

    /// <summary>
    /// Converts current object to string using the specified format.
    /// </summary>
    /// <param name="format">Target GUID format to use.</param>
    /// <returns>The formatted GUID string.</returns>
    public string ToString(EGuidFormats format) => format switch
    {
        EGuidFormats.DigitsWithHyphens => Value.ToString("D"),
        EGuidFormats.DigitsWithHyphensInBraces => Value.ToString("B"),
        EGuidFormats.DigitsWithHyphensInParentheses => Value.ToString("P"),
        EGuidFormats.HexValuesInBraces => Value.ToString("X"),
        _ => Value.ToString(),
    };
}
