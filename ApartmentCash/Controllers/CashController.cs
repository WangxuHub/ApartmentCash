using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ApartmentCash.Controllers
{
    [Authorize]
    public class CashController : Controller
    {
        // GET: Cash
        public ActionResult Index()
        {
            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();

            ;//   List<DBModel.PersonPayLog> list = ef.PersonPayLog.ToList();

            List<Models.CashViewModel> list = (from a in ef.PersonPayLog
                                               join b in ef.AspNetUsers on a.UserID equals b.Id
                                               select new Models.CashViewModel() {
                                                   PayDate=a.PayDate,
                                                   PayMoney=a.PayMoney,
                                                   PaySummary=a.PaySummary,
                                                   CheckStatus=a.CheckStatus,
                                                   UserID=a.UserID,
                                                   UserName=b.UserName
                                               }).ToList();

            return View(list);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(ApartmentCash.Models.CashViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();

            string userID = User.Identity.GetUserId();
            DBModel.PersonPayLog obj = new DBModel.PersonPayLog() {
                UserID = userID,
                PayDate =DateTime.Now,
                PaySummary=model.PaySummary,
                PayMoney=model.PayMoney,
                AddTime=DateTime.Now,
                AddUser= userID,
                CheckStatus =ApartmentCash.Models.eCheckStatus.apply.ToString()
            };

            ef.PersonPayLog.Add(obj);
            ef.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}