using Microsoft.AspNetCore.Mvc;

namespace YogurtCleaning.Extensions;

public static class ControllerExtensions
{
    public static string GetRequestFullPath(this ControllerBase controller) =>
        $"{controller.Request?.Scheme}://{controller.Request?.Host.Value}{controller.Request?.Path.Value}";
}