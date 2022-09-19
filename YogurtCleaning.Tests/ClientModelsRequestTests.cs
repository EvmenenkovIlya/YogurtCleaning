using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;
[ExcludeFromCodeCoverage]
public class ClientModelsRequestTests
{
    [TestCaseSource(typeof(ClientRegisterRequestTestSource))]
    public void ClientRegisterRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived(ClientRegisterRequest client, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();  
        
        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.That(actualMessage, Is.EqualTo(errorMessage));
    }

    [TestCaseSource(typeof(ClientUpdateRequestTestSource))]
    public void ClientUpdateRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived(ClientUpdateRequest client, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.That(actualMessage, Is.EqualTo(errorMessage));
    }

    [Test]
    public void ClientRegisterRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived()
    {
        //given
        ClientRegisterRequest client = new ClientRegisterRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.NameIsRequired, 
            ApiErrorMessages.LastNameIsRequired,
            ApiErrorMessages.PasswordIsRequired,
            ApiErrorMessages.ConfirmPasswordIsRequired,
            ApiErrorMessages.EmailIsRequired,
            ApiErrorMessages.PhoneIsRequired
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.That(actualMessage, Is.EqualTo(expectedMessages[i]));
        }
    }

    [Test]
    public void ClientUpdateRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived()
    {
        //given
        ClientUpdateRequest client = new ClientUpdateRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.NameIsRequired,
            ApiErrorMessages.LastNameIsRequired,        
            ApiErrorMessages.PhoneIsRequired
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.That(actualMessage, Is.EqualTo(expectedMessages[i]));
        }
    }

    [TestCase]
    public void ClientRegisterRequestValidation_WhenValidModelPassed_NoErrorsReceived()
    {
        //given
        ClientRegisterRequest client = new ClientRegisterRequest()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            ConfirmPassword = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);

        //then
        Assert.IsTrue(isValid);
        Assert.That(validationResults.Count(), Is.EqualTo(0));
    }

    [TestCase]
    public void ClientUpdateRequestValidation_WhenValidModelPassed_NoErrorsReceived()
    {
        //given
        ClientUpdateRequest client = new ClientUpdateRequest()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);

        //then
        Assert.IsTrue(isValid);
        Assert.That(validationResults.Count(), Is.EqualTo(0));   
    }
}