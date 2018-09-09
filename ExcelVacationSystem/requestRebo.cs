using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExcelVacationSystem.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ExcelVacationSystem
{
    public class requestRebo
    {
        public static List<request> GetRequestRecords()
        {
            var lstStudentRecords = new List<request>();
            string dbConnectionSettings = @"Server=.;Database=Excelsystems;Integrated Security=SSPI";

            using (var dbConnection = new SqlConnection(dbConnectionSettings))
            {
                dbConnection.Open();

                var sqlCommandText = @"SELECT [empID],[vacID],[date] FROM [dbo].[request]";

                using (var sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    AddSQLDependency(sqlCommand);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    lstStudentRecords = GetRequestRecords(reader);
                }
            }
            return lstStudentRecords;
        }

        /// <summary>
        /// Adds SQLDependency for change notification and passes the information to Student Hub for broadcasting
        /// </summary>
        /// <param name="sqlCommand"></param>
        

        private static void AddSQLDependency(SqlCommand sqlCommand)
        {
            int counter = 0; //initialization of help counter
            sqlCommand.Notification = null;
            counter = 0;
            var dependency = new SqlDependency(sqlCommand);

            dependency.OnChange += (sender, sqlNotificationEvents) =>
            {
                //if (sqlNotificationEvents.Type == SqlNotificationType.Change)
                //{
                //    StudentHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
                //}

                if (sqlNotificationEvents.Type == SqlNotificationType.Change && sqlNotificationEvents.Info == SqlNotificationInfo.Update && counter == 0)
                {
                    //Call SignalR  
                    StudentHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
                    counter++; //The update is done once
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
            var lstStudentRecords = new List<request>();
            var dt = new DataTable();
            dt.Load(reader);
            dt
                .AsEnumerable()
                .ToList()
                .ForEach
                (
                    i => lstStudentRecords.Add(new request()
                    {
                        empID = (int)i["empID"]
                            ,
                        vacID = (int)i["vacID"]
                            ,
                        date = Convert.ToDateTime(i["date"])
                          
                    })
                );
            return lstStudentRecords;
        }
    }

}