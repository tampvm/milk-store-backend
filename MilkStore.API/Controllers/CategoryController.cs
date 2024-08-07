﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.CategoryViewModel;

namespace MilkStore.API.Controllers
{
 
  
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO model)
        {
            var response = await _categoryService.CreateCategory(model);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategory(int pageIndex = 0, int pageSize = 10)
        {
            var response = await _categoryService.GetAllCategory(pageIndex, pageSize);
            return Ok(response);
          
          
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _categoryService.GetCategoryById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO model)
        {
            var response = await _categoryService.UpdateCategory(model, id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategory(id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
