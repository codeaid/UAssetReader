![Unreal Engine Logo](docs/logo.png "Unreal Engine logo")

# Unreal Engine Asset Reader

## About

This project contains a set of classes that can be used to read contents of
Unreal Engine 4 asset files (.uasset and .uexp).

Not
all [Unreal Engine property types](https://docs.unrealengine.com/4.27/en-US/API/Runtime/CoreUObject/UObject/)
are currently implemented as the list is rather extensive, however the most
common ones are available:

- FArrayProperty
- FBoolProperty
- FFloatProperty
- FIntProperty
- FObjectProperty
- FStrProperty
- FStructProperty

> To add support for additional properties declare a new property class by
> extending `FProperty` and implement an associated method in
> the `UPropertyReader`.

## Usage

### Opening file streams

To read asset files first instantiate a new `Stream` either by calling

```c#
using Stream headerStream = File.OpenRead(pathToHeader);
```

or by using the helper method available in `StreamUtils`:

```c#
using Stream headerStream = StreamUtils.CreateStreamFromPath(pathToHeader);
```

### Reading package file summary

Once the header stream is successfully created instantiate a header file reader
and extract the package file summary:

```c#
var headerReader = new UHeaderReader(headerStream);
FPackageFileSummary summary = headerReader.ReadFPackageFileSummary();
```

Package file summary contains a variety of information about the asset, such as
string names used in these files as well as lists of imports and exports
contained in the data file (same as the header file in older versions of Unreal
Engine).

### Populating global lists

Next read the lists of available names, exports and imports and store them
globally on the associated manager classes:

```c#
UNameManager.NameList = headerReader.ReadFNameEntryList(summary);
UObjectLinker.ExportList = headerReader.ReadFObjectExportList(summary);
List<FObjectImport> exportList = headerReader.ReadFObjectImportList(summary);
UObjectLinker.ImportList = exportList;
```

### Reading exports and their properties

Once package information has been read and name manager and object linker lists
have been updated, instantiate a new export reader:

```c#
using Stream exportStream = StreamUtils.CreateStreamFromPath(pathToExport);
var exportReader = new UPropertyReader(exportStream, logger);
```

Finally read properties of every export and store them in the way you prefer,
e.g. in tuples:

```c#
List<Tuple<FObjectExport, List<FProperty>>> exports = exportList.ConvertAll(
    export =>
    {
        List<FProperty> properties = exportReader.ReadProperties(summary, export);
        return new Tuple<FObjectExport, List<FProperty>>(export, properties);
    });
```

At this point you can process the list of exports how you like, be it by
filtering or serializing to JSON.

## Command Line Interface

The included CLI application can used by specifying the source header file path
as the `--header` option:

```shell
./UAssetReader --header "/path/to/header/file"
```

Additionally if the export data file does not reside in the same directory (with
the .uexp extension) then you can manually specify its location by including the
`--export` option:

```shell
./UAssetReader --header "/path/to/header/file" --export "/path/to/export/file"
```

> As can be seen in the [source code](/UAssetReader/Program.cs)
> of the `Program` class properties will only be read and will not be printed to
> the output so it is up to the user to implement such functionality.
