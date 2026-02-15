using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Repositories;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<CategoryViewDto>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            var activeCategories = categories.Where(c => !c.IsDeleted).ToList();
            var response = _mapper.Map<List<CategoryViewDto>>(activeCategories);
            return ServiceResponse<List<CategoryViewDto>>.SuccessResponse(response);
        }

        public async Task<ServiceResponse<CategoryViewDto>> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null || category.IsDeleted)
                return ServiceResponse<CategoryViewDto>.ErrorResponse("Kategori bulunamadı.");

            var result = _mapper.Map<CategoryViewDto>(category);
            return ServiceResponse<CategoryViewDto>.SuccessResponse(result);
        }

        public async Task<ServiceResponse<CategoryViewDto>> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.GetRepository<Category>().AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<CategoryViewDto>(category);
            return ServiceResponse<CategoryViewDto>.SuccessResponse(result, "Kategori başarıyla oluşturuldu.");
        }

        public async Task<ServiceResponse<CategoryViewDto>> UpdateCategoryAsync(int id, CategoryCreateDto categoryDto)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null || category.IsDeleted)
                return ServiceResponse<CategoryViewDto>.ErrorResponse("Kategori bulunamadı.");

            category.Name = categoryDto.Name;
            category.UpdatedDate = DateTime.Now;
            _unitOfWork.GetRepository<Category>().Update(category);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<CategoryViewDto>(category);
            return ServiceResponse<CategoryViewDto>.SuccessResponse(result, "Kategori güncellendi.");
        }

        public async Task<ServiceResponse<string>> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null || category.IsDeleted)
                return ServiceResponse<string>.ErrorResponse("Kategori bulunamadı.");

            category.IsDeleted = true;
            category.UpdatedDate = DateTime.Now;
            _unitOfWork.GetRepository<Category>().Update(category);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResponse<string>.SuccessResponse("", "Kategori silindi.");
        }
    }
}