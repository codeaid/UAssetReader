namespace UAssetReader.Runtime.Core.Math;

public readonly struct FVector
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
    /// Vector's Z component.
    /// </summary>
    public float Z { get; init; }

    /// <summary>
    /// Converts current vector to string.
    /// </summary>
    /// <returns>Tuple string representation of the current vector.</returns>
    public override string ToString() => $"({X},{Y},{Z})";
}
