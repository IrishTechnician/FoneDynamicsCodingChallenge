using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace FoneDynamicsCodingChallenge.Testing
{
    [TestFixture]
    public class ConcurrentDictionaryExtensionTests
    {
        private ConcurrentDictionary<int, int> _dictionary;

        [SetUp]
        public void Setup()
        {
            _dictionary = new ConcurrentDictionary<int, int>();
        }

        [Test]
        public void TestRemove()
        {
            _dictionary.TryAdd(1, 1);
            _dictionary.Remove(1);
            _dictionary.Count.ShouldBe(0);
        }


    }
}
