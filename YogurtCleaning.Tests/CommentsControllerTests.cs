using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Controllers;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ControllerSources;

namespace YogurtCleaning.Tests;

public class CommentsControllerTests
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
        Assert.AreEqual(errorMessage, actualMessage);
    }

    [Test]
    public async Task CommentRequestValidation_WhenInvalidModelPassed_ValidationErrorsReceived()
    {
        //given
        CommentRequest comment = new CommentRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.AuthorIdIsRequred,
            ApiErrorMessages.OrderIdIsRequred,
            ApiErrorMessages.RatingIsRequred
        };
        var validationResults = new List<ValidationResult>();
        
        //when
        var isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);
        
        //then
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.AreEqual(expectedMessages[i], actualMessage);
        }
    }

    [Test]
    public async Task CommentRequestValidation_WhenValidModelPassed_NoErrorsReceived()
    {
        //given
        CommentRequest comment = new CommentRequest()
        {
            Summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tempus suscipit tempus.",
            AuthorId = 1,
            OrderId = 1,
            Rating = 5
        };
        var validationResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);

        //then
        Assert.IsTrue(isValid);
        Assert.AreEqual(0, validationResults.Count());
    }
}