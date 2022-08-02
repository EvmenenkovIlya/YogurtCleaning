using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Business.Models;

public class BundleBusinessModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CleaningType Type { get; set; }
    public decimal Price { get; set; }
    public decimal PriceForCleaningObject { get; set; }
    public Measure Measure { get; set; }
    public List<Service> Services { get; set; }
    public decimal Duration { get; set; }
    public decimal DurationForCleaningObject { get; set; }
    public bool IsDeleted { get; set; }
    public List<Order> Orders { get; set; }

    public decimal GetDurationForCleaningObject(CleaningObject cleaningObject)
    {
        switch (Measure)
        {
            case Measure.Room:
                DurationForCleaningObject += Duration / 2 * (cleaningObject.NumberOfRooms - 1);
                return DurationForCleaningObject;
            case Measure.Apartment:
                DurationForCleaningObject = Duration;
                return DurationForCleaningObject;
            case Measure.SquareMeter:
                DurationForCleaningObject = Duration * cleaningObject.Square;
                return Duration;
            case Measure.Unit:
                DurationForCleaningObject = Duration * cleaningObject.NumberOfWindows + Duration * cleaningObject.NumberOfBalconies;
                return DurationForCleaningObject;
        }
        return DurationForCleaningObject;
    }

    public decimal GetPriceForCleaningObject(CleaningObject cleaningObject)
    {
        switch (Measure)
        {
            case Measure.Room:
                PriceForCleaningObject += Price / 2 * (cleaningObject.NumberOfRooms - 1);
                return PriceForCleaningObject;
            case Measure.Apartment:
                PriceForCleaningObject = Price;
                return PriceForCleaningObject;
            case Measure.SquareMeter:
                PriceForCleaningObject = Price * cleaningObject.Square;
                return PriceForCleaningObject;
            case Measure.Unit:
                PriceForCleaningObject = Price * cleaningObject.NumberOfWindows + Price * cleaningObject.NumberOfBalconies;
                return PriceForCleaningObject;
        }
        return PriceForCleaningObject;
    }
}
