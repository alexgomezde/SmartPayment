using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SmartPayment.Models;

namespace SmartPayment.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();


            var cliente = db.CLIENTEs.FirstOrDefault(e => e.CLI_CORREO_ELECTRONICO == email && e.CLI_CONTRASENNA == password);

            if (cliente != null)
            {
                FormsAuthentication.SetAuthCookie(cliente.CLI_CORREO_ELECTRONICO, true);
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                return RedirectToAction("Index", new { message = "Correo electrónico y/o contraseña" });
            }
 
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}