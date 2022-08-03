using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Controllers;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;

public class CommentModelRequestTests
{
    [TestCaseSource(typeof(CommentsControllerTestSource))]
    public async Task CommentRequestValidation_WhenInvalidModelPassed_ValidationErrorReceived(CommentRequest comment, string errorMessage)
    {
        //given
        var validationResults = new List<ValidationResult>();
        
        //when
        var isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);
        
        //then
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.That(actualMessage, Is.EqualTo(errorMessage));
    }

    [Test]
    public async Task CommentRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived()
    {
        //given
        CommentRequest comment = new CommentRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.OrderIdIsRequred
        };
        var validationResults = new List<ValidationResult>();
        
        //when
        var isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);
        
        //then
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.That(actualMessage, Is.EqualTo(expectedMessages[i]));
        }
    }

    [Test]
    public async Task CommentRequestValidation_WhenValidModelPassed_NoErrorsReceived()
    {
        //given
        CommentRequest comment = new CommentRequest()
        {
            Summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tempus suscipit tempus.",
            OrderId = 1,
            Rating = 5
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);

        //then
        Assert.IsTrue(isValid);
        Assert.That(validationResults.Count(), Is.EqualTo(0));
    }
}