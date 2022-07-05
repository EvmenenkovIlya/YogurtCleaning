using System.Collections;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

public class ClientUpdateRequestTestSource : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[]
        {
            new ClientUpdateRequest()
            {
                LastName = "Smith",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.NameIsRequired
        };
        yield return new object[]
        {
            new ClientUpdateRequest()
            {
                Name = "Adam",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.LastNameIsRequired
        };
        yield return new object[]
        {
            new ClientUpdateRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.PhoneIsRequired
        }; 
        yield return new object[]
        {
            new ClientUpdateRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Phone = "85559997264",
                BirthDate = null
            },
            ApiErrorMessages.BirthDateIsRequired
        };
        yield return new object[]
        {
            new ClientUpdateRequest()
            {
                Name = "This String has more than fifty chars. i promise123451",
                LastName = "Smith",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.NameMaxLength
        };
        yield return new object[]
        {
            new ClientUpdateRequest()
            {
                Name = "Adam",
                LastName = "This String has more than fifty chars. i promise123451",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.LastNameMaxLength
        };
    }
}