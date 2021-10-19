using FreeCourse.Frontends.Web.Helpers;
using FreeCourse.Frontends.Web.Models;
using FreeCourse.Frontends.Web.Models.Catalogs;
using FreeCourse.Frontends.Web.Services.Interfaces;
using FreeCourse.Shared.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);

            if(resultPhotoService != null)
            {
                courseCreateInput.Picture = resultPhotoService.URL;
            }

            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("courses", courseCreateInput);


            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"courses/{courseId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<List<CategoryViewModel>>>();

            return result.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var response = await _httpClient.GetAsync("courses");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<List<CourseViewModel>>>();

            result.Data.ForEach(x =>
            {
                x.Picture = _photoHelper.GetPhotoStockURL(x.Picture);
            });

            return result.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"courses/getAllByUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<List<CourseViewModel>>>();

            result.Data.ForEach(x =>
            {
                x.Picture = _photoHelper.GetPhotoStockURL(x.Picture);
            });

            return result.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/{courseId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<CourseViewModel>>();

            return result.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
                courseUpdateInput.Picture = resultPhotoService.URL;
            }

            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("courses", courseUpdateInput);

            return response.IsSuccessStatusCode;
        }
    }
}
