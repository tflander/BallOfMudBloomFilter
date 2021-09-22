using System;
using FluentAssertions;
using Xunit;

namespace SpamFilterTests
{
    public class SpamFilterTests
    {
        [Fact]
        public void Test1()
        {
            var sf = new SpamFilter.SpamFilter();
            sf.LoadSpamEmailAddresses("Spam.txt");
            
            sf.IsSpam("trustme@evilspammer.com").Should().BeTrue();
            sf.IsSpam("buyworthlessproduct@iwantyourmoney.com").Should().BeTrue();
            sf.IsSpam("joinus@pyramidscheme.com").Should().BeTrue();

            sf.IsSpam("anyone@accenture.com").Should().BeFalse();
        }
    }
}