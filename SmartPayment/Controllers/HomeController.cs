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
        public ActionResult Login(string email, string password )
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();


            var cliente = db.CLIENTEs.FirstOrDefault(e => e.CLI_CORREO_ELECTRONICO == email && e.CLI_CONTRASENNA == password);
            var admin = db.ADMINISTRADORs.FirstOrDefault(e => e.ADM_CORREO_ELECTRONICO == email && e.ADM_CONTRASENNA == password);
            var driver = db.CHOFERs.FirstOrDefault(e => e.CHO_CORREO_ELECTRONICO == email && e.CHO_CONTRASENNA == password);


            if (cliente != null)
            {
                FormsAuthentication.SetAuthCookie(cliente.CLI_CORREO_ELECTRONICO, true);
                return RedirectToAction("Index", "Profile");
            }
            else if (admin != null)
            {
                FormsAuthentication.SetAuthCookie(admin.ADM_CORREO_ELECTRONICO, true);
                return RedirectToAction("Index", "Admin");

            }
            else if (driver != null)
            {
                FormsAuthentication.SetAuthCookie(driver.CHO_CORREO_ELECTRONICO, true);
                return RedirectToAction("Index", "Admin");
            }
            else {

                return RedirectToAction("Index", new { message = "Correo electrónico y/o contraseña inválidos" });
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

        public ActionResult Register()
        {
            ViewBag.Message = "Your Register page.";

            return View();
        }

        [HttpPost]
        public ActionResult AddUser(string id, DateTime dateOfBirth, string name, string lastName, string secondLastName, string paymentMethod, string email, string password, string password2)
        {

            var client = new CLIENTE();

            client.CLI_IDENTIFICACION = id;
            client.CLI_FECHA_NACIMIENTO = dateOfBirth;
            client.CLI_NOMBRE = name;
            client.CLI_PRIMER_APELLIDO = lastName;
            client.CLI_SEGUNDO_APELLIDO = secondLastName;
            client.CLI_METODO_PAGO = paymentMethod;
            client.CLI_CORREO_ELECTRONICO = email;
            client.CLI_CONTRASENNA = password;
            client.CLI_MONEDERO = 0;
            client.CLI_TIPO = "cliente";

            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            db.Entry(client).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
            // var cliente = db.CLIENTEs.FirstOrDefault(e => e.CLI_CORREO_ELECTRONICO == email && e.CLI_CONTRASENNA == password);

        }

    }
}