using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.Web.Mvc;

namespace ApartmentCash.Controllers
{
    [Authorize(Users ="管理员")]
    public class DateWashController : Controller
    {
        [AllowAnonymous]
        // GET: DateWash
        public ActionResult Index(int id=1)
        {
            int start = id * 10;
            int end = (id - 1) * 10;
            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();

            List<Models.DateWashViewModel> list = (
                                               from a in ef.DateWash
                                               join b in ef.AspNetUsers.DefaultIfEmpty() on a.UserID equals b.Id into temp
                                               orderby a.DateStr
                                               from t in temp.DefaultIfEmpty()
                                               select new Models.DateWashViewModel()
                                               {
                                                   DateStr = a.DateStr,
                                                   UserID = a.UserID,
                                                   IsFinish= a.IsFinish,
                                                   UserName = t.UserName
                                               }).OrderBy(item=>item.DateStr).Take(start).Skip(end).ToList();


            List<DBModel.AspNetUsers> userList = ef.AspNetUsers.Where(item=>item.UserName!="管理员").OrderBy(item => item.UserName).ToList();
            ViewBag.userList = userList;
            return View(list);
        }

        public ActionResult CreateDate()
        {
            return View();
        }

        private Common.mJsonResult json = new Common.mJsonResult();



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

        [HttpPost]
        public ActionResult CreateDateWashPerson()
        {
            int createCount = Request["createCount"].ToInt();
            string [] personList = Request["personList"].Split(',');
            string startDate = Request["startDate"];

            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();

            System.Data.Entity.Database db = ef.Database;

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(db.Connection.ConnectionString))
            {
                conn.Open();
                System.Data.SqlClient.SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    for (int i = 0; i < createCount; i++)
                    {
                        string userID = personList[i % personList.Length];
                        string dateStr = DateTime.Parse(startDate).AddDays(i).ToString("yyyy-MM-dd");
                        string sql = "update DateWash set UserID=@UserID where DateStr=@DateStr";

                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conn, trans);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@DateStr", dateStr);
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();

                }
                catch (Exception ee)
                {
                    trans.Rollback();
                    json.success = false;
                    json.msg = ee.Message;
                    if (ee.InnerException != null)
                    {
                        json.msg += "  " + ee.InnerException.Message;
                    }
                }
            }




            json.success = true;
            json.msg = "生成成功";
            return Content(json.ToString());
        }
    }
}