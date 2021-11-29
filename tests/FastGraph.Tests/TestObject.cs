namespace FastGraph.Tests
{
    /// <summary>
    /// Object used for tests.
    /// </summary>
    internal sealed class TestObject
    {
        public TestObject(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}