using FreeCourse.Frontends.Web.Models.PhotoStocks;
using FreeCourse.Frontends.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using FreeCourse.Shared.Dtos.Response;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");

            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoStockViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length <= default(int))
            {
                return null;
            }

            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

            using var memoryStream = new MemoryStream();
            await photo.CopyToAsync(memoryStream);


            var multipartContent = new MultipartFormDataContent
            {
                { new ByteArrayContent(memoryStream.ToArray()), "photo", randomFileName }
            };

            var response = await _httpClient.PostAsync("photos", multipartContent);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<ResponseDTO<PhotoStockViewModel>>();
            return responseSuccess.Data;
        }
    }
}
