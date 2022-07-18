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

    public Cleaner? GetCleaner(int id, List<string>? identities)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);

        if (cleaner == null || cleaner.Id == 0)
        {
            throw new EntityNotFoundException($"Cleaner {id} not found");
        }
        if (identities[0] == (string)cleaner.Email || identities[1] == Role.Admin.ToString())
            return cleaner;
        else
        {
            throw new AccessException($"Access denied");
        }
    }

    public List<Cleaner> GetAllCleaners(List<string>? identities)
    {
        if (identities[1] == Role.Admin.ToString())
        {
            var cleaners = _cleanersRepository.GetAllCleaners();
            return cleaners;
        }
        else
        {
            throw new AccessException($"Access denied");
        }
    }

    public void DeleteCleaner(int id, List<string> identities)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);
        if (cleaner == null || cleaner.Id == 0)
        {
            throw new EntityNotFoundException($"Cleaner {id} not found");
        }
        if (!(identities[0] == (string)cleaner.Email || identities[1] == Role.Admin.ToString()))
        {

            throw new AccessException($"Access denied");

        }
        else
            _cleanersRepository.DeleteCleaner(id);
    }

    public void UpdateCleaner(Cleaner modelToUpdate, int id, List<string> identities)
    {
        Cleaner cleaner = _cleanersRepository.GetCleaner(id);
        if (cleaner == null || cleaner.Id == 0)
        {
            throw new EntityNotFoundException($"Cleaner {id} not found");
        }
        if (!(identities[0] == (string)cleaner.Email || identities[1] == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
        else
        {
            cleaner.FirstName = modelToUpdate.FirstName;
            cleaner.LastName = modelToUpdate.LastName;
            cleaner.Services = modelToUpdate.Services;
            cleaner.BirthDate = modelToUpdate.BirthDate;
            cleaner.Phone = modelToUpdate.Phone;
            _cleanersRepository.UpdateCleaner(cleaner);
        }
    }

    public int CreateCleaner(Cleaner cleaner)
    {
        // add checking password and confirm password
        var isChecked = CheckingEmailForUniqueness(cleaner.Email);

        if (isChecked)
        {
            throw new UniquenessException($"That email is registred");
        }
        if (cleaner.Phone is not null)
        {
            if (!(cleaner.Phone.StartsWith("+7") || cleaner.Phone.StartsWith("8") && cleaner.Phone.Length <= 11))
            {
                throw new DataException($"Invalid phone number");
            }
        }
        if (cleaner.BirthDate > DateTime.Now)
        {
            throw new DataException($"Invalid birthday");
        }
        else
            return _cleanersRepository.CreateCleaner(cleaner);

    }
    public List<Comment> GetCommentsByCleaner(int id, List<string> identities)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);
        var comments = _cleanersRepository.GetAllCommentsByCleaner(id);

        if (cleaner == null || cleaner.Id == 0)
        {
            throw new EntityNotFoundException($"Cleaner {id} not found");
        }

        if (!(identities[0] == (string)cleaner.Email || identities[1] == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
        return comments;
    }

    public List<Order> GetOrdersByCleaner(int id, List<string> identities)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);
        var orders = _cleanersRepository.GetAllOrdersByCleaner(id);

        if (cleaner == null || cleaner.Id == 0)
        {
            throw new EntityNotFoundException($"Orders by cleaner {id} not found");
        }
        if (!(identities[0] == (string)cleaner.Email || identities[1] == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
        else
            return orders;
    }

    private bool CheckingEmailForUniqueness(string email)
    {
        var cleaners = _cleanersRepository.GetAllCleaners();

        if (cleaners is not null)
        {
            var uniqueEmail = cleaners.Any(c => c.Email == email);
            return uniqueEmail;
        }
        else return false;
    }
}