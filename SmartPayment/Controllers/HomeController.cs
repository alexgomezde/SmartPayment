using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            bool validForm = true;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                validForm = false;
                return RedirectToAction("Index", new { message = "Espacios en blanco" });

            }
            else if (!IsValidEmailAddress(email)) 
            {
                validForm = false;
                return RedirectToAction("Index", new { message = "Debe ingresar un correo electrónico válido" });
            }


            if (validForm)
            {
                SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

                var cliente = db.CLIENTEs.FirstOrDefault(e => e.CLI_CORREO_ELECTRONICO == email && e.CLI_CONTRASENNA == password);
                var admin = db.ADMINISTRADORs.FirstOrDefault(e => e.ADM_CORREO_ELECTRONICO == email && e.ADM_CONTRASENNA == password);
                var driver = db.CHOFERs.FirstOrDefault(e => e.CHO_CORREO_ELECTRONICO == email && e.CHO_CONTRASENNA == password);


                if (cliente != null)
                {
                    if (cliente.CLI_ESTADO == true)
                    {
                        FormsAuthentication.SetAuthCookie(cliente.CLI_CORREO_ELECTRONICO, true);
                        return RedirectToAction("Index", "Profile");
                    }
                    else
                    {

                        return RedirectToAction("Index", new { message = "Cuenta deshabilitada. Por favor contactar al administrador" });
                    }

                }
                else if (admin != null)
                {

                    FormsAuthentication.SetAuthCookie(admin.ADM_CORREO_ELECTRONICO, true);
                    return RedirectToAction("Index", "Admin");

                }
                else if (driver != null)
                {

                    if (driver.CHO_ESTADO == true)
                    {
                        FormsAuthentication.SetAuthCookie(driver.CHO_CORREO_ELECTRONICO, true);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {

                        return RedirectToAction("Index", new { message = "Cuenta deshabilitada. Por favor contactar al administrador" });
                    }

                }
                else
                {

                    return RedirectToAction("Index", new { message = "Correo electrónico y/o contraseña inválidos" });
                }

            }
            else {

                return RedirectToAction("Index", "Home");
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

        public ActionResult Register(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(string id, DateTime dateOfBirth, string name, string lastName, string secondLastName, string paymentMethod, string email, string password, string password2)
        {
            bool validForm = true;

            if (string.IsNullOrEmpty(id) || dateOfBirth == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(secondLastName) || string.IsNullOrEmpty(paymentMethod) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password2))
            {
                validForm = false;
                ViewData["lblErrorMessage"] = "Espacios vacíos";
                //return Redirect(Request.Url.AbsoluteUri);
                return RedirectToAction("Register", new { message = "Campos vacíos" });
            }



            if (!IsValidEmailAddress(email.Trim()))
            {
                validForm = false;
                //ViewData["lblErrorMessage"] = "Correo Incorrecto";
                return RedirectToAction("Register", new { message = "Correo electrónico inválido" });
            }

            if (Int32.Parse(id) <= 0)
            {
                validForm = false;
                return RedirectToAction("Register", new { message = "El número de identificación debe ser mayor a 0" });
            }

            if (Int32.Parse(paymentMethod) <= 0)
            {
                validForm = false;
                return RedirectToAction("Register", new { message = "El método de pago debe ser mayor a 0" });
            }


            if (string.Equals(password.Trim(), password2.Trim()))
            {

                if (password.Trim().Length < 8)
                {
                    validForm = false;
                    return RedirectToAction("Register", new { message = "La longitud de la contraseña debe ser mayor o igual 8" });

                }
                else if (!IsValidPassword(password.Trim()))
                {
                    validForm = false;
                    return RedirectToAction("Register", new { message = "La contraseña debe contener números y letras" });
                }

            }
            else
            {
                validForm = false;
                return RedirectToAction("Register", new { message = "Contraseñas no coinciden" });

            }


            if (IsRegisteredEmail(email.Trim()))
            {
                validForm = false;
                return RedirectToAction("Register", new { message = "Correo electrónico ya está registrado" });
            }


            if (IsRegisteredID(id.Trim()))
            {
                validForm = false;
                return RedirectToAction("Register", new { message = "Identificación ya se encuentra registrada" });
            }

            if (validForm)
            {

                var client = new CLIENTE();

                client.CLI_IDENTIFICACION = id.ToString();
                client.CLI_FECHA_NACIMIENTO = dateOfBirth;
                client.CLI_NOMBRE = name.Trim();
                client.CLI_PRIMER_APELLIDO = lastName.Trim();
                client.CLI_SEGUNDO_APELLIDO = secondLastName.Trim();
                client.CLI_METODO_PAGO = paymentMethod.Trim();
                client.CLI_CORREO_ELECTRONICO = email.Trim();
                client.CLI_CONTRASENNA = password.Trim();
                client.CLI_MONEDERO = 0;
                client.CLI_ESTADO = true;

                SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

                db.Entry(client).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Register", new { message = "Nada" });
            }
            

            

        }

        public static bool IsValidEmailAddress(string emailAddress)
        {
            string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match match = Regex.Match(emailAddress.Trim(), pattern, RegexOptions.IgnoreCase);

            return match.Success;
        }

        public static bool IsValidPassword(string password)
        {
            string pattern = @"^[a-zA-Z][a-zA-Z0-9]*$";
            Match match = Regex.Match(password.Trim(), pattern, RegexOptions.IgnoreCase);

            return match.Success;
        }


        public static bool IsRegisteredEmail(string email)
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var cliente = db.CLIENTEs.FirstOrDefault(e => e.CLI_CORREO_ELECTRONICO == email);

            return (cliente != null) ? true : false;

        }

        public static bool IsRegisteredID(string id)
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var cliente = db.CLIENTEs.FirstOrDefault(e => e.CLI_IDENTIFICACION == id);

            return (cliente != null) ? true : false;

        }

    }
}