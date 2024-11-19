using KoiFishManager.Models.KoiFish;

namespace KoiFishManager.Web.Services
{
    public interface IKoiFishService
    {
        Task<bool> AddKoiFishAsync(int pondId, KoiFishRequest request);

        Task<List<KoiFishResponse>> GetAllKoiFishByUserIdAsync();

       
        Task<KoiFishDetailResponse> GetKoiFishByIdAsync(int id);

       
        Task<bool> UpdateKoiFishAsync(int id, KoiFishRequest request);

       
        Task DeleteKoiFishAsync(int id);
    }
}