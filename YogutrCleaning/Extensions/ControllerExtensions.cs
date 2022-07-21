using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business;

namespace YogurtCleaning.Extensions;

public static class ControllerExtensions
{
    public static string GetRequestFullPath(this ControllerBase controller) =>
        $"{controller.Request?.Scheme}://{controller.Request?.Host.Value}{controller.Request?.Path.Value}";

    public static UserValues GetClaimsValue(this ControllerBase controller)
    {
        UserValues userValues = new UserValues();
        if (controller.User != null)
        {
            var Claims = controller.User.Claims.ToList();
            userValues.Email = Claims[0].Value;
            userValues.Role = Claims[1].Value;
            userValues.Id = Convert.ToInt32(Claims[2].Value);
        }
        return userValues;
    }
}