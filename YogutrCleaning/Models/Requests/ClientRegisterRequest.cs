namespace YogurtCleaning.Models;

public class ClientRegisterRequest
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
