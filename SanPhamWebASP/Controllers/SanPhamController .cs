using SanPhamClassLiBrary.Controller;
using SanPhamClassLiBrary.Interface;
using SanPhamClassLiBrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPhamWebASP.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly IProductService _productService;
        private readonly ProductStatusService _statusService;
        public SanPhamController()
        {
            _productService = new ProductService();
            _statusService = new ProductStatusService();
        }

        // Hiển thị danh sách sản phẩm
        public ActionResult Index(string searchTerm , string isActive)
        { // Lấy danh sách trạng thái từ dịch vụ
            var statuses = _statusService.GetAllStatuses();
            ViewBag.Statuses = statuses;
            var products = _productService.GetAllProducts(searchTerm, isActive);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", products);
            }
            return View(products);
        }
        // Hiển thị form thêm sản phẩm
        public ActionResult Create()
        {
            // Lấy danh sách trạng thái từ dịch vụ
            var statuses = _statusService.GetAllStatuses();
            ViewBag.Statuses = statuses;
            return View();
        }

        // Xử lý dữ liệu khi người dùng gửi form
        [HttpPost]
        public JsonResult Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return Json(new { success = true });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return Json(new { success = false, errors = errors.Select(e => e.ErrorMessage) });
        }
        // Hiển thị form sửa sản phẩm
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id); // Lấy sản phẩm từ database
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // Xử lý dữ liệu khi người dùng gửi form sửa sản phẩm
     



        // Xóa sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productService.UpdateProduct(product);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating product: " + ex.Message);
                }
            }
            return View(product);
        }
        public JsonResult Delete(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}