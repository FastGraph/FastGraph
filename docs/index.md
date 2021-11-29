# FastGraph documentation

## Badges

| | |
| --- | --- |
| **Build** | [![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/brucificus/FastGraph?branch=master&svg=true)](https://ci.appveyor.com/project/brucificus/fastgraph) |
| **Coverage** | <sup>Coveralls</sup> [![Coverage Status](https://coveralls.io/repos/github/brucificus/FastGraph/badge.svg?branch=master)](https://coveralls.io/github/brucificus/fastgraph?branch=master) <sup>SonarQube</sup> [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=fastgraph&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=fastgraph) | 
| **Quality** | [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=fastgraph&metric=alert_status)](https://sonarcloud.io/dashboard?id=fastgraph) | 
| **License** | MS-PL |

## Introduction

FastGraph provides generic directed/undirected graph data structures and algorithms.

FastGraph comes with algorithms such as depth first search, breath first search, A* search, shortest path, k-shortest path, maximum flow, minimum spanning tree, etc.

*FastGraph was originally created by Jonathan "Peli" de Halleux in 2003 and named QuickGraph. It was later forked as YC.QuickGraph and QuikGraph.*

**This version** of QuickGraph, renamed **FastGraph**, is a fork of YC.QuickGraph *and* QuikGraph.

The plan is to target cutting-edge .NET 6 and C# 10 features, (initially at the expense of API stability).

You can find library sources on [GitHub](https://github.com/brucificus/FastGraph).

## Targets

- .NET Standard 1.3+
- .NET Core 1.0+
- .NET Framework 3.5+
- Works under [Unity 3D](https://github.com/brucificus/FastGraph/wiki/Unity3D-Integration)

Supports Source Link

## Packages

FastGraph is available on [NuGet](https://www.nuget.org) in several modules.

[![Nuget Status](https://img.shields.io/nuget/v/fastgraph.svg)](https://www.nuget.org/packages/FastGraph) [FastGraph](https://www.nuget.org/packages/FastGraph) (Core)

    PM> Install-Package FastGraph

[![Nuget Status](https://img.shields.io/nuget/v/fastgraph.serialization.svg)](https://www.nuget.org/packages/FastGraph.Serialization) [FastGraph.Serialization](https://www.nuget.org/packages/FastGraph.Serialization)

    PM> Install-Package FastGraph.Serialization

[![Nuget Status](https://img.shields.io/nuget/v/fastgraph.graphviz.svg)](https://www.nuget.org/packages/FastGraph.Graphviz) [FastGraph.Graphviz](https://www.nuget.org/packages/FastGraph.Graphviz)

    PM> Install-Package FastGraph.Graphviz

[![Nuget Status](https://img.shields.io/nuget/v/fastgraph.data.svg)](https://www.nuget.org/packages/FastGraph.Data) [FastGraph.Data](https://www.nuget.org/packages/FastGraph.Data)

    PM> Install-Package FastGraph.Data

[![Nuget Status](https://img.shields.io/nuget/v/fastgraph.msagl.svg)](https://www.nuget.org/packages/FastGraph.MSAGL) [FastGraph.MSAGL](https://www.nuget.org/packages/FastGraph.MSAGL)

    PM> Install-Package FastGraph.MSAGL

[![Nuget Status](https://img.shields.io/nuget/v/fastgraph.petri.svg)](https://www.nuget.org/packages/FastGraph.Petri) [FastGraph.Petri](https://www.nuget.org/packages/FastGraph.Petri)

    PM> Install-Package FastGraph.Petri

<img src="images/fastgraph_logo.png" width="128" height="128" style="display: block; margin-left: auto; margin-right: auto" />