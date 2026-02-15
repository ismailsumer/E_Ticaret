using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public interface ICommentService
    {
        Task<ServiceResponse<bool>> AddCommentAsync(int userId, CommentCreateDto dto);
        Task<ServiceResponse<List<CommentViewDto>>> GetProductCommentsAsync(int productId);
        Task<ServiceResponse<List<CommentViewDto>>> GetAllCommentsAsync();
        Task<ServiceResponse<string>> DeleteCommentAsync(int id);
    }
}