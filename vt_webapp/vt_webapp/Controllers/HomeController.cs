using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vt_webapp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Console.WriteLine("Index aufgerufen");
            return View();
        }

        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            Console.WriteLine("about aufgerufen");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            Console.WriteLine("contact aufgerufen");
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }
    }
}