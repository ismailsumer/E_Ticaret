using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Repositories;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ProductViewDto>> AddProductAsync(ProductCreateDto productDto)
        {
            // FK Kontrolleri: CategoryId ve BrandId geçerli mi?
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(productDto.CategoryId);
            if (category == null)
                return ServiceResponse<ProductViewDto>.ErrorResponse("Geçersiz kategori. Lütfen geçerli bir kategori seçiniz.");

            var brand = await _unitOfWork.GetRepository<Brand>().GetByIdAsync(productDto.BrandId);
            if (brand == null)
                return ServiceResponse<ProductViewDto>.ErrorResponse("Geçersiz marka. Lütfen geçerli bir marka seçiniz.");

            var product = _mapper.Map<Product>(productDto);

            // Resim dosyası varsa kaydet
            if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(productDto.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return ServiceResponse<ProductViewDto>.ErrorResponse("Sadece .jpg, .jpeg, .png, .gif ve .webp dosyaları yüklenebilir.");

                var fileName = $"{Guid.NewGuid()}{extension}";
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir); // Klasör yoksa oluştur
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productDto.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = $"/uploads/{fileName}";
            }

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<ProductViewDto>(product);
            return ServiceResponse<ProductViewDto>.SuccessResponse(response, "Ürün başarıyla eklendi.");
        }

        public async Task<ServiceResponse<ProductViewDto>> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<ProductViewDto>.ErrorResponse("Ürün bulunamadı.");

            var response = _mapper.Map<ProductViewDto>(product);
            return ServiceResponse<ProductViewDto>.SuccessResponse(response);
        }

        public async Task<ServiceResponse<ProductViewDto>> UpdateProductAsync(int id, ProductUpdateDto productDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<ProductViewDto>.ErrorResponse("Ürün bulunamadı.");

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.CategoryId = productDto.CategoryId;
            product.BrandId = productDto.BrandId;
            product.UpdatedDate = DateTime.Now;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<ProductViewDto>(product);
            return ServiceResponse<ProductViewDto>.SuccessResponse(response, "Ürün güncellendi.");
        }

        public async Task<ServiceResponse<string>> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<string>.ErrorResponse("Ürün bulunamadı.");

            product.IsDeleted = true;
            product.UpdatedDate = DateTime.Now;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResponse<string>.SuccessResponse("", "Ürün silindi.");
        }

        public async Task<PagedResponse<List<ProductViewDto>>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
             return await GetAllProductsAsync(pageNumber, pageSize, null, null, null);
        }

        public async Task<ServiceResponse<string>> UploadImageAsync(int productId, IFormFile file)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                return ServiceResponse<string>.ErrorResponse("Ürün bulunamadı.");

            if (file == null || file.Length == 0)
                return ServiceResponse<string>.ErrorResponse("Geçersiz dosya.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return ServiceResponse<string>.ErrorResponse("Sadece .jpg, .jpeg ve .png dosyaları yüklenebilir.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsDir);
            var path = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            product.ImageUrl = $"/uploads/{fileName}";
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResponse<string>.SuccessResponse(product.ImageUrl, "Resim başarıyla yüklendi.");
        }

        public async Task<PagedResponse<List<ProductViewDto>>> GetAllProductsAsync(
            int pageNumber,
            int pageSize,
            string? search,
            int? categoryId,
            int? brandId)
        {
            var (products, totalRecords) = await _unitOfWork.Products.GetProductsWithDetailsAsync(
                pageNumber, 
                pageSize, 
                search, 
                categoryId, 
                brandId);

            var responseDto = _mapper.Map<List<ProductViewDto>>(products);

            return new PagedResponse<List<ProductViewDto>>(
                responseDto,
                pageNumber,
                pageSize,
                totalRecords
            );
        }
    }
}
