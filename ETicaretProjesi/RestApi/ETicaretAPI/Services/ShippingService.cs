using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Repositories;
using ETicaretAPI.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Services
{
    public interface IShippingService
    {
        Task<ServiceResponse<List<ShippingViewDto>>> GetAllAsync();
        Task<ServiceResponse<ShippingViewDto>> CreateAsync(ShippingCreateDto dto);
        Task<ServiceResponse<string>> DeleteAsync(int id);
    }

    public class ShippingService : IShippingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShippingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<ShippingViewDto>>> GetAllAsync()
        {
            var items = await _unitOfWork.GetRepository<ShippingCompany>()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync();
            return ServiceResponse<List<ShippingViewDto>>.SuccessResponse(_mapper.Map<List<ShippingViewDto>>(items));
        }

        public async Task<ServiceResponse<ShippingViewDto>> CreateAsync(ShippingCreateDto dto)
        {
            var entity = _mapper.Map<ShippingCompany>(dto);
            await _unitOfWork.GetRepository<ShippingCompany>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResponse<ShippingViewDto>.SuccessResponse(_mapper.Map<ShippingViewDto>(entity), "Kargo firması eklendi.");
        }

        public async Task<ServiceResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<ShippingCompany>().GetByIdAsync(id);
            if (entity == null)
                return ServiceResponse<string>.ErrorResponse("Kargo firması bulunamadı.");

            entity.IsDeleted = true;
            _unitOfWork.GetRepository<ShippingCompany>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResponse<string>.SuccessResponse("", "Kargo firması silindi.");
        }
    }
}
