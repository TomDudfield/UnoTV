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

        public Player CurrentPlayer { get; private set; }
        public IList<Player> Players { get; private set; }
        public Queue<Card> PlayedCards { get; private set; }
        public bool Started { get; private set; }

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
            if (Players.Count > 9)
                throw new Exception("Too many players already in the game.");
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
                throw new Exception("Two or more players required to start.");
            if (Started)
                throw (new Exception("Can't start game that has already started."));

            Started = true;
            ResetGame();
        }

        /// <summary>
        /// Resets the game to initial hands.
        /// </summary>
        public void ResetGame()
        {
            _reverse = false;

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

            if (CurrentPlayer != null && card != null)
                CurrentPlayer.Hand.RemoveCard(card);

            if (Finished)
                Started = false;
            else
                MoveToNextPlayer(card);
        }

        private void MoveToNextPlayer(Card card)
        {
            if (CurrentPlayer == null)
                CurrentPlayer = Players.First();

            if (card != null)
                CurrentPlayer.Hand.RemoveCard(card);

            var index = Players.IndexOf(CurrentPlayer);

            index = UpdateIndex(index);

            if (card != null && card.Type == CardType.Skip)
                index = UpdateIndex(index);

            CurrentPlayer = Players[index];

            foreach (var playableCard in CurrentPlayer.Hand.PlayableCards)
            {
                playableCard.PickedUp = false;
            }

            if (card != null)
            {
                if (card.Type == CardType.WildDraw)
                {
                    CurrentPlayer.Hand.PlayableCards.Add(new PlayableCard(PlayedCards.Dequeue(), true));
                    CurrentPlayer.Hand.PlayableCards.Add(new PlayableCard(PlayedCards.Dequeue(), true));
                }

                if (card.Type == CardType.Draw || card.Type == CardType.WildDraw)
                {
                    CurrentPlayer.Hand.PlayableCards.Add(new PlayableCard(PlayedCards.Dequeue(), true));
                    CurrentPlayer.Hand.PlayableCards.Add(new PlayableCard(PlayedCards.Dequeue(), true));
                    index = UpdateIndex(index);
                    CurrentPlayer = Players[index];
                }
            }

            PlayableCards.Process(CurrentPlayer.Hand.PlayableCards, CurrentCard);

            if (CurrentPlayer.Hand.HasPlayableCard == false)
            {
                CurrentPlayer.Hand.PlayableCards.Add(new PlayableCard(PlayedCards.Dequeue(), true)
                {
                    Playable = false
                });
            }
        }

        private int UpdateIndex(int index)
        {
            if (_reverse)
                index--;
            else
                index++;

            if (index >= Players.Count)
                index = 0;
            if (index < 0)
                index = Players.Count - 1;

            return index;
        }
    }
}