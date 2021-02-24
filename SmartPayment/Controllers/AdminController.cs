using SmartPayment.Models;
using SmartPayment.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPayment.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Drivers()
        {
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
                           DateOfBirth = d.CHO_FECHA_NACIMIENTO
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
                           DateOfBirth = d.CLI_FECHA_NACIMIENTO
                       }).ToList();

            }
            return View(lst);

        }
    }
}