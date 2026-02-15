using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Repositories;
using ETicaretAPI.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> AddCommentAsync(int userId, CommentCreateDto dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            comment.UserId = userId;

            await _unitOfWork.GetRepository<Comment>().AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true, "Yorumunuz eklendi.");
        }

        public async Task<ServiceResponse<List<CommentViewDto>>> GetProductCommentsAsync(int productId)
        {
            var comments = await _unitOfWork.GetRepository<Comment>()
                .Where(x => x.ProductId == productId)
                .Include(x => x.User)
                .Include(x => x.Product)
                .ToListAsync();

            var result = _mapper.Map<List<CommentViewDto>>(comments);
            return ServiceResponse<List<CommentViewDto>>.SuccessResponse(result);
        }

        public async Task<ServiceResponse<List<CommentViewDto>>> GetAllCommentsAsync()
        {
            var comments = await _unitOfWork.GetRepository<Comment>()
                .Where(x => true)
                .Include(x => x.User)
                .Include(x => x.Product)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            var result = _mapper.Map<List<CommentViewDto>>(comments);
            return ServiceResponse<List<CommentViewDto>>.SuccessResponse(result);
        }

        public async Task<ServiceResponse<string>> DeleteCommentAsync(int id)
        {
            var comment = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id);
            if (comment == null)
                return ServiceResponse<string>.ErrorResponse("Yorum bulunamadı.");

            _unitOfWork.GetRepository<Comment>().Delete(comment);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResponse<string>.SuccessResponse("", "Yorum başarıyla silindi.");
        }
    }
}