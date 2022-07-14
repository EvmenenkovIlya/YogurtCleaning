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
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10"
            };
        }
        public IEnumerator GetEnumerator()
        {
            CleaningObjectRequest model = GetCleaningObjectRequestModel();
            model.ClientId = null;
            yield return new object[]
            {
                model,
                ApiErrorMessages.ClientIdIsRequred
            };

            model = GetCleaningObjectRequestModel();
            model.NumberOfRooms = null;
            yield return new object[]
            {
                model,
                ApiErrorMessages.NumberOfRoomsIsRequred
            };
            
            model = GetCleaningObjectRequestModel();
            model.NumberOfBathrooms = null;
            yield return new object[]
            {
                model,
                ApiErrorMessages.NumberOfBathroomsIsRequred
            };
            
            model = GetCleaningObjectRequestModel();
            model.Square = null;
            yield return new object[]
            {
                model,
                ApiErrorMessages.SquareIsRequred
            };
            
            model = GetCleaningObjectRequestModel();
            model.NumberOfWindows = null;
            yield return new object[]
            {
                model,
                ApiErrorMessages.NumberOfWindowsIsRequred
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

