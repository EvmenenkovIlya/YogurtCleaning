using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.DataLayer.Entities;
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

    public static void AuthorizeEnitiyAccess(CleaningObject cleaningObject, UserValues userValues)
    {
        if (!(userValues.Id == cleaningObject.Client.Id || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }

    public static void AuthorizeEnitiyAccess(UserValues userValues, Client client)
    {
        if (!(userValues.Email == client.Email || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }

    public static void AuthorizeEnitiyAccess(Cleaner cleaner, UserValues userValues)
    {
        if (!(userValues.Email == cleaner.Email || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }
}
