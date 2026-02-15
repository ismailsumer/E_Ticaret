using AutoMapper;
using ETicaretAPI.Data;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Services
{
    public class BasketService : IBasketService
    {
        private readonly ETicaretContext _context;
        private readonly IMapper _mapper;

        public BasketService(ETicaretContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<BasketItemViewDto>>> GetUserBasketAsync(int userId)
        {
            var items = await _context.BasketItems
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var dto = items.Select(x => new BasketItemViewDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product!.Name,
                Price = x.Product.Price,
                Quantity = x.Quantity,
                ImageUrl = x.Product.ImageUrl
            }).ToList();

            return ServiceResponse<List<BasketItemViewDto>>.SuccessResponse(dto);
        }

        public async Task<ServiceResponse<bool>> AddToBasketAsync(int userId, BasketItemCreateDto basketDto)
        {
            var existingItem = await _context.BasketItems
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == basketDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += basketDto.Quantity;
            }
            else
            {
                _context.BasketItems.Add(new BasketItem
                {
                    UserId = userId,
                    ProductId = basketDto.ProductId,
                    Quantity = basketDto.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return ServiceResponse<bool>.SuccessResponse(true, "Ürün sepete eklendi.");
        }

        public async Task<ServiceResponse<bool>> ClearBasketAsync(int userId)
        {
            var items = _context.BasketItems.Where(x => x.UserId == userId);
            _context.BasketItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return ServiceResponse<bool>.SuccessResponse(true, "Sepet boşaltıldı.");
        }
    }
}