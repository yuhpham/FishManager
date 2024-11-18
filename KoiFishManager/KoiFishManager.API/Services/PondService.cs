using KoiFishManager.Api.Data;
using KoiFishManager.Api.Entities;
using KoiFishManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KoiFishManager.Api.Services
{
    public class PondService : IPondService
    {
        private readonly KoiFishManagerDbContext _context;
        private readonly ILogger<PondService> _logger;

        public PondService(KoiFishManagerDbContext context, ILogger<PondService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Thêm hồ cá mới
        public async Task<PondResponse> AddAsync(Guid userId, PondRequest request)
        {
            try
            {
                var pond = new Pond
                {
                    Name = request.Name,
                    Size = request.Size,
                    Depth = request.Depth,
                    WaterQuality = request.WaterQuality,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Ponds.AddAsync(pond);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Pond added successfully with ID: {PondId}", pond.Id);
                return ToDto(pond);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the pond.");
                throw new InvalidOperationException("An error occurred while adding the pond.", ex);
            }
        }

        // Lấy danh sách hồ cá của người dùng
        public async Task<List<PondResponse>> GetAllByUserIdAsync(Guid userId)
        {
            try
            {
                var ponds = await _context.Ponds
                    .Include(p => p.KoiFishes)
                    .Include(p => p.FeedingSchedules)
                    .Include(p => p.PondStatuses)
                    .Where(p => p.UserId == userId)
                    .ToListAsync();

                return ponds.Select(ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching ponds for user {UserId}.", userId);
                throw;
            }
        }

        // Lấy thông tin hồ cá theo ID
        public async Task<PondResponse> GetByIdAsync(int id)
        {
            try
            {
                var pond = await _context.Ponds
                    .Include(p => p.KoiFishes)
                    .Include(p => p.FeedingSchedules)
                    .Include(p => p.PondStatuses)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (pond == null)
                {
                    _logger.LogWarning("Pond with ID {PondId} not found.", id);
                    return null; // Hoặc NotFound
                }

                return ToDto(pond);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching pond with ID {PondId}.", id);
                throw;
            }
        }

        // Phân trang hồ cá theo người dùng
        public async Task<(List<PondResponse>, int)> GetPagedByUserIdAsync(Guid userId, int page, int pageSize)
        {
            try
            {
                var query = _context.Ponds
                    .Include(p => p.KoiFishes)
                    .Include(p => p.FeedingSchedules)
                    .Include(p => p.PondStatuses)
                    .Where(p => p.UserId == userId);

                var totalItems = await query.CountAsync();
                var ponds = await query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pondDtos = ponds.Select(ToDto).ToList();
                return (pondDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paged ponds for user {UserId}.", userId);
                throw;
            }
        }

        // Cập nhật thông tin hồ cá
        public async Task<PondResponse> UpdateAsync(int id, PondRequest request)
        {
            try
            {
                var pond = await _context.Ponds.FindAsync(id);

                if (pond == null) throw new KeyNotFoundException("Pond not found");

                pond.Name = request.Name;
                pond.Size = request.Size;
                pond.Depth = request.Depth;
                pond.WaterQuality = request.WaterQuality;
                pond.UpdatedAt = DateTime.UtcNow;

                _context.Ponds.Update(pond);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Pond with ID {PondId} updated successfully.", id);
                return ToDto(pond);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating pond with ID {PondId}.", id);
                throw;
            }
        }

        // Xóa hồ cá
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var pond = await _context.Ponds.FindAsync(id);

                if (pond == null) throw new KeyNotFoundException("Pond not found");

                _context.Ponds.Remove(pond);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Pond with ID {PondId} deleted successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting pond with ID {PondId}.", id);
                throw;
            }
        }

        // Chuyển đổi thông tin hồ cá sang DTO
        private PondResponse ToDto(Pond pond)
        {
            return new PondResponse
            {
                Id = pond.Id,
                Name = pond.Name,
                Size = pond.Size,
                Depth = pond.Depth,
                WaterQuality = pond.WaterQuality,
                CreatedAt = pond.CreatedAt,
                UpdatedAt = pond.UpdatedAt,
                KoiFishes = pond.KoiFishes.Select(k => new KoiFishResponse
                {
                    Id = k.Id,
                    Name = k.Name,
                    Color = k.Color,
                    Size = k.Size,
                    Origin = k.Origin,
                    HealthStatus = k.HealthStatus
                }).ToList(),
                FeedingSchedules = pond.FeedingSchedules.Select(f => new FeedingScheduleResponse
                {
                    Id = f.Id,
                    FeedingTime = f.FeedingTime,
                    FoodType = f.FoodType,
                    Quantity = f.Quantity
                }).ToList(),
                PondStatuses = pond.PondStatuses.Select(ps => new PondStatusResponse
                {
                    Id = ps.Id,
                    Temperature = ps.Temperature,
                    PHLevel = ps.PHLevel,
                    WaterLevel = ps.WaterLevel,
                    RecordedAt = ps.RecordedAt
                }).ToList()
            };
        }
    }
}
