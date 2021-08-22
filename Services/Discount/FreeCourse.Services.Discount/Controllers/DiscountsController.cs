using FreeCourse.Services.Discount.Dtos.Discounts;
using FreeCourse.Services.Discount.Services.Interfaces;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController: CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAllAsync());
        }

        [Route("/api/[controller]/GetByCodeAsync/{code}")]
        [HttpGet]
        public async Task<IActionResult> GetByCodeAsync(string code)
        {
            return CreateActionResultInstance(await _discountService.GetByCodeAndUserIdAsync(code, _sharedIdentityService.GetUserId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return CreateActionResultInstance(await _discountService.GetByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return CreateActionResultInstance(await _discountService.DeleteAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(CreateDiscountDTO createDiscount)
        {
            createDiscount.UserId = _sharedIdentityService.GetUserId;
            return CreateActionResultInstance(await _discountService.SaveAsync(createDiscount));
        }

        [HttpPut]
        public async Task<IActionResult> SaveAsync(UpdateDiscountDTO updateDiscount)
        {
            updateDiscount.UserId = _sharedIdentityService.GetUserId;
            return CreateActionResultInstance(await _discountService.UpdateAsync(updateDiscount));
        }
    }
}
