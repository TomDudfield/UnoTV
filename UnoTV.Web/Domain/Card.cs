
using System.Collections.Generic;
namespace UnoTV.Web.Domain
{
    public class Card
    {
        public string Colour { get; set; }
        public int Value { get; set; }
    }

    public class CardColor
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
}