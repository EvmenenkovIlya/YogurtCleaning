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
    public RoomType RoomType { get; set; }
    public decimal Price { get; set; }
    public decimal PriceForCleaningObject { get; set; }
    public Measure Measure { get; set; }
    public List<Service> Services { get; set; }
    public decimal Duration { get; set; }
    public decimal DurationForCleaningObject { get; set; }
    public bool IsDeleted { get; set; }

    public void SetDurationForCleaningObject(CleaningObject cleaningObject)
    {
        DurationForCleaningObject = Measure switch
        {
            Measure.Room => Duration / 2 * (cleaningObject.NumberOfRooms - 1),
            Measure.Apartment => Duration,
            Measure.SquareMeter => Duration * cleaningObject.Square,
            Measure.Unit => Duration * cleaningObject.NumberOfWindows + Duration * cleaningObject.NumberOfBalconies,
            _ => 0,
        };
    }

    public void SetPriceForCleaningObject(CleaningObject cleaningObject)
    {
        PriceForCleaningObject = Measure switch
        {
            Measure.Room => Price / 2 * (cleaningObject.NumberOfRooms - 1),
            Measure.Apartment => Price,
            Measure.SquareMeter => Price * cleaningObject.Square,
            Measure.Unit => Price * cleaningObject.NumberOfWindows + Price * cleaningObject.NumberOfBalconies,
            _ => 0,
        };
    }
}
