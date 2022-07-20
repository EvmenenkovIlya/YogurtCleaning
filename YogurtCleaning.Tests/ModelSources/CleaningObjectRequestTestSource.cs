using System.Collections;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ModelSources;

    public class CleaningObjectRequestTestSource : IEnumerable
    {
        public CleaningObjectRequest GetCleaningObjectRequestModel()
        {

            return new CleaningObjectRequest()
            {
                ClientId = 42,
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 2,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10"
            };
        }
        public IEnumerator GetEnumerator()
        {
            CleaningObjectRequest model = new CleaningObjectRequest()
            {
                
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 4,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10"
            };
        yield return new object[]
        {
            model,
            ApiErrorMessages.ClientIdIsPositiveNumber
        };

        model = GetCleaningObjectRequestModel();
        model.NumberOfRooms = -1;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NumberOfRoomsIsPositiveNumber
        };

        model = GetCleaningObjectRequestModel();
        model.NumberOfBathrooms = -1;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NumberOfBathroomsIsPositiveNumber
        };

        model = GetCleaningObjectRequestModel();
        model.Square = -1;
        yield return new object[]
        {
            model,
            ApiErrorMessages.SquareIdIsPositiveNumber
        };

        model = GetCleaningObjectRequestModel();
        model.NumberOfWindows = -1;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NumberOfWindowsIdIsPositiveNumber
        };

        model = GetCleaningObjectRequestModel();
        model.NumberOfBalconies = -1;
        yield return new object[]
        {
            model,
            ApiErrorMessages.NumberOfBalconiesIdIsPositiveNumber
        };

        model = GetCleaningObjectRequestModel();
        model.Address = null;
        yield return new object[]
        {
            model,
            ApiErrorMessages.AddressIsRequired
        };

        model = GetCleaningObjectRequestModel();
        model.Address = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor." +
                " Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus." +
                " Donec quam felis, ultricies nec, pellentesque eu, pretium quis, s";
        yield return new object[]
        {
                model,
                ApiErrorMessages.AddressMaxLength
        };
    }
}

