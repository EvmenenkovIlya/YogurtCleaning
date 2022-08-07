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

    public CommentsService(ICommentsRepository commentsRepository, IClientsRepository clientsRepository, ICleanersRepository cleanersRepository)
    {
        _commentsRepository = commentsRepository;
        _clientsRepository = clientsRepository;
        _cleanersRepository = cleanersRepository;
    }

    public async Task<int> AddCommentByClient(Comment comment, int clientId)
    {
        comment.Client = await _clientsRepository.GetClient(clientId);
        var result = await _commentsRepository.AddComment(comment);

        return result;
    }

    public async Task<int> AddCommentByCleaner(Comment comment, int cleanerId)
    {
        comment.Cleaner = await _cleanersRepository.GetCleaner(cleanerId);
        var result = await _commentsRepository.AddComment(comment);

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
