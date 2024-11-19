using KoiFishManager.Api.Entities;

namespace KoiFishManager.API.Services
{
    public interface IKoiFishService
    {
        Task<IEnumerable<KoiFish>> GetAllKoiFishAsync();
        Task<KoiFish> GetKoiFishByIdAsync(int id);
        Task<KoiFish> CreateKoiFishAsync(KoiFish koiFish);
        Task<KoiFish> UpdateKoiFishAsync(int id, KoiFish koiFish);
        Task<bool> DeleteKoiFishAsync(int id);
    }
}
