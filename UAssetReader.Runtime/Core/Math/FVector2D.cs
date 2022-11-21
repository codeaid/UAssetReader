namespace UAssetReader.Runtime.Core.Math;

public readonly struct FVector2D
{
    /// <summary>
    /// Vector's X component.
    /// </summary>
    public float X { get; init; }

    /// <summary>
    /// Vector's Y component.
    /// </summary>
    public float Y { get; init; }

    /// <summary>
    /// Converts current 2D vector to string.
    /// </summary>
    /// <returns>Tuple representation of the current vector.</returns>
    public override string ToString() => $"({X},{Y})";
}
