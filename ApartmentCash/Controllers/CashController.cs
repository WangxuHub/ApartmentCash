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
        public ActionResult Add(ApartmentCash.Models.CashViewModel model, ApartmentCash.Models.ApplicationUser curUser)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();

            DBModel.PersonPayLog obj = new DBModel.PersonPayLog() {
                UserID = curUser.Id,
                PayDate=DateTime.Now,
                PaySummary=model.PaySummary,
                PayMoney=model.PayMoney,
                AddTime=DateTime.Now,
                AddUser=curUser.Id,
                CheckStatus=ApartmentCash.Models.eCheckStatus.apply.ToString()
            };

            ef.PersonPayLog.Add(obj);
            ef.SaveChanges();
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}