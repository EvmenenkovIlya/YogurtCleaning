using System.Collections;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ControllerSource
{
    public class ClientsControllerInvalidRequestTestSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            //ClientRegisterRequest model = GetClientRegisterRequestModel();
            yield return new object[]
            {
            new ClientRegisterRequest()
            {
                Name = "",
                LastName = "",
                Password = "",
                ConfirmPassword = "",
                Email = "",
                Phone = "",
                BirthDate = DateTime.Today
            },
            5
            };
        }
    }
}
