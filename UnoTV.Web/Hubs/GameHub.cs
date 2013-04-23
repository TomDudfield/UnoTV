using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using UnoTV.Web.Domain;
using UnoTV.Web.Game;

namespace UnoTV.Web.Hubs
{
    public class GameHub : Hub
    {
        private static GameState _game = new GameState();
        private static readonly List<Table> Tables = new List<Table>();

        public void NewTable(string gameId)
        {
            try
            {
                Tables.Add(new Table(Context.ConnectionId, gameId));
            }
            catch (Exception ex)
            {
                Clients.Caller.error(ex);
            }
        }

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
                Clients.All.gameStarted();

                foreach (var player in _game.Players)
                {
                    Clients.Client(player.Id).deal(player.Hand);
                    Clients.All.playerJoined(player.Name, player.Id, player.Hand.Total, player.Hand.CardCount);
                }

                Clients.All.cardPlayed(_game.CurrentCard);
                NotifyNextPlayer();
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
                Clients.All.cardPlayed(card);

                if (_game.Finished)
                    Clients.All.gameOver(_game.Winner);
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
            var table = Tables.FirstOrDefault(t => t.Id == Context.ConnectionId);

            if (table != null)
            {
                Tables.Remove(table);
                // todo end game
            }

            _game = new GameState();
            Clients.All.gameReset();
            return base.OnDisconnected();
        }

        private void NotifyNextPlayer()
        {
            Clients.Client(_game.CurrentPlayer.Id).turn(_game.CurrentPlayer.Hand);
            Clients.All.playerTurn(_game.CurrentPlayer.Name, _game.CurrentPlayer.Id, _game.CurrentPlayer.Hand.Total, _game.CurrentPlayer.Hand.CardCount);

            if (_game.CurrentPlayer.Hand.HasPlayableCard == false)
            {
                Clients.All.cardPickup(_game.CurrentPlayer.Name, _game.CurrentPlayer.Id);
                PlayCard(null);
            }
        }
    }
}