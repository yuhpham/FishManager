using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KoiFishManager.Models;
using KoiFishManager.Models.SeedWork;
using KoiFishManager.Web.Services;

namespace KoiFishManager.Api.Services
{
    public class PondAPIService : IPondService
    {
        private readonly HttpClient _httpClient;

        public PondAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PondResponse>> GetAllPondsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PondResponse>>("api/pond/all");
        }

        public async Task<PondResponse> GetPondByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PondResponse>($"api/pond/{id}");
        }

        public async Task CreatePondAsync(PondRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pond", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdatePondAsync(int id, PondRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/pond/{id}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePondAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/pond/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
