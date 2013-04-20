
using System.Collections.Generic;
namespace UnoTV.Web.Domain
{
    public class Card
    {
        public string Colour { get; set; }
        public int Value { get; set; }
        public string Type { get; set; }
    }

    public class CardColour
    {
        public const string Blue = "blue";
        public const string Green = "green";
        public const string Red = "red";
        public const string Yellow = "yellow";

        public static IList<string> AsList()
        {
            return new List<string> { Blue, Green, Red, Yellow };
        }
    }

    public class CardType
    {
        public const string Face = "face";
        public const string Reverse = "reverse";
        public const string Draw = "draw";
        public const string Skip = "skip";
        public const string Wild = "wild";
        public const string WildDraw = "wilddraw";

        public static IList<string> AsList()
        {
            return new List<string> { Face, Reverse, Draw, Skip, Wild, WildDraw };
        }
    }
}