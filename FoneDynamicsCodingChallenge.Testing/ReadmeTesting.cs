using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace FoneDynamicsCodingChallenge.Testing
{
    [TestFixture]
    public class ReadmeTesting
    {
        [Test]
        public void Main()
        {
            // Construct a cache
            Cache<int, int> cd = new Cache<int, int>(10000);

            // Bombard the Cache with 10000 competing AddOrUpdates
            Parallel.For(0, 10000, i => { cd.AddOrUpdate(i, i); });

            //After 10000 AddOrUpdates cd[9999] should be 9999
            cd.TryGetValue(9999, out var value);
            value.ShouldBe(9999);
        }
    }
}