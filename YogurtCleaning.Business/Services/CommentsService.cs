using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CommentsService : ICommentsService
{
    
    private readonly ICommentsRepository _commentsRepository;
    private readonly IClientsRepository _clientsRepository;
    private readonly ICleanersRepository _cleanersRepository;
    private readonly IOrdersRepository _ordersRepository;

    public CommentsService(ICommentsRepository commentsRepository, IClientsRepository clientsRepository, ICleanersRepository cleanersRepository, IOrdersRepository ordersRepository)
    {
        _commentsRepository = commentsRepository;
        _clientsRepository = clientsRepository;
        _cleanersRepository = cleanersRepository;
        _ordersRepository = ordersRepository;
    }

    public async Task<int> AddCommentByClient(Comment comment, int clientId)
    {
        comment.Client = await _clientsRepository.GetClient(clientId);
        Validator.CheckThatObjectNotNull(comment.Client, ExceptionsErrorMessages.ClientNotFound);
        comment.Order = await _ordersRepository.GetOrder(comment.Order.Id);
        Validator.CheckThatObjectNotNull(comment.Order, ExceptionsErrorMessages.OrderNotFound);
        var result = await _commentsRepository.AddComment(comment);
        comment.Order.CleanersBand.ForEach(async c => await _cleanersRepository.UpdateCleanerRating(c.Id));
        return result;
    }

    public async Task<int> AddCommentByCleaner(Comment comment, int cleanerId)
    {
        comment.Cleaner = await _cleanersRepository.GetCleaner(cleanerId);
        Validator.CheckThatObjectNotNull(comment.Cleaner, ExceptionsErrorMessages.CleanerNotFound);
        comment.Order = await _ordersRepository.GetOrder(comment.Order.Id);
        Validator.CheckThatObjectNotNull(comment.Order, ExceptionsErrorMessages.OrderNotFound);
        var result = await _commentsRepository.AddComment(comment);
        await _clientsRepository.UpdateClientRating(comment.Order.Client.Id);
        return result;
    }

    public async Task<List<Comment>> GetComments()
    {
        var result = await _commentsRepository.GetAllComments();
        return result;
    }

    public async Task DeleteComment(int id)
    {
        var comment = await _commentsRepository.GetCommentById(id);
        Validator.CheckThatObjectNotNull(comment, ExceptionsErrorMessages.CommentNotFound);
        await _commentsRepository.DeleteComment(comment);
    }

    
}
