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

namespace CoreWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View("View", await _context.ProductModel.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public async Task<IActionResult> Create([Bind("Title,Description,DisplayImage,UserID")] ProductModel productModel)
        {
            /**var filePath = Path.GetTempFileName();
            using (var inputStream = new FileStream(filePath, FileMode.Create))
            {
                // read file to stream
                await Request.Form.Files["DisplayImage"].CopyToAsync(inputStream);
                // stream to byte array
                byte[] array = new byte[inputStream.Length];
                productModel.DisplayImage = array;
                inputStream.Seek(0, SeekOrigin.Begin);
                inputStream.Read(array, 0, array.Length);
                // get file name
                string fName = Request.Form.Files["DisplayImage"].FileName;
            }**/
            Console.WriteLine(Request.Form.Files["DisplayImage"].FileName);
            if (ModelState.IsValid)
            {
                
                _context.Add(productModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,DisplayImage,UserID")] ProductModel productModel)
        {
            if (id != productModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool ProductModelExists(int id)
        {
            return _context.ProductModel.Any(e => e.ID == id);
        }
    }
}
