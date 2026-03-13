# Breaking Changes

## Since version 3.0.*

### Snapshot Helper Rework

The `SnapshotHelper` API has been significantly reworked with breaking changes to the available methods and their signatures:

#### Key Changes
- **Moved** from `SoloX.CodeQuality.Test.Helpers.XUnit` to `SoloX.CodeQuality.Test.Helpers` project and namespace.

- **Dependency change**: The `SnapshotHelper` class no longer depends on xUnit-specific types, making it more flexible and reusable
across different testing frameworks.

- **Static to instance**: The `SnapshotHelper` class is no longer static. You must create an instance of `SnapshotHelper` to use its methods.

- **New asynchronous API**: All snapshot methods are now async (`Task`-based) and must be awaited.
  - Old: `AssertSnapshot(string generated, string snapshotName, string location)`
  - New: `await CompareTextSnapshotAsync(string name, string content)`

- **PNG file snapshot support**: New support for comparing PNG image snapshots has been added via `ComparePngSnapshotAsync()`.
  - Includes configurable `differencesThreshold` parameter for image comparison tolerance.

- **Configuration-based initialization**: The constructor now uses a fluent configuration pattern.
  - Old: location was specified as parameter of the static AssertSnapshot method.
  - New: `new SnapshotHelper(options => options.IntermediateFolder = intermediateFolder)`

- **Force replace capability**: All comparison methods now support a `forceReplaceSnapshot` parameter to update existing snapshots.

#### Migration Example

```csharp
// Old code (no longer works)
var location = "Path where to read/write snapshot files";
SnapshotHelper.AssertSnapshot("Some text to match against the snapshot", snapshotName, location);

// New code
var helper = new SnapshotHelper(options => options.RootPath = "Root path where to read/write snapshot files");
await helper.CompareTextSnapshotAsync(snapshotName, "Some text to match against the snapshot");

// For PNG snapshots (new capability)
await helper.ComparePngSnapshotAsync(snapshotName, pngStream, differencesThreshold: 0.01);
```

#### Migration Steps

1. Setup with `SnapshotHelper` instantiation and use the configuration action.
2. Change all `SnapshotHelper.AssertSnapshot()` calls to `CompareTextSnapshotAsync()` and add `await`.
3. Review snapshot test methods and mark them as `async Task`.
4. Existing snapshot text files (`.snapshot`) are compatible; you just need to rename with (`.snapshot.ref.txt`).

### Dependency Changes

- **FluentAssertions** dependency has been **removed** from the package.
  - The package no longer includes FluentAssertions as a transitive dependency.
  - If your project requires FluentAssertions, you can still use it by explicitly adding it as a project dependency.
  - Consider using alternative assertion libraries such as:
    - xUnit's built-in assertions
    - Shouldly (already used in CodeQuality project)
    - Other assertion frameworks compatible with .NET

### xUnit V3 Support

- New **xUnit V3 support** has been added through the `SoloX.CodeQuality.Test.Helpers.XUnit.V3` package.
- Projects migrating to xUnit V3 should use the new V3-specific helpers instead of the legacy helpers.
- The legacy xUnit V2 support is still available but may not receive further updates.