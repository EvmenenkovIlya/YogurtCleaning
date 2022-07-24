using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface IAuthService
    {
        UserValues GetUserForLogin(string email, string password);

        string GetToken(UserValues model);
    }
}