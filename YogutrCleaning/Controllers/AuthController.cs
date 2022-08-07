using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IEmailSender _sender;

    public AuthController(IAuthService authService, IEmailSender sender)
    {
        _authService = authService;
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<string> Login([FromBody] UserLoginRequest request)
    {
        var user = await _authService.GetUserForLogin(request.Email, request.Password);

        return _authService.GetToken(user);
    }
}
