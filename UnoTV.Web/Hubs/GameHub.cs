using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using UnoTV.Web.Domain;
using UnoTV.Web.Game;

namespace UnoTV.Web.Hubs
{
    public class GameHub : Hub
    {
        private static readonly GameState Game = new GameState();

        public void Join(string playerName)
        {
            Game.AddPlayer(new Player(Context.ConnectionId, playerName));
            Clients.All.playerJoined(playerName); // table listens
        }

        public void StartGame()
        {
            if (Game.Players.Count < 2)
                return;

            Game.Start();
            Clients.All.gameStarted(); // all clients listen

            foreach (var player in Game.Players)
            {
                Clients.Client(player.Id).deal(player.Hand); // send to each client in turn
            }

            Clients.All.cardPlayed(Game.CurrentCard); // table listens

            Clients.Client(Game.CurrentPlayer.Id).turn(Game.CurrentPlayer.Hand); // send to current player
            Clients.All.playerTurn(Game.CurrentPlayer.Name); // table listens
        }

        public void PlayCard(Card card)
        {
            Game.PlayCard(card);
            Clients.All.cardPlayed(card); // table listens

            if (Game.Finished)
            {
                Clients.All.gameOver(Game.Winner); // all clients listen
            }
            else
            {
                Clients.Client(Game.CurrentPlayer.Id).turn(Game.CurrentPlayer.Hand); // send to current player
                Clients.All.playerTurn(Game.CurrentPlayer.Name); // table listens

                if (Game.CurrentPlayer.Hand.HasPlayableCard == false)
                    Clients.All.cardPickup(Game.CurrentPlayer.Name); // table listens
            }
        }
    }
}