# Release notes

## What's new in 2.3.0 February 4 2021

### QuikGraph

#### Fixes:
* Fix the serialization implementation of UndirectedGraph, ArrayUndirectedGraph and UndirectedBidirectionalGraph.
* Fix A\* implementation to also compute cost on tree edge.

#### Updates:
* Remove some serializable attributes from algorithms and predicates classes (homognization).
* Remove serializable attributes from delegate graphs implementations.
* All QuikGraph exceptions can be constructed with a custom message and an eventual inner exception.
* CompressedSparseRowGraph also implements IEdgeListGraph interface.
* EquateGraphs.Equate helpers now supports a wider range of graph comparisons.

#### New:
* Add the IDistancesCollection to interface the distance information retrieval from shortest path algorithms. Legacy accesses to distances are marked as obsolete.

#### Misc:
* Remove the dependency to System.Collections.NonGeneric for .NET Standard 1.3 target.

### QuikGraph.Serialization

#### Updates:
* Update package dependencies.

### QuikGraph.Graphviz

#### Updates:
* Update package dependencies.

#### New:
* Make all Dot structures serializable.

### QuikGraph.Data

#### Updates:
* Update package dependencies.

### QuikGraph.MSAGL

#### Fixes:
* Update reference to AutomaticGraphLayout packages in order to fix assembly strong naming issues.

#### Updates:
* Update package dependencies.

### QuikGraph.Petri

#### Updates:
* Update package dependencies.

---

## What's new in 2.2.2 July 18 2020

### QuikGraph.Serialization

#### Fixes:
* Add the possibility to use a custom binder during binary serialization to control deserialized types.
* Undirected graphs are now serializable to XML.

---

## What's new in 2.2.1 June 19 2020 and June 25 2020

### QuikGraph.Serialization

#### Fixes:
* Rely on embedded GraphML xsd to validate GraphML graphs during serialization (avoid issues when GraphML website is down).

### QuikGraph.Petri

This package is the same as 2.2.0 but with the right tags and description.

---

## What's new in 2.2.0 June 7 2020, June 11 2020, June 18 2020 and June 25 2020

### QuikGraph

#### New:
* Use signing key to strong name library assemby.

### QuikGraph.Serialization

#### New:
* Use signing key to strong name library assemby.

### QuikGraph.GraphViz

#### New:
* Use signing key to strong name library assemby.

### QuikGraph.Data

#### Misc:
* Clean the library code.
* Fully document library.
* Use JetBrains annotations all over the library as much as possible.

### QuikGraph.MSAGL

Migrate the library from GLEE to [MSAGL](https://www.microsoft.com/en-us/research/project/microsoft-automatic-graph-layout) (MSAGL is the successor of GLEE).

#### Fixes:
* Fix typo in populator algorithm.
* Fix a bug when converting undirected graph to MSAGL graph.

### API Breaks
* Some public API breaks (but should remain simple to do a migration).

#### Misc:
* Clean the library code.
* Fully document library.
* Use JetBrains annotations all over the library as much as possible.

### QuikGraph.Petri

#### Fixes:
* Fix PetriNetSimulator with possible collection modified while enumerating.

#### Updates:
* Really make IPetriNet immutable to enforce the difference with IMutablePetriNet.

### API Breaks
* Some public API breaks (but should remain simple to do a migration).

#### Misc:
* Clean the library code.
* Fully document library.
* Use JetBrains annotations all over the library as much as possible.

---

## What's new in 2.1.1 June 6 2020

### QuikGraph.GraphViz

#### New:
* Add support of PenWidth property for vertex, edge and graph.
* Add support of Splines property for graph.

---

## What's new in 2.1.0 May 11 2020 and June 3 2020

### QuikGraph

#### Optims:
* Use EqualityComparer&lt;T&gt; instead of non-generic object.Equals.

#### Updates:
* Cancellation of algorithm internally use exception to abort algorithm run.

### QuikGraph.Serialization

#### Fixes:
* Fix a security vulnerability regarding XML serialization on target .NET Framework 3.5.

### QuikGraph.GraphViz

Rework the original QuickGraph.Graphviz module into QuikGraph.Graphviz. This make possible to use the QuikGraph to Dot bridge.

#### Fixes:
* Fix the floating points formatting when converted to Dot (Invariant culture).
* Fix some implementation issues accross the library.
* Fix labels, comments, tooltips, records and ports escaping.
* Fix a lot of graph to Dot conversions issues (wrong properties, invalid formatting, typo, etc).

#### Updates:
* Add all color representations equivalent to System.Drawing.Color or System.Windows.Media.Color to GraphvizColor.

#### New:
* Add conversion extensions from System.Drawing.Font to GraphvizFont (and vice versa).

### API Breaks
* Dot escape method has been replaced by DotEscapers helpers that handle various escaping scenarios and are static.
* Some public API breaks (but should remain simple to do a migration).

#### Misc:
* Clean the library code.
* Fully document library including convenient links to official Graphviz documentation.
* Use JetBrains annotations all over the library as much as possible.

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