using FreeCourse.Frontends.Web.Models.Catalogs;
using FreeCourse.Frontends.Web.Services.Interfaces;
using FreeCourse.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            var course = await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId);
            return View(course);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();

            var selectList = new SelectList(categories, "Id", "Name");
            ViewBag.categoryList = selectList;

            return View();
        }
    }
}
