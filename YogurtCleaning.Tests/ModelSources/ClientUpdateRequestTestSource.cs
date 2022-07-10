using System.Collections;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

public class ClientUpdateRequestTestSource : IEnumerable
{
    public ClientUpdateRequest GetClientUpdateRequestModel()
    {
        return new ClientUpdateRequest()
        {
            Name = "Adam",
            LastName = "Smith",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };
    }
    
    public IEnumerator GetEnumerator()
    {
        ClientUpdateRequest model = GetClientUpdateRequestModel();
        model.Name = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameIsRequired
        };

        model = GetClientUpdateRequestModel();
        model.LastName = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.LastNameIsRequired
        };

        model = GetClientUpdateRequestModel();
        model.Phone = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PhoneIsRequired
        };

        model = GetClientUpdateRequestModel();
        model.BirthDate = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.BirthDateIsRequired
        };

        model = GetClientUpdateRequestModel();
        model.Name = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameMaxLength
        };

        model = GetClientUpdateRequestModel();
        model.LastName = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.LastNameMaxLength
        };

        model = GetClientUpdateRequestModel();
        model.Phone = "+123456789012345";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PhoneMaxLength
        };
    }
}