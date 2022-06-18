using Microsoft.AspNetCore.Mvc;
using Webshop_opp.Models;
using Webshop_opp.ViewModels;

namespace Webshop_opp.Controllers
{
    public class ShopController : Controller
    {
        Category category = new Category();
        Product product = new Product();       

        public IActionResult Index()
        {
            showProduct();
            return View();
        }

        public IActionResult ShopingCard()
        {
            if (HttpContext.Request.Cookies["Product_id"] != null)
            {
                showProductInProductCard();
            }
            else
            {
                ViewBag.messege = "Er staat niks in de winkelmand";
            }       
            return View();
        }

        public IActionResult MakeOrder()
        {
            return View();
        }

        public ActionResult showProduct()
        {
            AdminProductViewModel adminProductViewModel = new AdminProductViewModel()
            {
                Products = product.showProduct()
            };
            return View(adminProductViewModel);
        }

        [HttpPost]
        public IActionResult add_product(int button)
        {
            showProduct();
            string products = "";
            bool control = true;
            if (HttpContext.Request.Cookies["Product_id"] != null)
            {
                products = HttpContext.Request.Cookies["Product_id"];
                string[] textSplit = products.Split(" ");
                for (int i = 0; i < textSplit.Count(); i++)
                {
                    if (textSplit[i] == button.ToString())
                    {
                        control = false;
                    }
                }
            }
           
            //char[] myCharArray = myString.ToCharArray();
            //HttpContext.Response.Cookies.Append("producten", array);
            if (control)
            {
                HttpContext.Response.Cookies.Append("Product_id", products + " " + button.ToString());
            }
            return View("Index");
        }

        public IActionResult order(AccountViewModel Account)
        {
            ShoppingCard shoppingCard = new ShoppingCard(HttpContext.Request.Cookies["Product_id"]);
            bool check = shoppingCard.checkLogin(HttpContext.Request.Cookies["user_id"]);
            if (check)
            {               
                shoppingCard.make_order(Account.postalCode, Account.street, Account.houseNumber, Account.city, Account.country, Account.province, HttpContext.Request.Cookies["user_id"]);
                Response.Redirect("../Home");
            }
            else
            {
                ViewBag.messege = "U moet eerste zijn ingelogd om af te kunnen rekenen";
            }
            
            return View("MakeOrder");
        }

        public ActionResult showProductInProductCard()
        {
            ShoppingCard shoppingCard = new ShoppingCard(HttpContext.Request.Cookies["Product_id"]);
            AdminProductViewModel adminProductViewModel = new AdminProductViewModel()
            {
                ShoppingCard = shoppingCard.showProduct()
            };

            return View(adminProductViewModel);
        }
    }
}
