using System.Collections;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

public class ClientRegisterRequestTestSource : IEnumerable
{
    public ClientRegisterRequest GetClientRegisterRequestModel()
    {
        
        return new ClientRegisterRequest()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            ConfirmPassword = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };
    }

    public IEnumerator GetEnumerator()
    {
        ClientRegisterRequest model = GetClientRegisterRequestModel();
        model.FirstName = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameIsRequired
        };

        model = GetClientRegisterRequestModel();
        model.LastName = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.LastNameIsRequired
        };

        model = GetClientRegisterRequestModel();
        model.Password = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PasswordIsRequired
        };

        model = GetClientRegisterRequestModel();
        model.ConfirmPassword = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.ConfirmPasswordIsRequired
        };

        model = GetClientRegisterRequestModel();
        model.Email = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.EmailIsRequired
        };

        model = GetClientRegisterRequestModel();
        model.Phone = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PhoneIsRequired
        };

        model = GetClientRegisterRequestModel();
        model.BirthDate = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.BirthDateIsRequired
        };

        model = GetClientRegisterRequestModel();
        model.FirstName = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameMaxLength
        };

        model = GetClientRegisterRequestModel();
        model.LastName = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.LastNameMaxLength
        };

        model = GetClientRegisterRequestModel();
        model.Password = "1234567";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PasswordMinLength
        };

        model = GetClientRegisterRequestModel();
        model.Password = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PasswordMaxLength
        };

        model = GetClientRegisterRequestModel();
        model.ConfirmPassword = "1234567";
        yield return new object[]
        {
            model,
            ApiErrorMessages.ConfirmPasswordMinLength
        };

        model = GetClientRegisterRequestModel();
        model.ConfirmPassword = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.ConfirmPasswordMaxLength
        };

        model = GetClientRegisterRequestModel();
        model.Email = "NotEmail";
        yield return new object[]
        {
            model,
            ApiErrorMessages.EmailNotValid
        };

        model = GetClientRegisterRequestModel();
        model.Email = $"This string is more than 255 chars. qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq@qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq.qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq.";
        yield return new object[]
        {
            model,
            ApiErrorMessages.EmailMaxLength
        };
        
        model = GetClientRegisterRequestModel();
        model.Phone = "+123456789012345";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PhoneMaxLength
        };
    }
}