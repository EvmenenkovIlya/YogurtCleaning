namespace YogurtCleaning.Infrastructure;

public class ServerOptions
{
    private const string _localServer = @"Server=.;Database=YogurtCleaning.DB;Trusted_Connection=True;";
    public const string ConnectionOption = _localServer;
}
