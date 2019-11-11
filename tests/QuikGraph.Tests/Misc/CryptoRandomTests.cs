
using System;
#if SUPPORTS_CRYPTO_RANDOM
using NUnit.Framework;
using QuikGraph.Utils;

namespace QuikGraph.Tests.Utils
{
    /// <summary>
    /// Tests related to <see cref="CryptoRandom"/>.
    /// </summary>
    [TestFixture]
    internal class CryptoRandomTests
    {
        [Test]
        public void Constructor()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new CryptoRandom());
            Assert.DoesNotThrow(() => new CryptoRandom(123456));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Next()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.DoesNotThrow(() => rng.Next());
            Assert.DoesNotThrow(() => rng.Next());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextWithMax()
        {
            var rng = new CryptoRandom();
            Assert.LessOrEqual(rng.Next(int.MaxValue), int.MaxValue);
            Assert.LessOrEqual(rng.Next(10), 10);
            Assert.AreEqual(0, rng.Next(0));
        }

        [Test]
        public void NextWithMax_Throws()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentOutOfRangeException>(() => rng.Next(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => rng.Next(-12));
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
            Assert.AreEqual(10, rng.Next(10, 10));

            #region Local function

            void AssertBetween(int value, int min, int max)
            {
                Assert.LessOrEqual(value, max);
                Assert.GreaterOrEqual(value, min);
            }

            #endregion
        }

        [Test]
        public void NextWithMinMax_Throws()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentOutOfRangeException>(() => rng.Next(10, 9));
            Assert.Throws<ArgumentOutOfRangeException>(() => rng.Next(10, -9));
            Assert.Throws<ArgumentOutOfRangeException>(() => rng.Next(-10, -11));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextDouble()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.DoesNotThrow(() => rng.NextDouble());
            Assert.DoesNotThrow(() => rng.NextDouble());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextBytes()
        {
            var data = new byte[5];
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.DoesNotThrow(() => rng.NextBytes(data));
            Assert.DoesNotThrow(() => rng.NextBytes(data));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void NextBytes_Throws()
        {
            var rng = new CryptoRandom();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => rng.NextBytes(null));
        }
    }
}
#endif