﻿using YogurtCleaning.Business.Exceptions;
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
    public static void CheckThatListNotEmpty<T>(List<T> list, string errorMessage)
    {
        if (list == null || list.Count == 0)
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
    public static void AuthorizeEnitiyAccess(Order order, UserValues userValues)
    {
        if (!(userValues.Id == order.Client.Id || userValues.Role == Role.Admin ||
            (order.CleanersBand.Find(c => c.Id == userValues.Id) != null) && userValues.Role == Role.Cleaner))
        {
            throw new AccessException($"Access denied");
        }
    }

    public static void AuthorizeEnitiyAccess(string email, UserValues userValues)
    {
        if (!(userValues.Email == email || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
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
