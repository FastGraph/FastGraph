namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Enumeration of possible cases for component with edges in a graph.
    /// </summary>
    public enum ComponentWithEdges
    {
        /// <summary>
        /// Graph has no component.
        /// </summary>
        NoComponent,

        /// <summary>
        /// Graph has only one component.
        /// </summary>
        OneComponent,

        /// <summary>
        /// Graph has many components.
        /// </summary>
        ManyComponents
    }
}