using System;
using BloomFilterClean;
using Xunit;
using FluentAssertions;
    
namespace BloomFilterCleanTests
{
    public class BloomFilterCleanTests
    {
        [Fact]
        public void TestThem()
        {
            var bf = new BloomFilter();
            bf.Add("testing");
            bf.Add("one");

            bf.Contains("testing").Should().BeTrue();
            bf.Contains("one").Should().BeTrue();
            bf.Contains("two").Should().BeFalse();
            bf.Contains("three").Should().BeFalse();
        }
    }
}