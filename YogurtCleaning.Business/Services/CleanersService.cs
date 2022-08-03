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
        _cleanersRepository.DeleteCleaner(cleaner);
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
}