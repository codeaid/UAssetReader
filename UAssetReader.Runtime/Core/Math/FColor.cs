namespace UAssetReader.Runtime.Core.Math;

public readonly struct FColor
{
    /// <summary>
    /// Color's red component.
    /// </summary>
    public byte Red { get; init; }

    /// <summary>
    /// Color's green component.
    /// </summary>
    public byte Green { get; init; }

    /// <summary>
    /// Color's blue component.
    /// </summary>
    public byte Blue { get; init; }

    /// <summary>
    /// Color's alpha component.
    /// </summary>
    public byte Alpha { get; init; }

    /// <summary>
    /// Converts current color value to a string.
    /// </summary>
    /// <returns>Hex representation of the current color.</returns>
    public override string ToString() =>
        string.Concat("#", Array.ConvertAll(new[] {Red, Green, Blue, Alpha}, x => x.ToString("x2")));
}
