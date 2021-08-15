using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Services.Category.Interfaces;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.Category.Abstracts
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Models.Categories.Category> _categoryCollection;
        private readonly IMapper _maper;

        public CategoryService(IMapper maper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Models.Categories.Category>(databaseSettings.CategoryCollectionName);
            _maper = maper;
        }

        public async Task<ResponseDTO<List<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();
            return ResponseDTO<List<CategoryDTO>>.Success(_maper.Map<List<CategoryDTO>>(categories), HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<CategoryDTO>> CreateAsync(CategoryCreateDTO createCategory)
        {
            var category = _maper.Map<CategoryCreateDTO, Models.Categories.Category>(createCategory);
            await _categoryCollection.InsertOneAsync(category);

            var categoryDTO = _maper.Map<Models.Categories.Category, CategoryDTO>(category);


            return ResponseDTO<CategoryDTO>.Success(categoryDTO, HttpStatusCode.Created);
        }

        public async Task<ResponseDTO<CategoryDTO>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return ResponseDTO<CategoryDTO>.Fail("Category not found", HttpStatusCode.NotFound);
            }

            var categoryDTO = _maper.Map<Models.Categories.Category, CategoryDTO>(category);

            return ResponseDTO<CategoryDTO>.Success(categoryDTO, HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<NoContent>> UpdateAsync(CategoryUpdateDTO categoryUpdate)
        {
            var category = _maper.Map<CategoryUpdateDTO, Models.Categories.Category>(categoryUpdate);

            var result = await _categoryCollection.FindOneAndReplaceAsync(x => x.Id == categoryUpdate.Id, category);

            if (result == null)
            {
                return ResponseDTO<NoContent>.Fail("Course not found", HttpStatusCode.NotFound);
            }

            return ResponseDTO<NoContent>.Success(HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<NoContent>> DeleteAsync(string id)
        {
            var result = await _categoryCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount < 0)
            {
                return ResponseDTO<NoContent>.Fail("Course not found", HttpStatusCode.NotFound);
            }

            return ResponseDTO<NoContent>.Success(HttpStatusCode.OK);
        }
    }
}
