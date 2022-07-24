namespace YogurtCleaning.Infrastructure;

public class ServerOptions
{
    private const string _localServer = @"Server=.;Database=YogurtCleaning.DB;Trusted_Connection=True;";
    private const string _networkServer = @"Server=80.78.240.16;Database=YogurtCleaning.DB;User Id = Student; Password=qwe!23;";
    public const string ConnectionOption = _networkServer;
}
