using CommandLine;

namespace UAssetReader;

internal static class Program
{
    // ReSharper disable once InconsistentNaming
    public static int Main(string[] args) =>
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .MapResult(
                (options => 0),
                _ => 1);
}
