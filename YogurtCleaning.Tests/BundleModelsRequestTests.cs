using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;

public class BundleModelsRequestTests
{
    [TestCaseSource(typeof(BundleRequestTestSource))]
    public async Task BundleRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived(BundleRequest bundle, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();  
        
        //when
        var isValid = Validator.TryValidateObject(bundle, new ValidationContext(bundle), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }

    [Test]
    public async Task BundleRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived()
    {
        //given
        BundleRequest bundle = new BundleRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.NameIsRequired,
            ApiErrorMessages.TypeIsRequired,
            ApiErrorMessages.PriceIsRequired,
            ApiErrorMessages.MeasureIsRequired,
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(bundle, new ValidationContext(bundle), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.AreEqual(expectedMessages[i], actualMessage);
        }
    }

    [Test]
    public async Task BundleRequestValidation_WhenValidModelPassed_NoErrorsReceived()
    {
        //given
        BundleRequest bundle = new BundleRequest()
        {
            Name = "Kitchen regular cleaning",
            Type = CleaningType.Regular,
            Price = 1000,
            Measure = Measure.Room,
            Services = new List<ServiceResponse>()
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(bundle, new ValidationContext(bundle), validationResults, true);

        //then
        Assert.IsTrue(isValid);
        Assert.AreEqual(0, validationResults.Count());
    }
}