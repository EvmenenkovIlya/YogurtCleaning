using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class LoginService : ILoginService
{
    private readonly IClientsRepository _clientsRepository;
    private readonly ICleanersRepository _cleanersRepository;

    public LoginService(IClientsRepository clientsRepository, ICleanersRepository cleanersRepository)
    {
        _clientsRepository = clientsRepository;
        _cleanersRepository = cleanersRepository;
    }

    public Client GetClientByLoginData(LoginData loginData)
    {
        return _clientsRepository.GetClientByLogin(loginData);
    }

    public Cleaner GetCleanerByLoginData(LoginData loginData)
    {
        return _cleanersRepository.GetCleanerByLogin(loginData);
    }

}