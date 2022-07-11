using System.Collections;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace YogurtCleaning.Tests.ControllerSource
{
    public class ClientsControllerValidRequestTestSource : IEnumerable
    {
        public ClientRegisterRequest GetClientRegisterRequestModel()
        {

            return new ClientRegisterRequest()
            {
                Name = "Adam",
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
            //ClientRegisterRequest model = GetClientRegisterRequestModel();
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
                BirthDate = DateTime.Today
            },
            5
            };
        }
    }
}
