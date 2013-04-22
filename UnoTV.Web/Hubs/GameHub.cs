using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using UnoTV.Web.Domain;
using UnoTV.Web.Game;

namespace UnoTV.Web.Hubs
{
    public class GameHub : Hub
    {
        private static GameState _game = new GameState();

        public void Join(string playerName)
        {
            try
            {
                _game.AddPlayer(new Player(Context.ConnectionId, playerName));
            }
            catch (Exception ex)
            {
                Clients.Caller.error(ex);
            }
        }

        public void StartGame()
        {
            try
            {
                _game.Start();
                Clients.All.gameStarted(); // all clients listen

                foreach (var player in _game.Players)
                {
                    Clients.Client(player.Id).deal(player.Hand); // send to each client in turn
                    Clients.All.playerJoined(player.Name, player.Id, player.Hand.Total); // table listens
                }

                Clients.All.cardPlayed(_game.CurrentCard); // table listens
                NotifyNextPlayer();
            }
            catch (Exception ex)
            {
                Clients.Caller.error(ex);
            }
        }

        public void ResetGame()
        {
            try
            {
                _game = new GameState();
                Clients.All.gameReset(); // all clients listen
            }
            catch (Exception ex)
            {
                Clients.Caller.error(ex);
            }
        }

        public void PlayCard(Card card)
        {
            try
            {
                _game.PlayCard(card);
                Clients.All.cardPlayed(card); // table listens

                if (_game.Finished)
                    Clients.All.gameOver(_game.Winner); // all clients listen
                else
                    NotifyNextPlayer();
            }
            catch (Exception ex)
            {
                Clients.Caller.error(ex);
            }
        }

        public override Task OnDisconnected()
        {
            ResetGame();
            return base.OnDisconnected();
        }

        private void NotifyNextPlayer()
        {
            Clients.Client(_game.CurrentPlayer.Id).turn(_game.CurrentPlayer.Hand); // send to current player
            Clients.All.playerTurn(_game.CurrentPlayer.Name, _game.CurrentPlayer.Id, _game.CurrentPlayer.Hand.Total); // table listens

            if (_game.CurrentPlayer.Hand.HasPlayableCard == false)
            {
                Clients.All.cardPickup(_game.CurrentPlayer.Name); // table listens
                PlayCard(null);
            }
        }
    }
}