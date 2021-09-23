using System;
using System.Collections;
using BloomFilterDirty;
using FluentAssertions;
using Xunit;

namespace BloomFilterDirtyTests
{
    public class BallOfMudTests
    {
        private readonly BallOfMud _sut;
        private const string TestData = "testing";

        public BallOfMudTests()
        {
            _sut = new BallOfMud();
        }
        
        [Fact]
        public void TestThem()
        {
            _sut.Add("testing");
            _sut.Add("one");

            _sut.Contains("testing").Should().BeTrue();
            _sut.Contains("one").Should().BeTrue();
            _sut.Contains("two").Should().BeFalse();
            _sut.Contains("three").Should().BeFalse();
        }

        [Fact]
        public void ContainsReturnsTrueWhenAllThreeBitsAreSet()
        {
            _sut.HashBits = new BitArray(10_000);

            var primaryHash = TestData.GetHashCode();
            _sut.HashBits[Math.Abs(primaryHash % 10_000)] = true;

            var secondaryHash = BallOfMud.HashString(TestData);
            var h2 = (primaryHash +  secondaryHash) % 10_000; 
            var h3 = (primaryHash + (2 * secondaryHash)) % 10_000;  
            _sut.HashBits[Math.Abs(h2)] = true;
            _sut.HashBits[Math.Abs(h3)] = true;
            
            _sut.Contains(TestData).Should().BeTrue();
        }

        [Fact]
        public void ContainsReturnsFalseWhenOnlyThePrimaryHashBitIsSet()
        {
            _sut.HashBits = new BitArray(10_000);
            var primaryHash = Math.Abs(TestData.GetHashCode() % 10_000);
            _sut.HashBits[primaryHash] = true;

            _sut.Contains(TestData).Should().BeFalse();
        }        

        [Fact]
        public void ContainsReturnsFalseWhenTheSecondaryHashIsNotSet()
        {
            _sut.HashBits = new BitArray(10_000);
            var primaryHash = Math.Abs(TestData.GetHashCode() % 10_000);
            _sut.HashBits[primaryHash] = true;

            var secondaryHash = BallOfMud.HashString(TestData);
            var h3 = (primaryHash + (2 * secondaryHash)) % 10_000;  
            _sut.HashBits[Math.Abs(h3)] = true;

            _sut.Contains(TestData).Should().BeFalse();
        }        

        [Fact]
        public void ContainsReturnsFalseWhenTheThirdHashIsNotSet()
        {
            _sut.HashBits = new BitArray(10_000);
            var primaryHash = Math.Abs(TestData.GetHashCode() % 10_000);
            _sut.HashBits[primaryHash] = true;

            var secondaryHash = BallOfMud.HashString(TestData);
            var h2 = (primaryHash +  secondaryHash) % 10_000; 
            _sut.HashBits[Math.Abs(h2)] = true;

            _sut.Contains(TestData).Should().BeFalse();
        }        
        
        [Fact]
        public void ContainsReturnsFalseWhenNoBitsAreSet()
        {
            _sut.HashBits = new BitArray(10_000);
            _sut.Contains(TestData).Should().BeFalse();
        }
        
        [Fact]
        public void ContainsReturnsFalseWhenEveryBitIsSet()
        {
            _sut.HashBits = new BitArray(10_000);
            _sut.HashBits.SetAll(true);
            _sut.Contains(TestData).Should().BeTrue();
        }        
        
    }
}