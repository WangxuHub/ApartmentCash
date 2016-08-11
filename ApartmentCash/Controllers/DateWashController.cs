using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApartmentCash.Controllers
{
    [Authorize(Users ="管理员")]
    public class DateWashController : Controller
    {
        [AllowAnonymous]
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



        [HttpPost]
        public ActionResult CreateDate(Models.CreateDateModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.DateStart > model.DateEnd) {
                ModelState.AddModelError("","开始时间不能大于结束时间");
                return View(model);
            }

            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();
            int count = ef.DateWash.Count();
            if (count > 0)
            {
                DateTime maxDate = DateTime.Parse(ef.DateWash.Max(item => item.DateStr));
                DateTime minDate = DateTime.Parse(ef.DateWash.Min(item => item.DateStr));

                if ((minDate <= model.DateStart && model.DateStart <= maxDate) ||
                    (minDate <= model.DateEnd && model.DateEnd <= maxDate)
                   ) {
                    ModelState.AddModelError("","开始时间或结束时间与现有数据发生交错");
                    return View(model);
                }


            }

            DateTime dt = model.DateStart;
            while(true)
            {
                DBModel.DateWash obj = new DBModel.DateWash() {
                    DateStr = dt.ToString("yyyy-MM-dd")
                };
                ef.DateWash.Add(obj);
                dt = dt.AddDays(1);
                if (dt > model.DateEnd)
                {
                    break;
                }
            }

            ef.SaveChanges();


            return RedirectToAction("index");
        }
    }
}