using System.Collections;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

public class BundleRequestTestSource : IEnumerable
{
    public BundleRequest GetBundleRequestModel()
    {

        return new BundleRequest()
        {
            Name = "Kitchen regular cleaning",
            Price = 1000,
            Measure = Measure.Room,
            Services = new List<ServiceResponse>()
        };
    }

    public IEnumerator GetEnumerator()
    {
        BundleRequest model = GetBundleRequestModel();
        model.Name = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameIsRequired
        };

        model = GetBundleRequestModel();
        model.Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        yield return new object[]
        {
            model,
            ApiErrorMessages.BundleNameMaxLenght
        };

        model = GetBundleRequestModel();
        model.Price = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PriceIsRequired
        };

        model = GetBundleRequestModel();
        model.Measure = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.MeasureIsRequired
        };

        model = GetBundleRequestModel();
        model.Services = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.ServicesIsRequired
        };
    }
}