using Microsoft.Pex.Framework;
using NUnit.Framework;

namespace QuickGraph
{
    [TestFixture, PexClass]
    public partial class UndirectedGraphTest
    {
        [PexMethod]
        public static void IsAdjacentEdgesEmpty<T,E>([PexAssumeUnderTest]IUndirectedGraph<T, E> g)
            where E : IEdge<T>
        {
            foreach (T v in g.Vertices)
            {
                Assert.AreEqual(
                    g.IsAdjacentEdgesEmpty(v),
                    g.AdjacentDegree(v) == 0);
            }
        }
    }
}
