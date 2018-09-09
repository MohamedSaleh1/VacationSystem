using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace ExcelVacationSystem
{

    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

    public class StudentHub : Hub
    {
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}
        [HubMethodName("sendUptodateInformation")]
        public static  void SendUptodateInformation(string action)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();
            // the updateStudentInformation method will update the connected client about any recent changes in the server data
             context.Clients.All.updateStudentInformation(action);
           // context.Clients.Client(ConnectionId).updateStudentInformation(action);
           // context.Clients.All.updateStudentInformation(action);
        }

        public void Send( string message, string connectionid)
        {
            IHubContext cont = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();

            // Call the addNewMessageToPage method to update clients.
            cont.Clients.All.addNewMessageToPage( message, connectionid);
        }

        private readonly static ConnectionMapping<string> _connections =
           new ConnectionMapping<string>();

        public void SendChatMessage(string who, string message)
        {
            string name = Context.User.Identity.Name;

            foreach (var connectionId in _connections.GetConnections(who))
            {
                Clients.Client(connectionId).addChatMessage(name + ": " + message);
            }
        }

        public void SendMessage(string groupName, string message, string type = "msg")
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<StudentHub>();
            context.Clients.Group(groupName).addNewMessageToPage(type, message);
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
        public Task JoinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
    }

}
