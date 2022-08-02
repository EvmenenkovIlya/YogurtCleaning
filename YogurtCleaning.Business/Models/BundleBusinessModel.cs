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
    public Measure Measure { get; set; }
    public List<Service> Services { get; set; }
    public decimal Duration { get; set; }
    public bool IsDeleted { get; set; }
    public List<Order> Orders { get; set; }

    public decimal GetDuration (CleaningObject cleaningObject)
    {
        switch (Measure)
        {
            case Measure.Room:
                Duration += Duration / 2 * (cleaningObject.NumberOfRooms - 1);
                return Duration;
            case Measure.Apartment:
                return Duration;
            case Measure.SquareMeter:
                Duration = Duration * cleaningObject.Square;
                return Duration;
            case Measure.Unit:
                Duration = Duration * cleaningObject.NumberOfWindows + Duration * cleaningObject.NumberOfBalconies;
                return Duration;
        }
        return Duration;
    }

    public decimal GetPrice (CleaningObject cleaningObject)
    {
        switch (Measure)
        {
            case Measure.Room:
                Price += Price / 2 * (cleaningObject.NumberOfRooms - 1);
                return Price;
            case Measure.Apartment:
                return Price;
            case Measure.SquareMeter:
                Price = Price * cleaningObject.Square;
                return Price;
            case Measure.Unit:
                Price = Price * cleaningObject.NumberOfWindows + Price * cleaningObject.NumberOfBalconies;
                return Price;
        }
        return Price;
    }
}
