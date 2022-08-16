using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Business.Tests;

public class BundleBusinessModelTests
{


    [Fact]
    public void SetDurationForCleaningObject_WhenModelsGet_ThenDurationRecieved()
    {
        // given
        var bundle = new BundleBusinessModel()
        {
            Name = "Kitchen regular cleaning",
            Type = CleaningType.Regular,
            Price = 1000,
            Measure = Measure.Apartment,
            Services = new List<Service>()
        };

        var cleaningObject = new CleaningObject()
        {
            Client = new Client { Id = 42 },
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 2,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10"
        };

        var expectedDuration = bundle.Duration;

        // when
        bundle.SetDurationForCleaningObject(cleaningObject);
        var actual = bundle.DurationForCleaningObject;


        // then
        Assert.True(expectedDuration == actual);
    }

    [Fact]
    public void SetPriceForCleaningObject_WhenModelsGet_ThenDurationRecieved()
    {
        // given
        var bundle = new BundleBusinessModel()
        {
            Name = "Windows and balconies regular cleaning",
            Type = CleaningType.Windows,
            Price = 1000,
            Measure = Measure.Unit,
            Services = new List<Service>()
        };

        var cleaningObject = new CleaningObject()
        {
            Client = new Client { Id = 42 },
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 1,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10"
        };

        var expectedPrice = bundle.Price * 2;

        // when
        bundle.SetPriceForCleaningObject(cleaningObject);
        var actual = bundle.PriceForCleaningObject;


        // then
        Assert.True(expectedPrice == actual);
    }
}
