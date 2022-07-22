using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public string Login([FromBody] UserLoginRequest request)
    {
        var user = _authService.GetUserForLogin(request.Email, request.Password);

        return _authService.GetToken(user);
    }
}
