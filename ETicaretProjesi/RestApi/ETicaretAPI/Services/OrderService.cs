using ETicaretAPI.Data;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Wrappers;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ETicaretAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly ETicaretContext _context;

        public OrderService(ETicaretContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<int>> CreateOrderAsync(int userId)
        {
            var basketItems = await _context.BasketItems
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (!basketItems.Any())
                return ServiceResponse<int>.ErrorResponse("Sepetiniz boş.");

            var order = new Order
            {
                UserId = userId,
                TotalPrice = basketItems.Sum(x => x.Quantity * x.Product!.Price),
                Status = "Beklemede",
                OrderItems = basketItems.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Product!.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            Log.Information("Yeni Sipariş Oluşturuldu. Sipariş ID: {OrderId}, Kullanıcı ID: {UserId}", order.Id, userId);

            _context.BasketItems.RemoveRange(basketItems);
            await _context.SaveChangesAsync();

            return ServiceResponse<int>.SuccessResponse(order.Id, "Siparişiniz başarıyla alındı.");
        }

        public async Task<ServiceResponse<List<OrderViewDto>>> GetUserOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .Select(o => new OrderViewDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    Username = o.User != null ? o.User.Username : "",
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductName = oi.Product!.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                }).ToListAsync();

            return ServiceResponse<List<OrderViewDto>>.SuccessResponse(orders);
        }

        public async Task<ServiceResponse<List<OrderViewDto>>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .OrderByDescending(x => x.OrderDate)
                .Select(o => new OrderViewDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    Username = o.User != null ? o.User.Username : "",
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductName = oi.Product!.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                }).ToListAsync();

            return ServiceResponse<List<OrderViewDto>>.SuccessResponse(orders);
        }

        public async Task<ServiceResponse<string>> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return ServiceResponse<string>.ErrorResponse("Sipariş bulunamadı.");

            var validStatuses = new[] { "Beklemede", "Hazırlanıyor", "Kargoda", "Teslim Edildi", "İptal Edildi" };
            if (!validStatuses.Contains(status))
                return ServiceResponse<string>.ErrorResponse("Geçersiz sipariş durumu.");

            order.Status = status;
            await _context.SaveChangesAsync();

            return ServiceResponse<string>.SuccessResponse("", $"Sipariş durumu '{status}' olarak güncellendi.");
        }
    }
}