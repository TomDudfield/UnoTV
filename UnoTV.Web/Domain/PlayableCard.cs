﻿
using Newtonsoft.Json;

namespace UnoTV.Web.Domain
{
    public class PlayableCard
    {
        public bool Playable { get; set; }
        public bool PickedUp { get; set; }

        /// <summary>
        /// Ignoring this as we don't want to send it to the client.
        /// </summary>
        [JsonIgnore]
        public Card Card { get; set; }

        public string Colour
        {
            get { return Card.Colour; }
        }

        public int Value
        {
            get { return Card.Value; }
        }

        public PlayableCard(Card card)
        {
            Card = card;
        }

        public PlayableCard() { }
    }
}