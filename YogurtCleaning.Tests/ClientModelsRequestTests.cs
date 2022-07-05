using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Controllers;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;

public class Tests
{
    [TestCaseSource(typeof(ClientRegisterRequestTestSource))]
    public async Task ClientRegisterRequestValidation(ClientRegisterRequest client, string errorMessage)
    {
        var validationResults = new List<ValidationResult>();       
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }
    [TestCaseSource(typeof(ClientUpdateRequestTestSource))]
    public async Task ClientUpdateRequestValidation(ClientUpdateRequest client, string errorMessage)
    {
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }
    [TestCase]
    public async Task ClientRegisterRequestValidationByAllValue()
    {
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
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.AreEqual(expectedMessages[i], actualMessage);
        }
    }
    [TestCase]
    public async Task ClientUpdateRequestValidationByAllValue()
    {
        ClientUpdateRequest client = new ClientUpdateRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.NameIsRequired,
            ApiErrorMessages.LastNameIsRequired,
            ApiErrorMessages.BirthDateIsRequired,           
            ApiErrorMessages.PhoneIsRequired
        };
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.AreEqual(expectedMessages[i], actualMessage);
        }
    }
}