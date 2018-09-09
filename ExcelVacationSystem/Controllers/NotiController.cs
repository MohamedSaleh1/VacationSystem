using ExcelVacationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelVacationSystem.Controllers
{
    public class NotiController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();

        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendRequestNotification()
        {
            //requestRebo studentService = new requestRebo();
            return PartialView("_requestList", requestRebo.GetRequestRecords());
        }
        
        public JsonResult sendNoti()
        {
            int HRSSN;
            try
            {
                 HRSSN = db.Employees.SingleOrDefault(x => x.Department.dep_no == 4).emp_ssn;
            }
            catch (Exception e)
            {

                throw e;
            }
            int ssn =( Int32.Parse(Session["ssn"].ToString()));
            string query;
            if(ssn == HRSSN)
            {
                query = @"SELECT [empID],[vacID],[date],[approval],[name] FROM [dbo].[request] inner join [dbo].[Employee]  ON [dbo].[Employee].[emp_ssn]=  [dbo].[request].[empID]  where  [dbo].[request].[approval] =0 ";
            }
            else
            {
                query = @"SELECT [empID],[vacID],[date],[approval],[name] FROM [dbo].[request] inner join [dbo].[Employee]  ON [dbo].[Employee].[emp_ssn]=  [dbo].[request].[empID]  where  [dbo].[request].[approval] =0 and [dbo].[Employee].[mgr_ssn]=" + Int32.Parse(Session["ssn"].ToString()); 
            }

            var lstRequestRecords = new List<request>();
            var appeardList = new List<request>();
            string dbConnectionSettings = @"Server=.;Database=Excelsystems;Integrated Security=SSPI";

            using (var dbConnection = new SqlConnection(dbConnectionSettings))
            {
                dbConnection.Open();


                //  var sqlCommandText = @"SELECT [empID],[vacID],[date],[approval],[name] FROM [dbo].[request] inner join [dbo].[Employee]  ON [dbo].[Employee].[emp_ssn]=  [dbo].[request].[empID]  where  [dbo].[request].[approval] =0 and [dbo].[Employee].[mgr_ssn]="+ Int32.Parse(Session["ssn"].ToString()) ;
                var sqlCommandText = query;
                using (var sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    
                   
                    AddSQLDependency(sqlCommand);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    
                    var reader = sqlCommand.ExecuteReader();
                    lstRequestRecords = GetRequestRecords(reader);
                    //var ssnOFCUrrentManager = (Int32.Parse(Session["ssn"].ToString()));
                    //appeardList = lstRequestRecords.Where(e => e.Employee.mgr_ssn == ssnOFCUrrentManager).ToList();
                  
                }

            }

            return Json(lstRequestRecords, JsonRequestBehavior.AllowGet);

            
        }

        //private static void AddSQLDependency(SqlCommand sqlCommand)
        //{
        //    sqlCommand.Notification = null;

        //    var dependency = new SqlDependency(sqlCommand);

        //    dependency.OnChange += (sender, sqlNotificationEvents) =>
        //    {
        //        if (sqlNotificationEvents.Type == SqlNotificationType.Change)
        //        {
        //            StudentHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
        //        }
        //    };
        //}
        public static int counter = 0;
        private static void AddSQLDependency(SqlCommand sqlCommand)
        {
            sqlCommand.Notification = null;
            var dependency = new SqlDependency(sqlCommand);
            dependency.OnChange += (sender, sqlNotificationEvents) =>
            {
                bool chanched = dependency.HasChanges;

                //if (sqlNotificationEvents.Type == SqlNotificationType.Change)
                //{
                //    StudentHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
                //}

                int x = counter;
                if (sqlNotificationEvents.Type == SqlNotificationType.Change && sqlNotificationEvents.Info == SqlNotificationInfo.Insert && counter == 0)
                {
                    //Call SignalR  
                    StudentHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
                    counter++;
                chanched = false;
                    //The update is done once
                }
                else
                {
                    counter = 0; //if the update is needed in the same iteration, please don't update and set the counter to 0
                }
            };
            
        }


        /// <summary>
        /// Fills the Student Records
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<request> GetRequestRecords(SqlDataReader reader)
        {

            var lstRequestRecords = new List<request>();
            var dt = new DataTable();
            dt.Load(reader);
            dt
                .AsEnumerable()
                .ToList()
                .ForEach
                (
                    i => lstRequestRecords.Add(new request()
                    {
                        empID = (int)i["empID"]
                            ,
                        vacID = (int)i["vacID"]
                            ,
                        date = Convert.ToDateTime(i["date"])
                       ,
                        empName = (string)i["name"]
                        ,
                        approval = (int)i["approval"]


                    })
                    
                );
            return lstRequestRecords;
            //var name = lstRequestRecords.SingleOrDefault().Employee.name;            

        }

      

    }
}
    
