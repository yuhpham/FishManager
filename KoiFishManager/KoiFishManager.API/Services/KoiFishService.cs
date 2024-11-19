using KoiFishManager.Api.Data;
using KoiFishManager.Api.Entities;
using KoiFishManager.Models.KoiFish;
using Microsoft.EntityFrameworkCore;

namespace KoiFishManager.API.Services
{
    public class KoiFishService : IKoiFishService
    {
        private readonly KoiFishManagerDbContext _context;

        public KoiFishService(KoiFishManagerDbContext context)
        {
            _context = context;
        }

        public async Task<KoiFishResponse> AddAsync(int pondId, KoiFishRequest request)
        {
            // Tạo đối tượng KoiFish từ request
            var koiFish = new KoiFish
            {
                Name = request.Name,
                Color = request.Color,
                Size = request.Size,
                Origin = request.Origin,
                HealthStatus = request.HealthStatus,
                PondId = pondId
            };

            // Thêm cá koi vào cơ sở dữ liệu
            _context.KoiFishes.Add(koiFish);
            await _context.SaveChangesAsync();

            // Trả về thông tin cá koi đã thêm
            return new KoiFishResponse
            {
                Id = koiFish.Id,
                Name = koiFish.Name,
                Color = koiFish.Color,
                Size = koiFish.Size,
                Origin = koiFish.Origin,
                HealthStatus = koiFish.HealthStatus,
                PondId = koiFish.PondId
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var koiFish = await _context.KoiFishes.FindAsync(id); // Tìm cá koi theo Id
            if (koiFish == null)
            {
                return false; // Nếu không tìm thấy cá koi, trả về false
            }

            _context.KoiFishes.Remove(koiFish); // Xóa cá koi khỏi DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            return true; // Trả về true khi xóa thành công
        }

        public async Task<List<KoiFishResponse>> GetAllByUserIdAsync(Guid userId)
        {

            // Truy vấn tất cả các cá koi của người dùng theo UserId
            var koiFishes = await _context.Ponds
                .Where(p => p.UserId == userId) // Lọc hồ cá theo UserId
                .SelectMany(p => p.KoiFishes)  // Lấy tất cả cá koi của các hồ cá
                .ToListAsync();

            // Chuyển đổi danh sách cá koi thành KoiFishResponse
            return koiFishes.Select(koiFish => new KoiFishResponse
            {
                Id = koiFish.Id,
                Name = koiFish.Name,
                Color = koiFish.Color,
                Size = koiFish.Size,
                Origin = koiFish.Origin,
                HealthStatus = koiFish.HealthStatus,
                CreatedAt = koiFish.CreatedAt,
                PondId = koiFish.PondId
            }).ToList();
        }

        public async Task<KoiFishResponse> GetByIdAsync(int id)
        {
            // Lấy cá koi theo ID
            var koiFish = await _context.KoiFishes
                .FirstOrDefaultAsync(k => k.Id == id);

            if (koiFish == null)
            {
                return null; // hoặc throw exception nếu cần
            }

            // Trả về thông tin cá koi
            return new KoiFishResponse
            {
                Id = koiFish.Id,
                Name = koiFish.Name,
                Color = koiFish.Color,
                Size = koiFish.Size,
                Origin = koiFish.Origin,
                HealthStatus = koiFish.HealthStatus,
                PondId = koiFish.PondId
            };
        }

        public async Task<KoiFishResponse> UpdateAsync(int id, KoiFishRequest request)
        {
            var koiFish = await _context.KoiFishes.FindAsync(id); // Tìm cá koi theo Id
            if (koiFish == null)
            {
                return null; // Nếu không tìm thấy cá koi, trả về null
            }

            // Cập nhật các thuộc tính của cá koi từ yêu cầu
            koiFish.Name = request.Name ?? koiFish.Name;
            koiFish.Color = request.Color ?? koiFish.Color;
            koiFish.Size = request.Size > 0 ? request.Size : koiFish.Size;
            koiFish.Origin = request.Origin ?? koiFish.Origin;
            koiFish.HealthStatus = request.HealthStatus ?? koiFish.HealthStatus;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Trả về đối tượng KoiFishResponse sau khi cập nhật
            return new KoiFishResponse
            {
                Id = koiFish.Id,
                Name = koiFish.Name,
                Color = koiFish.Color,
                Size = koiFish.Size,
                Origin = koiFish.Origin,
                HealthStatus = koiFish.HealthStatus,
                PondId = koiFish.PondId
            };
        }
    }
}
