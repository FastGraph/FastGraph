| | |
| --- | --- |
| **Build** | [![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/KeRNeLith/QuikGraph?branch=master&svg=true)](https://ci.appveyor.com/project/KeRNeLith/quikgraph) |
| **Coverage** | <sup>Coveralls</sup> [![Coverage Status](https://coveralls.io/repos/github/KeRNeLith/QuikGraph/badge.svg?branch=master)](https://coveralls.io/github/KeRNeLith/QuikGraph?branch=master) <sup>SonarQube</sup> [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=quikgraph&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=quikgraph) | 
| **Quality** | [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=quikgraph&metric=alert_status)](https://sonarcloud.io/dashboard?id=quikgraph) | 
| **Nuget** | [![Nuget Status](https://img.shields.io/nuget/v/quikgraph.svg)](https://www.nuget.org/packages/QuikGraph) |
| **License** | MS-PL |

# QuikGraph

## What is **QuikGraph**?

QuikGraph provides generic directed/undirected graph data structures and algorithms for .NET.

QuikGraph comes with algorithms such as depth first search, breath first search, A* search, shortest path, k-shortest path, maximum flow, minimum spanning tree, etc.

*QuikGraph was originally created by Jonathan "Peli" de Halleux in 2003 and named QuickGraph.*

It was then updated to become YC.QuickGraph.

This version of QuickGraph, renamed QuikGraph, is a fork of YC.QuickGraph, and I tried to clean the Core of the library to provide it as a clean NuGet package using modern C# development.

The plan would be to fully clean the original library and all its non Core parts and unit test it more.

---

## Target

- .NET Standard 1.3+
- .NET Core 1.0+
- .NET Framework 2.0+

---

## Contributing

### Build

* Clone this repository.
* Open QuikGraph.sln.

### Notes

It uses NUnit3 for unit testing (not published).

I would be very pleased to receive pull requests to further **test** or **improve** the library.

### Where to go next?

* [Documentation](https://kernelith.github.io/QuikGraph/)
* [External Information](https://quickgraph.codeplex.com/documentation)

### Maintainer(s)

* [@KeRNeLith](https://github.com/KeRNeLith)

---