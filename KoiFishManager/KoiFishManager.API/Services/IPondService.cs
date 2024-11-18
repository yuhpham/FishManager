
using KoiFishManager.Api.Entities;
using KoiFishManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFishManager.Api.Services
{
    public interface IPondService
    {
        // Thêm hồ cá mới
        Task<PondResponse> AddAsync(Guid userId, PondRequest request);

        // Lấy tất cả hồ cá của người dùng
        Task<List<PondResponse>> GetAllByUserIdAsync(Guid userId);
        Task<(List<PondResponse>, int)> GetPagedByUserIdAsync(Guid userId, int page, int pageSize);

        // Lấy hồ cá theo ID
        Task<PondResponse> GetByIdAsync(int id);

        // Cập nhật hồ cá
        Task<PondResponse> UpdateAsync(int id,PondRequest pond);

        // Xóa hồ cá
        Task<bool> DeleteAsync(int id);
    }
}
