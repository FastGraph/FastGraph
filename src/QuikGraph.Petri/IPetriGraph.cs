using System;

namespace QuikGraph.Petri
{
    public interface IPetriGraph<Token> : IMutableBidirectionalGraph<IPetriVertex, IArc<Token>>
    {}
}
