using System;
using System.Collections.Generic;
using UnoTV.Web.Domain;
using UnoTV.Web.Utils;
using System.Linq;

namespace UnoTV.Web.Game
{
    public class GameState
    {
        private bool _reverse;

        public bool Finished
        {
            get { return Players.Any(p => p.Hand.PlayableCards.Count == 0); }
        }

        public Player Winner
        {
            get { return Players.FirstOrDefault(p => p.Hand.PlayableCards.Count == 0); }
        }

        public Card CurrentCard
        {
            get { return PlayedCards.LastOrDefault(); }
        }

        public Player CurrentPlayer { get; set; }
        public IList<Player> Players { get; set; }
        public Queue<Card> PlayedCards { get; set; }

        public bool Started { get; set; }

        public GameState()
        {
            Players = new List<Player>();
            _reverse = false;
        }

        /// <summary>
        /// Adds a new player to the game.
        /// </summary>
        public void AddPlayer(Player player)
        {
            if (Started)
                throw (new Exception("Can't join game that has already started."));

            Players.Add(player);
        }

        /// <summary>
        /// Starts the game which determines the player order and deals
        /// the initial hands to the players.
        /// </summary>
        public void Start()
        {
            if (Players.Count < 2)
                throw new Exception("Two or more players required to start");

            if (Started)
                throw (new Exception("Can't start game that has already started."));

            Started = true;

            Players.Shuffle();
            var cards = Dealer.CreateCards();
            Dealer.Deal(Players, cards);

            PlayedCards = new Queue<Card>(cards);
            var card = PlayedCards.Dequeue();
            PlayCard(card);
        }

        /// <summary>
        /// Plays a card, removes from face down pile and adds to played pile
        /// </summary>
        /// <param name="card"></param>
        public void PlayCard(Card card)
        {
            if (card != null)
                PlayedCards.Enqueue(card);

            if (CurrentPlayer == null)
            {
                CurrentPlayer = Players.First();
            }
            else
            {
                if (card != null)
                    CurrentPlayer.Hand.RemoveCard(card);

                var index = Players.IndexOf(CurrentPlayer);

                if (_reverse)
                    index--;
                else
                    index++;

                if (index >= Players.Count)
                    index = 0;
                if (index < 0)
                    index = Players.Count - 1;

                CurrentPlayer = Players[index];
            }
            
            PlayableCards.Process(CurrentPlayer.Hand.PlayableCards, CurrentCard);

            if (CurrentPlayer.Hand.HasPlayableCard == false)
            {
                CurrentPlayer.Hand.PlayableCards.Add(new PlayableCard { Card = PlayedCards.Dequeue(), PickedUp = true, Playable = false });
            }
        }
    }
}