using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PcSoluciones.Controllers
{
    public class HomeController : Controller
    {
        // Usuarios y contraseñas codificados directamente
        private Dictionary<string, string> users = new Dictionary<string, string>
        {
            { "administrador", "abretecesamo" },
            { "usuario2", "contraseña2" },
            { "mtoapanta", "Toapanta123" },
            { "jdpincha", "Pincha123" },
            { "mtaco", "Taco123" },
            { "sfranchesco", "Franchesco123" },
        };

        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            if (Session["User"] != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (users.Any(u => u.Key == username && u.Value == password))
            {
                Session["User"] = username;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.LoginError = "Nombre de usuario o contraseña incorrectos";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
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
