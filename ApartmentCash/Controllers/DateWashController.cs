using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApartmentCash.Controllers
{
    public class DateWashController : Controller
    {
        // GET: DateWash
        public ActionResult Index()
        {
            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();

            List<Models.DateWashViewModel> list = (
                                               from a in ef.DateWash
                                               join b in ef.AspNetUsers on a.UserID equals b.Id
                                               select new Models.DateWashViewModel()
                                               {
                                                   DateStr = a.DateStr,
                                                   UserID = a.UserID,
                                                   IsFinish= a.IsFinish,
                                                   UserName = b.UserName
                                               }).ToList();

            return View(list);
        }

        public ActionResult CreateDate()
        {
            return View();
        }
    }
}