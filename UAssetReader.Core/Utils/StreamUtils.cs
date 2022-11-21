namespace UAssetReader.Core.Utils;

public static class StreamUtils
{
    /// <summary>
    /// Create a new file stream from the specified path name.
    /// </summary>
    /// <param name="path">Target path name.</param>
    /// <param name="bufferSize">Buffer size to use when reading the file.</param>
    /// <returns>Instance of the file stream.</returns>
    public static Stream CreateStreamFromPath(string path, int bufferSize = 4096) =>
        new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
}
