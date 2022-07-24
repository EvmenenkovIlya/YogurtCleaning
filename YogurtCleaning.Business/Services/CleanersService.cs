using YogurtCleaning.Business.Exceptions;
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

    public Cleaner? GetCleaner(int id, UserValues userValues)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);

        if (cleaner == null )
        {
            throw new EntityNotFoundException($"Cleaner {id} not found");
        }
        CheckPossibilityOfAccess(cleaner, userValues);
        return cleaner;
    }

    public List<Cleaner> GetAllCleaners() => _cleanersRepository.GetAllCleaners();


    public void DeleteCleaner(int id, UserValues userValues)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);
        CheckThatUserNotNull(cleaner, ExceptionsErrorMessages.CleanerNotFound);
        CheckPossibilityOfAccess(cleaner, userValues);
        _cleanersRepository.DeleteCleaner(id);
    }

    public void UpdateCleaner(Cleaner modelToUpdate, int id, UserValues userValues)
    {
        Cleaner cleaner = _cleanersRepository.GetCleaner(id);
        CheckThatUserNotNull(cleaner, ExceptionsErrorMessages.CleanerNotFound);
        CheckPossibilityOfAccess(cleaner, userValues);
        cleaner.FirstName = modelToUpdate.FirstName;
        cleaner.LastName = modelToUpdate.LastName;
        cleaner.Services = modelToUpdate.Services;
        cleaner.BirthDate = modelToUpdate.BirthDate;
        cleaner.Phone = modelToUpdate.Phone;
        _cleanersRepository.UpdateCleaner(cleaner);
    }

    public int CreateCleaner(Cleaner cleaner)
    {
        var isChecked = CheckEmailForUniqueness(cleaner.Email);
        if (!isChecked)
        {
            throw new UniquenessException($"That email is registred");
        }
        return _cleanersRepository.CreateCleaner(cleaner);

    }

    public List<Comment> GetCommentsByCleaner(int id, UserValues userValues)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);

        CheckThatUserNotNull(cleaner, ExceptionsErrorMessages.CleanerCommentsNotFound);
        CheckPossibilityOfAccess(cleaner, userValues);
        return _cleanersRepository.GetAllCommentsByCleaner(id);
    }

    public List<Order> GetOrdersByCleaner(int id, UserValues userValues)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);

        CheckThatUserNotNull(cleaner, ExceptionsErrorMessages.CleanerOrdersNotFound);
        CheckPossibilityOfAccess(cleaner, userValues);
        return _cleanersRepository.GetAllOrdersByCleaner(id);
    }

    private bool CheckEmailForUniqueness(string email) => _cleanersRepository.GetCleanerByEmail(email) == null;

    private void CheckPossibilityOfAccess(Cleaner cleaner, UserValues userValues)
    {
        if (!(userValues.Email == cleaner.Email || userValues.Role == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
    }

    private void CheckThatUserNotNull(Cleaner cleaner, string errorMesage)
    {
        if (cleaner == null)
        {
            throw new BadRequestException(errorMesage);
        } 
    }

    public List<Cleaner> GetFreeCleanersForOrder(Order order)
    {
        var freeCleaners = new List<Cleaner>();
        var workingCleaners = _cleanersRepository.GetAllCleaners()
            .Where(c => (c.Schedule is Schedule.ShiftWork && Convert.ToInt32((order.StartTime - c.DateOfStartWork).TotalDays % 4) < 2) ||
            (c.Schedule is Schedule.FullTime && order.StartTime.DayOfWeek != DayOfWeek.Sunday && order.StartTime.DayOfWeek != DayOfWeek.Saturday))
            .ToList();

        foreach (var c in workingCleaners)
        {
            bool isMatch = true;

            foreach (var o in c.Orders)
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

            if (isMatch is true)
            {
                freeCleaners.Add(c);
            }
        }
        return freeCleaners;
    }
}