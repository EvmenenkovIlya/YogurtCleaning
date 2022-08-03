using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests
{
    public class CleaningObjectModelsRequestTests
    {
        [TestCaseSource(typeof(CleaningObjectRequestTestSource))]
        public async Task CleaningObjectRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived(CleaningObjectRequest cleaningObject, string errorMessage)
        {
            //given
            var validationResults = new List<ValidationResult>();

            //when
            var isValid = Validator.TryValidateObject(cleaningObject, new ValidationContext(cleaningObject), validationResults, true);

            //then
            Assert.IsFalse(isValid);
            var actualMessage = validationResults[0].ErrorMessage;
            Assert.That(actualMessage, Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task CleaningObjectRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived()
        {
            //given
            CleaningObjectRequest cleaningObject = new CleaningObjectRequest();
            List<string> expectedMessages = new List<string>() {

            ApiErrorMessages.ClientIdIsPositiveNumber,
            ApiErrorMessages.AddressIsRequired
            

        };
            var validationResults = new List<ValidationResult>();

            //when
            var isValid = Validator.TryValidateObject(cleaningObject, new ValidationContext(cleaningObject), validationResults, true);

            //then
            Assert.IsFalse(isValid);
            for (int i = 0; i < expectedMessages.Count(); i++)
            {
                var actualMessage = validationResults[i].ErrorMessage;
                Assert.That(actualMessage, Is.EqualTo(expectedMessages[i]));
            }
        }

        [Test]
        public async Task CleaningObjectRequestValidation_WhenValidModelPassed_NoErrorsReceived()
        {
            //given
            CleaningObjectRequest cleaningObject = new CleaningObjectRequest()
            {
                ClientId = 42,
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10"
            };
            var validationResults = new List<ValidationResult>();

            //when
            var isValid = Validator.TryValidateObject(cleaningObject, new ValidationContext(cleaningObject), validationResults, true);

            //then
            Assert.IsTrue(isValid);
            Assert.That(validationResults.Count(), Is.EqualTo(0));
        }
    }
}
