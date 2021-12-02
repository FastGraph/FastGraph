| | |
| --- | --- |
| **Build** | [![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/brucificus/FastGraph?branch=master&svg=true)](https://ci.appveyor.com/project/brucificus/fastgraph) |
| **Coverage** | <sup>Coveralls</sup> [![Coverage Status](https://coveralls.io/repos/github/brucificus/FastGraph/badge.svg?branch=master)](https://coveralls.io/github/brucificus/FastGraph?branch=master) <sup>SonarQube</sup> [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=fastgraph&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=fastgraph) | 
| **Quality** | [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=fastgraph&metric=alert_status)](https://sonarcloud.io/dashboard?id=fastgraph) | 
| **Nugets** | [![Nuget Status](https://img.shields.io/nuget/v/fastgraph.svg)](https://www.nuget.org/packages/FastGraph) FastGraph |
| | [![Nuget Status](https://img.shields.io/nuget/v/fastgraph.serialization.svg)](https://www.nuget.org/packages/FastGraph.Serialization) FastGraph.Serialization |
| | [![Nuget Status](https://img.shields.io/nuget/v/fastgraph.graphviz.svg)](https://www.nuget.org/packages/FastGraph.Graphviz) FastGraph.Graphviz |
| | [![Nuget Status](https://img.shields.io/nuget/v/fastgraph.data.svg)](https://www.nuget.org/packages/FastGraph.Data) FastGraph.Data |
| | [![Nuget Status](https://img.shields.io/nuget/v/fastgraph.msagl.svg)](https://www.nuget.org/packages/FastGraph.MSAGL) FastGraph.MSAGL |
| | [![Nuget Status](https://img.shields.io/nuget/v/fastgraph.petri.svg)](https://www.nuget.org/packages/FastGraph.Petri) FastGraph.Petri |
| **License** | MS-PL |

# FastGraph

## What is **FastGraph**?

FastGraph provides generic directed/undirected graph data structures and algorithms.

FastGraph comes with algorithms such as depth first search, breath first search, A* search, shortest path, k-shortest path, maximum flow, minimum spanning tree, etc.

*FastGraph was originally created by Jonathan "Peli" de Halleux in 2003 and named QuickGraph. It was later forked as YC.QuickGraph and QuikGraph.*

**This version** of QuickGraph, renamed **FastGraph**, is a fork of YC.QuickGraph *and* QuikGraph.

The plan is to target cutting-edge .NET 6 and C# 10 features, (initially at the expense of API stability).

**[Getting started with FastGraph](https://github.com/brucificus/FastGraph/wiki)**

---

## Targets

- [![.NET Standard](https://img.shields.io/badge/.NET%20Standard-%3E%3D%202.0-blue.svg)](#)
- [![.NET Core](https://img.shields.io/badge/.NET%20Core-%3E%3D%203.1-blue.svg)](#)
- [![.NET 5](https://img.shields.io/badge/.NET-%3E%3D%205.0-blue.svg)](#)
- Works under [Unity 3D](https://github.com/brucificus/FastGraph/wiki/Unity3D-Integration)

---

## Contributing

### Build

* Clone this repository.
* Open FastGraph.sln.

### Notes

The library get rid of PEX that was previously used for unit tests and now uses NUnit3 (not published).

I would be very pleased to receive pull requests to further **test**, **improve** or add new features to the library.

---

## Usage

### Packages

FastGraph is available on [NuGet](https://www.nuget.org) in several modules.

- [FastGraph](https://www.nuget.org/packages/FastGraph) (Core)
- [FastGraph.Serialization](https://www.nuget.org/packages/FastGraph.Serialization)
- [FastGraph.Graphviz](https://www.nuget.org/packages/FastGraph.Graphviz)
- [FastGraph.Data](https://www.nuget.org/packages/FastGraph.Data)
- [FastGraph.MSAGL](https://www.nuget.org/packages/FastGraph.MSAGL)
- [FastGraph.Petri](https://www.nuget.org/packages/FastGraph.Petri)

### Where to go next?

* [Wiki](https://github.com/brucificus/FastGraph/wiki)

---

## Maintainer(s)

[![](https://github.com/brucificus.png?size=50)](https://github.com/brucificus)

## Notable Network Contributor(s)

This project exists thanks to all the people who have contributed to the code base.

[![](https://github.com/KeRNeLith.png?size=50)](https://github.com/KeRNeLith)
[![](https://github.com/gsvgit.png?size=50)](https://github.com/gsvgit)
[![](https://github.com/jnyrup.png?size=50)](https://github.com/jnyrup)
[![](https://github.com/SimonTC.png?size=50)](https://github.com/SimonTC)
[![](https://github.com/tuwuhs.png?size=50)](https://github.com/tuwuhs)
[![](https://github.com/pelikhan.png?size=50)](https://github.com/pelikhan)

---