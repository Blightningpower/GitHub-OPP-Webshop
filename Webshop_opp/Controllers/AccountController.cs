using Microsoft.AspNetCore.Mvc;
using Webshop_opp.Models;
using Webshop_opp.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Webshop_opp.Controllers
{
    public class AccountController : Controller
    {
        Singelton instance = Singelton.GetInstance();
        Account account = new Account();

        public IActionResult CreateAccount()
        {
            if (HttpContext.Request.Cookies["user_id"] != null) 
            {
                Response.Redirect("../Home");
            }
            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Request.Cookies["user_id"] != null)
            {
                Response.Redirect("../Home");
            }
            return View();
        }

        public IActionResult ChangeAccount()
        {
            if (HttpContext.Request.Cookies["user_id"] == null)
            {
                Response.Redirect("../Home");
            }
            return View();
        }

        public IActionResult create_account(AccountViewModel Account)
        {
            if (!String.IsNullOrEmpty(Account.firstName) && !String.IsNullOrEmpty(Account.postalCode) && !String.IsNullOrEmpty(Account.lastName) && !String.IsNullOrEmpty(Account.email) && !String.IsNullOrEmpty(Account.phoneNumber.ToString()) && !String.IsNullOrEmpty(Account.street) && !String.IsNullOrEmpty(Account.houseNumber) && !String.IsNullOrEmpty(Account.city) && !String.IsNullOrEmpty(Account.country) && !String.IsNullOrEmpty(Account.password) && !String.IsNullOrEmpty(Account.province))
            {
                bool EmailExist = account.checkEmailExist(Account.email);
                if (EmailExist)
                {
                    ViewBag.message = "Het email adres bestaad al";
                }
                else
                {
                    account.createAccount(Account.firstName, Account.middleName, Account.lastName, Account.email, Account.phoneNumber, Account.postalCode, Account.street, Account.houseNumber, Account.city, Account.country, Account.password, Account.province);
                    Response.Redirect("Login");
                }              
            }
            else
            {
                ViewBag.message = "Alles moet worden ingevuld";              
            }
            return View("CreateAccount");
        }
        
        public IActionResult login_account(AccountViewModel Account)
        {
            if (!String.IsNullOrEmpty(Account.password) && !String.IsNullOrEmpty(Account.email) )
            {
                string loggedIn = account.login(Account.email, Account.password);
                if (loggedIn == "no")
                {
                    ViewBag.message = "Uw inlog gegevens zijn incorect";                   
                }
                else
                {
                    HttpContext.Response.Cookies.Append("user_id", loggedIn);
                    //HttpContext.Session.SetString("Id", loggedIn);
                    showAccount(loggedIn);
                    //createSessionEmployee("isEmployee", Convert.ToInt32(Account.isEmployee));
                    Console.WriteLine(HttpContext.Request.Cookies["user_id"]);
                    Response.Redirect("../Home");
                }              
            }
            else
            {
                ViewBag.message = "Alles moet worden ingevuld";
            }
            return View("Login");
        }


        public ActionResult showAccount(string id)
        {
            AccountViewModel accountViewModel = new AccountViewModel()
            {
                Accounts = account.showAccount(int.Parse(id))
            };

            return View(accountViewModel);
        }

        public IActionResult log_out()
        {
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("isEmployee");
            return View();
        }
        
        public IActionResult edit_profile()
        {
            ViewBag.ButtonClicked = "eddit_profile";
            return View("ChangeAccount");
        }

        public IActionResult profile_edit(AccountViewModel Account)
        {

            string id = HttpContext.Request.Cookies["user_id"];
            if (!String.IsNullOrEmpty(Account.firstName) && !String.IsNullOrEmpty(Account.postalCode) && !String.IsNullOrEmpty(Account.lastName) && !String.IsNullOrEmpty(Account.email) && !String.IsNullOrEmpty(Account.phoneNumber.ToString()) && !String.IsNullOrEmpty(Account.street) && !String.IsNullOrEmpty(Account.houseNumber) && !String.IsNullOrEmpty(Account.city) && !String.IsNullOrEmpty(Account.country) && !String.IsNullOrEmpty(Account.password) && !String.IsNullOrEmpty(Account.province))
            {
                bool EmailExist = account.checkEmailExist(Account.email);
                if (EmailExist)
                {
                    ViewBag.message = "Het email adres bestaad al";
                }
                else
                {
                    account.editAccount(Account.firstName, Account.middleName, Account.lastName, Account.email, Account.phoneNumber, Account.postalCode, Account.street, Account.houseNumber, Account.city, Account.country, Account.password, Account.province, id);
                }
            }
            else
            {
                ViewBag.message = "Alles moet worden ingevuld";
            }
            return View("ChangeAccount");
        }
    }
}
