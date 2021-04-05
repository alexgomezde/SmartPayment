using SmartPayment.Models;
using SmartPayment.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SmartPayment.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Drivers(string message = "", string id = "", string name = "", string lastName = "", string secondLastName = "", string email = "", string password = "", string password2 = "")
        {
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);

            ViewData["id"] = id;
            ViewData["name"] = name;
            ViewData["lastName"] = lastName;
            ViewData["secondLastName"] = secondLastName;
            ViewData["email"] = email;
            ViewData["password"] = password;
            ViewData["password2"] = password2;
            ViewBag.Message = message;
            List<DriversTableViewModel> lst;
            using (SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities())
            {
                lst = (from d in db.CHOFERs
                       select new DriversTableViewModel
                       {
                           Id = d.CHO_IDENTIFICACION,
                           Email = d.CHO_CORREO_ELECTRONICO,
                           Name = d.CHO_NOMBRE,
                           Lastname = d.CHO_PRIMER_APELLIDO,
                           SecondLastname = d.CHO_SEGUNDO_APELLIDO,
                           State = d.CHO_ESTADO
                       }).ToList();

            }
            return View(lst);

        }

        public ActionResult Clients()
        {
            List<ClientsTableViewModel> lst;
            using (SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities())
            {
                lst = (from d in db.CLIENTEs
                       select new ClientsTableViewModel
                       {
                           Id = d.CLI_IDENTIFICACION,
                           Email = d.CLI_CORREO_ELECTRONICO,
                           Name = d.CLI_NOMBRE,
                           Lastname = d.CLI_PRIMER_APELLIDO,
                           SecondLastname = d.CLI_SEGUNDO_APELLIDO,
                           State = d.CLI_ESTADO
                       }).ToList();

            }
            return View(lst);

        }

        public ActionResult Routes(string message = "", string provincia = "", string canton = "", string nombre = "", decimal costo = 0)
        {
            ViewData["provincia"] = provincia;
            ViewData["canton"] = canton;
            ViewData["nombre"] = nombre;
            ViewData["costo"] = costo;
            ViewBag.Message = message;
            List<RoutesTableViewModel> lst;
            using (SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities())
            {
                lst = (from d in db.RUTAs
                       select new RoutesTableViewModel
                       {
                           Code = d.RUT_CODIGO_CTP,
                           Provincia = d.RUT_PROVINCIA,
                           Canton = d.RUT_CANTON,
                           Nombre = d.RUT_NOMBRE,
                           Costo = d.RUT_COSTO,
                           State = d.RUT_ESTADO
                       }).ToList();

            }
            return View(lst);

        }

        public ActionResult AddNewDriver(string id, DateTime dateOfBirth, string name, string lastName, string secondLastName, string email, string password, string password2)
        {

            if (string.IsNullOrEmpty(id) || dateOfBirth == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(secondLastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password2))
            {

                return RedirectToAction("Drivers", "Admin", new { message = "Campos vacíos", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });
            }

            if (dateOfBirth > DateTime.Today)
            {
               
                return RedirectToAction("Drivers", "Admin", new { message = "La fecha de nacimiento no puede ser mayor a la fecha actual", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });
            }

            if (Int32.Parse(id) <= 0)
            {
                return RedirectToAction("Drivers", "Admin", new { message = "El número de identificación debe ser mayor a 0", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });
            }

            if (!IsValidEmailAddress(email.Trim()))
            {
                return RedirectToAction("Drivers", "Admin", new { message = "Correo electrónico inválido", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });
            }

            if (string.Equals(password.Trim(), password2.Trim()))
            {

                if (password.Trim().Length < 8)
                {
                    return RedirectToAction("Drivers", "Admin", new { message = "La longitud de la contraseña debe ser mayor o igual 8", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });

                }
                else if (!IsValidPassword(password.Trim()))
                {
                    return RedirectToAction("Drivers", "Admin", new { message = "La contraseña debe contener números y letras", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });
                }

            }
            else
            {
                return RedirectToAction("Drivers", "Admin", new { message = "Contraseñas no coinciden", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });
            }

            if (IsRegisteredID(id.Trim()))
            {
                return RedirectToAction("Drivers", "Admin", new { message = "Identificación ya se encuentra registrada", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });
            }


            if (IsRegisteredEmail(email.Trim()))
            {
                return RedirectToAction("Drivers", "Admin", new { message = "Correo electrónico ya está registrado", id = id, name = name, lastName = lastName, secondLastName = secondLastName, email = email, password = password, password2 = password2 });
            }

            var chofer = new CHOFER();

            chofer.CHO_IDENTIFICACION= id.Trim();
            chofer.CHO_FECHA_NACIMIENTO = dateOfBirth;
            chofer.CHO_NOMBRE = name.Trim();
            chofer.CHO_PRIMER_APELLIDO = lastName.Trim();
            chofer.CHO_SEGUNDO_APELLIDO = secondLastName.Trim();
            chofer.CHO_CORREO_ELECTRONICO = email.Trim();
            chofer.CHO_CONTRASENNA = password.Trim();
            chofer.CHO_ESTADO = true;

            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            db.Entry(chofer).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

           return RedirectToAction("Drivers", "Admin");

      
        }

        public ActionResult AddNewRoute(string provincia, string canton, string nombre, decimal costo)
        {

            if (string.IsNullOrEmpty(provincia) || string.IsNullOrEmpty(canton) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(costo.ToString()))
            {

                return RedirectToAction("Routes", "Admin", new { message = "Campos vacíos", provincia = provincia, canton = canton, nombre = nombre, costo = costo });
            }


            if(costo < 1)
            {
                return RedirectToAction("Routes", "Admin", new { message = "Costo debe ser mayor a 0", provincia = provincia, canton = canton, nombre = nombre, costo = costo });

            }


            if (IsRegisteredName(nombre.Trim()))
            {
                return RedirectToAction("Routes", "Admin", new { message = "Nombre de la ruta ya existe", provincia = provincia, canton = canton, nombre = nombre, costo = costo });
            }


            var ruta = new RUTA();

            ruta.RUT_PROVINCIA = provincia.Trim();
            ruta.RUT_CANTON = canton.Trim();
            ruta.RUT_NOMBRE = nombre.Trim();
            ruta.RUT_COSTO = costo;
            ruta.RUT_ESTADO = true;

            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            db.Entry(ruta).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Routes", "Admin");


        }


        public ActionResult DisableClient(string id)
        {

            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var client = db.CLIENTEs.FirstOrDefault(x => x.CLI_IDENTIFICACION == id);


            if (client.CLI_ESTADO == true)
            {
                client.CLI_ESTADO = false;
            }
            else {
                client.CLI_ESTADO = true;
            }

            db.Entry(client).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Clients", "Admin");

        }

        public ActionResult DisableDriver(string id)
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var driver = db.CHOFERs.FirstOrDefault(x => x.CHO_IDENTIFICACION == id);


            if (driver.CHO_ESTADO == true)
            {
                driver.CHO_ESTADO = false;
            }
            else
            {
                driver.CHO_ESTADO = true;
            }

            db.Entry(driver).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Drivers", "Admin");

        }

        public ActionResult DisableRoute(int id)
        {

            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var route = db.RUTAs.FirstOrDefault(x => x.RUT_CODIGO_CTP == id);


            if (route.RUT_ESTADO == true)
            {
                route.RUT_ESTADO = false;
            }
            else
            {
                route.RUT_ESTADO = true;
            }

            db.Entry(route).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Routes", "Admin");

        }

        public static bool IsRegisteredEmail(string email)
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var chofer = db.CHOFERs.FirstOrDefault(e => e.CHO_CORREO_ELECTRONICO == email);

            return (chofer != null) ? true : false;

        }

        public static bool IsRegisteredID(string id)
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var chofer = db.CHOFERs.FirstOrDefault(e => e.CHO_IDENTIFICACION == id);

            return (chofer != null) ? true : false;

        }

        public static bool IsRegisteredName(string nombre)
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var ruta = db.RUTAs.FirstOrDefault(e => e.RUT_NOMBRE == nombre);

            return (ruta != null) ? true : false;

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
    }
}