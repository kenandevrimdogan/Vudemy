﻿using FreeCourse.Services.Catalog.Services.Category.Interfaces;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();

            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _categoryService.GetByIdAsync(id);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dtos.Category.CategoryCreateDTO categoryCreateDTO)
        {
            var response = await _categoryService.CreateAsync(categoryCreateDTO);

            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Dtos.Category.CategoryUpdateDTO categoryUpdateDTO)
        {
            var response = await _categoryService.UpdateAsync(categoryUpdateDTO);

            return CreateActionResultInstance(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _categoryService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }
    }
}
