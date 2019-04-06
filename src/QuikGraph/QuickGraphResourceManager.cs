using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Drawing;
using System.IO;

namespace QuickGraph
{
    public static class QuickGraphResourceManager
    {
        public static Image GetLogo()
        {
            return GetImage("quickgraph");
        }

        public static Image GetBanner()
        {
            return GetImage("quickgraph.banner");
        }

        private static Image GetImage(string name)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(name != null);
#endif

            using (Stream stream = typeof(QuickGraphResourceManager).Assembly.GetManifestResourceStream(String.Format("QuickGraph.{0}.png", name)))
                return Image.FromStream(stream);
        }

        public static void DumpResources(string path)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(path != null);
#endif

            GetLogo().Save(Path.Combine(path, "quickgraph.png"), System.Drawing.Imaging.ImageFormat.Png);
            GetBanner().Save(Path.Combine(path, "quickgraph.banner.png"), System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
