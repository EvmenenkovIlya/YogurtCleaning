using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Business;

public static class Validator
{
    public static void CheckThatObjectNotNull<T>(T obj, string errorMessage)
    {
        if (obj == null)
        {
            throw new BadRequestException(errorMessage);
        }
    }

    public static void CheckRequestAndDbList<T, B>(List<T> fromRequest, List<B> fromDb)
    {
        if (fromRequest.Count != fromDb.Count)
        {
            throw new BadRequestException($"One of {typeof(T)} not found in Db");
        }
    }

    public static void AuthorizeEnitiyAccess(UserValues userValues, int id)
    {
        if (!(userValues.Id == id || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }
}
