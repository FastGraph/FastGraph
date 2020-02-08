#if SUPPORTS_GRAPHS_SERIALIZATION
using System;
using System.Net;
using System.Xml;
using JetBrains.Annotations;

namespace QuikGraph.Serialization
{
    /// <summary>
    /// A resolver that loads graphML DTD and XSD schemas from embedded resources.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class GraphMLXmlResolver : XmlResolver
    {
        [NotNull]
        private readonly XmlResolver _baseResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphMLXmlResolver"/> class.
        /// </summary>
        public GraphMLXmlResolver()
            : this(new XmlUrlResolver())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphMLXmlResolver"/> class.
        /// </summary>
        /// <param name="baseResolver">Base XML resolver.</param>
        public GraphMLXmlResolver([NotNull] XmlResolver baseResolver)
        {
            _baseResolver = baseResolver ?? throw new ArgumentNullException(nameof(baseResolver));
        }

        /// <summary>
        /// Graph ML XML namespace.
        /// </summary>
        [NotNull]
        // ReSharper disable once InconsistentNaming
        public const string GraphMLNamespace = "http://graphml.graphdrawing.org/xmlns";

        /// <inheritdoc />
        public override ICredentials Credentials
        {
            set
            {
                // Do nothing
            }
        }

        /// <inheritdoc />
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri.AbsoluteUri == "http://www.graphdrawing.org/dtds/graphml.dtd")
                return typeof(GraphMLExtensions).Assembly.GetManifestResourceStream(typeof(GraphMLExtensions), "graphml.dtd");
            if (absoluteUri.AbsoluteUri == "http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd")
                return typeof(GraphMLExtensions).Assembly.GetManifestResourceStream(typeof(GraphMLExtensions), "graphml.xsd");
            if (absoluteUri.AbsoluteUri == "http://graphml.graphdrawing.org/xmlns/1.0/graphml-structure.xsd")
                return typeof(GraphExtensions).Assembly.GetManifestResourceStream(typeof(GraphMLExtensions), "graphml-structure.xsd");
            return _baseResolver.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}
#endif