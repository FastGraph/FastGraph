using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Algorithms.Ranking;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="PageRankAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class PageRankAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new PageRankAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new PageRankAlgorithm<int, Edge<int>>(graph)
            {
                Damping = 0.96
            };
            AssertAlgorithmProperties(algorithm, graph, 0.96);

            algorithm = new PageRankAlgorithm<int, Edge<int>>(graph)
            {
                Tolerance = double.Epsilon
            };
            AssertAlgorithmProperties(algorithm, graph, t: double.Epsilon);

            algorithm = new PageRankAlgorithm<int, Edge<int>>(graph)
            {
                MaxIterations = 12
            };
            AssertAlgorithmProperties(algorithm, graph, iterations: 12);

            algorithm = new PageRankAlgorithm<int, Edge<int>>(graph)
            {
                Damping = 0.91,
                Tolerance = 3 * double.Epsilon,
                MaxIterations = 50
            };
            AssertAlgorithmProperties(algorithm, graph, 0.91, 3 * double.Epsilon, 50);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                PageRankAlgorithm<TVertex, TEdge> algo,
                IBidirectionalGraph<TVertex, TEdge> g,
                double d = -1,
                double t = -1,
                int iterations = -1)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                CollectionAssert.IsEmpty(algo.Ranks);
                if (d >= 0)
                {
                    Assert.AreEqual(d, algo.Damping);
                }
                else
                {
                    Assert.GreaterOrEqual(algo.Damping, 0);
                    Assert.LessOrEqual(algo.Damping, 1);
                }
                if (t >= 0)
                {
                    Assert.AreEqual(t, algo.Tolerance);
                }
                else
                {
                    Assert.GreaterOrEqual(algo.Tolerance, 0);
                }
                if (iterations > 0)
                {
                    Assert.AreEqual(iterations, algo.MaxIterations);
                }
                else
                {
                    Assert.Positive(algo.MaxIterations);
                }
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new PageRankAlgorithm<int, Edge<int>>(null));

            var algorithm = new PageRankAlgorithm<int, Edge<int>>(graph);
            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.Damping = -10.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.Damping = -0.01);
            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.Damping = 1.01);
            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.Damping = 10.0);

            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.Tolerance = -10.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.Tolerance = -1);

            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.MaxIterations = -10);
            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.MaxIterations = -1);
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void PageRank()
        {
            var graph = new BidirectionalGraph<string, Edge<string>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<string>("Amazon", "Twitter"),
                new Edge<string>("Amazon", "Microsoft"),
                new Edge<string>("Microsoft", "Amazon"),
                new Edge<string>("Microsoft", "Facebook"),
                new Edge<string>("Microsoft", "Twitter"),
                new Edge<string>("Microsoft", "Apple"),
                new Edge<string>("Facebook", "Amazon"),
                new Edge<string>("Facebook", "Twitter"),
                new Edge<string>("Twitter", "Microsoft"),
                new Edge<string>("Apple", "Twitter")
            });

            var algorithm = new PageRankAlgorithm<string, Edge<string>>(graph);
            algorithm.Compute();

            IEnumerable<string> order = algorithm.Ranks.OrderByDescending(pair => pair.Value).Select(pair => pair.Key);
            CollectionAssert.AreEqual(
                new[] { "Microsoft", "Twitter", "Amazon", "Facebook", "Apple" },
                order);
            Assert.Positive(algorithm.GetRanksSum());
            double rankSum = algorithm.Ranks.Sum(pair => pair.Value);
            Assert.AreEqual(rankSum, algorithm.GetRanksSum());

            Assert.Positive(algorithm.GetRanksSum());
            Assert.AreEqual(rankSum / graph.VertexCount, algorithm.GetRanksMean());
        }
    }
}