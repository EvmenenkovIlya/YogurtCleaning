using System.Collections;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

public class CleanerUpdateRequestTestSource : IEnumerable
{
    public CleanerUpdateRequest GetCleanerUpdateRequestModel()
    {       
        return new CleanerUpdateRequest()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            ServicesIds = new List<int>() { 1, 2 },
            Districts = new List<DistrictEnum>() {DistrictEnum.Vasileostrovskiy, DistrictEnum.Primorsky}
        };
    }

    public IEnumerator GetEnumerator()
    {
        CleanerUpdateRequest model = GetCleanerUpdateRequestModel();
        model.FirstName = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameIsRequired
        };

        model = GetCleanerUpdateRequestModel();
        model.LastName = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.LastNameIsRequired
        };

        model = GetCleanerUpdateRequestModel();
        model.Phone = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PhoneIsRequired
        };

        model = GetCleanerUpdateRequestModel();
        model.FirstName = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameMaxLength
        };

        model = GetCleanerUpdateRequestModel();
        model.LastName = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.LastNameMaxLength
        };

        model = GetCleanerUpdateRequestModel();
        model.Phone = "+123456789012345";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PhoneMaxLength
        };
    }
}