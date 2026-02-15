using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Repositories;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<BrandViewDto>>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<Brand>().GetAllAsync();
            var activeBrands = brands.Where(b => b.IsActive).ToList();
            var response = _mapper.Map<List<BrandViewDto>>(activeBrands);
            return ServiceResponse<List<BrandViewDto>>.SuccessResponse(response);
        }

        public async Task<ServiceResponse<BrandViewDto>> CreateBrandAsync(BrandCreateDto brandDto)
        {
            var brand = _mapper.Map<Brand>(brandDto);
            await _unitOfWork.GetRepository<Brand>().AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<BrandViewDto>(brand);
            return ServiceResponse<BrandViewDto>.SuccessResponse(result, "Marka başarıyla oluşturuldu.");
        }

        public async Task<ServiceResponse<BrandViewDto>> UpdateBrandAsync(int id, BrandCreateDto brandDto)
        {
            var brand = await _unitOfWork.GetRepository<Brand>().GetByIdAsync(id);
            if (brand == null)
                return ServiceResponse<BrandViewDto>.ErrorResponse("Marka bulunamadı.");

            brand.Name = brandDto.Name;
            _unitOfWork.GetRepository<Brand>().Update(brand);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<BrandViewDto>(brand);
            return ServiceResponse<BrandViewDto>.SuccessResponse(result, "Marka güncellendi.");
        }

        public async Task<ServiceResponse<string>> DeleteBrandAsync(int id)
        {
            var brand = await _unitOfWork.GetRepository<Brand>().GetByIdAsync(id);
            if (brand == null)
                return ServiceResponse<string>.ErrorResponse("Marka bulunamadı.");

            brand.IsActive = false;
            _unitOfWork.GetRepository<Brand>().Update(brand);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResponse<string>.SuccessResponse("", "Marka silindi.");
        }
    }
}
