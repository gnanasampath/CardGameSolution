using System.Collections.Generic;

namespace CardGame
{
    public class Player
    {
        private int _id;
        private string _name;
        public int NumberOfAvailableCards
        {
            get
            {
                return this.DiscardPile.Count + this.DrawPile.Count + 1;
            }
        }
        // Player's Name
        public string Name
        {
            get
            {
                return _name;
            }
        }

        // Players's Unique ID
        public int ID
        {
            get
            {
                return _id;
            }
        }
        // Player's Collection of Cards to draw
        public List<Card> DrawPile { get; set; }
        // The collection of cards won by the player
        public List<Card> DiscardPile { get; set; }

        public Player(string playerName, int playerID)
        {
            _id = playerID;
            _name = playerName;
            DrawPile = new List<Card>();
            DiscardPile = new List<Card>();
        }
    }
}
