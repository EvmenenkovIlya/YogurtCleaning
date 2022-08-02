using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Business.Exceptions;

namespace YogurtCleaning.Business.Tests;

public class AuthServicesTests
{
    private AuthService _sut;
    private Mock<IClientsRepository> _clientsRepositoryMock;
    private Mock<ICleanersRepository> _cleanersRepository;
    private Mock<IAdminsRepository> _adminRepository;

    public AuthServicesTests()
    {
        _clientsRepositoryMock = new Mock<IClientsRepository>();
        _cleanersRepository = new Mock<ICleanersRepository>();
        _adminRepository = new Mock<IAdminsRepository>();
        _sut = new AuthService(_clientsRepositoryMock.Object, _cleanersRepository.Object, _adminRepository.Object);
    }

    [Fact]
    public void Login_ValidAdminEmailAndPassword_ClaimModelReturned()
    {
        //given
        var adminPassword = "12345678954";
        var adminExpected = new Admin()
        {
            Password = PasswordHash.HashPassword("12345678954"),
            Email = "J@gmail.com",
            IsDeleted = false,
        };

        _adminRepository.Setup(m => m.GetAdminByEmail(adminExpected.Email)).Returns(adminExpected);
        //when

        var claim = _sut.GetUserForLogin(adminExpected.Email, adminPassword);
        //then

        Assert.Equal(Role.Admin, claim.Role);
        Assert.Equal(claim.Email, adminExpected.Email);
        _adminRepository.Verify(c => c.GetAdminByEmail(It.IsAny<string>()), Times.Once);
        _cleanersRepository.Verify(c => c.GetCleanerByEmail(It.IsAny<string>()), Times.Never);
        _clientsRepositoryMock.Verify(c => c.GetClientByEmail(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Login_ValidClientEmailAndPassword_ClaimModelReturned()
    {
        //given
        var clientPassword = "12345678";
        var clientExpected = new Client()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = PasswordHash.HashPassword("12345678"),
            Email = "AdamSmith@gmail.com568",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
        };

        _clientsRepositoryMock.Setup(c => c.GetClientByEmail(clientExpected.Email)).Returns(clientExpected);

        //when
        var claim = _sut.GetUserForLogin(clientExpected.Email, clientPassword);

        //then
        Assert.Equal(Role.Client, claim.Role);
        Assert.Equal(claim.Email, clientExpected.Email);
        _clientsRepositoryMock.Verify(c => c.GetClientByEmail(It.IsAny<string>()), Times.Once);
        _adminRepository.Verify(c => c.GetAdminByEmail(It.IsAny<string>()), Times.Once);
        _cleanersRepository.Verify(c => c.GetCleanerByEmail(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Login_ValidCleanersEmailAndPassword_ClaimModelReturned()
    {
        //given
        var passwordCleanersExpected = "12334534";
        var cleanersExpected = new Cleaner()
        {
            FirstName = "Dantes",
            LastName = "Don",
            Email = "ros@fja.com",
            Password = PasswordHash.HashPassword("12334534")
        };

        _cleanersRepository.Setup(c => c.GetCleanerByEmail(cleanersExpected.Email)).Returns(cleanersExpected);

        //when
        var claim = _sut.GetUserForLogin(cleanersExpected.Email, passwordCleanersExpected);

        //then
        Assert.Equal(Role.Cleaner, claim.Role);
        Assert.Equal(claim.Email, cleanersExpected.Email);
        _clientsRepositoryMock.Verify(c => c.GetClientByEmail(It.IsAny<string>()), Times.Once);
        _adminRepository.Verify(c => c.GetAdminByEmail(It.IsAny<string>()), Times.Once);
        _cleanersRepository.Verify(c => c.GetCleanerByEmail(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Login_BadPassword_ThrowEntityNotFoundException()
    {
        //given
        var badPassword = "123456789541";
        var cleanersExpected = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = PasswordHash.HashPassword("12334534"),
            Email = "AdamSmith@gmail.com568",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
        };

        _cleanersRepository.Setup(c => c.GetCleanerByEmail(cleanersExpected.Email)).Returns(cleanersExpected);

        //when, then
        Assert.Throws<EntityNotFoundException>(() => _sut.GetUserForLogin(cleanersExpected.Email, badPassword));

    }

    [Fact]
    public void Login_IsDeletedTrue_ThrowEntityNotFoundException()
    {
        //given
        var password = "12334534";
        var clientExpected = new Client()
        {
            FirstName = "Dantes",
            LastName = "Don",
            Email = "ros@fja.com",
            Password = PasswordHash.HashPassword("12334534"),
            IsDeleted = true
        };

        _clientsRepositoryMock.Setup(c => c.GetClientByEmail(clientExpected.Email)).Returns(clientExpected);

        //when, then
        Assert.Throws<EntityNotFoundException>(() => _sut.GetUserForLogin(clientExpected.Email, password));

    }

    [Fact]
    public void Login_UserNotFound_ThrowEntityNotFoundException()
    {
        //given
        var badEmail = "ad@mmm.com";
        var clientExpected = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com568",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
        };

        //when, then
        Assert.Throws<EntityNotFoundException>(() => _sut.GetUserForLogin(badEmail, clientExpected.Password));
    }

    [Fact]
    public void GetToken_ValidData_TokenReturned()
    {
        //given
        var model = new UserValues()
        {
            Email = "ada@gmail.com",
            Role = Role.Client
        };

        //when
        var actual = _sut.GetToken(model);

        //then

        Assert.NotNull(actual);
    }

    [Fact]
    public void GetToken_EmailEmpty_ThrowDataException()
    {
        //given
        var model = new UserValues()
        {
            Email = null,
            Role = Role.Client
        };

        //when, then
        Assert.Throws<DataException>(() => _sut.GetToken(model));
    }

    [Fact]
    public void GetToken_PropertysEmpty_ThrowDataException()
    {
        //given
        var model = new UserValues();

        //when, then
        Assert.Throws<DataException>(() => _sut.GetToken(model));
    }
}
