using CommandLine;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;
using UAssetReader.Core.IO;
using UAssetReader.Core.Utils;
using UAssetReader.Runtime.Core.UObject;
using UAssetReader.Runtime.CoreUObject.UObject;
using UAssetReader.Runtime.Linkers;

namespace UAssetReader;

internal static class Program
{
    // ReSharper disable once InconsistentNaming
    public static int Main(string[] args) =>
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .MapResult(
                (options =>
                {
                    // Instantiate application debug logger.
                    Logger logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                        .CreateLogger();

                    // Ensure asset file exists.
                    if (!File.Exists(options.HeaderPath))
                    {
                        logger.Error("Specified asset file does not exist");
                        return 1;
                    }

                    // Ensure export file exists.
                    if (options.ExportPath != null)
                    {
                        if (!File.Exists(options.ExportPath))
                        {
                            logger.Error("Specified export file does not exist");
                            return 2;
                        }
                    }
                    else
                    {
                        // Check if a .uexp file exists alongside the specified header file.
                        string exportPath = Path.ChangeExtension(options.HeaderPath, "uexp");
                        if (File.Exists(exportPath))
                        {
                            logger.Information($"Using discovered export file at {Path.GetFileName(exportPath)}.");
                            logger.Information("Please use the --export option to avoid automatic detection.");

                            // Use automatically discovered export file found alongside the current header file.
                            options.ExportPath = exportPath;
                        }
                        else
                        {
                            logger.Information($"Using header file as export file.");
                            logger.Information("Please use the --export option to avoid automatic detection.");

                            // Use asset file also as an export file (older Unreal Engine versions that didn't split
                            // asset header and body into separate files.
                            options.ExportPath = options.HeaderPath;
                        }
                    }

                    // Read header file package information.
                    using Stream headerStream = StreamUtils.CreateStreamFromPath(options.HeaderPath);
                    var headerReader = new UHeaderReader(headerStream, logger);
                    FPackageFileSummary summary = headerReader.ReadFPackageFileSummary();

                    // Read list of name entries specified in the package information and store them globally.
                    List<FNameEntry> nameList = headerReader.ReadFNameEntryList(summary);
                    UNameManager.NameList = nameList;

                    // Read list of available export objects and store them globally.
                    List<FObjectExport> exportList = headerReader.ReadFObjectExportList(summary);
                    UObjectLinker.ExportList = exportList;

                    // Read list of available import objects and store them globally.
                    List<FObjectImport> importList = headerReader.ReadFObjectImportList(summary);
                    UObjectLinker.ImportList = importList;

                    // Initialise the export file reader.
                    using Stream exportStream = StreamUtils.CreateStreamFromPath(options.ExportPath);
                    var exportReader = new UPropertyReader(exportStream, logger);

                    // Read properties of all export objects.
                    // ReSharper disable once UnusedVariable
                    List<Tuple<FObjectExport, List<FProperty>>> exports = exportList.ConvertAll(export =>
                    {
                        List<FProperty> properties = exportReader.ReadProperties(summary, export);
                        return new Tuple<FObjectExport, List<FProperty>>(export, properties);
                    });

                    // Place a breakpoint on the following line to inspect tuples containing exports and their props.
                    return 0;
                }),
                _ => 1);
}
