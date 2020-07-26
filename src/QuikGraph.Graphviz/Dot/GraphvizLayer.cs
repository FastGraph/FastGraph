using System;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Graphviz layer.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class GraphvizLayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizLayer"/> class.
        /// </summary>
        /// <param name="name">Layer name.</param>
        public GraphvizLayer([NotNull] string name)
        {
            SetName(name);
        }

        private void SetName([NotNull] string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            _name = name;
        }

        private string _name;

        /// <summary>
        /// Layer name.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetName(value);
        }
    }
}