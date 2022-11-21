using CommandLine;

namespace UAssetReader;

// ReSharper disable once ClassNeverInstantiated.Global
public class CommandLineOptions
{
    [Option('e', "export", Required = false, HelpText = "Path to asset data (.uexp) file")]
    public string? ExportPath { get; set; }

    [Option('h', "header", Required = true, HelpText = "Path to asset header (.uasset) file")]
    public string HeaderPath { get; set; } = null!;
}
