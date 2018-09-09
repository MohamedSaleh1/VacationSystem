using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ExcelVacationSystem
{
   
    public class RequestHub : Hub
    {
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}
        public static void SendUptodateInformation(string action)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<RequestHub>();

            // the updateStudentInformation method will update the connected client about any recent changes in the server data
            context.Clients.All.updateRequestInformation(action);

        }
    }
}