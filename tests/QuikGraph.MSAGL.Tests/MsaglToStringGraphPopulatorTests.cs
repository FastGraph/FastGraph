using System;
using NUnit.Framework;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglToStringGraphPopulator{TVertex,TEdge}"/>.
    /// </summary>
    internal class MsaglToStringGraphPopulatorTests : MsaglGraphPopulatorTestsBase
    {
        #region Test classes

        private class NullVertexTestFormatProvider : IFormatProvider
        {
            public object GetFormat(Type formatType)
            {
                return null;
            }
        }

        private class VertexTestFormatProvider : IFormatProvider, ICustomFormatter
        {
            public object GetFormat(Type formatType)
            {
                return formatType == typeof(ICustomFormatter) ? this : null;
            }

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                return $"MySpecialFormatProvider {arg}";
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var formatProvider = new NullVertexTestFormatProvider();
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph);
            AssertPopulatorProperties(populator, graph);

            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph, formatProvider: formatProvider);
            AssertPopulatorProperties(populator, graph, provider: formatProvider);
            
            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph, "Format {0}");
            AssertPopulatorProperties(populator, graph, "Format {0}");

            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph, "Format2 {0}", formatProvider);
            AssertPopulatorProperties(populator, graph, "Format2 {0}",formatProvider);

            var undirectedGraph = new UndirectedGraph<int, Edge<int>>();
            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(undirectedGraph);
            AssertPopulatorProperties(populator, undirectedGraph);

            #region Local function

            void AssertPopulatorProperties<TVertex, TEdge>(
                MsaglToStringGraphPopulator<TVertex, TEdge> p,
                IEdgeListGraph<TVertex, TEdge> g,
                string f = null,
                IFormatProvider provider = null)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(p, g);
                Assert.IsNull(p.MsaglGraph);
                Assert.AreEqual(f ?? "{0}", p.Format);
                if (provider is null)
                {
                    Assert.IsNull(p.FormatProvider);
                }
                else
                {
                    Assert.AreSame(provider, p.FormatProvider);
                }
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new MsaglToStringGraphPopulator<int, Edge<int>>(null));
        }

        [Test]
        public void Compute()
        {
            Compute_Test(graph => new MsaglToStringGraphPopulator<int, Edge<int>>(graph));
        }

        [Test]
        public void Handlers()
        {
            Handlers_Test(graph => new MsaglToStringGraphPopulator<int, Edge<int>>(graph));
        }

        [Test]
        public void VertexId()
        {
            var nullFormatProvider = new NullVertexTestFormatProvider();
            var formatProvider = new VertexTestFormatProvider();
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3)
            });
            graph.AddVertexRange(new[] { 5, 6 });

            // No special format
            var populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph);
            populator.Compute();

            // Check vertices has been well formatted
            Assert.IsNull(populator.MsaglGraph.FindNode("0"));
            Assert.IsNotNull(populator.MsaglGraph.FindNode("1"));


            // No special format (2)
            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph, formatProvider: nullFormatProvider);
            populator.Compute();

            // Check vertices has been well formatted
            Assert.IsNull(populator.MsaglGraph.FindNode("0"));
            Assert.IsNotNull(populator.MsaglGraph.FindNode("1"));



            // With special format
            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph, "MyTestFormat {0} Vertex");
            populator.Compute();

            // Check vertices has been well formatted
            Assert.IsNull(populator.MsaglGraph.FindNode("MyTestFormat 0 Vertex"));
            Assert.IsNotNull(populator.MsaglGraph.FindNode("MyTestFormat 1 Vertex"));


            // With special format (2)
            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph, "MyTestFormat {0} Vertex", nullFormatProvider);
            populator.Compute();

            // Check vertices has been well formatted
            Assert.IsNull(populator.MsaglGraph.FindNode("MyTestFormat 0 Vertex"));
            Assert.IsNotNull(populator.MsaglGraph.FindNode("MyTestFormat 1 Vertex"));


            // With special format (3)
            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph, formatProvider: formatProvider);
            populator.Compute();

            // Check vertices has been well formatted
            Assert.IsNull(populator.MsaglGraph.FindNode("MySpecialFormatProvider 0"));
            Assert.IsNotNull(populator.MsaglGraph.FindNode("MySpecialFormatProvider 1"));


            // With special format (4)
            populator = new MsaglToStringGraphPopulator<int, Edge<int>>(graph, "MyTestFormat {0} Vertex", formatProvider);
            populator.Compute();

            // Check vertices has been well formatted
            Assert.IsNull(populator.MsaglGraph.FindNode("MyTestFormat MySpecialFormatProvider 0 Vertex"));
            Assert.IsNotNull(populator.MsaglGraph.FindNode("MyTestFormat MySpecialFormatProvider 1 Vertex"));
        }
    }
}