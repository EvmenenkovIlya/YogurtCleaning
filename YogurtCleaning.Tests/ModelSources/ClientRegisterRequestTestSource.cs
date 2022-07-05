using System.Collections;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

public class ClientRegisterRequestTestSource : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                LastName = "Smith",
                Password = "12345678",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.NameIsRequired
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                Password = "12345678",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.LastNameIsRequired
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.PasswordIsRequired
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.ConfirmPasswordIsRequired
        }; 
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "12345678",
                ConfirmPassword = "12345678",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.EmailIsRequired
        }; 
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "12345678",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.PhoneIsRequired
        }; 
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "12345678",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = null
            },
            ApiErrorMessages.BirthDateIsRequired
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "This String has more than fifty chars. i promise123451",
                LastName = "Smith",
                Password = "12345678",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.NameMaxLength
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "This String has more than fifty chars. i promise123451",
                Password = "12345678",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.LastNameMaxLength
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "12345",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.PasswordMinLength
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "This String has more than fifty chars. i promise123451",
                ConfirmPassword = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.PasswordMaxLength
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "12345678",
                ConfirmPassword = "This String has more than fifty chars. i promise123451",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.ConfirmPasswordMaxLength
        }; 
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "12345678",
                ConfirmPassword = "1234567",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.ConfirmPasswordMinLength
        };
        yield return new object[]
        {
            new ClientRegisterRequest()
            {
                Name = "Adam",
                LastName = "Smith",
                Password = "12345678",
                ConfirmPassword = "12345678",
                Email = "NotEmail",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            ApiErrorMessages.EmailNotValid
        };
    }
}
