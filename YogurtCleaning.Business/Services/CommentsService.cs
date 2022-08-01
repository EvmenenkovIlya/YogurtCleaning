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

    public int AddCommentByClient(Comment comment, int clientId)
    {
        comment.Client = _clientsRepository.GetClient(clientId);
        var result = _commentsRepository.AddComment(comment);

        return result;
    }

    public int AddCommentByCleaner(Comment comment, int cleanerId)
    {
        comment.Cleaner = _cleanersRepository.GetCleaner(cleanerId);
        var result = _commentsRepository.AddComment(comment);

        return result;
    }

    public List<Comment> GetComments()
    {
        var result = _commentsRepository.GetAllComments();
        return result;
    }

    public void DeleteComment(int id)
    {
        var comment = _commentsRepository.GetCommentById(id);
        Validator.CheckThatObjectNotNull(comment, ExceptionsErrorMessages.CommentNotFound);
        _commentsRepository.DeleteComment(comment);
    }
}
