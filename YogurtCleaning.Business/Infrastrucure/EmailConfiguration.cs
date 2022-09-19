using Microsoft.Extensions.Options;

namespace YogurtCleaning.Business;

public class EmailConfiguration
{
    public string YOGFrom { get; set; }
    public string YOGSmtpServer { get; set; }
    public int YOGPort { get; set; }
    public string YOGUserName { get; set; }
    public string YOGPassword { get; set; }
}
