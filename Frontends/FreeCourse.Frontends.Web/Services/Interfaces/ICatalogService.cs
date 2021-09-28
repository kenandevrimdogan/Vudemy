using FreeCourse.Frontends.Web.Models;
using FreeCourse.Frontends.Web.Models.Catalogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();

        Task<List<CategoryViewModel>> GetAllCategoryAsync();

        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);

        Task<CourseViewModel> GetByCourseIdAsync(string courseId);

        Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput);

        Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);

        Task<bool> DeleteCourseAsync(string courseId);

    }
}
