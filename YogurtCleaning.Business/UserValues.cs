using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Business;

public class UserValues
{
    public int Id { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string Password { get; set; }
}