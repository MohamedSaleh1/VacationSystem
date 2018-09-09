using ExcelVacationSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelVacationSystem.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        //private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();


        public ActionResult Index()
        {
            using (ExcelsystemsEntities2 db = new ExcelsystemsEntities2())
            {
                List<Employee> Employeelist = db.Employees.ToList();
                ViewBag.listOFEmployees = new SelectList(Employeelist, "id", "name");
            }

            return View();

        }

        public JsonResult GetEmployeesList()
        {



            try
            {
                using (ExcelsystemsEntities2 db = new ExcelsystemsEntities2())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    // List<Employee> 
                    var emp = db.Employees.Select(p => new
                    {
                        ssn = p.emp_ssn,
                        name = p.name,
                        mail = p.Email,
                        days = p.Total_Available_days,
                        emp_name = p.Employee2.name,
                        dep = p.Department.name
                    }).ToList();
                    return Json(emp, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e) { return null; }
        }
        public JsonResult GetEmployeeById(int id)
        {
            using (ExcelsystemsEntities2 db = new ExcelsystemsEntities2())
            {


                var model = db.Employees.Where(x => x.emp_ssn == id).Select(p=> new  mappingClassForEmployees{
                   emp_ssn = p.emp_ssn,
                    name = p.name,
                     Email= p.Email,
                    Total_Available_days = p.Total_Available_days,
                    Regular_vacation = p.Regular_vacation,
                    Hiring_Date = p.Hiring_Date,
                    Casual_vacation = p.Casual_vacation,
                   Dno = p.Dno

                }).ToList()
                ;

                if (model.Count() != 0)
                    return Json(model.FirstOrDefault(), JsonRequestBehavior.AllowGet);
                //string value = string.Empty;
                //value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //});
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveData(Employee model)
        {
            var result = false;
            try
            {


                if (model.emp_ssn > 0)
                {
                    using (ExcelsystemsEntities2 db = new ExcelsystemsEntities2())
                    {

                        Employee e = new Employee();
                        e = db.Employees.SingleOrDefault(x => x.emp_ssn == model.emp_ssn);
                        if (e != null)
                        {
                            e.emp_ssn = model.emp_ssn;
                            e.name = model.name;
                            e.Email = model.Email;
                            e.mgr_ssn = model.mgr_ssn;
                            e.Casual_vacation = model.Casual_vacation;
                            e.Hiring_Date = model.Hiring_Date;
                            e.isManager = model.isManager;
                            e.Social_insurance = model.Social_insurance;
                            e.Regular_vacation = model.Regular_vacation;
                            e.Dno = model.Dno;
                            e.Total_Available_days = model.Total_Available_days;
                            db.SaveChanges();
                            result = true;
                        }
                        else
                        {
                            e = new Employee();
                            e.emp_ssn = model.emp_ssn;
                            e.name = model.name;
                            e.Email = model.Email;
                            e.mgr_ssn = model.mgr_ssn;
                            e.Casual_vacation = model.Casual_vacation;
                            e.Hiring_Date = model.Hiring_Date;
                            e.isManager = model.isManager;
                            e.Social_insurance = model.Social_insurance;
                            e.Regular_vacation = model.Regular_vacation;
                            e.Dno = model.Dno;
                            e.Total_Available_days = model.Total_Available_days;
                            db.Employees.Add(e);
                            db.SaveChanges();
                            result = true;

                        }
                    }
                }
            }

            catch (Exception e)
            {
                return null;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteEmployeeRecord(int id)
        {
            bool result = false;
            using (ExcelsystemsEntities2 db = new ExcelsystemsEntities2())
            {

                Employee u = db.Employees.SingleOrDefault(x => x.emp_ssn == id);
                if (u != null)
                {
                    db.Employees.Remove(u);
                    db.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}