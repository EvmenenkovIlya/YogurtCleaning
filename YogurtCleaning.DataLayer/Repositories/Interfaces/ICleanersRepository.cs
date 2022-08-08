﻿using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface ICleanersRepository
{
    Task<int> CreateCleaner(Cleaner cleaner);
    Task<Cleaner?> GetCleaner(int clientId);
    Task UpdateCleaner(Cleaner cleaner);
    Task DeleteCleaner(Cleaner cleaner);
    Task<List<Cleaner>> GetAllCleaners();
    Task<List<Comment>> GetAllCommentsByCleaner(int cleanerId);
    Task<List<Order>> GetAllOrdersByCleaner(int id);
    Task<Cleaner?> GetCleanerByEmail(string email);
    Task<List<Cleaner>> GetWorkingCleanersForDate(DateTime orderDate);
    Task<List<Service>> GetServices(List<Service> services);
    Task<List<District>> GetDistricts(List<District> districts);
}