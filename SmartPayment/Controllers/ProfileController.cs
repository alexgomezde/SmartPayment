using SmartPayment.Models;
using SmartPayment.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPayment.Controllers
{
    public class ProfileController : Controller
    {
        CLIENTE client = new CLIENTE();
      
        [Authorize]
        // GET: Profile
        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            GetUsername();
            return View();
        }

        public ActionResult Reports(string message = "")
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Reportes = GetReports();
            ViewBag.Message = message;
            return View(mymodel);
        }


        public ActionResult GetUsername()
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            client = db.CLIENTEs.FirstOrDefault(x => x.CLI_CORREO_ELECTRONICO == System.Web.HttpContext.Current.User.Identity.Name);


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


        public ActionResult UpdateCoinPurse(string ammount) 
        {
            if(string.IsNullOrEmpty(ammount))
            {
                return RedirectToAction("Index", "Profile", new { message = "Campos en blanco" });
            }

            if(Decimal.Parse(ammount) > 0 )
            {
                SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

                client = db.CLIENTEs.FirstOrDefault(x => x.CLI_CORREO_ELECTRONICO == System.Web.HttpContext.Current.User.Identity.Name);

                client.CLI_MONEDERO += Decimal.Parse(ammount);

                db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                ViewData["monedero"] = client.CLI_MONEDERO;

                return RedirectToAction("Index", "Profile");

            } else
            {

                return RedirectToAction("Index", "Profile", new { message = "Debe ingresar un monto mayor a 0" });
            }
            
        }

        public List<ReportsClientTableVM> GetReports()
        {
            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();
            client = db.CLIENTEs.FirstOrDefault(x => x.CLI_CORREO_ELECTRONICO == System.Web.HttpContext.Current.User.Identity.Name);

            

            List<PaymentsTableViewModel> lstPayment;

            lstPayment = (from d in db.PAGOes
                        select new PaymentsTableViewModel
                        {
                            Id = d.PAG_ID,
                            Id_Client = d.PAG_IDENTIFICACION_CLIENTE,
                            Id_Driver = d.PAG_IDENTIFICACION_CHOFER,
                            Id_Route = d.PAG_CODIGO_CTP_RUTA,
                            DateOfPayment = d.PAG_FECHA,
                            State = d.RECHAZADO
                        }).ToList();

            List<RoutesTableViewModel> lstRoutes;

            lstRoutes = (from d in db.RUTAs
                   select new RoutesTableViewModel
                   {
                       Code = d.RUT_CODIGO_CTP,
                       Provincia = d.RUT_PROVINCIA,
                       Canton = d.RUT_CANTON,
                       Nombre = d.RUT_NOMBRE,
                       Costo = d.RUT_COSTO,
                       State = d.RUT_ESTADO
                   }).ToList();



            List<PaymentsTableViewModel> filteredListPayment = new List<PaymentsTableViewModel>();
            List<ReportsClientTableVM> filteredReports = new List<ReportsClientTableVM>();

            

            foreach ( var payment in lstPayment)
            {
                
                if (client.CLI_IDENTIFICACION.Equals(payment.Id_Client))
                {
                    ReportsClientTableVM newEntryReport = new ReportsClientTableVM();

                    foreach( var route in lstRoutes)
                    {
                        if(payment.Id_Route == route.Code)
                        {
                            newEntryReport.Id = payment.Id;
                            newEntryReport.Nombre = route.Nombre;
                            newEntryReport.Costo = route.Costo;
                            newEntryReport.DateOfPayment = payment.DateOfPayment;
                            newEntryReport.State = payment.State;

                            filteredReports.Add(newEntryReport);
                        }
                    }

                    
                }
            }

            return filteredReports;

        }
    }
}