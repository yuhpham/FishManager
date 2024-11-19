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
        public async Task<bool> AddKoiFishAsync(int pondId, KoiFishRequest request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/koiFish/{pondId}", request);

            // Kiểm tra nếu mã trạng thái HTTP là thành công (2xx)
            return response.IsSuccessStatusCode;
        }

        // Lấy tất cả cá koi của người dùng
        public async Task<List<KoiFishResponse>> GetAllKoiFishByUserIdAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<KoiFishResponse>>("api/KoiFish");
        }

        // Lấy cá koi theo ID
        public async Task<KoiFishDetailResponse> GetKoiFishByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<KoiFishDetailResponse>($"api/koiFish/{id}");
        }

        // Cập nhật thông tin cá koi
        public async Task<bool> UpdateKoiFishAsync(int id, KoiFishRequest request)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/koiFish/{id}", request);

            return response.IsSuccessStatusCode;
        }

        // Xóa cá koi
        public async Task DeleteKoiFishAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/koiFish/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
