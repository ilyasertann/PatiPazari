using System;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace ppUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService cityService)
        {
            _categoryService = cityService;
        }
        public IActionResult Index()
        {
            try
            {
                var categories = _categoryService.GetAll();
                return View(categories);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error getting categories: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            try
            {
                _categoryService.Insert(category);
                TempData["SuccessMessage"] = "Category added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding category: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            // Boş bir Category modeli oluşturup view'a iletebilirsiniz.
            var emptyCategory = new Category();
            return View(emptyCategory);
        }



        [HttpGet()]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var category = _categoryService.GetById(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = $"Category with ID {id} not found";
                    return RedirectToAction("Index");
                }

                _categoryService.Delete(category);
                TempData["SuccessMessage"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting category: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categories = _categoryService.GetAll();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting categories: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                var category = _categoryService.GetById(id);
                if (category == null)
                    return NotFound($"Category with ID {id} not found");

                return View(category);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting category: {ex.Message}");
            }
        }

      
        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            // Gerekli veriyi al ve modeli hazırla
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                // Kategori bulunamadıysa, uygun bir hata mesajı verebilir veya başka bir işlem yapabilirsiniz.
                return NotFound();
            }

            // Modeli view'a ile
            return View(category);
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            try
            {
                _categoryService.Update(category);
                TempData["SuccessMessage"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating category: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
