#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Collections
{
    /// <summary>
    /// Base class for heap tests.
    /// </summary>
    [TestFixture]
    internal abstract class HeapTestsBase
    {
        #region Test classes

        protected sealed class TestPriority : IEquatable<TestPriority>, IComparable<TestPriority>
        {
            public TestPriority(int priority)
            {
                _priority = priority;
            }

            private readonly int _priority;

            public bool Equals(TestPriority? other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return _priority == other._priority;
            }

            public override bool Equals(object? obj)
            {
#pragma warning disable CS8604
                return ReferenceEquals(this, obj) || Equals(obj as TestPriority);
#pragma warning restore CS8604
            }

            public int CompareTo(TestPriority? other)
            {
                if (other is null)
                    return 1;
                if (ReferenceEquals(this, other))
                    return 0;
                return _priority.CompareTo(other._priority);
            }

            public override int GetHashCode()
            {
                return _priority.GetHashCode();
            }
        }

        #endregion
    }
}
