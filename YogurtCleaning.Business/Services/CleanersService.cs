using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CleanersService : ICleanersService
{
    private readonly ICleanersRepository _cleanersRepository;
    private readonly IOrdersRepository _ordersRepository;

    public CleanersService(ICleanersRepository cleanersRepository, IOrdersRepository ordersRepository)
    {
        _cleanersRepository = cleanersRepository;
        _ordersRepository = ordersRepository;
    }

    public async Task<Cleaner?> GetCleaner(int id, UserValues userValues)
    {
        var cleaner = await _cleanersRepository.GetCleaner(id);

        if (cleaner == null )
        {
            throw new EntityNotFoundException($"Cleaner {id} not found");
        }
        AuthorizeEnitiyAccess(cleaner, userValues);
        return cleaner;
    }

    public async Task<List<Cleaner>> GetAllCleaners() => await _cleanersRepository.GetAllCleaners();


    public async Task DeleteCleaner(int id, UserValues userValues)
    {
        var cleaner = await _cleanersRepository.GetCleaner(id);
        Validator.CheckThatObjectNotNull(cleaner, ExceptionsErrorMessages.CleanerNotFound);
        AuthorizeEnitiyAccess(cleaner, userValues);
        await _cleanersRepository.DeleteCleaner(cleaner);
    }

    public async Task UpdateCleaner(Cleaner modelToUpdate, int id, UserValues userValues)
    {
        Cleaner cleaner = await _cleanersRepository.GetCleaner(id);
        Validator.CheckThatObjectNotNull(cleaner, ExceptionsErrorMessages.CleanerNotFound);
        AuthorizeEnitiyAccess(cleaner, userValues);
        cleaner.FirstName = modelToUpdate.FirstName;
        cleaner.LastName = modelToUpdate.LastName;
        cleaner.Services = modelToUpdate.Services;
        cleaner.BirthDate = modelToUpdate.BirthDate;
        cleaner.Phone = modelToUpdate.Phone;
        await _cleanersRepository.UpdateCleaner(cleaner);
    }

    public async Task<int> CreateCleaner(Cleaner cleaner)
    {
        var isChecked = await CheckEmailForUniqueness(cleaner.Email);
        if (!isChecked)
        {
            throw new UniquenessException($"That email is registred");
        }
        List<Service> services = cleaner.Services;
        List<District> districts = cleaner.Districts;
        cleaner.Services = await _cleanersRepository.GetServices(cleaner.Services);
        cleaner.Districts = await _cleanersRepository.GetDistricts(cleaner.Districts);
        Validator.CheckRequestAndDbList(services, cleaner.Services);
        Validator.CheckRequestAndDbList(districts, cleaner.Districts);
        return await _cleanersRepository.CreateCleaner(cleaner);

    }

    public async Task<List<Comment>> GetCommentsByCleaner(int id, UserValues userValues)
    {
        var cleaner = await _cleanersRepository.GetCleaner(id);

        Validator.CheckThatObjectNotNull(cleaner, ExceptionsErrorMessages.CleanerCommentsNotFound);
        AuthorizeEnitiyAccess(cleaner, userValues);
        return await _cleanersRepository.GetAllCommentsByCleaner(id);
    }

    public async Task<List<Order>> GetOrdersByCleaner(int id, UserValues userValues)
    {
        var cleaner = await _cleanersRepository.GetCleaner(id);

        Validator.CheckThatObjectNotNull(cleaner, ExceptionsErrorMessages.CleanerOrdersNotFound);
        AuthorizeEnitiyAccess(cleaner, userValues);
        return await _cleanersRepository.GetAllOrdersByCleaner(id);
    }

    private async Task<bool> CheckEmailForUniqueness(string email) => await _cleanersRepository.GetCleanerByEmail(email) == null;

    private void AuthorizeEnitiyAccess(Cleaner cleaner, UserValues userValues)
    {
        if (!(userValues.Email == cleaner.Email || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }

    public async Task<List<Cleaner>> GetFreeCleanersForOrder(OrderBusinessModel order)
    {
        var freeCleaners = new List<Cleaner>();
        var workingCleaners = await _cleanersRepository.GetWorkingCleanersForDate(order.StartTime);

        foreach (var cleaner in workingCleaners)
        {
            bool isMatch = true;
            var filteredOrders = cleaner.Orders.Where(o => o.StartTime.Date == order.StartTime.Date).ToList();

            if(filteredOrders.Count == 0)
            {
                isMatch = true;
            }
            else
            {
                foreach (var o in cleaner.Orders)
                {
                    if (o.StartTime.Date != order.StartTime.Date || ((o.StartTime >= order.EndTime.AddHours(1) || o.EndTime.AddHours(1) <= order.StartTime)))
                    {
                        isMatch = true;
                    }
                    else
                    {
                        isMatch = false;
                        break;
                    }
                }
            }

            if (isMatch is true)
            {
                freeCleaners.Add(cleaner);
            }
        }
        return freeCleaners;
    }
}