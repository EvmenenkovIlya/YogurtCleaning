using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IMapper _mapper;
    private readonly ILoginService _loginService;

    public AuthController(IMapper mapper, ILoginService loginService)
    {
        _mapper = mapper;
        _loginService = loginService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public string Login([FromBody] UserLoginRequest request)
    {
        if (request == default || request.Email == default) return string.Empty;

        var client = _loginService.GetClientByLoginData(_mapper.Map<LoginData>(request));
        var cleaner = _loginService.GetCleanerByLoginData(_mapper.Map<LoginData>(request));
        Claim roleClaim;
        Claim idClaim;
        if (request.Email == "Admin@gmail.com" && request.Password == "QWERTY123")
        {
            roleClaim = new Claim(ClaimTypes.Role, Role.Admin.ToString());
            idClaim = new Claim(ClaimTypes.NameIdentifier, 1.ToString());
        }
        else if (cleaner != null)
        {
            roleClaim = new Claim(ClaimTypes.Role, Role.Cleaner.ToString());
            idClaim = new Claim(ClaimTypes.NameIdentifier, cleaner.Id.ToString());
        }
        else if (client != null)
        {
            roleClaim = new Claim(ClaimTypes.Role, Role.Client.ToString());
            idClaim = new Claim(ClaimTypes.NameIdentifier, client.Id.ToString());
        }
        else
        {
            return string.Empty;
        }
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, request.Email), roleClaim, idClaim};
        var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)), // время действия 10 минут
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
