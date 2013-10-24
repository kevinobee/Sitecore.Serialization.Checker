using Should;
using Xunit;
using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Infrastructure.Intg.Test.SerializedItemFileTests
{
    public class ItemValidationBehaviour : SerializedItemFileTest
    {
        private readonly IItemValidator _validator;

        public ItemValidationBehaviour()
        {
            _validator = new ItemValidator();
        }

        [Fact]
        public void Should_return_IsValid_for_valid_serialized_items()
        {
            var isValid = _validator.IsValid(TestDataPath + "Home.item");

            isValid.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_not_valid_for_corrupted_serialized_items()
        {
            var isValid = _validator.IsValid(TestDataPath + "Home-Corrupt.item");

            isValid.ShouldBeFalse();
        }
    }
}
