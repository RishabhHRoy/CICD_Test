using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(/*string categorySlug = "", int p = 1*/)
        {
            //int pageSize = 3;
            //ViewBag.PageNumber = p;
            //ViewBag.PageRange = pageSize;
            //ViewBag.CategorySlug = categorySlug;

            //if (categorySlug == "")
            //{
            //        ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            //        return View(await _context.Products.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
            //}

            //Category category = await _context.Categories.Where(c => c.Slug == categorySlug).FirstOrDefaultAsync();
            //if (category == null) return RedirectToAction("Index");

            //var productsByCategory = _context.Products.Where(p => p.CategoryId == category.Id);
            //ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsByCategory.Count() / pageSize);

            //return View(await productsByCategory.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
            return View(await _context.Products.Include(c => c.Category).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);

        }

        [HttpPost]
        [ActionName("Detail")]
        public ActionResult Detail(int? id)
        {
            List<Product> products = new List<Product>();
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.Include(c => c.Category).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            //products = HttpContext.Session.Get<List<Product>>("products");
            //if (products == null)
            //{
            //    products = new List<Product>();
            //}
            //products.Add(product);
            //HttpContext.Session.Set("products", product);
            return RedirectToAction(nameof(Index));
        }


        //GET Remove action methdo
        //[ActionName("Remove")]
        //public IActionResult RemoveToCart(int? id)
        //{
        //    List<Product> products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products != null)
        //    {
        //        var product = products.FirstOrDefault(c => c.Id == id);
        //        if (product != null)
        //        {
        //            products.Remove(product);
        //            HttpContext.Session.Set("products", products);
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]

        //public IActionResult Remove(int? id)
        //{
        //    List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products != null)
        //    {
        //        var product = products.FirstOrDefault(c => c.Id == id);
        //        if (product != null)
        //        {
        //            products.Remove(product);
        //            HttpContext.Session.Set("products", products);
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        ////GET product Cart action method

        //public IActionResult Cart()
        //{
        //    List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products == null)
        //    {
        //        products = new List<Products>();
        //    }
        //    return View(products);
        //}


    }
    }
