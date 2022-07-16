using Microsoft.AspNetCore.Mvc;

namespace YogurtCleaning.Extensions;

public static class ControllerExtensions
{
    public static string GetRequestFullPath(this ControllerBase controller) =>
        $"{controller.Request?.Scheme}://{controller.Request?.Host.Value}{controller.Request?.Path.Value}";

    public static List<string> GetClaimsValue(this ControllerBase controller)
    {
        List<string> claimValues = new List<string>();
        if (controller.User != null)
        {
            var Claims = controller.User.Claims.ToList();
            foreach (var claim in Claims)
            {
                var value = claim.Value;
                claimValues.Add(value);
            }
        }
        return claimValues;
    }
}