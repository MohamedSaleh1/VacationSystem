using ExcelVacationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelVacationSystem.Controllers
{
    public class checkNewNotisController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();

        // GET: checkNewNotis
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult check(int ssn)
        {
            var UnApprovedEmps = db.requests.Join(db.Employees,       // Target table
               (r => r.empID),   // ID of request column
               (e => e.emp_ssn), // Corresponding ID for employees to join on
               (r, e)=> new {
               empID = e.emp_ssn,
               mgr_ssn = e.mgr_ssn,
               unApproved = r.approval,


               }).ToList();
            int request = -1;
            var newRequests =UnApprovedEmps.Where(e=>e.mgr_ssn == ssn && e.unApproved == 0).ToList();
            int count = newRequests.Count();
            if (newRequests!=null)
              {
                request = count;
              }
            else
            {
                request = 0;
            }
           
            return Json(request, JsonRequestBehavior.AllowGet);
            

        }
    }
}