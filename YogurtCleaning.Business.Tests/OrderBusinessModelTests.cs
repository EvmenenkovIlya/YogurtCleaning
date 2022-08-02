using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Business.Tests;

public class OrderBusinessModelTests
{
    [Fact]
    public void GetTotalDurationTest_WhenModelGet_ThenDurationRecieved()
    {
        // given
        var order = new OrderBusinessModel
        {
            Client = new() { Id = 11 },
            CleaningObject = new() { Id = 56 },
            StartTime = DateTime.Now.AddDays(1),
            Bundles = new List<BundleBusinessModel> { new BundleBusinessModel { Id = 2, Duration = 2, Measure = Measure.Apartment } },
            Services = new List<Service> { new Service { Id = 42, Duration = 2} }
        };

        var expectedDuration = (decimal)4;

        // when
        var actualDuration = order.GetTotalDuration();

        // then
        Assert.True(expectedDuration == actualDuration);
    }

    [Fact]
    public void GetCleanersCountTest_WhenTotalDurationMoreThanEndOfWorkingDay_ThenMorethanOneRecieved()
    {
        // given
        var order = new OrderBusinessModel
        {
            Client = new() { Id = 11 },
            CleaningObject = new() { Id = 56 },
            StartTime = new DateTime(2022, 8, 1, 14, 00, 00),
            TotalDuration = 10
        };

        var expectedCount = 2;

        // when
        var actualCount = order.GetCleanersCount();

        // then
        Assert.True(expectedCount == actualCount);
    }

    [Fact]
    public void GetCleanersCountTest_WhenTotalDurationLessThanEndOfWorkingDay_ThenOneRecieved()
    {
        // given
        var order = new OrderBusinessModel
        {
            Client = new() { Id = 11 },
            CleaningObject = new() { Id = 56 },
            StartTime = new DateTime(2022, 8, 1, 14, 00, 00),
            TotalDuration = 4
        };

        var expectedCount = 1;

        // when
        var actualCount = order.GetCleanersCount();

        // then
        Assert.True(expectedCount == actualCount);
    }

    [Fact]
    public void GetEndTimeTest_When_Then()
    {
        // given
        var order = new OrderBusinessModel
        {
            StartTime = new DateTime(2022, 8, 1, 14, 00, 00),
            TotalDuration = 12,
            CleanersCount = 2
        };

        var expectedEndTime = new DateTime(2022, 8, 1, 20, 00, 00);

        // when
        var actualEndTime = order.GetEndTime();

        // then
        Assert.True(expectedEndTime == actualEndTime);
    }
}
