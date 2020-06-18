# QuikGraph documentation

## Badges

| | |
| --- | --- |
| **Build** | [![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/KeRNeLith/QuikGraph?branch=master&svg=true)](https://ci.appveyor.com/project/KeRNeLith/quikgraph) |
| **Coverage** | <sup>Coveralls</sup> [![Coverage Status](https://coveralls.io/repos/github/KeRNeLith/QuikGraph/badge.svg?branch=master)](https://coveralls.io/github/KeRNeLith/quikgraph?branch=master) <sup>SonarQube</sup> [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=quikgraph&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=quikgraph) | 
| **Quality** | [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=quikgraph&metric=alert_status)](https://sonarcloud.io/dashboard?id=quikgraph) | 
| **License** | MS-PL |

## Introduction

QuikGraph provides generic directed/undirected graph data structures and algorithms for .NET.

QuikGraph comes with algorithms such as depth first search, breath first search, A* search, shortest path, k-shortest path, maximum flow, minimum spanning tree, etc.

*QuikGraph was originally created by Jonathan "Peli" de Halleux in 2003 and named QuickGraph.*

It was then updated to become YC.QuickGraph.

**This version** of QuickGraph, renamed **QuikGraph**, is a fork of YC.QuickGraph, and I tried to clean the library to provide it as a clean NuGet packages using modern C# development (.NET Core).

The plan is to to fully clean, fix issues of the original library and all its non Core parts, and improve it.

You can find library sources on [GitHub](https://github.com/KeRNeLith/QuikGraph).

## Targets

- .NET Standard 1.3+
- .NET Core 1.0+
- .NET Framework 3.5+

Supports Source Link

## Packages

QuikGraph is available on [NuGet](https://www.nuget.org) in several modules.

[![Nuget Status](https://img.shields.io/nuget/v/quikgraph.svg)](https://www.nuget.org/packages/QuikGraph) [QuikGraph](https://www.nuget.org/packages/QuikGraph) (Core)

    PM> Install-Package QuikGraph

[![Nuget Status](https://img.shields.io/nuget/v/quikgraph.serialization.svg)](https://www.nuget.org/packages/QuikGraph.Serialization) [QuikGraph.Serialization](https://www.nuget.org/packages/QuikGraph.Serialization)

    PM> Install-Package QuikGraph.Serialization

[![Nuget Status](https://img.shields.io/nuget/v/quikgraph.graphviz.svg)](https://www.nuget.org/packages/QuikGraph.Graphviz) [QuikGraph.Graphviz](https://www.nuget.org/packages/QuikGraph.Graphviz)

    PM> Install-Package QuikGraph.Graphviz

[![Nuget Status](https://img.shields.io/nuget/v/quikgraph.data.svg)](https://www.nuget.org/packages/QuikGraph.Data) [QuikGraph.Data](https://www.nuget.org/packages/QuikGraph.Data)

    PM> Install-Package QuikGraph.Data

[![Nuget Status](https://img.shields.io/nuget/v/quikgraph.msagl.svg)](https://www.nuget.org/packages/QuikGraph.MSAGL) [QuikGraph.MSAGL](https://www.nuget.org/packages/QuikGraph.MSAGL)

    PM> Install-Package QuikGraph.MSAGL

[![Nuget Status](https://img.shields.io/nuget/v/quikgraph.petri.svg)](https://www.nuget.org/packages/QuikGraph.Petri) [QuikGraph.Petri](https://www.nuget.org/packages/QuikGraph.Petri)

    PM> Install-Package QuikGraph.Petri

<img src="images/quikgraph_logo.png" width="128" height="128" style="display: block; margin-left: auto; margin-right: auto" />