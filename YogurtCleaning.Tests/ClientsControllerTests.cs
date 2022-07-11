using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Controllers;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Tests.ControllerSource;
using YogurtCleaning.Tests.ModelSources;

namespace YogurtCleaning.Tests;
public class ClientsControllerTests
{
    private ClientsController _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new ClientsController();
    }

    [TestCaseSource(typeof(ClientsControllerValidRequestTestSource))]
    public async Task AddClientWhenValidRequestPassedCreatedResultResived(ClientRegisterRequest client, int expectedId)
    {
        //given

        //when
        var actual = _sut.AddClient(client);

        //then
        var actualResult = actual.Result as CreatedResult;
        Assert.AreEqual(expectedId, actualResult.Value);
        Assert.AreEqual(StatusCodes.Status201Created, actualResult.StatusCode);
    }

    [TestCaseSource(typeof(ClientsControllerInvalidRequestTestSource))]
    public async Task AddClientWhenValidRequesPassedCreatedResultResived(ClientRegisterRequest client, int expetedId)
    {
        //given

        //when
        var actual = _sut.AddClient(client);

        //then
        var actualResult = actual.Result as CreatedResult;
        //Assert.AreEqual(expetedId, actualResult.Value);
        Assert.AreEqual(StatusCodes.Status422UnprocessableEntity, actualResult.StatusCode);
    }

    [TestCase]
    public async Task GetClientWhenValidRequestPassedCreatedResultResived()
    {
        //given
        ClientResponse expectedClient = new ClientResponse
        {
            Id = 5,
            Name = "Adam",
            LastName = "Smith",
            Phone = "+79994524757",
            BirthDate = DateTime.Now,
            Email = "AdamSmith@gmail.com",
            RegistrationDate = DateTime.Now
        };
        int id = 5;

        //when
        var actual = _sut.GetClient(id);

        //then
        var actualResult = actual.Result as CreatedResult;
        Assert.AreEqual(expectedClient, actualResult.Value);
        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
    }

    [TestCase]
    public async Task GetAllClientsReturnedOkResult()
    {
        //given
        List<ClientResponse> expectedClients = new List<ClientResponse>();

        //when
        var actual = _sut.GetAllClients();

        //then
        var actualResult = actual.Result as CreatedResult;
        Assert.AreEqual(expectedClients, actualResult.Value);
        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
    }
}

