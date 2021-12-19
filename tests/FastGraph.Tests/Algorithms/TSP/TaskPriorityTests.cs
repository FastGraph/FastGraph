#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.TSP;

namespace FastGraph.Tests.Algorithms.TSP
{
    /// <summary>
    /// Tests for <see cref="TaskPriority"/>.
    /// </summary>
    [TestFixture]
    internal sealed class TaskPriorityTests
    {
        [Test]
        public void Constructor()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Invoking((Func<TaskPriority>)(() => new TaskPriority(10.0, 5))).Should().NotThrow();
        }

        [Test]
        public void Equals()
        {
            var priority1 = new TaskPriority(1.0, 2);
            var priority2 = new TaskPriority(1.0, 2);
            var priority3 = new TaskPriority(2.0, 2);
            var priority4 = new TaskPriority(1.0, 1);
            var priority5 = new TaskPriority(2.0, 1);

            priority1.Should().Be(priority1);
            priority2.Should().Be(priority1);
            (priority1 == priority2).Should().BeTrue();
            (priority2 == priority1).Should().BeTrue();
            (priority1 != priority2).Should().BeFalse();
            (priority2 != priority1).Should().BeFalse();

            priority3.Should().NotBe(priority1);
            (priority1 == priority3).Should().BeFalse();
            (priority3 == priority1).Should().BeFalse();
            (priority1 != priority3).Should().BeTrue();
            (priority3 != priority1).Should().BeTrue();

            priority4.Should().NotBe(priority1);
            (priority1 == priority4).Should().BeFalse();
            (priority4 == priority1).Should().BeFalse();
            (priority1 != priority4).Should().BeTrue();
            (priority4 != priority1).Should().BeTrue();

            priority5.Should().NotBe(priority1);
            (priority1 == priority5).Should().BeFalse();
            (priority5 == priority1).Should().BeFalse();
            (priority1 != priority5).Should().BeTrue();
            (priority5 != priority1).Should().BeTrue();

#pragma warning disable CS8625
#pragma warning disable CS8604
            priority1.Equals(default).Should().BeFalse();
            (priority1 == default).Should().BeFalse();
            (default == priority1).Should().BeFalse();
            (priority1 != default).Should().BeTrue();
            (default != priority1).Should().BeTrue();
#pragma warning restore CS8604
#pragma warning restore CS8625
        }

        [Test]
        public void Hashcode()
        {
            var priority1 = new TaskPriority(1.0, 2);
            var priority2 = new TaskPriority(1.0, 2);
            var priority3 = new TaskPriority(2.0, 2);

            priority2.GetHashCode().Should().Be(priority1.GetHashCode());
            priority3.GetHashCode().Should().NotBe(priority1.GetHashCode());
        }

        [Test]
        public void Comparison()
        {
            var priority1 = new TaskPriority(1.0, 2);
            var priority2 = new TaskPriority(1.0, 2);
            var priority3 = new TaskPriority(2.0, 2);
            var priority4 = new TaskPriority(1.0, 1);

            (priority1 < priority2).Should().BeFalse();
            (priority1 <= priority2).Should().BeTrue();
            (priority1 > priority2).Should().BeFalse();
            (priority1 >= priority2).Should().BeTrue();

            (priority1 < priority3).Should().BeTrue();
            (priority1 <= priority3).Should().BeTrue();
            (priority1 > priority3).Should().BeFalse();
            (priority1 >= priority3).Should().BeFalse();

            (priority1 < priority4).Should().BeTrue();
            (priority1 <= priority4).Should().BeTrue();
            (priority1 > priority4).Should().BeFalse();
            (priority1 >= priority4).Should().BeFalse();

#pragma warning disable CS8625
            (priority1 < null).Should().BeFalse();
            (priority1 <= null).Should().BeFalse();
            (priority1 > null).Should().BeTrue();
            (priority1 >= null).Should().BeTrue();
#pragma warning restore CS8625
        }
    }
}
