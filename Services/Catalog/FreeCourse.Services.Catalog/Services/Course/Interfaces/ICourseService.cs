using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.Course.Interfaces
{
    public interface ICourseService
    {
        Task<ResponseDTO<List<CourseDTO>>> GetAllAsync();

        Task<ResponseDTO<CourseDTO>> GetByIdAsync(string id);


        Task<ResponseDTO<List<CourseDTO>>> GetAllByUserIdAsync(string userId);


        Task<ResponseDTO<CourseDTO>> CreateAsync(CourseCreateDTO createcourse);


        Task<ResponseDTO<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdate);


        Task<ResponseDTO<NoContent>> DeleteAsync(string id);

    }
}
