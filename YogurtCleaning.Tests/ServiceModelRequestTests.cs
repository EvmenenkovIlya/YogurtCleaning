using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;

public class ServiceModelsRequestTests
{
    [TestCaseSource(typeof(ServiceRequestTestSource))]
    public async Task ServiceRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived(ServiceRequest service, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();  
        
        //when
        var isValid = Validator.TryValidateObject(service, new ValidationContext(service), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }

    [Test]
    public async Task ServiceRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived()
    {
        //given
        ServiceRequest service = new ServiceRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.NameIsRequired, 
            ApiErrorMessages.PriceIsRequired,
            ApiErrorMessages.MeasureIsRequired
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(service, new ValidationContext(service), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.AreEqual(expectedMessages[i], actualMessage);
        }
    }

    [Test]
    public async Task ServiceRequestValidation_WhenValidModelPassed_NoErrorsReceived()
    {
        //given
        ServiceRequest service = new ServiceRequest()
        {
            Name = "This is some service name",
            Price = 500,
            Unit = "Room"
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(service, new ValidationContext(service), validationResults, true);

        //then
        Assert.IsTrue(isValid);
        Assert.AreEqual(0, validationResults.Count());   
    }
}