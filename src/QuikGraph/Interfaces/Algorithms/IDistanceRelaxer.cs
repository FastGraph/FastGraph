using System;
using System.Collections.Generic;
using System.Text;

namespace QuikGraph.Algorithms
{
    public interface IDistanceRelaxer 
        : IComparer<double>
    {
        double InitialDistance { get;}
        double Combine(double distance, double weight);
    }
}
