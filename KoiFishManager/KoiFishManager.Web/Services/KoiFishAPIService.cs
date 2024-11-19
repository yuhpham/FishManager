using KoiFishManager.Models.KoiFish;
using System.Net.Http.Json;

namespace KoiFishManager.Web.Services
{
    public class KoiFishAPIService : IKoiFishService
    {
        private readonly HttpClient _httpClient;

        public KoiFishAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Thêm cá koi mới vào hồ
        public async Task<KoiFishResponse> AddKoiFishAsync(int pondId, KoiFishRequest request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/koiFish/{pondId}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<KoiFishResponse>();
        }

        // Lấy tất cả cá koi của người dùng
        public async Task<List<KoiFishResponse>> GetAllKoiFishByUserIdAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<KoiFishResponse>>("api/koiFish");
        }

        // Lấy cá koi theo ID
        public async Task<KoiFishResponse> GetKoiFishByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<KoiFishResponse>($"api/koiFish/{id}");
        }

        // Cập nhật thông tin cá koi
        public async Task<KoiFishResponse> UpdateKoiFishAsync(int id, KoiFishRequest request)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/koiFish/{id}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<KoiFishResponse>();
        }

        // Xóa cá koi
        public async Task DeleteKoiFishAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/koiFish/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
