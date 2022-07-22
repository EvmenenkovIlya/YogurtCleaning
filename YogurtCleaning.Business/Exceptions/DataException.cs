namespace YogurtCleaning.Business.Exceptions;

public class DataException : Exception
{
    public DataException(string message) : base(message) { }
}