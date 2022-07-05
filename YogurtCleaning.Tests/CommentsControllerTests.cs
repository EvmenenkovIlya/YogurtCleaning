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
    public async Task CommentRequestValidation(CommentRequest comment, string errorMessage)
    {
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }

    [TestCase]
    public async Task CommentRequestValidationByAllValue()
    {
        CommentRequest comment = new CommentRequest();
        List<string> expectedMessages = new List<string>() {
            ApiErrorMessages.SummaryMaxLenght,
            ApiErrorMessages.AuthorIdIsRequred,
            ApiErrorMessages.OrderIdIsRequred,
            ApiErrorMessages.RatingIsRequred,
            ApiErrorMessages.RatingIsOutOfRange
        };
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);
        Assert.IsFalse(isValid);
        for (int i = 0; i < expectedMessages.Count(); i++)
        {
            var actualMessage = validationResults[i].ErrorMessage;
            Assert.AreEqual(expectedMessages[i], actualMessage);
        }
    }
}