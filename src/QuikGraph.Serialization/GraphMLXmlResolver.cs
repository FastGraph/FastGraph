#if SUPPORTS_GRAPHS_SERIALIZATION
using System;
using System.Diagnostics;
using System.IO;
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
            if (absoluteUri.AbsoluteUri.EndsWith("graphml.dtd"))
                return GetResource("graphml.dtd");

            if (absoluteUri.AbsoluteUri.EndsWith("graphml.xsd"))
                return GetResource("graphml.xsd");
            
            if (absoluteUri.AbsoluteUri.EndsWith("graphml-structure.xsd"))
                return GetResource("graphml-structure.xsd");
            
            if (absoluteUri.AbsoluteUri.EndsWith("graphml-attributes.xsd"))
                return GetResource("graphml-attributes.xsd");
            
            if (absoluteUri.AbsoluteUri.EndsWith("graphml-parseinfo.xsd"))
                return GetResource("graphml-parseinfo.xsd");

            if (absoluteUri.AbsoluteUri.EndsWith("xlink.xsd"))
                return GetResource("xlink.xsd");
            
            return _baseResolver.GetEntity(absoluteUri, role, ofObjectToReturn);

            #region Local function

            Stream GetResource(string resourceName)
            {
                Stream resourceStream = typeof(GraphMLExtensions).Assembly
                    .GetManifestResourceStream(
                        typeof(GraphMLExtensions),
                        resourceName);
                
                Debug.Assert(resourceStream != null);
                return resourceStream;
            }

            #endregion
        }
    }
}
#endif