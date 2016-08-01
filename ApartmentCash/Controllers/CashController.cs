using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApartmentCash.Controllers
{
    [Authorize]
    public class CashController : Controller
    {
        // GET: Cash
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string sss)
        {
            return View();
        }
    }
}