using Should;
using Xunit;

using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Infrastructure.Intg.Test
{
    public class ItemValidationBehaviour
    {
        private readonly IItemValidator _validator;

        public ItemValidationBehaviour()
        {
            _validator = new ItemValidator();
        }

        [Fact]
        public void Should_return_IsValid_for_valid_serialized_items()
        {
            bool isValid = _validator.IsValid(@"..\..\..\test-data\content\Home - Original.item");


            isValid.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_not_valid_for_corrupted_serialized_items()
        {
            bool isValid = _validator.IsValid(@"..\..\..\test-data\content\Home.item");

            isValid.ShouldBeFalse();
        }
    }
}
