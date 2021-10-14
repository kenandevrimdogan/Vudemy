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

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            courseCreateInput.UserId = _sharedIdentityService.GetUserId;

            await _catalogService.CreateCourseAsync(courseCreateInput);

            return RedirectToAction(nameof(CoursesController.Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetByCourseIdAsync(id);

            if(course == null)
            {
                return RedirectToAction(nameof(CoursesController.Index));
            }

            var categories = await _catalogService.GetAllCategoryAsync();
            var selectList = new SelectList(categories, "Id", "Name", course.CategoryId);
            ViewBag.categoryList = selectList;

            var courseUpdateInput = new CourseUpdateInput
            {
                Id = course.Id,
                CategoryId = course.CategoryId,
                Description = course.Description,
                Feature = course.Feature,
                Name = course.Name,
                Picture = course.Picture,
                Price = course.Price,
                UserId = course.UserId
            };

            return View(courseUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _catalogService.UpdateCourseAsync(courseUpdateInput);

            return RedirectToAction(nameof(CoursesController.Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }

            await _catalogService.DeleteCourseAsync(id);

            return RedirectToAction(nameof(CoursesController.Index));
        }
    }
}
