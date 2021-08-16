namespace CardGame
{
    public class Card
    {
        private int _cardValue;

        // The number printed on the card
        public int CardValue
        {
            get
            {
                return _cardValue;
            }
        }

        /// <summary>
        /// Constructor which creates instance of the class and sets the card value
        /// </summary>
        /// <param name="cardValue">The number printed on the card</param>
        public Card(int cardValue)
        {
            this._cardValue = cardValue;
        }


    }
}
