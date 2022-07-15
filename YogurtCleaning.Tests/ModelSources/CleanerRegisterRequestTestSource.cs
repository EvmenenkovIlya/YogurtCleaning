using System.Collections;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

public class CleanerRegisterRequestTestSource : IEnumerable
{
    public CleanerRegisterRequest GetCleanerRegisterRequestModel()
    {

        return new CleanerRegisterRequest()
        {
            Name = "Adam",
            LastName = "Smith",
            Password = "12345678",
            ConfirmPassword = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            Passport =  "0000123456",
            Schedule = Schedule.FullTime,
            BirthDate = DateTime.Today,
            ServicesIds = new List<int>() { 1, 2 },
            Districts = new List<DistrictEnum>() { DistrictEnum.Vasileostrovskiy, DistrictEnum.Primorsky }
        };
    }

    public IEnumerator GetEnumerator()
    {
        CleanerRegisterRequest model = GetCleanerRegisterRequestModel();
        model.Name = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameIsRequired
        };

        model = GetCleanerRegisterRequestModel();
        model.LastName = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.LastNameIsRequired
        };

        model = GetCleanerRegisterRequestModel();
        model.Password = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PasswordIsRequired
        };

        model = GetCleanerRegisterRequestModel();
        model.ConfirmPassword = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.ConfirmPasswordIsRequired
        };

        model = GetCleanerRegisterRequestModel();
        model.Email = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.EmailIsRequired
        };

        model = GetCleanerRegisterRequestModel();
        model.Phone = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PhoneIsRequired
        };

        model = GetCleanerRegisterRequestModel();
        model.Name = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.NameMaxLength
        };

        model = GetCleanerRegisterRequestModel();
        model.LastName = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.LastNameMaxLength
        };

        model = GetCleanerRegisterRequestModel();
        model.Password = "1234567";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PasswordMinLength
        };

        model = GetCleanerRegisterRequestModel();
        model.Password = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PasswordMaxLength
        };

        model = GetCleanerRegisterRequestModel();
        model.ConfirmPassword = "1234567";
        yield return new object[]
        {
            model,
            ApiErrorMessages.ConfirmPasswordMinLength
        };

        model = GetCleanerRegisterRequestModel();
        model.ConfirmPassword = "This String has more than fifty chars. i promise123451";
        yield return new object[]
        {
            model,
            ApiErrorMessages.ConfirmPasswordMaxLength
        };

        model = GetCleanerRegisterRequestModel();
        model.Email = "NotEmail";
        yield return new object[]
        {
            model,
            ApiErrorMessages.EmailNotValid
        };

        model = GetCleanerRegisterRequestModel();
        model.Email = $"This string is more than 255 chars. qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq@qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq.qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq.";
        yield return new object[]
        {
            model,
            ApiErrorMessages.EmailMaxLength
        };

        model = GetCleanerRegisterRequestModel();
        model.Phone = "+123456789012345";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PhoneMaxLength
        };

        model = GetCleanerRegisterRequestModel();
        model.Passport = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.PassportIsRequired
        };

        model = GetCleanerRegisterRequestModel();
        model.Passport = "123456789";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PassportLength
        };

        model = GetCleanerRegisterRequestModel();
        model.Passport = "12345678901";
        yield return new object[]
        {
            model,
            ApiErrorMessages.PassportLength
        };
    }

}