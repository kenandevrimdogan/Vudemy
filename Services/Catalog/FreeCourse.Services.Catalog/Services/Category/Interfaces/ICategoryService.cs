using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Shared.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.Category.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseDTO<List<CategoryDTO>>> GetAllAsync();

        Task<ResponseDTO<CategoryDTO>> CreateAsync(CategoryCreateDTO createCategory);

        Task<ResponseDTO<CategoryDTO>> GetByIdAsync(string id);

    }
}
