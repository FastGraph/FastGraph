#nullable enable

using HtmlAgilityPack;
using NUnit.Framework;
using FastGraph.Graphviz.Dot;
using static FastGraph.Tests.FastGraphUnitTestsHelpers;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="SvgHtmlWrapper"/>.
    /// </summary>
    [TestFixture]
    internal sealed class SvgHtmlWrapperTests
    {
        #region Test helpers

        private static string CheckValidHtmlFile(string htmlFilePath)
        {
            Assert.IsTrue(File.Exists(htmlFilePath));
            var htmlDocument = new HtmlDocument();
            string htmlContent = File.ReadAllText(htmlFilePath);
            htmlDocument.LoadHtml(htmlContent);
            Assert.IsFalse(htmlDocument.ParseErrors.Any());
            return htmlContent;
        }

        #endregion

        [Test]
        public void DumpHtml()
        {
            const string svgFile = "input.svg";
            string svgFilePath = Path.Combine(GetTemporaryTestDirectory(), svgFile);
            string outputFilePath = SvgHtmlWrapper.DumpHtml(new GraphvizSize(150, 150), svgFilePath);

            // File exists and is valid HTML
            string htmlContent = CheckValidHtmlFile(outputFilePath);
            StringAssert.Contains(svgFilePath, htmlContent);
        }

        [Test]
        public void DumpHtml_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => SvgHtmlWrapper.DumpHtml(new GraphvizSize(150, 150), default));
#pragma warning restore CS8625
        }

        private static readonly string SampleSvg =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
            "<svg xmlns = \"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"300\" height=\"200\">" + Environment.NewLine +
            "  <title>Simple SVG figure example</title>" + Environment.NewLine +
            "  <desc>" + Environment.NewLine +
            "    This figure is constituted of a rectangle," + Environment.NewLine +
            "    a segment and a circle." + Environment.NewLine +
            "  </desc>" + Environment.NewLine +
            Environment.NewLine +
            "  <rect width = \"100\" height=\"80\" x=\"0\" y=\"70\" fill=\"green\" />" + Environment.NewLine +
            "  <line x1=\"5\" y1=\"5\" x2=\"250\" y2=\"95\" stroke=\"red\" />" + Environment.NewLine +
            "  <circle cx=\"90\" cy=\"80\" r=\"50\" fill=\"blue\" />" + Environment.NewLine +
            "  <text x=\"180\" y=\"60\">A text</text>" + Environment.NewLine +
            "</svg>";

        private static readonly string MissingSizeSampleSvg =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
            "<svg xmlns = \"http://www.w3.org/2000/svg\" version=\"1.1\">" + Environment.NewLine +
            "  <rect width = \"100\" height=\"80\" x=\"0\" y=\"70\" fill=\"green\" />" + Environment.NewLine +
            "</svg>";

        [Test]
        public void WrapSvg()
        {
            const string svgFile = "sample.svg";
            string svgFilePath = Path.Combine(GetTemporaryTestDirectory(), svgFile);
            File.WriteAllText(svgFilePath, SampleSvg);
            if (!File.Exists(svgFilePath))
                throw new InvalidOperationException("Failed to create test svg file.");

            string outputFilePath = SvgHtmlWrapper.WrapSvg(svgFilePath);

            // File exists and is valid HTML
            string htmlContent = CheckValidHtmlFile(outputFilePath);
            StringAssert.Contains(svgFilePath, htmlContent);
        }

        [Test]
        public void WrapSvg_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => SvgHtmlWrapper.WrapSvg(default));
#pragma warning restore CS8625
        }

        [Test]
        public void ParseSize()
        {
            GraphvizSize size = SvgHtmlWrapper.ParseSize(SampleSvg);
            Assert.AreEqual(new GraphvizSize(300, 200), size);

            // Size not found => fallback
            size = SvgHtmlWrapper.ParseSize(MissingSizeSampleSvg);
            Assert.AreEqual(new GraphvizSize(400, 400), size);
        }

        [Test]
        public void ParseSize_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => SvgHtmlWrapper.ParseSize(default));
#pragma warning restore CS8625
        }
    }
}
