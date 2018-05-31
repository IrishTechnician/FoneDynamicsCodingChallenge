using NUnit.Framework;
using Shouldly;

namespace FoneDynamicsCodingChallenge.Testing
{
    [TestFixture]
    public class RetrievalTesting
    {
        private Cache<int, int> _cache;

        [SetUp]
        public void Setup()
        {
            _cache = new Cache<int,int>(10);
            for (int i = 0; i < 9; i++)
            {
                _cache.AddOrUpdate(i,i);
            }
        }

        [Test]
        public void CheckValue()
        {
            RetrieveValue(0).ShouldBe(0);
        }

        [Test]
        public void UpdateValue()
        {
            _cache.AddOrUpdate(0,100);
            RetrieveValue(0).ShouldBe(100);
        }

        private int RetrieveValue(int key)
        {
            _cache.TryGetValue(key, out var value);
            return value;
        }
    }
}