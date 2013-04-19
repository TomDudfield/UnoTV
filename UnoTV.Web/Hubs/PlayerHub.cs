using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace UnoTV.Web.Hubs
{
    public class Message
    {
        public string PersonName { get; set; }

        public string Data { get; set; }
    }
    
    public class PlayerHub : Hub
    {
        private static readonly List<Message> Messages = new List<Message>();
        
        public void Send(Message message)
        {
            message.PersonName += " (" + Context.ConnectionId + ")";
            Messages.Add(message);
            Clients.All.addNewMessageToPage(message);
        }

        public void GetAll()
        {
            Clients.Caller.allMessages(Messages);
        }
    }
}