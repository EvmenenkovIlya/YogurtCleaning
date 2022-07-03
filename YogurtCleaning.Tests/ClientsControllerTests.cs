using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Controllers;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ControllerSources;

namespace YogurtCleaning.Tests;

public class Tests
{
    private readonly ILogger<ClientsController> _logger;
    private ClientsController _clientsController;

    [SetUp]
    public void Setup()
    {
        _clientsController = new ClientsController(_logger);
    }

    [TestCaseSource(typeof(ClientsControllerTestSource))]
    public async Task ClientValidation(ClientRegisterRequest client, string errorMessage)
    {
        var validationResults = new List<ValidationResult>();       
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults);
        Assert.IsFalse(isValid);
        var actualMessage = validationResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }
}