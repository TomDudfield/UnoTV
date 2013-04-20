using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnoTV.Web.Domain
{
    public class Hand
    {
        public IList<PlayableCard> PlayableCards { get; set; }

        /// <summary>
        /// Returns flag indicating whether the player currently
        /// has a playable card.
        /// </summary>
        [JsonIgnore]
        public bool HasPlayableCard
        {
            get { return PlayableCards.Where(x => x.Playable).Count() > 0; }
        }

        public Hand()
        {
            PlayableCards = new List<PlayableCard>();
        }

        /// <summary>
        /// Removes card from PlayableCards that matches the card provided.
        /// </summary>
        public void RemoveCard(Card card)
        {
            foreach (var playableCard in PlayableCards)
            {
                // does the colour & value match, if so this is the card to remove.
                if (playableCard.Colour == card.Colour && playableCard.Value == card.Value)
                {
                    PlayableCards.Remove(playableCard);
                    return;
                }
            }
        }
    }
}