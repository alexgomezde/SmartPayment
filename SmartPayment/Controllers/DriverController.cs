using SmartPayment.Models;
using SmartPayment.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartPayment.Controllers
{
    public class DriverController : Controller
    {
        // GET: Driver
        public static DateTime Today { get; }
        public static string currentEmail;
        public ActionResult Index( string message = "", string email = "" , string cedula ="",  string ruta = "")
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Rutas = GetRoutes();
            mymodel.Clientes = GetClients();
            currentEmail = email;
            ViewData["cedula"] = cedula;
            ViewData["ruta"] = ruta;
            ViewBag.Message = message;
            return View(mymodel);
        }

        public List<RoutesTableViewModel> GetRoutes()
        {
 
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
            return lst;

        }

        public List<ClientsTableViewModel> GetClients()
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
            return lst;

        }

        public ActionResult MakePayment(string cedula, string ruta)
        {

            if (string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(ruta))
            {
                return RedirectToAction("Index", new { message = "Campos en blanco", email = currentEmail, cedula = cedula, ruta = ruta });
            }

            int rutaId = Int32.Parse(ruta);
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            var client = db.CLIENTEs.FirstOrDefault(x => x.CLI_IDENTIFICACION == cedula);
            var route = db.RUTAs.FirstOrDefault(x => x.RUT_CODIGO_CTP == rutaId);
            var driver = db.CHOFERs.FirstOrDefault(x => x.CHO_CORREO_ELECTRONICO == currentEmail);

            decimal total = client.CLI_MONEDERO - route.RUT_COSTO;
            DateTime today = System.DateTime.Now;

            var pago = new PAGO();

            pago.PAG_IDENTIFICACION_CLIENTE = client.CLI_IDENTIFICACION;
            pago.PAG_IDENTIFICACION_CHOFER = driver.CHO_IDENTIFICACION;
            pago.PAG_CODIGO_CTP_RUTA = route.RUT_CODIGO_CTP;
            pago.PAG_FECHA = today;
            pago.RECHAZADO = false;


            if (total < 0)
            {
                pago.RECHAZADO = true;

                db.Entry(pago).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index", new { message = "Saldo Insuficiente", email = currentEmail, cedula = cedula, ruta = ruta });
            }
            else
            {
                client.CLI_MONEDERO = total;
                db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                db.Entry(pago).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("Index", new { message = "Pago Exitoso", email = currentEmail, cedula = cedula, ruta = ruta });
            }

        }

        
    }
}