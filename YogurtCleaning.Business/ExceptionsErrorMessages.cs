namespace YogurtCleaning.Business;

public static class ExceptionsErrorMessages
{
    public const string ClientNotFound = "Client with this requestId not found";
    public const string CleanerNotFound = "Cleaner with this requestId not found:";

    public const string OrderNotFound = "Order with this requestId not found:";
    public const string ClientOrdersNotFound = "Orders for Client with this requestId not found:";
    public const string CleanerOrdersNotFound = "Order for Cleaner with this requestId not found:";
    public const string ClientCommentsNotFound = "Comments for Client with this requestId not found:";
    public const string CleanerCommentsNotFound = "Comments for Cleaner with this requestId not found";
}