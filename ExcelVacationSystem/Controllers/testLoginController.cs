using ExcelVacationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.SignalR;
using System.Web.Script.Serialization;

namespace ExcelVacationSystem.Controllers
{
    public class testLoginController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();

        // GET: testLogin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public JsonResult log(string mail)
        {

            //var obj = db.Employees.Where(a => a.Email.Equals(mail).());

            try
            {

                db.Configuration.ProxyCreationEnabled = false;
                var obj = db.Employees.Where(a => a.Email.Equals(mail)).SingleOrDefault();
                if (obj != null)
                {
                    Session["mail"] = obj.Email.ToString();
                    Session["name"] = obj.name.ToString();
                    Session["ssn"] = obj.emp_ssn.ToString();
                    Session["avail"] = obj.Total_Available_days.ToString();
                    Session["casual"] = obj.Casual_vacation.ToString();
                    Session["regular"] = obj.Regular_vacation.ToString();
                    Session["mgr"] = obj.mgr_ssn;
                    Session["dep"] = obj.Dno;
                    Session["ism"] = obj.isManager;
                    var jsonObj = new JavaScriptSerializer().Serialize(obj);
                    return Json(jsonObj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {

            }
        

            return null;
        }

        [HttpPost]

        public ActionResult Login(Employee objEmp)
        {
            if (ModelState.IsValid)
            {


                var obj = db.Employees.Where(a => a.Email.Equals(objEmp.Email)).FirstOrDefault();
                if (obj != null)
                {
                    Session["mail"] = obj.Email.ToString();
                    Session["name"] = obj.name.ToString();
                    Session["ssn"] = obj.emp_ssn.ToString();
                    Session["avail"] = obj.Total_Available_days.ToString();
                    Session["casual"] = obj.Casual_vacation.ToString();
                    Session["regular"] = obj.Regular_vacation.ToString();
                    Session["mgr"] = obj.Employee2.name;
                    Session["dep"] = obj.Department.name;
                    Session["ism"] = obj.isManager;
                    return RedirectToAction("UserDashBoard");
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            return View(objEmp);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "TestLogin");
        }

        public ActionResult UserDashBoard()
        {
            db.Configuration.ProxyCreationEnabled = false;

            if ((Session["mail"] != null))
            {
                var mail = Session["mail"].ToString();

                var name = Session["name"].ToString();
                var ssn = Session["ssn"].ToString();
                var avail = Session["avail"].ToString();
                var mgr = Session["mgr"].ToString();
                var dep = Session["dep"].ToString();
                var ism = Session["ism"].ToString();

                var managers = db.Employees                                 //for droplist
                                 .Where(item => item.isManager == 1)
                                 .Select(c => new SelectListItem()
                                 {
                                     Text = c.name,
                                     Value = c.emp_ssn.ToString()
                                 });
                ViewData["Managers"] = managers;

                //var departs = new SelectList(db.Departments.ToList(), "dep_no", "name"); // fro department list
                //ViewData["departments"] = departs;

                var depats = db.Employees                                 //for droplist
                                .Select(c => new SelectListItem()
                                {
                                    Text = c.Department.name,
                                    Value = c.Department.dep_no.ToString()
                                }).Distinct();
                ViewData["departments"] = depats;

                var isManager = new List<convertEnum>();
                foreach (isManager type in Enum.GetValues(typeof(isManager)))
                    isManager.Add(new convertEnum
                    {
                        value = (int)type,
                        name = type.ToString()
                    });
                ViewBag.Manager = isManager;



                var e = db.Employees.Where(x => x.Email == mail).ToList();
                

                return View();

            }
            else
            {
                return RedirectToAction("Index");
                //return null;
            }
        }
        public JsonResult employee()
        {
            var mail = Session["mail"].ToString();

            // var isma = Session["ism"].ToString();



            var employee = db.Employees.Where(x => x.Email == mail).Select(p => new
            {
                ssn = p.emp_ssn,
                name = p.name,
                mail = p.Email,
                Total_Available_days = p.Total_Available_days,
                Casual_vacation = p.Casual_vacation,
                Regular_vacation = p.Regular_vacation,
                emp_name = p.Employee2.name,
                dep = p.Department.name,
                hiringDate = p.Hiring_Date
                //mgr = p.isManager
            }).ToList();


            return Json(employee, JsonRequestBehavior.AllowGet);


        }
        public JsonResult managerEmployees()
        {

            var ssn = Session["ssn"].ToString();
            int ToCheckHRssn = Int32.Parse(ssn);
            int HRSSN = 8;
            if (ToCheckHRssn == HRSSN)
            {
                var allEmployees = db.Employees.Select(p => new
                {
                    ssn = p.emp_ssn,
                    name = p.name,
                    mail = p.Email,
                    Total_Available_days = p.Total_Available_days,
                    Casual_vacation = p.Casual_vacation,
                    Regular_vacation = p.Regular_vacation,
                    emp_name = p.Employee2.name,
                    dep = p.Department.name,
                    hiringDate = p.Hiring_Date
                    //mgr = p.isManager
                }).ToList();
                return Json(allEmployees, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var employeesUnderThisManager = db.Employees.Where(z => z.mgr_ssn.ToString() == ssn).Select(p => new
                {
                    ssn = p.emp_ssn,
                    name = p.name,
                    mail = p.Email,
                    Total_Available_days = p.Total_Available_days,
                    Casual_vacation = p.Casual_vacation,
                    Regular_vacation = p.Regular_vacation,
                    emp_name = p.Employee2.name,
                    dep = p.Department.name,
                    hiringDate = p.Hiring_Date
                    //mgr = p.isManager
                }).ToList();

                if (employeesUnderThisManager != null)
                    return Json(employeesUnderThisManager, JsonRequestBehavior.AllowGet);
                else
                    return null;
            }

        }
        [HttpPost]
        public JsonResult SaveData(Employee model)
        {
            var mgr = Session["mgr"].ToString();
            var dep = Session["dep"].ToString();

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
                            // e.Total_Available_days = model.Total_Available_days;
                            e.Casual_vacation = model.Casual_vacation;
                            e.Regular_vacation = model.Regular_vacation;
                            e.Dno = model.Dno;
                            e.Hiring_Date = model.Hiring_Date;
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
                            // e.Total_Available_days = model.Total_Available_days;
                            e.Casual_vacation = model.Casual_vacation;
                            e.Regular_vacation = model.Regular_vacation;
                            e.Dno = model.Dno;
                            e.isManager = 0;
                            e.Hiring_Date = model.Hiring_Date;
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
            Employee u = db.Employees.SingleOrDefault(x => x.emp_ssn == id);
            if (u != null)
            {
                db.Employees.Remove(u);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
    public class NotificationHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendNotifications(string message)
        {
            Clients.All.receiveNotification(message);
        }
    }
}