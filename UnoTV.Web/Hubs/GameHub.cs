using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace UnoTV.Web.Hubs
{
    public class Message
    {
        public string PersonName { get; set; }

        public string Data { get; set; }
    }
    
    public class GameHub : Hub
    {
        private static readonly List<Message> Messages = new List<Message>();
        private static readonly Dictionary<string, string> Players = new Dictionary<string, string>();
        
        public void Send(Message message)
        {
            message.PersonName += " (" + Context.ConnectionId + ")";
            Messages.Add(message);
            Clients.All.addNewMessageToPage(message);
        }

        public void Join(string personName)
        {
            Players.Add(Context.ConnectionId, personName);
            Clients.All.playerJoined(personName);
        }

        public void GetAll()
        {
            Clients.Caller.allMessages(Messages);
        }

        public override Task OnDisconnected()
        {

            if (Players.ContainsKey(Context.ConnectionId))
            {
                Clients.All.playerLeft(Players[Context.ConnectionId]);
                Players.Remove(Context.ConnectionId);
            }

            return base.OnDisconnected();
        }
    }
}