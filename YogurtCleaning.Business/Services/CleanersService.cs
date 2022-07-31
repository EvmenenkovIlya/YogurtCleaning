using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CleanersService : ICleanersService
{
    private readonly ICleanersRepository _cleanersRepository;

    public CleanersService(ICleanersRepository cleanersRepository)
    {
        _cleanersRepository = cleanersRepository;
    }

    public Cleaner? GetCleaner(int id, UserValues userValues)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);

        if (cleaner == null )
        {
            throw new EntityNotFoundException($"Cleaner {id} not found");
        }
        AuthorizeEnitiyAccess(cleaner, userValues);
        return cleaner;
    }

    public List<Cleaner> GetAllCleaners() => _cleanersRepository.GetAllCleaners();


    public void DeleteCleaner(int id, UserValues userValues)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);
        Validator.CheckThatObjectNotNull(cleaner, ExceptionsErrorMessages.CleanerNotFound);
        AuthorizeEnitiyAccess(cleaner, userValues);
        _cleanersRepository.DeleteCleaner(cleaner);
    }

    public void UpdateCleaner(Cleaner modelToUpdate, int id, UserValues userValues)
    {
        Cleaner cleaner = _cleanersRepository.GetCleaner(id);
        Validator.CheckThatObjectNotNull(cleaner, ExceptionsErrorMessages.CleanerNotFound);
        AuthorizeEnitiyAccess(cleaner, userValues);
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

        Validator.CheckThatObjectNotNull(cleaner, ExceptionsErrorMessages.CleanerCommentsNotFound);
        AuthorizeEnitiyAccess(cleaner, userValues);
        return _cleanersRepository.GetAllCommentsByCleaner(id);
    }

    public List<Order> GetOrdersByCleaner(int id, UserValues userValues)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);

        Validator.CheckThatObjectNotNull(cleaner, ExceptionsErrorMessages.CleanerOrdersNotFound);
        AuthorizeEnitiyAccess(cleaner, userValues);
        return _cleanersRepository.GetAllOrdersByCleaner(id);
    }

    private bool CheckEmailForUniqueness(string email) => _cleanersRepository.GetCleanerByEmail(email) == null;

    private void AuthorizeEnitiyAccess(Cleaner cleaner, UserValues userValues)
    {
        if (!(userValues.Email == cleaner.Email || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }
}