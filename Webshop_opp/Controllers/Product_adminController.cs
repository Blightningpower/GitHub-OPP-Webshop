using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System.Data;
using Webshop_opp.Models;
using Webshop_opp.ViewModels;

namespace Webshop_opp.Controllers
{
    public class Product_adminController : Controller
    {
        Category category = new Category();
        Product product = new Product();
        // GET: Product_adminController
        public ActionResult Index()
        {
            //if (HttpContext.Session.GetString("isEmployee") == null && HttpContext.Session.GetString("isEmployee") != "True")
            //{
                //Response.Redirect("../Home");
            //}
            return View();
        }

        public IActionResult edit_product()
        {
            //AdminProductViewModel adminProductViewModel = new AdminProductViewModel()
            //{
            //    Categories = category.GetCategories()
            //};
            //return View(adminProductViewModel);
            return View("Index");
        }

        public IActionResult create_category()
        {
            ViewBag.ButtonClicked = "create_category";
            return View("Index");
        }

        public IActionResult category_create(AdminProductViewModel Category)
        {
            category.create_caterory(Category.Category_name_input);
            return View("Index");
        }

        public IActionResult delete_category()
        {          
            ViewBag.ButtonClicked = "delete_category";
            showCategory();
            return View("Index");
        }

        public ActionResult showCategory()
        {
            AdminProductViewModel adminProductViewModel = new AdminProductViewModel()
            {
                Categories = category.GetCategories()
            };

            return View(adminProductViewModel);
        }

        public IActionResult category_delete()
        {
            category.delete_caterory(Int32.Parse(Request.Form["categoryName"]));
            return View("Index");
        }

        public IActionResult create_product()
        {

            ViewBag.ButtonClicked = "create_product";
            showCategory();
            return View("Index");
        }

        [HttpPost]
        public IActionResult product_create(IFormFile file , AdminProductViewModel Product)
        {
            if (!String.IsNullOrEmpty(Product.Product_name_input) && !String.IsNullOrEmpty(Product.Product_description_input) && !String.IsNullOrEmpty(Product.Product_price_input.ToString()) && !String.IsNullOrEmpty(Product.Product_height_input.ToString()) && !String.IsNullOrEmpty(Product.Product_width_input.ToString()) && !String.IsNullOrEmpty(Product.Product_weight_input.ToString()))
            {
                using var image = Image.Load(file.OpenReadStream());
                //image.Mutate(x => x.Resize(256, 256));
                string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                image.Save("wwwroot/Images/" + filename);
                int categoryId = Int32.Parse(Request.Form["categoryName"]);
                product.createProduct(filename, Product.Product_name_input, Product.Product_description_input, Product.Product_price_input, Product.Product_height_input, Product.Product_width_input, Product.Product_weight_input, categoryId);
            }
            else
            {
                ViewBag.message = "Alles moet worden ingevuld";
            }
            return View("Index");
        }

        public IActionResult delete_product()
        {

            ViewBag.ButtonClicked = "delete_product";
            showProduct();
            return View("Index");
        }

        public ActionResult showProduct()
        {
            AdminProductViewModel adminProductViewModel = new AdminProductViewModel()
            {
                Products = product.showProduct ()
            };

            return View(adminProductViewModel);
        }

        public IActionResult Product_delete()
        {
            product.deleteProduct(Int32.Parse(Request.Form["productName"]));
            return View("Index");
        }
    }
}
