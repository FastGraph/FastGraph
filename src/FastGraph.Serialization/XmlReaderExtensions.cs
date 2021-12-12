#nullable enable

using System.Xml;
using JetBrains.Annotations;

namespace FastGraph.Serialization
{
    /// <summary>
    /// Extensions for <see cref="T:System.Xml.XmlReader"/> to help deserializing graph data.
    /// </summary>
    public static class XmlReaderExtensions
    {
        /// <summary>
        /// Reads the content of a named element as an array of booleans.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Boolean array.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="namespaceURI"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="localName"/> is empty.</exception>
        [Pure]
        public static bool[]? ReadElementContentAsBooleanArray(
            XmlReader xmlReader,
            string localName,
            string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToBoolean);
        }

        /// <summary>
        /// Reads the content of a named element as an array of ints.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Int array.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="namespaceURI"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="localName"/> is empty.</exception>
        [Pure]
        public static int[]? ReadElementContentAsInt32Array(
            XmlReader xmlReader,
            string localName,
            string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToInt32);
        }

        /// <summary>
        /// Reads the content of a named element as an array of longs.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Long array.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="namespaceURI"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="localName"/> is empty.</exception>
        [Pure]
        public static long[]? ReadElementContentAsInt64Array(
            XmlReader xmlReader,
            string localName,
            string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToInt64);
        }

        /// <summary>
        /// Reads the content of a named element as an array of floats.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Float array.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="namespaceURI"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="localName"/> is empty.</exception>
        [Pure]
        public static float[]? ReadElementContentAsSingleArray(
            XmlReader xmlReader,
            string localName,
            string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToSingle);
        }

        /// <summary>
        /// Reads the content of a named element as an array of doubles.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Double array.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="namespaceURI"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="localName"/> is empty.</exception>
        [Pure]
        public static double[]? ReadElementContentAsDoubleArray(
            XmlReader xmlReader,
            string localName,
            string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToDouble);
        }

        /// <summary>
        /// Reads the content of a named element as an array of strings.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>String array.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="namespaceURI"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="localName"/> is empty.</exception>
        [Pure]
        public static string[]? ReadElementContentAsStringArray(
            XmlReader xmlReader,
            string localName,
            string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, str => str);
        }

        /// <summary>
        /// Read contents of an XML element as an array of type T.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <param name="stringToT">Converts the XML element string as <typeparamref name="T"/>.</param>
        /// <returns>Array of <typeparamref name="T"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="namespaceURI"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="localName"/> is empty.</exception>
        [Pure]
        public static T[]? ReadElementContentAsArray<T>(
            XmlReader xmlReader,
            string localName,
            string namespaceURI,
            [InstantHandle] Func<string, T> stringToT)
        {
            string str = xmlReader.ReadElementContentAsString(localName, namespaceURI);
            if (str == "default")
                return default;

            if (str.Length > 0 && str[str.Length - 1] == ' ')
            {
                str = str.Remove(str.Length - 1);
            }

            string[] strArray = str.Split(' ');

            var array = new T[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                array[i] = stringToT(strArray[i]);
            }

            return array;
        }
    }
}
