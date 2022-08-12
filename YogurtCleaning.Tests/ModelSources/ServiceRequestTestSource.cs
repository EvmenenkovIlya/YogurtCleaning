using System.Collections;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

public class ServiceRequestTestSource : IEnumerable
{
    public ServiceRequest GetServiceRequestModel()
    {
        
        return new ServiceRequest()
        {
            Name = "This is some service name",
            RoomType = RoomType.Kitchen,
            Price = 500,
            Unit = "Hour"
        };
    }

    public IEnumerator GetEnumerator()
    {
        ServiceRequest model = GetServiceRequestModel();
        model.Name = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameIsRequired
        };

        model = GetServiceRequestModel();
        model.Price = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PriceIsRequired
        };

        model = GetServiceRequestModel();
        model.Unit = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.MeasureIsRequired
        };

        model = GetServiceRequestModel();
        model.Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameMaxLength
        };
    }
}