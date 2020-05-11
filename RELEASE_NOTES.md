# Release notes

## What's new in 2.1.0 May 11 2020

### QuikGraph

#### Optims:
* Use EqualityComparer&lt;T&gt; instead of non-generic object.Equals.

#### Updates:
* Cancellation of algorithm internally use exception to abort algorithm run.

### QuikGraph.Serialization

#### Fixes:
* Fix a security vulnerability regarding XML serialization on target .NET Framework 3.5.

---

## What's new in 2.0.0 February 16 2020

### QuikGraph

Split QuikGraph package into 2 packages to extract serialization features:
- QuikGraph
- QuikGraph.Serialization

#### New:
* Add struct based edge implementations that were in original QuickGraph (STaggedEdge, STaggedUndirectedEdge and SEquatableTaggedEdge).

#### Misc:
* Package no more reference JetBrains.Annotations package, it rather uses some as internal implementation (same development experience for package consumer).

### QuikGraph.Serialization

Serialization features extracted from core package.

---

## What's new in 1.0.1 January 20 2020

### Fixes:
* Properly deal with isolated vertices for transitive reduction algorithm.
* Properly deal with isolated vertices for transitive closure algorithm.

---

## What's new in 1.0.0 December 10 2019

This release is based on YC.QuikGraph 3.7.5-deta with a lot of updates.

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