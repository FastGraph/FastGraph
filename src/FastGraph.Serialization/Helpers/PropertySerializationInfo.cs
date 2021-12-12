#nullable enable

#if SUPPORTS_GRAPHS_SERIALIZATION
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Annotations;

namespace FastGraph.Serialization
{
    internal struct PropertySerializationInfo
    {
        /// <summary>
        /// Gets the embedded <see cref="PropertyInfo"/>.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public string Name { get; }

        private readonly bool _hasValue;

        private readonly object? _value;

        public PropertySerializationInfo(PropertyInfo property, string name)
            : this(property, name, default)
        {
        }

        public PropertySerializationInfo(
            PropertyInfo property,
            string name,
            object? value)
        {
            Property = property;
            Name = name;
            _value = value;
            _hasValue = _value != default;
        }

        [Pure]
        public bool TryGetDefaultValue([NotNullWhen(true)] out object? value)
        {
            value = _value;
            return _hasValue;
        }
    }
}
#endif
