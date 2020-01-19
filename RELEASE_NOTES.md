# Release notes

## What's new in 1.0.1 January 20 2020

### Fixes:
* Properly deal with isolated vertices for transitive reduction algorithm.
* Properly deal with isolated vertices for transitive closure algorithm.

## What's new in 1.0.0 December 10 2019

This release is was based on YC.QuikGraph 3.7.5-deta with a lot of updates.

### General:
* Fully clean the library code.
* Extend support of the library to .NET Framework 3.5+.
* Extend support of the library to .NET Core 1.0+.
* Various fixes for graphs and algorithms implementations.
* Uniformize APIs and behaviors of graphs and algorithms implementations.

Note: Only keep the Core of QuikGraph for this package (feature split).

### API Breaks
* Some public API breaks (but should remain simple to do a migration).
* Some edges structures are removed, the classes implementations are preferred due to C# limitations.
* Some algorithms are not usable for now (wrong implementations) and are removed from public API.

### Misc:
* Generate a documentation for the library via DocFX.
* Use JetBrains annotations all over the library as much as possible.