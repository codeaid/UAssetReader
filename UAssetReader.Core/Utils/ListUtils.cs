namespace UAssetReader.Core.Utils;

public static class ListUtils
{
    /// <summary>
    /// Run a function the specified number of times and build a list of results from the calls.
    /// </summary>
    /// <param name="times">Number of values to read from the stream.</param>
    /// <param name="generator">Callback to invoke for each value.</param>
    /// <typeparam name="T">Type of the values being read.</typeparam>
    /// <returns>List of values read from the stream.</returns>
    public static List<T> CreateWithGenerator<T>(int times, Func<T> generator) =>
        Enumerable.Range(0, times).Select(i => generator()).ToList();
}
