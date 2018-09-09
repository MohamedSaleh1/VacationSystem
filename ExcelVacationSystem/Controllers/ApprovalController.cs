using ExcelVacationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;


namespace ExcelVacationSystem.Controllers
{
    public class ApprovalController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();
        static string from;
        static string to;
        static string nDays;
        static bool casual;
        static bool regular;
        static string requestdName;
        static int Essn;
        static string mngName;
        static string mgrMail;
        static bool permission;
        static int VacationID;
        static int hours;
        static string type;
        static int DaysOrHour;
        // GET: Approval
        public ActionResult Index(int vid)
        {

            ViewBag.vID = vid;
            VacationID = vid;
            var request = db.vacations.Where(r => r.vac_ID == vid);
            from = request.SingleOrDefault().fromDate.ToString();
            ViewBag.from = from;
            to = request.SingleOrDefault().toDate.ToString();
            ViewBag.to = to;
            nDays = request.SingleOrDefault().num_days.ToString();
            Session["days"] = nDays.ToString();
            ViewBag.days = nDays;
            bool.TryParse(request.SingleOrDefault().isCasual.ToString(), out casual);
            bool.TryParse(request.SingleOrDefault().isRegular.ToString(), out regular);
            Essn = request.SingleOrDefault().requests.SingleOrDefault().empID;
            bool.TryParse(request.SingleOrDefault().isPermission.ToString(), out permission);
            hours = (Int32.Parse(request.SingleOrDefault().hours.ToString()));
            requestdName = db.Employees.SingleOrDefault(e => e.emp_ssn == Essn).name;
            mngName = db.Employees.SingleOrDefault(z => z.emp_ssn == Essn).Employee2.name;
            mgrMail = db.Employees.SingleOrDefault(x => x.emp_ssn == Essn).Employee2.Email;

            ViewBag.name = requestdName;
            if (casual == true && permission == false && regular == false)
            {
                type = " Casual Vacation";
                DaysOrHour = 0;


            }
            else if (casual == false && permission == true && regular == false)
            {
                type = " Permission ";
                DaysOrHour = 1;


            }
            else if (casual == false && permission == false && regular == true)
            {
                type = " Regular Vacation";
                DaysOrHour = 0;

            }
            ViewBag.type = type;
            ViewBag.daysOrHour = DaysOrHour;
            Session["daysOrHours"] = DaysOrHour.ToString();
            ViewBag.Hours = hours;
            return View();
        }
        public JsonResult approval(int check, string text, string ComingPassword)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = "no reason typed";
            }
            string CominPass = ComingPassword;
            var name = Session["name"].ToString();
            int ssn = Int32.Parse(Session["ssn"].ToString());
            var empSender = db.Employees.SingleOrDefault(x => x.emp_ssn == ssn);
            var request = db.vacations.Where(r => r.vac_ID == VacationID);
            int requesterSSN = request.SingleOrDefault().requests.SingleOrDefault().empID;
            string requesterEmail = db.Employees.SingleOrDefault(f => f.emp_ssn == requesterSSN).Email;
            string HREmail = db.Employees.SingleOrDefault(h => h.emp_ssn == 1011).Email;
            int hrSSN = db.Employees.SingleOrDefault(r => r.Department.dep_no == 4).emp_ssn;


            string userMail;
            string userPass;
            //string HRPassword = ComingPassword;
            if (ssn == hrSSN)
            {
                userMail = HREmail;
                userPass = ComingPassword;
            }
            else
            {
                userMail = empSender.Email;

                userPass = ComingPassword;
            }



            //var vid = Session["VacationID"].ToString();
            //var from = Session["from"].ToString();
            //var to = Session["to"].ToString();

            //bool.TryParse(Session["cas"].ToString(), out casual);

            //bool.TryParse(Session["type"].ToString(), out permission);

            if (permission == true && casual == false && regular == false)
            {


                if (check == 1)
                {

                    MailMessage mail = new MailMessage();
                    mail.To.Add(requesterEmail);
                    // mail.CC.Add(HREmail);
                    mail.From = new MailAddress(userMail);

                    mail.Subject = "PermissionApproval";
                    string Body = @"<h2>Permission  Approval </h2><br />  your requested  Permission for " + hours + " Hours <label> from </label> " + from + " <label>To</label> " + to + " <br /> is ACCEPTED, enjoy ur time..";
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.excelsystems-eg.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    System.Net.NetworkCredential NetworkCredential = new NetworkCredential(userMail, ComingPassword);
                    smtp.Credentials = NetworkCredential; // Enter seders User name and password  
                    smtp.EnableSsl = false;
                    smtp.Send(mail);
                    db.requests.SingleOrDefault(e => e.vacID == VacationID).approval = 1;
                    db.SaveChanges();
                }
                else if (check == 2)
                {
                    // ssn = Session["ssn"].ToString();
                    //empSender = db.Employees.SingleOrDefault(x => x.emp_ssn == ssn);
                    // userMail = empSender.Email;
                    // userPass = empSender.password;
                    MailMessage mail = new MailMessage();
                    mail.To.Add(requesterEmail);
                    // mail.CC.Add(HREmail);
                    mail.From = new MailAddress(userMail);

                    mail.Subject = "PermissionApproval";
                    string Body = @"<h2>Permission Approval </h2><br /> sorry to inform u that your requested  permission for " + hours + " hours <label> from </label> " + from + " <label>To</label> " + to + " <br /> is rejected with the reason: " + text;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.excelsystems-eg.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    NetworkCredential Networkcredential = new NetworkCredential(userMail, ComingPassword);
                    smtp.Credentials = Networkcredential; // Enter seders User name and password  
                    smtp.EnableSsl = false;
                    smtp.Send(mail);
                    db.requests.SingleOrDefault(e => e.vacID == VacationID).approval = -1;
                    db.SaveChanges();
                }

            }
            else if (permission == false && casual == true && regular == false)
            {
                if (check == 1)
                {




                    var requstd = Session["days"].ToString();


                    //var empSender = db.Employees.SingleOrDefault(x => x.emp_ssn.ToString() == ssn);
                    //string username = empSender.Email;
                    //string userPass = empSender.password;
                    MailMessage mail = new MailMessage();
                    mail.To.Add(requesterEmail);
                    // mail.CC.Add(HREmail);
                    mail.From = new MailAddress(userMail);
                    mail.Subject = "VacationApproval";
                    string Body = @"<h2>Vacation Approval </h2><br />  your requested  vacation for " + requstd + " days <label> from </label> " + from + " <label>To</label> " + to + " <br /> as a casual vaction is <b> ACCEPTED</b>, enjoy ur time..";
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.excelsystems-eg.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    System.Net.NetworkCredential NetworkCredential = new NetworkCredential(userMail, ComingPassword);
                    smtp.Credentials = NetworkCredential; // Enter seders User name and password  
                    smtp.EnableSsl = false;
                    smtp.Send(mail);
                    db.requests.SingleOrDefault(e => e.vacID == VacationID).approval = 1;
                    db.Employees.SingleOrDefault(e => e.emp_ssn.ToString() == requesterSSN.ToString()).Total_Available_days -= (Int32.Parse(requstd));
                    db.Employees.SingleOrDefault(e => e.emp_ssn.ToString() == requesterSSN.ToString()).Casual_vacation += (Int32.Parse(requstd));
                    db.SaveChanges();

                }
                else if (check == 2)
                {

                    //var name = Session["name"].ToString();

                    var requstd = Session["days"].ToString();

                    MailMessage mail = new MailMessage();
                    mail.To.Add(requesterEmail);
                    // mail.CC.Add(HREmail);
                    mail.From = new MailAddress(userMail);

                    mail.Subject = "VacationRequetApproval";
                    string Body = @"<h2>Vacation  Approval </h2><br /> sorry to inform u that your requested  vacation for " + requstd + " days <label> from </label> " + from + " <label>To</label> " + to + " <br /> is rejected with the reason: " + text;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.excelsystems-eg.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    NetworkCredential Networkcredential = new NetworkCredential(userMail, ComingPassword);
                    smtp.Credentials = Networkcredential; // Enter seders User name and password  
                    smtp.EnableSsl = false;
                    smtp.Send(mail);
                    db.requests.SingleOrDefault(e => e.vacID == VacationID).approval = -1;
                    db.SaveChanges();
                }
            }
            else if (permission == false && casual == false && regular == true)
            {
                if (check == 1)
                {

                    var requstd = Session["days"].ToString();

                    MailMessage mail = new MailMessage();
                    mail.To.Add(requesterEmail);
                    // mail.CC.Add(HREmail);
                    mail.From = new MailAddress(userMail);

                    mail.Subject = "VacationApproval";
                    string Body = @"<h2>Vacation Approval </h2><br />  your requested  vacation for " + requstd + " days <label> from </label> " + from + " <label>To</label> " + to + " <br /> as a Regular vaction is <b> ACCEPTED</b>, enjoy ur time..";
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.excelsystems-eg.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    System.Net.NetworkCredential NetworkCredential = new NetworkCredential(userMail, ComingPassword);
                    smtp.Credentials = NetworkCredential; // Enter seders User name and password  
                    smtp.EnableSsl = false;
                    smtp.Send(mail);
                    db.requests.SingleOrDefault(e => e.vacID == VacationID).approval = 1;
                    //db.Employees.SingleOrDefault(e => e.emp_ssn == ssn).Regular_vacation -= (Int32.Parse(requstd));
                    db.Employees.SingleOrDefault(e => e.emp_ssn.ToString() == requesterSSN.ToString()).Regular_vacation += (Int32.Parse(requstd));
                    db.Employees.SingleOrDefault(e => e.emp_ssn.ToString() == requesterSSN.ToString()).Total_Available_days -= (Int32.Parse(requstd));

                    db.SaveChanges();
                }
                else if (check == 2)
                {

                    var requstd = Session["days"].ToString();


                    MailMessage mail = new MailMessage();
                    mail.To.Add(requesterEmail);
                    // mail.CC.Add(HREmail);
                    mail.From = new MailAddress(userMail);

                    mail.Subject = "PermissionApproval";
                    string Body = @"<h2>Vacation  Approval </h2><br /> sorry to inform u that your requested  vacation for " + requstd + " days <label> from </label> " + from + " <label>To</label> " + to + " <br /> is rejected with the reason: " + text;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.excelsystems-eg.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    NetworkCredential Networkcredential = new NetworkCredential(userMail, ComingPassword);
                    smtp.Credentials = Networkcredential; // Enter seders User name and password  
                    smtp.EnableSsl = false;
                    smtp.Send(mail);
                    db.requests.SingleOrDefault(e => e.vacID == VacationID).approval = -1;
                    db.SaveChanges();
                }

            }
            return null;
        }

    }
}