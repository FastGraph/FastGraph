using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using NUnit.Framework;

namespace QuikGraph.Serialization.Tests
{
    #region Test classes

    public sealed class TestGraph : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_string")]
        [DefaultValue("defaultValue")]
        public string String { get; set; }

        [XmlAttribute("g_int")]
        [DefaultValue(1)]
        public int Int { get; set; }

        [XmlAttribute("g_long")]
        [DefaultValue(2L)]
        public long Long { get; set; }

        [XmlAttribute("g_bool")]
        [DefaultValue(false)]
        public bool Bool { get; set; }

        [XmlAttribute("g_float")]
        [DefaultValue(42.0F)]
        public float Float { get; set; }

        [XmlAttribute("g_double")]
        [DefaultValue(12.0)]
        public double Double { get; set; }

        [XmlAttribute("g_stringArray")]
        public string[] StringArray { get; set; }

        [XmlAttribute("g_intArray")]
        public int[] IntArray { get; set; }

        [XmlAttribute("g_longArray")]
        public long[] LongArray { get; set; }

        [XmlAttribute("g_boolArray")]
        public bool[] BoolArray { get; set; }

        [XmlAttribute("g_floatArray")]
        public float[] FloatArray { get; set; }

        [XmlAttribute("g_doubleArray")]
        public double[] DoubleArray { get; set; }

        [XmlAttribute("g_nullArray")]
        public int[] NullArray
        {
            get => null;
            set
            {
                Assert.IsNull(value);
            }
        }

        #region IList properties

        [XmlAttribute("g_stringIList")]
        public IList<string> StringIList { get; set; }

        [XmlAttribute("g_intIList")]
        public IList<int> IntIList { get; set; }

        [XmlAttribute("g_longIList")]
        public IList<long> LongIList { get; set; }

        [XmlAttribute("g_boolIList")]
        public IList<bool> BoolIList { get; set; }

        [XmlAttribute("g_floatIList")]
        public IList<float> FloatIList { get; set; }

        [XmlAttribute("g_doubleIList")]
        public IList<double> DoubleIList { get; set; }

        #endregion
    }

    public sealed class EquatableTestGraph : AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>, IEquatable<EquatableTestGraph>
    {
        [XmlAttribute("g_string")]
        [DefaultValue("defaultValue")]
        public string String { get; set; }

        [XmlAttribute("g_int")]
        [DefaultValue(1)]
        public int Int { get; set; }

        public bool Equals(EquatableTestGraph other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(String, other.String) && Int == other.Int;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EquatableTestGraph);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                return ((String != null ? String.GetHashCode() : 0) * 397) ^ Int;
            }
        }
    }

    public sealed class TestGraphArrayDefaultValue : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_stringArray")]
        [DefaultValue(new string[0])]
        public string[] StringArray { get; set; }
    }

    public sealed class TestGraphNoSetter : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_string")]
        public string String { get; }
    }

    public sealed class TestGraphNoGetter : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_string")]
        public string String { private get; set; }
    }

    public sealed class TestGraphNullDefaultValue : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_int")]
        [DefaultValue(null)]
        public int Int { get; set; }
    }

    public sealed class TestGraphWrongDefaultValue : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_int")]
        [DefaultValue(1.0f)]
        public int Int { get; set; }
    }

    public sealed class TestGraphWrongDefaultValue2 : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_int")]
        [DefaultValue('a')]
        public int Int { get; set; }
    }

    public sealed class TestGraphNotSupportedType : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_class")]
        public TestVertex TestObject { get; set; }
    }

    public sealed class TestGraphNotSupportedType2 : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_char")]
        [DefaultValue('a')]
        public char Char { get; set; }
    }

    public sealed class TestGraphNotSupportedType3 : AdjacencyGraph<TestVertex, TestEdge>
    {
        [XmlAttribute("g_chars")]
        public char[] Chars { get; set; }
    }

    public sealed class TestGraphObjectDefaultValue : AdjacencyGraph<TestVertex, TestEdge>
    {
        public class DefaultValueObject : DefaultValueAttribute
        {
            public DefaultValueObject() 
                : base(new TestVertex("0"))
            {
            }
        }

        [XmlAttribute("g_object")]
        [DefaultValueObject]
        public TestVertex Object { get; set; }
    }

    #endregion
}