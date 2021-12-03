﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FastGraph.Algorithms.Exploration;
using FastGraph.Tests.Structures;
using static FastGraph.Tests.AssertHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Algorithms.Exploration
{
    /// <summary>
    /// Tests for <see cref="TransitionFactoryImplicitGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class TransitionFactoryImplicitGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            AssertGraphProperties(graph);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
                TransitionFactoryImplicitGraph<TVertex, TEdge> g)
                where TVertex : ICloneable
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.IsTrue(g.AllowParallelEdges);
                Assert.IsNotNull(g.SuccessorVertexPredicate);
                Assert.IsNotNull(g.SuccessorEdgePredicate);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.SuccessorVertexPredicate = null);
            Assert.Throws<ArgumentNullException>(() => graph.SuccessorEdgePredicate = null);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #region Factory manipulations

        [Test]
        public void AddTransitionFactory()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactory(factory1);

            Assert.IsTrue(graph.ContainsTransitionFactory(factory1));

            var vertex2 = new CloneableTestVertex("2");
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactory(factory2);

            Assert.IsTrue(graph.ContainsTransitionFactory(factory2));

            graph.AddTransitionFactory(factory1);

            Assert.IsTrue(graph.ContainsTransitionFactory(factory1));
        }

        [Test]
        public void AddTransitionFactory_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddTransitionFactory(null));
        }

        [Test]
        public void AddTransitionFactories()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactories(new[] { factory1, factory2 });

            Assert.IsTrue(graph.ContainsTransitionFactory(factory1));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory2));

            var vertex3 = new CloneableTestVertex("3");
            var factory3 = new TestTransitionFactory<CloneableTestVertex>(vertex3, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactory(factory3);

            Assert.IsTrue(graph.ContainsTransitionFactory(factory1));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory2));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory3));
        }

        [Test]
        public void AddTransitionFactories_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddTransitionFactories(null));
        }

        [Test]
        public void RemoveTransitionFactories()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            Assert.IsFalse(graph.RemoveTransitionFactory(null));

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            var factory3 = new TestTransitionFactory<CloneableTestVertex>(vertex3, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactories(new[] { factory1, factory2 });

            Assert.IsFalse(graph.ContainsTransitionFactory(null));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory1));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory2));

            Assert.IsFalse(graph.RemoveTransitionFactory(factory3));
            Assert.IsTrue(graph.RemoveTransitionFactory(factory1));
            Assert.IsFalse(graph.RemoveTransitionFactory(factory1));
            Assert.IsTrue(graph.RemoveTransitionFactory(factory2));

            var factory4 = new TestTransitionFactory<CloneableTestVertex>(
                vertex1,
                new[]
                {
                    new Edge<CloneableTestVertex>(vertex1, vertex2),
                    new Edge<CloneableTestVertex>(vertex1, vertex3)
                });
            graph.AddTransitionFactory(factory4);
            Assert.IsTrue(graph.ContainsTransitionFactory(factory4));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex1);    // Force exploration from vertex1

            Assert.IsTrue(graph.RemoveTransitionFactory(factory4));
        }

        [Test]
        public void ContainsTransitionFactories()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());

            Assert.IsFalse(graph.ContainsTransitionFactory(null));
            Assert.IsFalse(graph.ContainsTransitionFactory(factory1));

            graph.AddTransitionFactory(factory1);

            Assert.IsFalse(graph.ContainsTransitionFactory(null));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory1));

            var vertex2 = new CloneableTestVertex("2");
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactory(factory2);

            Assert.IsFalse(graph.ContainsTransitionFactory(null));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory1));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory2));

            graph.RemoveTransitionFactory(factory1);

            Assert.IsFalse(graph.ContainsTransitionFactory(null));
            Assert.IsFalse(graph.ContainsTransitionFactory(factory1));
            Assert.IsTrue(graph.ContainsTransitionFactory(factory2));
        }

        [Test]
        public void ClearTransitionFactories()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");

            var edge11 = new Edge<CloneableTestVertex>(vertex1, vertex1);
            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge23 = new Edge<CloneableTestVertex>(vertex2, vertex3);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(new[]
                {
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, new[] { edge11, edge12, edge13 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, new[] { edge23 })
                }));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex3, new[] { edge33 }));

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed => trigger caching of edges
            graph.OutEdges(vertex1);
            graph.OutEdges(vertex2);
            graph.OutEdges(vertex3);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3 });

            graph.ClearTransitionFactories();

            AssertNoVertices(graph, new[] { vertex1, vertex2, vertex3 });
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var otherVertex1 = new CloneableTestVertex("1");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");

            var edge34 = new Edge<CloneableTestVertex>(vertex3, vertex4);

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex3));
            Assert.IsFalse(graph.ContainsVertex(vertex4));

            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactory(factory1);
            Assert.IsFalse(graph.ContainsVertex(vertex1));  // Not explored yet
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex3));
            Assert.IsFalse(graph.ContainsVertex(vertex4));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex1);

            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex3));
            Assert.IsFalse(graph.ContainsVertex(vertex4));

            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactory(factory2);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));  // Not explored yet
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex3));
            Assert.IsFalse(graph.ContainsVertex(vertex4));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex2);

            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex3));
            Assert.IsFalse(graph.ContainsVertex(vertex4));

            var factoryOther1 = new TestTransitionFactory<CloneableTestVertex>(otherVertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph.AddTransitionFactory(factoryOther1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1)); // Not explored yet
            Assert.IsFalse(graph.ContainsVertex(vertex3));
            Assert.IsFalse(graph.ContainsVertex(vertex4));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(otherVertex1);

            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(vertex2));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex3));
            Assert.IsFalse(graph.ContainsVertex(vertex4));

            var factory3 = new TestTransitionFactory<CloneableTestVertex>(vertex3, new[] { edge34 });
            graph.AddTransitionFactory(factory3);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(vertex2));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex3));  // Not explored yet
            Assert.IsFalse(graph.ContainsVertex(vertex4));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex3);

            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(vertex2));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
            Assert.IsTrue(graph.ContainsVertex(vertex3));
            Assert.IsTrue(graph.ContainsVertex(vertex4));   // Discovered when requesting vertex3
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");

            var edge11 = new Edge<CloneableTestVertex>(vertex1, vertex1);
            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge24 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);
            var edge41 = new Edge<CloneableTestVertex>(vertex4, vertex1);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(new[]
                {
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, new[] { edge11, edge12 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, new[] { edge24 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, new[] { edge33 }),
                }));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, new[] { edge13 }));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, new[] { edge41 }));

            Assert.AreSame(edge11, graph.OutEdge(vertex1, 0));
            Assert.AreSame(edge13, graph.OutEdge(vertex1, 2));
            Assert.AreSame(edge24, graph.OutEdge(vertex2, 0));
            Assert.AreSame(edge33, graph.OutEdge(vertex3, 0));
            Assert.AreSame(edge41, graph.OutEdge(vertex4, 0));
            Assert.AreSame(edge41, graph.OutEdge(vertex4, 0));
        }

        [Test]
        public void OutEdge_WithFilter()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");
            var vertex5 = new CloneableTestVertex("5");
            var vertex6 = new CloneableTestVertex("6");
            var vertex7 = new CloneableTestVertex("7");

            var edge11 = new Edge<CloneableTestVertex>(vertex1, vertex1);
            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge54 = new Edge<CloneableTestVertex>(vertex5, vertex4);
            var edge61 = new Edge<CloneableTestVertex>(vertex6, vertex1);
            var edge67 = new Edge<CloneableTestVertex>(vertex6, vertex7);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(new[]
                {
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, new[] { edge11, edge12, edge13 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex5, new[] { edge54 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex6, new[] { edge61, edge67 }),
                }));

            graph.SuccessorVertexPredicate = vertex => vertex != vertex4;
            graph.SuccessorEdgePredicate = edge => edge != edge61;

            Assert.AreSame(edge11, graph.OutEdge(vertex1, 0));
            Assert.AreSame(edge13, graph.OutEdge(vertex1, 2));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => graph.OutEdge(vertex5, 0));    // Filtered
            Assert.AreSame(edge67, graph.OutEdge(vertex6, 0));  // Because of the filter
            AssertIndexOutOfRange(() => graph.OutEdge(vertex6, 1));    // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // Restore no filter
            graph.SuccessorVertexPredicate = _ => true;
            graph.SuccessorEdgePredicate = _ => true;

            Assert.AreSame(edge54, graph.OutEdge(vertex5, 0));
            Assert.AreSame(edge61, graph.OutEdge(vertex6, 0));
            Assert.AreSame(edge67, graph.OutEdge(vertex6, 1));
        }

        [Test]
        public void OutEdge_Throws()
        {
            var graph1 = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            OutEdge_NullThrows_Test(graph1);

            var graph2 = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph2.OutEdge(vertex1, 0));

            var factory1 = new TestTransitionFactory<CloneableTestVertex>(
                vertex1,
                Enumerable.Empty<Edge<CloneableTestVertex>>());
            graph2.AddTransitionFactory(factory1);
            graph2.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>()));
            AssertIndexOutOfRange(() => graph2.OutEdge(vertex1, 0));

            graph2.RemoveTransitionFactory(factory1);
            graph2.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, new[] { new Edge<CloneableTestVertex>(vertex1, vertex2) }));
            AssertIndexOutOfRange(() => graph2.OutEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void OutEdges()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");

            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge14 = new Edge<CloneableTestVertex>(vertex1, vertex4);
            var edge24 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge31 = new Edge<CloneableTestVertex>(vertex3, vertex1);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>()));
            AssertNoOutEdge(graph, vertex1);

            graph.ClearTransitionFactories();
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(new[]
                {
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, new[] { edge12, edge13 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, new[] { edge24 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, new[] { edge31, edge33 }),
                }));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, new[] { edge14 }));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, Enumerable.Empty<Edge<CloneableTestVertex>>()));

            AssertHasOutEdges(graph, vertex1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, vertex2, new[] { edge24 });
            AssertHasOutEdges(graph, vertex3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, vertex4);
        }

        [Test]
        public void OutEdges_WithFilter()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");

            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge14 = new Edge<CloneableTestVertex>(vertex1, vertex4);
            var edge24 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge31 = new Edge<CloneableTestVertex>(vertex3, vertex1);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>()));
            AssertNoOutEdge(graph, vertex1);

            graph.ClearTransitionFactories();
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(new[]
                {
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, new[] { edge12, edge13 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, new[] { edge24 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, new[] { edge31, edge33 }),
                }));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, new[] { edge14 }));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, Enumerable.Empty<Edge<CloneableTestVertex>>()));

            graph.SuccessorVertexPredicate = vertex => vertex != vertex2;
            graph.SuccessorEdgePredicate = edge => edge.Source != edge.Target;

            AssertHasOutEdges(graph, vertex1, new[] { edge13, edge14 });    // Filtered
            AssertHasOutEdges(graph, vertex2, new[] { edge24 });
            AssertHasOutEdges(graph, vertex3, new[] { edge31 });            // Filtered
            AssertNoOutEdge(graph, vertex4);

            // Restore no filter
            graph.SuccessorVertexPredicate = _ => true;
            graph.SuccessorEdgePredicate = _ => true;

            AssertHasOutEdges(graph, vertex1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, vertex2, new[] { edge24 });
            AssertHasOutEdges(graph, vertex3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, vertex4);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var graph1 = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            OutEdges_NullThrows_Test(graph1);

            var graph2 = new TransitionFactoryImplicitGraph<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>();
            OutEdges_Throws_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetOutEdges()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex0 = new CloneableTestVertex("0");
            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");
            var vertex5 = new CloneableTestVertex("5");

            var edge1 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge2 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge3 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge4 = new Edge<CloneableTestVertex>(vertex2, vertex2);
            var edge5 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge6 = new Edge<CloneableTestVertex>(vertex3, vertex1);
            var edge7 = new Edge<CloneableTestVertex>(vertex4, vertex5);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>()));
            AssertNoOutEdge(graph, vertex1);

            graph.ClearTransitionFactories();
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(new[]
                {
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, new[] { edge1, edge2, edge3 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, new[] { edge4 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, new[] { edge6 }),
                }));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex2, new[] { edge5 }));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, new[] { edge7 }));

            Assert.IsFalse(graph.TryGetOutEdges(vertex0, out _));

            Assert.IsFalse(graph.TryGetOutEdges(vertex5, out _));   // Vertex5 was not discovered

            Assert.IsTrue(graph.TryGetOutEdges(vertex3, out IEnumerable<Edge<CloneableTestVertex>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(vertex1, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);

            // Trigger discover of vertex5
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex4);

            Assert.IsTrue(graph.TryGetOutEdges(vertex5, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);
        }

        [Test]
        public void TryGetOutEdges_WithFilter()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");
            var vertex5 = new CloneableTestVertex("5");

            var edge1 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge2 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge3 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge4 = new Edge<CloneableTestVertex>(vertex2, vertex2);
            var edge5 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge6 = new Edge<CloneableTestVertex>(vertex3, vertex1);
            var edge7 = new Edge<CloneableTestVertex>(vertex4, vertex5);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>()));
            AssertNoOutEdge(graph, vertex1);

            graph.ClearTransitionFactories();
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(new[]
                {
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, new[] { edge1, edge2, edge3 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, new[] { edge4 }),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, new[] { edge6 }),
                }));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex2, new[] { edge5 }));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, new[] { edge7 }));

            graph.SuccessorVertexPredicate = vertex => vertex != vertex4;
            graph.SuccessorEdgePredicate = edge => edge.Source != edge.Target;

            Assert.IsTrue(graph.TryGetOutEdges(vertex2, out IEnumerable<Edge<CloneableTestVertex>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges); // Both edges filtered by the 2 filters combined

            // Restore no filter
            graph.SuccessorVertexPredicate = _ => true;
            graph.SuccessorEdgePredicate = _ => true;

            Assert.IsTrue(graph.TryGetOutEdges(vertex2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge4, edge5 }, gotEdges);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion
    }
}
