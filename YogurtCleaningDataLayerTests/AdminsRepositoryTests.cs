using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.DataLayer.Tests;

public class AdminRepositoryTests
{
    private DbContextOptions<YogurtCleaningContext> _dbContextOptions;

    public AdminRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public void GetAdminByEmail_WhenCorrectEmail_AdminReturned()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new AdminsRepository(context);
        //given
        var expectedAdminFirst = new Admin()
        {
            Email = "Va@gmail.com",
            Password = "12345678dad"
        };

        var expectedAdminSecond = new Admin()
        {
            Email = "aaa@gmail.com",
            Password = "12345678dad"
        };

        context.Admins.Add(expectedAdminFirst);
        context.Admins.Add(expectedAdminSecond);
        context.SaveChanges();

        //when
        var actualUser = sut.GetAdminByEmail(expectedAdminSecond.Email);

        //then

        Assert.Equal(expectedAdminSecond.Password, actualUser.Password);
        Assert.Equal(expectedAdminSecond.Email, actualUser.Email);
    }
}
