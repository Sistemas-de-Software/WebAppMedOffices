using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMedOffices.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }

            if (User.IsInRole("Secretaria"))
            {
                return RedirectToAction("Index", "GestionTurnos");
            }

            if (User.IsInRole("Medico"))
            {
                return RedirectToAction("ListarPacientesHoy", "FichaMedica");
            }

            return View();
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