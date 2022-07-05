
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Entities;

public class Cleaner
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal Rating { get; set; }
    public List<District> Districts { get; set; }
    public List<Service> Services { get; set; }
}
