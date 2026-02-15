using AutoMapper;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Repositories;
using ETicaretAPI.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Services
{
    public interface ISettingsService
    {
        Task<ServiceResponse<List<SiteSettingDto>>> GetAllAsync();
        Task<ServiceResponse<string>> UpsertAsync(SiteSettingUpdateDto dto);
    }

    public class SettingsService : ISettingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SettingsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<SiteSettingDto>>> GetAllAsync()
        {
            var items = await _unitOfWork.GetRepository<SiteSetting>()
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            return ServiceResponse<List<SiteSettingDto>>.SuccessResponse(_mapper.Map<List<SiteSettingDto>>(items));
        }

        public async Task<ServiceResponse<string>> UpsertAsync(SiteSettingUpdateDto dto)
        {
            var existing = await _unitOfWork.GetRepository<SiteSetting>()
                .Where(x => x.Key == dto.Key && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                existing.Value = dto.Value;
                _unitOfWork.GetRepository<SiteSetting>().Update(existing);
            }
            else
            {
                var entity = new SiteSetting { Key = dto.Key, Value = dto.Value };
                await _unitOfWork.GetRepository<SiteSetting>().AddAsync(entity);
            }

            await _unitOfWork.SaveChangesAsync();
            return ServiceResponse<string>.SuccessResponse("", "Ayar kaydedildi.");
        }
    }
}
