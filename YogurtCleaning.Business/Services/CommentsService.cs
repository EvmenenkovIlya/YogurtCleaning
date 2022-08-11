using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CommentsService : ICommentsService
{
    
    private readonly ICommentsRepository _commentsRepository;
    private readonly IClientsRepository _clientsRepository;
    private readonly ICleanersRepository _cleanersRepository;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IClientsService _clientsService;
    private readonly ICleanersService _cleanersService;

    public CommentsService(
        ICommentsRepository commentsRepository, 
        IClientsRepository clientsRepository,
        ICleanersRepository cleanersRepository,
        IOrdersRepository ordersRepository,
        IClientsService clientsService,
        ICleanersService cleanersService
        )
    {
        _commentsRepository = commentsRepository;
        _clientsRepository = clientsRepository;
        _cleanersRepository = cleanersRepository;
        _ordersRepository = ordersRepository;
        _clientsService = clientsService;
        _cleanersService = cleanersService;
    }

    public async Task<int> AddCommentByClient(Comment comment, int clientId)
    {
        comment.Client = await _clientsRepository.GetClient(clientId);
        Validator.CheckThatObjectNotNull(comment.Client, ExceptionsErrorMessages.ClientNotFound);
        comment.Order = await _ordersRepository.GetOrder(comment.Order.Id);
        Validator.CheckThatObjectNotNull(comment.Order, ExceptionsErrorMessages.OrderNotFound);
        var result = await _commentsRepository.AddComment(comment);
        comment.Order.CleanersBand.ForEach(async c => await _cleanersService.UpdateCleanerRating(c.Id));
        return result;
    }

    public async Task<int> AddCommentByCleaner(Comment comment, int cleanerId)
    {
        comment.Cleaner = await _cleanersRepository.GetCleaner(cleanerId);
        Validator.CheckThatObjectNotNull(comment.Cleaner, ExceptionsErrorMessages.CleanerNotFound);
        comment.Order = await _ordersRepository.GetOrder(comment.Order.Id);
        Validator.CheckThatObjectNotNull(comment.Order, ExceptionsErrorMessages.OrderNotFound);
        var result = await _commentsRepository.AddComment(comment);
        await _clientsService.UpdateClientRating(comment.Order.Client.Id);
        return result;
    }

    public async Task<List<Comment>> GetComments() => await _commentsRepository.GetAllComments();

    public async Task DeleteComment(int id)
    {
        var comment = await _commentsRepository.GetCommentById(id);
        Validator.CheckThatObjectNotNull(comment, ExceptionsErrorMessages.CommentNotFound);
        await _commentsRepository.DeleteComment(comment);
        await _clientsService.UpdateClientRating(comment.Order.Client.Id);
        comment.Order.CleanersBand.ForEach(async c => await _cleanersService.UpdateCleanerRating(c.Id));
    }

}
