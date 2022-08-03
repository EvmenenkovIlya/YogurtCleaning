using Microsoft.AspNetCore.Authorization;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Infrastructure;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] roles) : base()
    {
        var rolesList = roles.ToList();
        rolesList.Add(Role.Admin);
        Roles = string.Join(",", rolesList);
    }
}