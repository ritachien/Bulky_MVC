using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryController(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public IActionResult Index()
    {
        var objCategoryList = _categoryRepo.GetAll().ToList();
        return View(objCategoryList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name.");
        }

        if (!ModelState.IsValid) return View(obj);

        _categoryRepo.Add(obj);
        _categoryRepo.Save();
        TempData["success"] = "Category created successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int? id)
    {
        if (id is 0 or null)
        {
            return NotFound();
        }

        var categoryFromDb = _categoryRepo.Get(c => c.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }

        return View(categoryFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (!ModelState.IsValid) return View();
        
        _categoryRepo.Update(obj);
        _categoryRepo.Save();
        TempData["success"] = "Category Edited successfully";
        return RedirectToAction("Index");

    }

    public IActionResult Delete(int? id)
    {
        if (id is 0 or null)
        {
            return NotFound();
        }

        var categoryFromDb = _categoryRepo.Get(c => c.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }

        return View(categoryFromDb);
    }
    
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _categoryRepo.Get(c => c.Id == id);
        if (obj is null)
        {
            return NotFound();
        }

        _categoryRepo.Remove(obj);
        _categoryRepo.Save();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");
    }
}