using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace FoneDynamicsCodingChallenge.Testing
{
    [TestFixture]
    public class CapacityTesting
    {
        [Test]
        public void TestNegativeConstructor()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => new Cache<int, int>(-5));
        }

        [Test]
        public void TestZeroConstructor()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => new Cache<int, int>(0));
        }

        [Test]
        public void TestMaxIntConstructor()
        {
            //Possibly passable if enough memory is present, with 16Gb ram, this fails on my machine.
            Should.Throw<OutOfMemoryException>(() => new Cache<int, int>(int.MaxValue));
        }

        [Test]
        public void TestLargeSize()
        {
            var cache = new Cache<int, int>(1000000);

            Parallel.For(0, 1000000, i =>
            {
                cache.AddOrUpdate(i,i);
            });

            Thread.Sleep(1);

            Parallel.For(2, 1000000, i =>
            {
                cache.TryGetValue(i, out int outValue).ShouldBeTrue();
                outValue.ShouldBe(i);
            });
        }

        [Test]
        public void TestBasicExpiry()
        {
            var cache = new Cache<int,int>(3);
            for (var i = 0; i < 6; i++)
            {
                cache.AddOrUpdate(i,i);
            }

            for (var i = 0; i < 3; i++)
            {
                cache.TryGetValue(0, out _).ShouldBeFalse();
            }

            for (var i = 3; i < 6; i++)
            {
                cache.TryGetValue(i, out var outValue).ShouldBeTrue();
                outValue.ShouldBe(i);
            }
        }
    }
}