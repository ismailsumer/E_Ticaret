using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Repositories;
using ETicaretAPI.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Services
{
    public interface ISliderService
    {
        Task<ServiceResponse<List<SliderViewDto>>> GetAllAsync();
        Task<ServiceResponse<SliderViewDto>> CreateAsync(SliderCreateDto dto);
        Task<ServiceResponse<string>> DeleteAsync(int id);
    }

    public class SliderService : ISliderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SliderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<SliderViewDto>>> GetAllAsync()
        {
            var items = await _unitOfWork.GetRepository<Slider>()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
            return ServiceResponse<List<SliderViewDto>>.SuccessResponse(_mapper.Map<List<SliderViewDto>>(items));
        }

        public async Task<ServiceResponse<SliderViewDto>> CreateAsync(SliderCreateDto dto)
        {
            var entity = _mapper.Map<Slider>(dto);

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var extension = Path.GetExtension(dto.ImageFile.FileName).ToLower();
                var fileName = $"{Guid.NewGuid()}{extension}";
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);
                var filePath = Path.Combine(uploadsDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.ImageFile.CopyToAsync(stream);
                entity.ImageUrl = $"/uploads/{fileName}";
            }

            await _unitOfWork.GetRepository<Slider>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResponse<SliderViewDto>.SuccessResponse(_mapper.Map<SliderViewDto>(entity), "Slider eklendi.");
        }

        public async Task<ServiceResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<Slider>().GetByIdAsync(id);
            if (entity == null)
                return ServiceResponse<string>.ErrorResponse("Slider bulunamadı.");

            entity.IsDeleted = true;
            _unitOfWork.GetRepository<Slider>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResponse<string>.SuccessResponse("", "Slider silindi.");
        }
    }
}
