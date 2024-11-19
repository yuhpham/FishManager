using KoiFishManager.Models.KoiFish;

namespace KoiFishManager.Web.Services
{
    public interface IKoiFishService
    {
        Task<KoiFishResponse> AddKoiFishAsync(int pondId, KoiFishRequest request);

        Task<List<KoiFishResponse>> GetAllKoiFishByUserIdAsync();

       
        Task<KoiFishResponse> GetKoiFishByIdAsync(int id);

       
        Task<KoiFishResponse> UpdateKoiFishAsync(int id, KoiFishRequest request);

       
        Task DeleteKoiFishAsync(int id);
    }
}