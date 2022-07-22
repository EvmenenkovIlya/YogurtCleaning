using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface ILoginService
    {
        Cleaner GetCleanerByLoginData(LoginData loginData);
        Client GetClientByLoginData(LoginData loginData);
    }
}