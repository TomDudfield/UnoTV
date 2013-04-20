
namespace UnoTV.Web.Domain
{
    public class PlayableCard
    {
        private Card _card;

        public bool Playable { get; set; }
        public bool PickedUp { get; set; }

        public string Colour
        {
            get { return _card.Colour; }
        }

        public int Value
        {
            get { return _card.Value; }
        }

        public PlayableCard(Card card)
        {
            _card = card;
        }
    }
}