using ExcelVacationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelVacationSystem.Controllers
{
    public class fullCalenderController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();

        // GET: fullCalender
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult checkInnerVacations(int ssn,DateTime start,DateTime end)
        {
            try
            {
                int flag = -1;
                var request = db.requests.Where(s => s.empID == ssn).ToList();
                //    .Select(c => new vacation
                //{
                //    fromDate = c.vacation.fromDate,
                //    toDate = c.vacation.toDate
                //});

                if(request.Count()<1)
                {
                    flag = 0; // can request
                }
                foreach (var item in request)
                {
                    foreach (var itemTo in request)
                    {
                        if (item.vacation.fromDate <= start && itemTo.vacation.toDate >= end)
                        {
                            flag = 1; // inner vacation
                        }
                        else
                        {
                            flag = 0; // can request
                        }
                    }
                   
                }

                return Json(flag, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {

                throw e;
            }
           
        }

        public JsonResult GetEvents()
        {
               // var unApprovedEvents = db.requests.SingleOrDefault(c => c.approval == 0);

                var events = db.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            int lastID;
                vacation vac = new vacation();

            if (e.EventID > 0)
                {
                    //Update the event
                    var v = db.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                        v.isCasual = e.isCasual;
                        v.isRegular = e.isRegular;
                        v.isPermission = e.isPermission;
                    }
                }
                else
                {
                    db.Events.Add(e);
                vac.fromDate = e.Start;
                vac.toDate = e.End;
                vac.Reason = e.Description;
                vac.num_days = (int)e.days;
                vac.isCasual =(bool) e.isCasual;
                vac.isRegular = (bool)e.isRegular;
                vac.isPermission = (bool)e.isPermission;
                vac.hours = e.hours;
                db.vacations.Add(vac);
                
                Session["from"] = (vac.fromDate).ToString();
                Session["to"] = (vac.toDate).ToString();
                Session["days"] = (vac.num_days).ToString();
                Session["cas"] = (vac.isCasual).ToString();
                Session["reg"] = (vac.isRegular).ToString();
                Session["permisstion"] = (vac.isPermission).ToString();
                Session["hours"] = (vac.hours).ToString();
                if (string.IsNullOrEmpty(vac.Reason))
                {
                    string x = "special reason";
                    Session["reason"] = x.ToString();
                    
                }
                else
                {
                    Session["reason"] = (vac.Reason).ToString();

                }
                db.SaveChanges();
                lastID = vac.vac_ID;
                Session["lastID"] = lastID.ToString();

            }
            db.SaveChanges();
            status = true;

            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
           
                var v = db.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    db.Events.Remove(v);
                    db.SaveChanges();
                    status = true;
                }
            
            return new JsonResult { Data = new { status = status } };
        }
        public JsonResult CanRequest(int ssn)
        {
            DateTime hiring = (DateTime)db.Employees.SingleOrDefault(e => e.emp_ssn == ssn).Hiring_Date;
            DateTime date = DateTime.Now;
            var diff = date - hiring;
            var canRe = -1;
            if(diff.TotalDays >= (30*6))
            {
                canRe = 1;
            }
            else
            {
                canRe = 0;
            }
            return Json(canRe, JsonRequestBehavior.AllowGet);

        }
    }
}