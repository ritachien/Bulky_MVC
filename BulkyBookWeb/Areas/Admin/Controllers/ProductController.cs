﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var objProductList = _unitOfWork.Product.GetAll().ToList();
        return View(objProductList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product obj)
    {
        if (!ModelState.IsValid) return View(obj);

        _unitOfWork.Product.Add(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product created successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int? id)
    {
        if (id is 0 or null)
        {
            return NotFound();
        }

        var productFromDb = _unitOfWork.Product.Get(c => c.Id == id);
        if (productFromDb == null)
        {
            return NotFound();
        }

        return View(productFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Product obj)
    {
        if (!ModelState.IsValid) return View();

        _unitOfWork.Product.Update(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product Edited successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int? id)
    {
        if (id is 0 or null)
        {
            return NotFound();
        }

        var productFromDb = _unitOfWork.Product.Get(c => c.Id == id);
        if (productFromDb == null)
        {
            return NotFound();
        }

        return View(productFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitOfWork.Product.Get(c => c.Id == id);
        if (obj is null)
        {
            return NotFound();
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted successfully";
        return RedirectToAction("Index");
    }
}