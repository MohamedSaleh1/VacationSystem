using ExcelVacationSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExcelVacationSystem.Controllers
{
    public class RequestsLogController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();

        // GET: RequestsLog
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult RequestsLog()
        {
            var log = db.requests.Select(p => new
            {
                ssn = p.empID,
                name = p.Employee.name,
                RequestDate = p.date,
                approval = p.approval,
                from = p.vacation.fromDate,
                to = p.vacation.toDate,
                nDays = p.vacation.num_days,
                type = p.vacation.isPermission,
                hours = p.vacation.hours
            }).ToList();

            if (log != null)
                return Json(new { Data = log }, JsonRequestBehavior.AllowGet);
            else
                return null;


        }
       
        public ActionResult Requests_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<request> lst = new List<Models.request>();

            //Convert the Product entities to ProductViewModel instances.
            var result = db.requests.Select(p => new
            {
                ssn = p.empID,
                name = p.Employee.name,
                RequestDate = p.date,
                approval = p.approval,
                from = p.vacation.fromDate,
                to = p.vacation.toDate,
                nDays = p.vacation.num_days,
                type = p.vacation.isPermission,
                hours = p.vacation.hours
            }).ToList();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            
        }
    }
}