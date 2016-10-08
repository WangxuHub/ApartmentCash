using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.Web.Mvc;

namespace ApartmentCash.Controllers
{
    [Authorize(Users ="管理员")]
    //[Route("DateWash/{action}")]
    public class DateWashController : Controller
    {
        [AllowAnonymous]
        [Route("DateWash/{year:int}/{month:int}")]
        [Route("DateWash/{year:int}")]
        [Route("DateWash")]
        // GET: DateWash
        public ActionResult Index(int year=0,int month=0)
        {
            if(year<=0)
            {
                year = DateTime.Now.Year;
            }
            if(month <= 0)
            {
                month = DateTime.Now.Month;
            }

            int start = month * 10;
            int end = (month - 1) * 10;
            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();


            string dateStr = new DateTime(year, month, 1).ToString("yyyy-MM");
            List<Models.DateWashViewModel> list = (
                                               from a in ef.DateWash
                                               join b in ef.AspNetUsers.DefaultIfEmpty() on a.UserID equals b.Id into temp
                                               where a.DateStr.Contains(dateStr)
                                               orderby a.DateStr
                                               from t in temp.DefaultIfEmpty()
                                               select new Models.DateWashViewModel()
                                               {
                                                   DateStr = a.DateStr,
                                                   UserID = a.UserID,
                                                   IsFinish = a.IsFinish,
                                                   UserName = t.UserName
                                               }).OrderBy(item => item.DateStr).ToList();//.Take(start).Skip(end).ToList();


            List<DBModel.AspNetUsers> userList = ef.AspNetUsers.Where(item=>item.UserName!="管理员").OrderBy(item => item.UserName).ToList();
            ViewBag.userList = userList;
            ViewBag.month =ViewBag.pageIndex = month;
            ViewBag.year = year;
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

        //按人员排洗
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

        //轮空处理
        [HttpPost]
        public ActionResult NoInTrun()
        {
            string dateStr = Request["dateStr"];

            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();

            System.Data.Entity.Database db = ef.Database;

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(db.Connection.ConnectionString))
            {
                conn.Open();
                System.Data.SqlClient.SqlTransaction trans = conn.BeginTransaction();
             
                try
                {


                    string sql2 = string.Format(@"
                        update  a
                        set a.UserID=b.UserID
                        from DateWash a
                        inner join DateWash b on a.DateWashID=b.DateWashID+1
                        where a.DateStr>'{0}'", dateStr);

                    System.Data.SqlClient.SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand(sql2, conn, trans);
                    cmd2.ExecuteNonQuery();

                    string sql = string.Format("update DateWash set UserID=null where DateStr='{0}'",dateStr);

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conn, trans);
                    cmd.ExecuteNonQuery();


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
            json.msg = "操作成功";
            return Content(json.ToString());
        }

        #region 日历
        [ChildActionOnly]
        [AllowAnonymous]
        [Route("DateWash/CalendarShow/{year:int}/{month:int}/{day:int}")]
        [Route("DateWash/CalendarShow/{year:int}/{month:int}")]
        [Route("DateWash/CalendarShow/{year:int}")]
        [Route("DateWash/CalendarShow")]
        public ActionResult CalendarShow(int year=0,int month=0,int day=0)
        {
            DateTime date = DateTime.Now;
            if (year <= 0) year = date.Year;
            if (month <= 0) month = date.Month;
            if (day <= 0) day = date.Day;

            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.day = day;
            

            int start = month * 10;
            int end = (month - 1) * 10;
            DBModel.ApartmentCashEntities ef = new DBModel.ApartmentCashEntities();


            string dateStr = new DateTime(year, month, 1).ToString("yyyy-MM");
            string dateStr1 = new DateTime(year, month - 1, 1).ToString("yyyy-MM");
            string dateStr2 = new DateTime(year, month + 1, 1).ToString("yyyy-MM");
            List<Models.DateWashViewModel> list = (
                                               from a in ef.DateWash
                                               join b in ef.AspNetUsers.DefaultIfEmpty() on a.UserID equals b.Id into temp
                                               where a.DateStr.Contains(dateStr) || a.DateStr.Contains(dateStr1) || a.DateStr.Contains(dateStr2)
                                               
                                               orderby a.DateStr
                                               from t in temp.DefaultIfEmpty()
                                               select new Models.DateWashViewModel()
                                               {
                                                   DateStr = a.DateStr,
                                                   UserID = a.UserID,
                                                   IsFinish = a.IsFinish,
                                                   UserName = t.UserName
                                               }).OrderBy(item => item.DateStr).ToList();//.Take(start).Skip(end).ToList();

            ViewBag.listJson = list.ToJson();

            return PartialView("~/Views/DateWash/_PartialDateShow.cshtml");
        }
        #endregion


        [AllowAnonymous]
        [Route("DateWash/Test/{year:int}/{month:int}/{day:int}")]
        [Route("DateWash/Test/{year:int}/{month:int}")]
        [Route("DateWash/Test/{year:int}")]
        [Route("DateWash/Test")]
        public ActionResult Test(int year = 0, int month = 0, int day = 0)
        {
            DateTime date = DateTime.Now;
            if (year <= 0) year = date.Year;
            if (month <= 0) month = date.Month;
            if (day <= 0) day = date.Day;

            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.day = day;
            return View();
        }

    }
}