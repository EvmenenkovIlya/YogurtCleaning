using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ControllerSources;

public class ClientsControllerTestSource : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "",
                LastName = "ddd",
                Password = "ddddd",
                ConfirmPassword = "ddd",
                Email = "ddddd@nvk.com",
                Phone = "88977",
                BirthDate = DateTime.Now
            },
            ApiErrorMessages.NameIsRequired
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "dddd",
                LastName = "",
                Password = "ddddd",
                ConfirmPassword = "ddd",
                Email = "ddddd@nvk.com",
                Phone = "88977",
                BirthDate = DateTime.Now
            },
            ApiErrorMessages.LastNameIsRequired
        };
    }
}
