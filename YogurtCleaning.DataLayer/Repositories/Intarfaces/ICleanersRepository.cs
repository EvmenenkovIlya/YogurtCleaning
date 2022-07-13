﻿using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories.Intarfaces
{
    public interface ICleanersRepository
    {
        int CreateCleaner(Cleaner cleaner);
        void DeleteCleaner(int cleanerId);
        List<Cleaner> GetAllCleaners();
        List<Comment> GetAllCommentsByCleaner(int cleanerId);
        Cleaner? GetCleaner(int clientId);
        void UpdateCleaner(Cleaner cleaner);
    }
}