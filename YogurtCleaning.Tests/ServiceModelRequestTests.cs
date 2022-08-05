using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;

public class ServiceModelsRequestTests
{
    [TestCaseSource(typeof(ServiceRequestTestSource))]
    public void ServiceRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived(ServiceRequest service, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();  
        
        //when
        var isValid = Validator.TryValidateObject(service, new ValidationContext(service), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.That(actualMessage, Is.EqualTo(errorMessage));
    }

    [Test]
    public void ServiceRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived()
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
            Assert.That(actualMessage, Is.EqualTo(expectedMessages[i]));
        }
    }

    [Test]
    public void ServiceRequestValidation_WhenValidModelPassed_NoErrorsReceived()
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
        Assert.That(validationResults.Count(), Is.EqualTo(0));   
    }
}