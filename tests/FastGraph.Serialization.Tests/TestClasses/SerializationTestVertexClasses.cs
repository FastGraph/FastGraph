#nullable enable

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FastGraph.Serialization.Tests
{
    #region Test classes

    public class TestVertex
    {
        public TestVertex(string id)
        {
            ID = id;
        }

        public string ID { get; }

        [XmlAttribute("v_stringDefault")]
        [DefaultValue("defaultDefaultString")]
        public string? StringDefault { get; set; }

        [XmlAttribute("v_string")]
        [DefaultValue("defaultString")]
        public string? String { get; set; }

        [XmlAttribute("v_int")]
        [DefaultValue(1)]
        public int Int { get; set; }

        [XmlAttribute("v_long")]
        [DefaultValue(2L)]
        public long Long { get; set; }

        [XmlAttribute("v_bool")]
        public bool Bool { get; set; }

        [XmlAttribute("v_float")]
        public float Float { get; set; }

        [XmlAttribute("v_double")]
        public double Double { get; set; }

        [XmlAttribute("v_intArray")]
        public int[]? IntArray { get; set; }

        [XmlAttribute("v_intIList")]
        public IList<int>? IntIList { get; set; }
    }

    public sealed class EquatableTestVertex : TestVertex, IEquatable<EquatableTestVertex>
    {
        public EquatableTestVertex(string id)
            : base(id)
        {
        }

        public bool Equals(EquatableTestVertex? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(ID, other.ID)
                   && string.Equals(StringDefault, other.StringDefault)
                   && string.Equals(String, other.String)
                   && Int == other.Int
                   && Long == other.Long
                   && Bool == other.Bool
                   && Float.Equals(other.Float)
                   && Double.Equals(other.Double)
                   && Equals(IntArray, other.IntArray)
                   && Equals(IntIList, other.IntIList);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EquatableTestVertex);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ID.GetHashCode();
                hashCode = (hashCode * 397) ^ (StringDefault != default ? StringDefault.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (String != default ? String.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Int;
                hashCode = (hashCode * 397) ^ Long.GetHashCode();
                hashCode = (hashCode * 397) ^ Bool.GetHashCode();
                hashCode = (hashCode * 397) ^ Float.GetHashCode();
                hashCode = (hashCode * 397) ^ Double.GetHashCode();
                hashCode = (hashCode * 397) ^ (IntArray != default ? IntArray.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (IntIList != default ? IntIList.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    #endregion
}
