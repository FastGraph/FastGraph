using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Tests;

namespace QuikGraph.Tests
{
    [TestFixture]
    internal class DataStructureTests : QuikGraphUnitTests
    {
        [Test]
        public void DisplayLinkedList()
        {
            var target = new LinkedList<int>();
            target.AddFirst(0);
            target.AddFirst(1);
        }
    }
}
