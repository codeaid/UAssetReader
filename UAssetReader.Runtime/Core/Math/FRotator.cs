namespace UAssetReader.Runtime.Core.Math;

public readonly struct FRotator
{
    /// <summary>
    /// Rotator's pitch value.
    /// </summary>
    public float Pitch { get; init; }

    /// <summary>
    /// Rotator's roll value.
    /// </summary>
    public float Roll { get; init; }

    /// <summary>
    /// Rotators yaw value.
    /// </summary>
    public float Yaw { get; init; }

    /// <summary>
    /// Converts current rotator to string.
    /// </summary>
    /// <returns>Tuple string representation of the current rotator.</returns>
    public override string ToString() => $"({Pitch},{Roll},{Yaw})";
}
