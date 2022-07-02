using Microsoft.Extensions.Logging;
using Moq;
using YogurtCleaning.Controllers;


namespace YogurtCleaning.Tests;

public class Tests
{
    private ILogger<ClientsController> logger;
    private ClientsController _clientsController;

    [SetUp]
    public void Setup()
    {
        _clientsController = new ClientsController(logger);
    }

    [Test]
    public async Task GetClient()
    {

    }
}