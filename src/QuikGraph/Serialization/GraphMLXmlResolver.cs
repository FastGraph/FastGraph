#if SUPPORTS_GRAPHS_SERIALIZATION
using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Net;
using System.Xml;

namespace QuickGraph.Serialization
{
    /// <summary>
    /// A resolver that loads graphML DTD and XSD schemas 
    /// from embedded resources.
    /// </summary>
    public sealed class GraphMLXmlResolver : XmlResolver
    {
        readonly XmlResolver baseResolver;

        public GraphMLXmlResolver()
            :this(new XmlUrlResolver())
        {
        }


        public GraphMLXmlResolver(XmlResolver baseResolver)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(baseResolver != null);
#endif

            this.baseResolver = baseResolver;
        }

        public const string GraphMLNamespace = "http://graphml.graphdrawing.org/xmlns";

        ICredentials _credentials;
        public override ICredentials Credentials
        {
            set
            {
                this._credentials = value;
            }
        }
 
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri.AbsoluteUri == "http://www.graphdrawing.org/dtds/graphml.dtd")
                return typeof(GraphMLExtensions).Assembly.GetManifestResourceStream(typeof(GraphMLExtensions), "graphml.dtd");
            else if (absoluteUri.AbsoluteUri == "http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd")
                return typeof(GraphMLExtensions).Assembly.GetManifestResourceStream(typeof(GraphMLExtensions), "graphml.xsd");
            else if (absoluteUri.AbsoluteUri == "http://graphml.graphdrawing.org/xmlns/1.0/graphml-structure.xsd")
                return typeof(GraphExtensions).Assembly.GetManifestResourceStream(typeof(GraphMLExtensions), "graphml-structure.xsd");

            return this.baseResolver.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}
#endif