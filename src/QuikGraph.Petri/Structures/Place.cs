using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace QuikGraph.Petri
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
        public Place([NotNull] string name)
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
            builder.AppendLine(ToString());

            foreach (TToken token in Marking)
            {
                builder.AppendLine($"\t{token.GetType().Name}");
            }

            return builder.ToString();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"P({Name}|{Marking.Count})";
        }
    }
}