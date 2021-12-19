#nullable enable

using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz
{
    /// <summary>
    /// Represents a Dot engine runner.
    /// </summary>
    public interface IDotEngine
    {
        /// <summary>
        /// Runs the Dot engine using the given <paramref name="dot"/> content and outputs
        /// the result in given <paramref name="outputFilePath"/> respecting <paramref name="imageType"/>.
        /// </summary>
        /// <param name="imageType">Image type.</param>
        /// <param name="dot">Graph serialized using Dot language.</param>
        /// <param name="outputFilePath">Target file path.</param>
        /// <returns>Path to the saved result.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="dot"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="outputFilePath"/> is <see langword="null"/> or empty.</exception>
        string Run(
            GraphvizImageType imageType,
            string dot,
            string outputFilePath);
    }
}
