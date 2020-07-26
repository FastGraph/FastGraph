using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace QuikGraph.Serialization.Tests
{
    #region Test classes

    public class TestVertex
    {
        public TestVertex([NotNull] string id)
        {
            ID = id;
        }

        [NotNull]
        public string ID { get; }

        [XmlAttribute("v_stringDefault")]
        [DefaultValue("defaultDefaultString")]
        public string StringDefault { get; set; }

        [XmlAttribute("v_string")]
        [DefaultValue("defaultString")]
        public string String { get; set; }

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
        public int[] IntArray { get; set; }

        [XmlAttribute("v_intIList")]
        public IList<int> IntIList { get; set; }
    }

    public sealed class EquatableTestVertex : TestVertex, IEquatable<EquatableTestVertex>
    {
        public EquatableTestVertex([NotNull] string id)
            : base(id)
        {
        }

        public bool Equals(EquatableTestVertex other)
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

        public override bool Equals(object obj)
        {
            return Equals(obj as EquatableTestVertex);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ID.GetHashCode();
                hashCode = (hashCode * 397) ^ (StringDefault != null ? StringDefault.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (String != null ? String.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Int;
                hashCode = (hashCode * 397) ^ Long.GetHashCode();
                hashCode = (hashCode * 397) ^ Bool.GetHashCode();
                hashCode = (hashCode * 397) ^ Float.GetHashCode();
                hashCode = (hashCode * 397) ^ Double.GetHashCode();
                hashCode = (hashCode * 397) ^ (IntArray != null ? IntArray.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (IntIList != null ? IntIList.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    #endregion
}