using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;

public class ClientModelsRequestTests
{
    [TestCaseSource(typeof(ClientRegisterRequestTestSource))]
    public async Task ClientRegisterRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived(ClientRegisterRequest client, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();  
        
        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }

    [TestCaseSource(typeof(ClientUpdateRequestTestSource))]
    public async Task ClientUpdateRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived(ClientUpdateRequest client, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);

        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }

    [TestCase]
    public async Task ClientRegisterRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived()
    {
        //given
        ClientRegisterRequest client = new ClientRegisterRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.NameIsRequired, 
            ApiErrorMessages.LastNameIsRequired,
            ApiErrorMessages.BirthDateIsRequired,
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
            Assert.AreEqual(expectedMessages[i], actualMessage);
        }
    }

    [TestCase]
    public async Task ClientUpdateRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived()
    {
        //given
        ClientUpdateRequest client = new ClientUpdateRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.NameIsRequired,
            ApiErrorMessages.LastNameIsRequired,
            ApiErrorMessages.BirthDateIsRequired,           
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
            Assert.AreEqual(expectedMessages[i], actualMessage);
        }
    }

    [TestCase]
    public async Task ClientRegisterRequestValidation_WhenValidModelPassed_NoErrorsReceived()
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
        Assert.AreEqual(0, validationResults.Count());
    }

    [TestCase]
    public async Task ClientUpdateRequestValidation_WhenValidModelPassed_NoErrorsReceived()
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
        Assert.AreEqual(0, validationResults.Count());   
    }
}