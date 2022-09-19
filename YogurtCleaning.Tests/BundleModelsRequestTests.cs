using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;

public class BundleModelsRequestTests
{
    [TestCaseSource(typeof(BundleRequestTestSource))]
    public void BundleRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived(BundleRequest bundle, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();  
        
        //when
        var isValid = Validator.TryValidateObject(bundle, new ValidationContext(bundle), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.That(actualMessage, Is.EqualTo(errorMessage));
    }

    [Test]
    public void BundleRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived()
    {
        //given
        BundleRequest bundle = new BundleRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.NameIsRequired,
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(bundle, new ValidationContext(bundle), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.That(actualMessage, Is.EqualTo(expectedMessages[i]));
        }
    }

    [Test]
    public void BundleRequestValidation_WhenValidModelPassed_NoErrorsReceived()
    {
        //given
        BundleRequest bundle = new BundleRequest()
        {
            Name = "Kitchen regular cleaning",
            Type = CleaningType.Regular,
            Price = 1000,
            Measure = Measure.Room,
            ServicesIds = new List<int>()
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(bundle, new ValidationContext(bundle), validationResults, true);

        //then
        Assert.IsTrue(isValid);
        Assert.That(validationResults.Count(), Is.EqualTo(0));
    }
}