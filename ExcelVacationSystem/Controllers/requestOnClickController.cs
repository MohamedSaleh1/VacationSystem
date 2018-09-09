using ExcelVacationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Net.Mime;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Globalization;

namespace ExcelVacationSystem.Controllers
{
    public class requestOnClickController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();

        // GET: requestOnClick
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult RequestedDays(int d,string from,string to)
        {
            var ssn = Session["ssn"].ToString();
            int canRequest = 0;
            var CasualAvail = db.Employees.SingleOrDefault(x => x.emp_ssn.ToString() == ssn).Casual_vacation;
            var RegularAvail = db.Employees.SingleOrDefault(x => x.emp_ssn.ToString() == ssn).Regular_vacation;
            int requested = d;
            Session["requested"] = requested.ToString();
            Session["from"] = from.ToString();
            Session["to"] = to.ToString();
            if (CasualAvail >= d)
            {
                canRequest = 1;
                return Json(canRequest, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(canRequest, JsonRequestBehavior.AllowGet);

            }

        }
        static string Body;
        static string subject;
        public ActionResult sendMail(string ComingPassword)
        {
            bool flagCa;
            bool flagreg;
            bool permissionFlag;
            var name = Session["name"].ToString();
            var ssn = Session["ssn"].ToString();
            var casualAvail = Session["casual"].ToString();
            var regularAvail = Session["regular"].ToString();
            var requested = Session["days"].ToString();
            var from =DateTime.Parse(Session["from"].ToString());
            var to =DateTime.Parse(Session["to"].ToString());
            var id = Session["lastID"].ToString();
            var isCasual = Session["cas"].ToString();
            var isRegular = Session["reg"].ToString();
            var reason = Session["reason"].ToString();
            var isPermission = Session["permisstion"].ToString();
            int Hours = (Int32.Parse(Session["Hours"].ToString()));
            bool.TryParse(isCasual, out flagCa);
            bool.TryParse(isRegular, out flagreg);
            bool.TryParse(isPermission, out permissionFlag);
            var empSender = db.Employees.SingleOrDefault(x => x.emp_ssn.ToString() == ssn);
            string username = empSender.Email;
            string userPass = ComingPassword;
            MailMessage mail = new MailMessage();
            mail.To.Add(empSender.Employee2.Email);
            mail.From = new MailAddress(empSender.Email);
           // mail.Subject = "VacationRequet";
              
            if (flagCa == true && flagreg == false && permissionFlag == false)
            {
                subject = "VacationRequet";
                 Body = @"<h2>Vacation Request </h2><br /> " + name + " &nbsp requested a vacation for " + requested +
               "&nbsp days <label> from </label> " + from + " <label>To</label> " + to + " &nbsp as a casual vacation for " + reason + " &nbsp knowing that he has " + casualAvail + "&nbsp  available casual vacation days " +
               @" <br /> for approval plz click <a href='http://localhost:2982/Approval/index'> Here</a> ";
            }

            else if (flagCa == false && flagreg == true&& permissionFlag == false)
            {
                subject = "VacationRequet";
                Body = @"<h2>Vacation Request </h2><br /> " + name + " &nbsp requested a vacation for " + requested +
              " &nbsp days <label> from </label> " + from + " &nbsp <label>To</label> " + to + " &nbsp as a Regular vacation for " + reason + " &nbsp knowing that he has " + regularAvail + " &nbsp available regular vacation days " +
              @" <br /> for approval plz click <a href='http://localhost:2982/Approval/index'> Here</a> ";
            }
            else if (flagCa == false && flagreg == false && permissionFlag == true)
            {
                subject = "PermissionRequest";
                Body = @"<h2>Permission Request </h2><br /> " + name + " &nbsp requested a Permission for " + Hours +
             " &nbsp Hours <label> from </label> " + from + "&nbsp <label>To</label> " + to + " &nbsp for  " + reason + "&nbsp ." +
             @" <br /> for approval plz click <a href='http://localhost:2982/Approval/index'> Here</a> ";
            }
            mail.Subject = subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.excelsystems-eg.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            System.Net.NetworkCredential NetworkCredential = new NetworkCredential(username, userPass);
            smtp.Credentials = NetworkCredential; // Enter seders User name and password  
            smtp.EnableSsl = false;
            smtp.Send(mail);
            //try
            //{
            //    vacation v = new vacation();
            //    v.fromDate = from;
            //    v.toDate = to;
            //    v.num_days = Int32.Parse(requested);
            //    v.isCasual = false;
            //    v.isRegular = true;
            //    db.vacations.Add(v);
            //    db.SaveChanges();
            //   lastID = v.vac_ID;
            //}
            //catch (Exception ex)
            //{

            //    return null;
            //}

            try
            {
                

                request r = new request();
                r.date = DateTime.Now;
                r.empID = Int32.Parse(ssn);
                r.vacID = Int32.Parse(id);
                r.approval = 0;
                db.requests.Add(r);
                db.SaveChanges();
            }
            catch (Exception e )
            {

                return null;
            }

            //string msg = Body;
           
            return RedirectToAction("UserDashBoard","testLogin");
        }
       
    }

}
