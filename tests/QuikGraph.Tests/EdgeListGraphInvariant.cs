using NUnit.Framework;

namespace QuickGraph
{
    [TestFixture]
    public static class EdgeListGraphTest
    {
        public static void Iteration<T,E>(IEdgeListGraph<T, E> g)
            where E : IEdge<T>
        {
            int n = g.EdgeCount;
            int i = 0;
            foreach (E e in g.Edges)
                ++i;
        }

        public static void Count<T,E>(IEdgeListGraph<T, E> g)
            where E : IEdge<T>
        {
            int n = g.EdgeCount;
            if (n == 0)
                Assert.IsTrue(g.IsEdgesEmpty);

            int i = 0;
            foreach (E e in g.Edges)
            {
                e.ToString();
                ++i;
            }
            Assert.AreEqual(n, i);
        }
    }
}
