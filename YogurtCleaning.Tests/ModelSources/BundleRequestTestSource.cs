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
            Type = CleaningType.Regular,
            Price = 1000,
            Measure = Measure.Room,
            ServicesIds = new List<int>()
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
        model.ServicesIds = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.ServicesIsRequired
        };
    }
}