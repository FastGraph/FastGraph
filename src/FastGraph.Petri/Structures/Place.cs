#nullable enable

using System.Text;

namespace FastGraph.Petri
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    internal sealed class Place<TToken> : IPlace<TToken>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Place{TToken}"/> class.
        /// </summary>
        /// <param name="name">Place name.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        public Place(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IList<TToken> Marking { get; } = new List<TToken>();

        /// <inheritdoc />
        public string ToStringWithMarking()
        {
            var builder = new StringBuilder();
            builder.Append(ToString());

            if (Marking.Count > 0)
            {
                builder.AppendLine();
                builder.Append("\t");
            }

            builder.Append(
                string.Join(
                    Environment.NewLine + "\t", Marking.Select(token => token!.GetType().Name)));

            return builder.ToString();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"P({Name}|{Marking.Count})";
        }
    }
}
