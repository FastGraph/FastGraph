﻿#nullable enable

namespace FastGraph.Graphviz.Helpers
{
    /// <summary>
    /// String representing HTML content.
    /// </summary>
    internal struct HtmlString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlString"/> struct.
        /// </summary>
        /// <param name="html">HTML string.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="html"/> is <see langword="null"/>.</exception>
        public HtmlString(string html)
        {
            String = html;
        }

        /// <summary>
        /// HTML string.
        /// </summary>
        public string String { get; }
    }
}
