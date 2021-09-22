using System;
using BloomFilterDirty;
using FluentAssertions;
using Xunit;

namespace BloomFilterDirtyTests
{
    public class BallOfMudTests
    {
        [Fact]
        public void TestThem()
        {
            var bf = new BallOfMud();
            bf.Add("testing");
            bf.Add("one");

            bf.Contains("testing").Should().BeTrue();
            bf.Contains("one").Should().BeTrue();
            bf.Contains("two").Should().BeFalse();
            bf.Contains("three").Should().BeFalse();
        }
    }
}