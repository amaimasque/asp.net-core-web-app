using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreWebApp.Data;
using CoreWebApp.Models;
using System.IO;
using CoreWebApp.Data.Repositories;
using CoreWebApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace CoreWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryWrapper _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ApplicationDbContext context, IRepositoryWrapper repositoryWrapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _repository = repositoryWrapper;
            _userManager = userManager;
        }

        // GET: Product
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return View("View", await _repository.Product.GetAllProductsByUserId(user.Id));
        }

        // GET: Product/Details/5
        public async Task<IActionResult> DetailsAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _repository.Product.GetProductByIdAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        public IActionResult Add()
        {
            return View();
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View("Add");
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductModel productModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var product = new ProductModel
            {
                Title = productModel.Title,
                Description = productModel.Description,
                UserID = user.Id
            };
            if (ModelState.IsValid)
            {
                using (var stream = new MemoryStream())
                {
                    await Request.Form.Files["DisplayImage"].CopyToAsync(stream);
                    product.DisplayImage = stream.ToArray();
                }

                _repository.Product.CreateProduct(product);
                await _repository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Add");
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModel.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            return View(productModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProductModel productModel)
        {;
            var selectedProduct = await _repository.Product.GetProductByIdAsync(productModel.ID);
            if (selectedProduct == null)
            {
                return NotFound($"Product not found.");
            }
            _repository.Product.UpdateProduct(productModel);

            if (ModelState.IsValid)
            {
                using (var stream = new MemoryStream())
                {
                    await Request.Form.Files["DisplayImage"].CopyToAsync(stream);
                    productModel.DisplayImage = stream.ToArray();
                }

                if(Request.Form.Files.Count == 0 || !Request.Form.Files.Where(item => item.Name == "DisplayImage").Any())
                {
                    productModel.DisplayImage = selectedProduct.DisplayImage;
                }

                productModel.UserID = selectedProduct.UserID;

                await _repository.SaveAsync();
                return RedirectToAction();
            }
            return View(productModel);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.ProductModel.FindAsync(id);
            _context.ProductModel.Remove(productModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductModelExists(int id)
        {
            return _context.ProductModel.Any(e => e.ID == id);
        }
    }
}
