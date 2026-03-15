# Breaking Changes

## Since version 3.0.*

### Snapshot Helper Rework

The `SnapshotHelper` API has been significantly reworked with breaking changes to the available methods and their signatures:

#### Key Changes
- **Moved** from `SoloX.CodeQuality.Test.Helpers.XUnit` to `SoloX.CodeQuality.Test.Helpers` project and namespace.

- **Renamed** from `SnapshotHelper` to `SnapshotTest`.

- **Dependency change**: The snapshot test classes no longer depends on xUnit-specific types, making it more flexible and reusable
across different testing frameworks.

- **Static to builder**: The `SnapshotHelper` class is no longer static. You must use the builder to get an instance of `SnapshotTest` to use its methods.

- **New asynchronous API**: All snapshot methods are now async (`Task`-based) and must be awaited.
  - Old: `AssertSnapshot(string generated, string snapshotName, string location)`
  - New: `await CompareSnapshotAsync(string name, string content)`

- **PNG file snapshot support**: New support for comparing PNG image snapshots has been added via a Png Snapshot strategy.
  - Includes configurable `differencesThreshold` parameter for image comparison tolerance.

- **Force replace capability**: The comparison methods now support a `forceReplaceSnapshot` parameter to update existing snapshots.

#### Migration Example

```csharp
// Old code (no longer works)
var location = "Path where to read/write snapshot files";
SnapshotHelper.AssertSnapshot("Some text to match against the snapshot", snapshotName, location);

// New code
var snapshotTest= SnapshotTestBuilder
    .Create()
    .WithLocation("Root path where to read/write snapshot files")
    .WithTextStrategy()
    .Build();

await snapshotTest.CompareSnapshotAsync(snapshotName, "Some text to match against the snapshot");

// For PNG snapshots (new capability)
var snapshotTest= SnapshotTestBuilder
    .Create()
    .WithLocation("Root path where to read/write snapshot files")
    .WithPngStrategy(differencesThreshold: 0.01)
    .Build();

await snapshotTest.CompareSnapshotAsync(snapshotName, pngStream);
```

#### Migration Steps

1. Setup with `SnapshotTestBuilder` instantiation and use the configuration action.
2. Change all `SnapshotHelper.AssertSnapshot()` calls to `CompareSnapshotAsync()` and add `await`.
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