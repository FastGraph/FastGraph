#nullable enable

#if SUPPORTS_CRYPTO_RANDOM
using NUnit.Framework;
using FastGraph.Utils;

namespace FastGraph.Tests.Utils
{
    /// <summary>
    /// Tests related to <see cref="CryptoRandom"/>.
    /// </summary>
    [TestFixture]
    internal sealed class CryptoRandomTests
    {
        [Test]
        public void Constructor()
        {
            // ReSharper disable ObjectCreationAsStatement
            Invoking((Func<CryptoRandom>)(() => new CryptoRandom())).Should().NotThrow();
            Invoking((Func<CryptoRandom>)(() => new CryptoRandom(123456))).Should().NotThrow();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Next()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking((Func<int>)(() => rng.Next())).Should().NotThrow();
            Invoking((Func<int>)(() => rng.Next())).Should().NotThrow();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextWithMax()
        {
            var rng = new CryptoRandom();
            rng.Next(int.MaxValue).Should().BeLessThanOrEqualTo(int.MaxValue);
            rng.Next(10).Should().BeLessThanOrEqualTo(10);
            rng.Next(0).Should().Be(0);
        }

        [Test]
        public void NextWithMax_Throws()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => rng.Next(-1)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => rng.Next(-12)).Should().Throw<ArgumentOutOfRangeException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextWithMinMax()
        {
            var rng = new CryptoRandom();
            AssertBetween(rng.Next(int.MinValue, int.MaxValue), int.MinValue, int.MaxValue);
            AssertBetween(rng.Next(10, int.MaxValue), 10, int.MaxValue);
            AssertBetween(rng.Next(int.MinValue, 10), int.MinValue, 10);
            AssertBetween(rng.Next(-10, 10), -10, 10);
            AssertBetween(rng.Next(-10, 0), -10, 0);
            AssertBetween(rng.Next(-10, -1), -10, -1);
            rng.Next(10, 10).Should().Be(10);

            #region Local function

            void AssertBetween(int value, int min, int max)
            {
                value.Should().BeLessThanOrEqualTo(max);
                value.Should().BeGreaterThanOrEqualTo(min);
            }

            #endregion
        }

        [Test]
        public void NextWithMinMax_Throws()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => rng.Next(10, 9)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => rng.Next(10, -9)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => rng.Next(-10, -11)).Should().Throw<ArgumentOutOfRangeException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextDouble()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking((Func<double>)(() => rng.NextDouble())).Should().NotThrow();
            Invoking((Func<double>)(() => rng.NextDouble())).Should().NotThrow();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextBytes()
        {
            byte[] data = new byte[5];
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => rng.NextBytes(data)).Should().NotThrow();
            Invoking(() => rng.NextBytes(data)).Should().NotThrow();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextBytes_Throws()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => rng.NextBytes(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
#endif
