using System;
using Xunit;

namespace Redskap.Tests
{
    public class PostCodesTests
    {
        [Fact]
        public void TryGetPostalName_Existing_Returns_True()
        {
            Assert.True(PostCodes.TryGetPostalName("4550", out var postalName));
            Assert.Equal("FARSUND", postalName);
        }

        [Fact]
        public void TryGetPostalName_Missing_Returns_False()
        {
            Assert.False(PostCodes.TryGetPostalName("9999", out var postalName));
            Assert.Null(postalName);
        }

        [Fact]
        public void TryGetPostalName_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => PostCodes.TryGetPostalName(null!, out _));
        }

        [Fact]
        public void IsValid_Existing_Returns_True()
        {
            Assert.True(PostCodes.IsValid("4550"));
        }

        [Fact]
        public void IsValid_Missing_Returns_False()
        {
            Assert.False(PostCodes.IsValid("9999"));
        }

        [Fact]
        public void IsValid_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => PostCodes.IsValid(null!));
        }

        [Fact]
        public void GetAll_Returns_All()
        {
            Assert.InRange(PostCodes.GetAll().Count, 5000, 6000);
        }
    }
}
