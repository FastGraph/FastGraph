using System;
using System.Collections.Generic;
using System.Text;

namespace QuikGraph.Algorithms
{
    public delegate void AlgorithmEventHandler<TGraph>(
        IAlgorithm<TGraph> sender,
        EventArgs e);
}
