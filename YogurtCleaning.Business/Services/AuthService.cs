using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.Business.Infrastrucure;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class AuthService : IAuthService
{
    private readonly IClientsRepository _clientsRepository;
    private readonly ICleanersRepository _cleanersRepository;
    private readonly IAdminsRepository _adminsRepository;

    public AuthService(IClientsRepository clientsRepository, ICleanersRepository cleanersRepository, IAdminsRepository adminsRepository)
    {
        _clientsRepository = clientsRepository;
        _cleanersRepository = cleanersRepository;
        _adminsRepository = adminsRepository;
    }

    public UserValues GetUserForLogin(string email, string password)
    {
        UserValues userValues = new();

        var admin = _adminsRepository.GetAdminByEmail(email);

        if (admin is not null && email == admin.Email &&
            PasswordHash.ValidatePassword(password, admin.Password) && !admin.IsDeleted)
        {
            userValues.Email = email;
            userValues.Role = Role.Admin.ToString();
        }
        else
        {
            var client = _clientsRepository.GetClientByEmail(email);
            var cleaner = _cleanersRepository.GetCleanerByEmail(email);
            CheckUserInBase(client, cleaner);            
            dynamic user = client != null ? client : cleaner;
            if (user.IsDeleted)
            {
                throw new EntityNotFoundException("User was deleted");
            }
            ValidatePassword(password, user.Password);
            userValues.Email = user.Email;
            userValues.Role = client != null ? Role.Client.ToString() : Role.Cleaner.ToString();
            userValues.Id = user.Id;
        }
        CheckUserValuesNotNull(userValues);

        return userValues;
    }

    public string GetToken(UserValues model)
    {
        if (model is null || model.Email is null || model.Role is null)
        {
            throw new DataException("Object or part of it is empty");
        }
        Claim idClaim = new Claim(ClaimTypes.NameIdentifier, model.Id.ToString());
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Email), { new Claim(ClaimTypes.Role, model.Role) }, idClaim };

        var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }


    private void ValidatePassword(string password, string userPassword)
    {
        if (!PasswordHash.ValidatePassword(password, userPassword))
        {
            throw new EntityNotFoundException("Invalid  password");
        }
    }

    private void CheckUserValuesNotNull(UserValues userValues)
    {
        if (userValues is null)
        {
            throw new EntityNotFoundException("Invalid  password");
        }
    }

    private void CheckUserInBase(Client client, Cleaner cleaner)
    {
        if (client == null && cleaner == null)
        {
            throw new EntityNotFoundException("User not found");
        }
    }
    
}