using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Repositories;
using ETicaretAPI.Wrappers;
using Microsoft.EntityFrameworkCore; // ToListAsync() için şart

namespace ETicaretAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> AddAddressAsync(int userId, AddressDto addressDto)
        {
            // DTO'yu Entity'ye çeviriyoruz
            var address = _mapper.Map<Address>(addressDto);
            address.UserId = userId; // Kullanıcıyı eşliyoruz

            // Repository üzerinden ekliyoruz
            await _unitOfWork.GetRepository<Address>().AddAsync(address);
            await _unitOfWork.SaveChangesAsync();
            
            return ServiceResponse<bool>.SuccessResponse(true, "Adres başarıyla eklendi.");
        }

        public async Task<ServiceResponse<List<AddressDto>>> GetUserAddressesAsync(int userId)
        {
            // Kullanıcıya ait adresleri çekiyoruz
            var addresses = await _unitOfWork.GetRepository<Address>()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var dto = _mapper.Map<List<AddressDto>>(addresses);
            return ServiceResponse<List<AddressDto>>.SuccessResponse(dto);
        }
    }
}