using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.TSP;

namespace QuikGraph.Tests.Algorithms.TSP
{
    /// <summary>
    /// Tests for <see cref="TSP{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal class TSPTests : QuikGraphUnitTests
    {
        #region Helpers and test classes

        private class TestCase
        {
            [NotNull]
            public BidirectionalGraph<string, EquatableEdge<string>> Graph { get; } = new BidirectionalGraph<string, EquatableEdge<string>>();

            [NotNull]
            private readonly Dictionary<EquatableEdge<string>, double> _weightsDict = new Dictionary<EquatableEdge<string>, double>();

            [NotNull]
            public TestCase AddVertex([NotNull] string vertex)
            {
                Graph.AddVertex(vertex);

                return this;
            }

            [NotNull]
            public TestCase AddUndirectedEdge([NotNull] string source, [NotNull] string target, double weight)
            {
                AddDirectedEdge(source, target, weight);
                AddDirectedEdge(target, source, weight);

                return this;
            }

            [NotNull]
            public TestCase AddDirectedEdge([NotNull] string source, [NotNull] string target, double weight)
            {
                var edge = new EquatableEdge<string>(source, target);
                Graph.AddEdge(edge);
                _weightsDict.Add(edge, weight);

                return this;
            }

            [Pure]
            [NotNull]
            public Func<EquatableEdge<string>, double> GetFuncWeights()
            {
                return edge => _weightsDict[edge];
            }
        }

        #endregion

        [Test]
        public void UndirectedFullGraph()
        {
            var testCase = new TestCase();
            testCase.AddVertex("n1")
                .AddVertex("n2")
                .AddVertex("n3")
                .AddVertex("n4")
                .AddVertex("n5");

            testCase.AddUndirectedEdge("n1", "n2", 16)
                .AddUndirectedEdge("n1", "n3", 9)
                .AddUndirectedEdge("n1", "n4", 15)
                .AddUndirectedEdge("n1", "n5", 3)
                .AddUndirectedEdge("n2", "n3", 14)
                .AddUndirectedEdge("n2", "n4", 4)
                .AddUndirectedEdge("n2", "n5", 5)
                .AddUndirectedEdge("n3", "n4", 4)
                .AddUndirectedEdge("n3", "n5", 2)
                .AddUndirectedEdge("n4", "n5", 1);

            var tsp = new TSP<string, EquatableEdge<string>, BidirectionalGraph<string, EquatableEdge<string>>>(
                testCase.Graph, testCase.GetFuncWeights());
            tsp.Compute();

            Assert.AreEqual(tsp.BestCost, 25);
            Assert.IsFalse(tsp.ResultPath.IsDirectedAcyclicGraph());
        }

        [Test]
        public void UndirectedSparseGraph()
        {
            var testCase = new TestCase();
            testCase.AddVertex("n1")
                .AddVertex("n2")
                .AddVertex("n3")
                .AddVertex("n4")
                .AddVertex("n5")
                .AddVertex("n6");

            testCase.AddUndirectedEdge("n1", "n2", 10)
                .AddUndirectedEdge("n2", "n3", 8)
                .AddUndirectedEdge("n3", "n4", 11)
                .AddUndirectedEdge("n4", "n5", 6)
                .AddUndirectedEdge("n5", "n6", 9)
                .AddUndirectedEdge("n1", "n6", 3)
                .AddUndirectedEdge("n2", "n6", 5)
                .AddUndirectedEdge("n3", "n6", 18)
                .AddUndirectedEdge("n3", "n5", 21);

            var tsp = new TSP<string, EquatableEdge<string>, BidirectionalGraph<string, EquatableEdge<string>>>(
                testCase.Graph, testCase.GetFuncWeights());
            tsp.Compute();

            Assert.AreEqual(tsp.BestCost, 47);
            Assert.IsFalse(tsp.ResultPath.IsDirectedAcyclicGraph());
        }

        [Test]
        public void DirectedSparseGraphWithoutPath()
        {
            var testCase = new TestCase();
            testCase.AddVertex("n1")
                .AddVertex("n2")
                .AddVertex("n3")
                .AddVertex("n4")
                .AddVertex("n5")
                .AddVertex("n6");

            testCase.AddDirectedEdge("n1", "n2", 10)
                .AddDirectedEdge("n2", "n3", 8)
                .AddDirectedEdge("n3", "n4", 11)
                .AddDirectedEdge("n4", "n5", 6)
                .AddDirectedEdge("n5", "n6", 9)
                .AddDirectedEdge("n1", "n6", 3)
                .AddDirectedEdge("n2", "n6", 5)
                .AddDirectedEdge("n3", "n6", 18)
                .AddDirectedEdge("n3", "n5", 21);

            var tsp = new TSP<string, EquatableEdge<string>, BidirectionalGraph<string, EquatableEdge<string>>>(
                testCase.Graph, testCase.GetFuncWeights());
            tsp.Compute();

            Assert.AreEqual(tsp.BestCost, double.PositiveInfinity);
            Assert.IsTrue(tsp.ResultPath == null);
        }

        [Test]
        public void DirectedSparseGraph()
        {
            var testCase = new TestCase();
            testCase.AddVertex("n1")
                .AddVertex("n2")
                .AddVertex("n3")
                .AddVertex("n4")
                .AddVertex("n5")
                .AddVertex("n6");

            testCase.AddDirectedEdge("n1", "n2", 10)
                .AddDirectedEdge("n2", "n3", 8)
                .AddDirectedEdge("n3", "n4", 11)
                .AddDirectedEdge("n4", "n5", 6)
                .AddDirectedEdge("n5", "n6", 9)
                .AddDirectedEdge("n1", "n6", 3)
                .AddDirectedEdge("n2", "n6", 5)
                .AddDirectedEdge("n3", "n6", 18)
                .AddDirectedEdge("n3", "n5", 21)
                .AddDirectedEdge("n6", "n1", 1);

            var tsp = new TSP<string, EquatableEdge<string>, BidirectionalGraph<string, EquatableEdge<string>>>(
                testCase.Graph, testCase.GetFuncWeights());
            tsp.Compute();

            Assert.AreEqual(tsp.BestCost, 45);
            Assert.IsFalse(tsp.ResultPath.IsDirectedAcyclicGraph());
        }
    }
}