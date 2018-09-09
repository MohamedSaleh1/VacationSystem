using ExcelVacationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ExcelVacationSystem.Controllers
{
    public class sendMailController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();

        // GET: sendMail
        [HttpGet]
        public ActionResult sendMail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult sendMail(mailModel obj)
        {

           
            var ssn = Session["ssn"].ToString();
            var MangerEmail = db.Employees.SingleOrDefault(x => x.emp_ssn.ToString() == ssn).Employee2.Email;
            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application 
                 
                WebMail.UserName = "";
                WebMail.Password = "";

                //Sender email address.  
                WebMail.From = "";

                //Send email  
                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody, cc: obj.EmailCC, bcc: obj.EmailBCC, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }
            return View();
        }
    }
}
