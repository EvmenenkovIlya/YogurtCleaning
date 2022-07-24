using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Facts;

public class CleaningObjectServiceFacts
{
    private CleaningObjectsService _sut;
    private Mock<ICleaningObjectsRepository> _cleaningObjectRepositoryMock;

    private void Setup()
    {
        _cleaningObjectRepositoryMock = new Mock<ICleaningObjectsRepository>();
        _sut = new CleaningObjectsService(_cleaningObjectRepositoryMock.Object);
    }

    [Fact]
    public void CreateCleaningObject_WhenValidRequestPassed_CleaningObjectAdded()
    {
        //given
        Setup();       
        int expectedId = 1;
        _cleaningObjectRepositoryMock.Setup(c => c.CreateCleaningObject(It.IsAny<CleaningObject>()))
             .Returns(expectedId);

        var cleaningObject = new CleaningObject()
        {
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
            IsDeleted = false
        };
        UserValues userValues = new UserValues() { Id = expectedId };

        //when
        var actual = _sut.CreateCleaningObject(cleaningObject, userValues);

        //then
        Assert.True(actual == expectedId);
        _cleaningObjectRepositoryMock.Verify(c => c.CreateCleaningObject(cleaningObject), Times.Once);
        _cleaningObjectRepositoryMock.Verify(c => c.CreateCleaningObject(It.Is<CleaningObject>(c => c.Client.Id == userValues.Id)), Times.Once);

    }

}