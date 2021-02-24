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
        [Authorize]
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

        public ActionResult AddNewDriver(string id, DateTime dateOfBirth, string name, string lastName, string secondLastName, string email, string password, string password2)
        {
            var chofer = new CHOFER();

            chofer.CHO_IDENTIFICACION= id;
            chofer.CHO_FECHA_NACIMIENTO = dateOfBirth;
            chofer.CHO_NOMBRE = name;
            chofer.CHO_PRIMER_APELLIDO = lastName;
            chofer.CHO_SEGUNDO_APELLIDO = secondLastName;
            chofer.CHO_CORREO_ELECTRONICO = email;
            chofer.CHO_CONTRASENNA = password;
            chofer.CHO_TIPO = "chofer";

            SMART_PAYMENT_DBEntities db = new SMART_PAYMENT_DBEntities();

            db.Entry(chofer).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

           return RedirectToAction("Drivers", "Admin");

      
        }
    }
}