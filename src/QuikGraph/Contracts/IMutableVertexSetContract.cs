using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IMutableVertexSet<>))]
#endif
    abstract class IMutableVertexSetContract<TVertex>
        : IMutableVertexSet<TVertex>
    {
#region IMutableVertexSet<TVertex> Members

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexAdded
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        bool IMutableVertexSet<TVertex>.AddVertex(TVertex v)
        {
            IMutableVertexSet<TVertex> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Ensures(Contract.Result<bool>() == Contract.OldValue(!ithis.ContainsVertex(v)));
            Contract.Ensures(ithis.ContainsVertex(v));
            Contract.Ensures(ithis.VertexCount == Contract.OldValue(ithis.VertexCount) + (Contract.Result<bool>() ? 1 : 0));
#endif

            return default(bool);
        }

        int IMutableVertexSet<TVertex>.AddVertexRange(IEnumerable<TVertex> vertices)
        {
            IMutableVertexSet<TVertex> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
            Contract.Requires(Enumerable.All(vertices, v => v != null));
            Contract.Ensures(Enumerable.All(vertices, v => ithis.ContainsVertex(v)));
            Contract.Ensures(ithis.VertexCount == Contract.OldValue(ithis.VertexCount) + Contract.Result<int>());
#endif

            return default(int);
        }

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexRemoved
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        bool IMutableVertexSet<TVertex>.RemoveVertex(TVertex v)
        {
            IMutableVertexSet<TVertex> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Ensures(Contract.Result<bool>() == Contract.OldValue(ithis.ContainsVertex(v)));
            Contract.Ensures(!ithis.ContainsVertex(v));
            Contract.Ensures(ithis.VertexCount == Contract.OldValue(ithis.VertexCount) - (Contract.Result<bool>() ? 1 : 0));
#endif

            return default(bool);
        }

        int IMutableVertexSet<TVertex>.RemoveVertexIf(VertexPredicate<TVertex> pred)
        {
            IMutableVertexSet<TVertex> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(pred != null);
            Contract.Ensures(Contract.Result<int>() == Contract.OldValue(Enumerable.Count(ithis.Vertices, v => pred(v))));
            Contract.Ensures(Enumerable.All(ithis.Vertices, v => !pred(v)));
            Contract.Ensures(ithis.VertexCount == Contract.OldValue(ithis.VertexCount) - Contract.Result<int>());
#endif

            return default(int);
        }

#endregion

#region IVertexSet<TVertex> Members

        public bool IsVerticesEmpty {
          get { throw new NotImplementedException(); }
        }

        public int VertexCount {
          get { throw new NotImplementedException(); }
        }

        public IEnumerable<TVertex> Vertices {
          get { throw new NotImplementedException(); }
        }

#endregion

#region IImplicitVertexSet<TVertex> Members

        public bool ContainsVertex(TVertex vertex) {
          throw new NotImplementedException();
        }

#endregion
    }
}
