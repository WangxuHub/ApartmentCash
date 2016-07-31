using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApartmentCash.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "公寓管理系统，更加方便高效的管理~~~.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "联系.";

            return View();
        }
    }
}