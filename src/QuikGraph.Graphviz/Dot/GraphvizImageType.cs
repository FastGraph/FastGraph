using System.ComponentModel;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Enumeration of possible image types.
    /// </summary>
    public enum GraphvizImageType
    {
        /// <summary>
        /// Client side imagemaps.
        /// </summary>
        [Description("Client-side imagemaps")]
        Cmap = 6,

        /// <summary>
        /// Figure format.
        /// </summary>
        [Description("Figure format")]
        Fig = 0,

        /// <summary>
        /// Gd format.
        /// </summary>
        [Description("Gd format")]
        Gd = 1,

        /// <summary>
        /// Gd2 format.
        /// </summary>
        [Description("Gd2 format")]
        Gd2 = 2,

        /// <summary>
        /// GIF format.
        /// </summary>
        [Description("GIF format")]
        Gif = 3,

        /// <summary>
        /// HP-GL/2 format.
        /// </summary>
        [Description("HP-GL/2 format")]
        Hpgl = 4,

        /// <summary>
        /// Server-side imagemaps.
        /// </summary>
        [Description("Server-side imagemaps")]
        Imap = 5,

        /// <summary>
        /// JPEG format.
        /// </summary>
        [Description("JPEG format")]
        Jpeg = 7,

        /// <summary>
        /// FrameMaker MIF format.
        /// </summary>
        [Description("FrameMaker MIF format")]
        Mif = 8,

        /// <summary>
        /// MetaPost.
        /// </summary>
        [Description("MetaPost")]
        Mp = 9,

        /// <summary>
        /// PCL format.
        /// </summary>
        [Description("PCL format")]
        Pcl = 10,

        /// <summary>
        /// PIC format.
        /// </summary>
        [Description("PIC format")]
        Pic = 11,

        /// <summary>
        /// Plain text format.
        /// </summary>
        [Description("Plain text format")]
        PlainText = 12,

        /// <summary>
        /// Portable Network Graphics format
        /// </summary>
        [Description("Portable Network Graphics format")]
        Png = 13,

        /// <summary>
        /// PostScript.
        /// </summary>
        [Description("Postscript")]
        Ps = 14,

        /// <summary>
        /// PostScript for PDF.
        /// </summary>
        [Description("PostScript for PDF")]
        Ps2 = 15,

        /// <summary>
        /// Scalable Vector Graphics.
        /// </summary>
        [Description("Scalable Vector Graphics")]
        Svg = 0x10,

        /// <summary>
        /// Scalable Vector Graphics, gzipped.
        /// </summary>
        [Description("Scalable Vector Graphics, gzipped")]
        Svgz = 0x11,

        /// <summary>
        /// VRML.
        /// </summary>
        [Description("VRML")]
        Vrml = 0x12,

        /// <summary>
        /// Visual Thought format.
        /// </summary>
        [Description("Visual Thought format")]
        Vtx = 0x13,

        /// <summary>
        /// Wireless BitMap format.
        /// </summary>
        [Description("Wireless BitMap format")]
        Wbmp = 20
    }
}