#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Ranking;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="PageRankAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class PageRankAlgorithmTests
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
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.Ranks.Should().BeEmpty();
                if (d >= 0)
                {
                    algo.Damping.Should().Be(d);
                }
                else
                {
                    algo.Damping.Should().BeGreaterThanOrEqualTo(0);
                    algo.Damping.Should().BeLessThanOrEqualTo(1);
                }
                if (t >= 0)
                {
                    algo.Tolerance.Should().Be(t);
                }
                else
                {
                    algo.Tolerance.Should().BeGreaterThanOrEqualTo(0);
                }
                if (iterations > 0)
                {
                    algo.MaxIterations.Should().Be(iterations);
                }
                else
                {
                    algo.MaxIterations.Should().BePositive();
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
#pragma warning disable CS8625
            Invoking(() => new PageRankAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625

            var algorithm = new PageRankAlgorithm<int, Edge<int>>(graph);
            Invoking(() => algorithm.Damping = -10.0).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => algorithm.Damping = -0.01).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => algorithm.Damping = 1.01).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => algorithm.Damping = 10.0).Should().Throw<ArgumentOutOfRangeException>();

            Invoking(() => algorithm.Tolerance = -10.0).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => algorithm.Tolerance = -1).Should().Throw<ArgumentOutOfRangeException>();

            Invoking(() => algorithm.MaxIterations = -10).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => algorithm.MaxIterations = -1).Should().Throw<ArgumentOutOfRangeException>();
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
            new[] { "Microsoft", "Twitter", "Amazon", "Facebook", "Apple" }.Should().BeEquivalentTo(order);
            algorithm.GetRanksSum().Should().BePositive();
            double rankSum = algorithm.Ranks.Sum(pair => pair.Value);
            algorithm.GetRanksSum().Should().Be(rankSum);

            algorithm.GetRanksSum().Should().BePositive();
            algorithm.GetRanksMean().Should().Be(rankSum / graph.VertexCount);
        }
    }
}
