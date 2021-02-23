using SmartPayment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPayment.Controllers
{
    public class ProfileController : Controller
    {

        [Authorize]
        // GET: Profile
        public ActionResult Index()
        {
            GetUsername();
            return View();
        }

        public ActionResult GetUsername()
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var client = db.CLIENTEs.FirstOrDefault(x => x.CLI_CORREO_ELECTRONICO == System.Web.HttpContext.Current.User.Identity.Name);


            string dob = client.CLI_FECHA_NACIMIENTO.ToString("yyyy-MM-dd");


            ViewData["id"] = client.CLI_IDENTIFICACION;
            ViewData["dateOfBirth"] = dob;
            ViewData["name"] = client.CLI_NOMBRE;
            ViewData["lastName"] = client.CLI_PRIMER_APELLIDO;
            ViewData["secondLastName"] = client.CLI_SEGUNDO_APELLIDO;
            ViewData["paymentMethod"] = client.CLI_METODO_PAGO;
            ViewData["email"] = client.CLI_CORREO_ELECTRONICO;
            ViewData["password"] = client.CLI_CONTRASENNA;
            ViewData["password2"] = client.CLI_CONTRASENNA;
            ViewData["monedero"] = client.CLI_MONEDERO;
            return View();
        }
    }
}