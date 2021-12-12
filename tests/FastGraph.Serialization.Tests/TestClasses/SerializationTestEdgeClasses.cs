#nullable enable

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FastGraph.Serialization.Tests
{
    #region Test classes

    public sealed class TestEdge : Edge<TestVertex>
    {
        public TestEdge(
            TestVertex source,
            TestVertex target,
            string id)
            : base(source, target)
        {
            ID = id;
        }

        public TestEdge(
            TestVertex source,
            TestVertex target,
            string id,
            string @string,
            int @int,
            long @long,
            float @float,
            double @double,
            bool @bool)
            : this(source, target, id)
        {
            String = @string;
            Int = @int;
            Long = @long;
            Float = @float;
            Double = @double;
            Bool = @bool;
        }

        public string ID { get; }

        [XmlAttribute("e_string")]
        [DefaultValue("defaultString")]
        public string? String { get; set; }

        [XmlAttribute("e_int")]
        [DefaultValue(1)]
        public int Int { get; set; }

        [XmlAttribute("e_long")]
        [DefaultValue(2L)]
        public long Long { get; set; }

        [XmlAttribute("e_double")]
        public double Double { get; set; }

        [XmlAttribute("e_bool")]
        public bool Bool { get; set; }

        [XmlAttribute("e_float")]
        public float Float { get; set; }
    }

    public sealed class EquatableTestEdge : Edge<EquatableTestVertex>, IEquatable<EquatableTestEdge>
    {
        public EquatableTestEdge(
            EquatableTestVertex source,
            EquatableTestVertex target,
            string id)
            : base(source, target)
        {
            ID = id;
        }

        public string ID { get; }

        [XmlAttribute("e_string")]
        [DefaultValue("defaultString")]
        public string? String { get; set; }

        [XmlAttribute("e_int")]
        [DefaultValue(1)]
        public int Int { get; set; }

        [XmlAttribute("e_long")]
        [DefaultValue(2L)]
        public long Long { get; set; }

        [XmlAttribute("e_double")]
        public double Double { get; set; }

        [XmlAttribute("e_bool")]
        public bool Bool { get; set; }

        [XmlAttribute("e_float")]
        public float Float { get; set; }

        public bool Equals(EquatableTestEdge? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(ID, other.ID)
                   && string.Equals(String, other.String)
                   && Int == other.Int
                   && Long == other.Long
                   && Double.Equals(other.Double)
                   && Bool == other.Bool
                   && Float.Equals(other.Float);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EquatableTestEdge);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ID.GetHashCode();
                hashCode = (hashCode * 397) ^ (String != default ? String.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Int;
                hashCode = (hashCode * 397) ^ Long.GetHashCode();
                hashCode = (hashCode * 397) ^ Double.GetHashCode();
                hashCode = (hashCode * 397) ^ Bool.GetHashCode();
                hashCode = (hashCode * 397) ^ Float.GetHashCode();
                return hashCode;
            }
        }
    }

    public sealed class TestEdgeNoSetter : Edge<TestVertex>
    {
        public TestEdgeNoSetter(
            TestVertex source,
            TestVertex target,
            string id)
            : base(source, target)
        {
            ID = id;
        }

        public string ID { get; }

        [XmlAttribute("e_string")]
        public string? String { get; }
    }

    #endregion
}
