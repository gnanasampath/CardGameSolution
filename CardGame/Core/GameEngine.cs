using System;
using System.Collections.Generic;
using System.Linq;


namespace CardGame
{
    public class GameEngine
    {

        // Cards will be added the tiedCardsDeck when all the players draw cards with same value.
        List<Card> tiedCardsDeck = null;
        // Class instance
        private static GameEngine _instance = null;
        public static GameEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameEngine();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initialize the class and create drawDeck Object.
        /// </summary>
        private GameEngine()
        {
            tiedCardsDeck = new List<Card>();
        }
        /// <summary>
        /// To Clear the tie Deck
        /// </summary>
        public void ClearTieDeck()
        {
            tiedCardsDeck.Clear();
        }

        /// <summary>
        /// Takes the cards lists and returns the shuffled list
        /// </summary>
        /// <param name="cardsDeck">List of cards to be shuffled</param>
        /// <returns>Shuffled List of cards</returns>

        public List<Card> Shuffle(List<Card> cardsDeck)
        {
            try
            {
                if (cardsDeck == null)
                    return null;

                // Creating a object for Random class
                var randomIndexGenerator = new Random();

                // Start from the last element and swap one by one.
                for (var shuffleIterator = cardsDeck.Count - 1; shuffleIterator > 0; shuffleIterator--)
                {
                    // Pick a random index from 0 to shuffleIterator
                    var randomIndex = randomIndexGenerator.Next(0, shuffleIterator + 1);

                    // Swap cardsDeck[shuffleIterator] with the element at random index
                    Card temp = cardsDeck[shuffleIterator];
                    cardsDeck[shuffleIterator] = cardsDeck[randomIndex];
                    cardsDeck[randomIndex] = temp;
                }

                return cardsDeck;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks winner of the game after each round. 
        /// </summary>
        /// <param name="players">List of player objects</param>
        /// <returns>Status of the Game - Enum</returns>
        public GameStatus CheckGameWinner(ref List<Player> players)
        {
            try
            {
                var winningPlayer = players.Where(player => player.DiscardPile.Count > 0 || player.DrawPile.Count > 0).Select(player => player).ToList();

                // If there is only one player has more than 0 Cards he will be the winner
                if (winningPlayer.Count == 1)
                {
                    Dashboard.Instance.SetGameWinner(winningPlayer[0]);
                    tiedCardsDeck.Clear();
                    return GameStatus.WIN;
                }
                // If all cards are there in tiedCardsDeck 
                else if (tiedCardsDeck.Count == Constants.TotalDeckSize)
                {
                    tiedCardsDeck.Clear();
                    return GameStatus.DRAW;
                }

                // Indicates that match is not yet completed
                return GameStatus.CONTINUE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Creates Default Card Deck to Play
        /// </summary>
        /// <param name="players">List of playes playing the game</param>
        /// <returns>Returns true if the players are created else false</returns>
        public bool CreatePlayersDrawPile(ref List<Player> players)
        {
            try
            {
                var totalCardsDeck = new List<Card>();
                var deckSize = Constants.TotalDeckSize;

                // Create and add cards to the deck 
                for (var DeckIterator = 1; DeckIterator <= deckSize; DeckIterator++)
                    totalCardsDeck.Add(new Card(DeckIterator % 10 == 0 ? 10 : DeckIterator % 10));

                // Shuffles the created cards
                totalCardsDeck = Shuffle(totalCardsDeck);

                var cardsStartIndex = 0;
                var eachPlayerCardCount = deckSize / players.Count;

                // Assign equivalent cards to the players
                foreach (Player player in players)
                {
                    player.DrawPile.AddRange(totalCardsDeck.GetRange(cardsStartIndex, Math.Min(eachPlayerCardCount, totalCardsDeck.Count - cardsStartIndex)));
                    if (cardsStartIndex >= deckSize)
                        break;
                    cardsStartIndex += eachPlayerCardCount;
                }

                return totalCardsDeck.Count == deckSize ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Compares the drawn cards and returns the winner Id
        /// </summary>
        /// <param name="drawnCards">List of drawn cards from all the players</param>
        /// <param name="players">List of all the players</param>
        /// <returns>Status of the round - Enum</returns>
        public RoundStatus CompareDrawnCards(Dictionary<int, Card> drawnCards, ref List<Player> players)
        {
            try
            {
                // Round Draw Scenario
                if (drawnCards[1].CardValue == drawnCards[2].CardValue)
                {
                    tiedCardsDeck.AddRange(drawnCards.Select(card => card.Value));
                    return RoundStatus.DRAW;
                }

                // Getting Winner Id based on the values
                var winningPlayerID = (drawnCards[1].CardValue > drawnCards[2].CardValue) ? 1 : 2;

                // Adding the won cards to the discard pile of the player
                foreach (Player player in players)
                {
                    if (player.ID == winningPlayerID)
                    {
                        // Sets the round winner to the 
                        Dashboard.Instance.SetRoundWinner(player);
                        if (tiedCardsDeck.Count > 0)
                        {
                            player.DiscardPile.AddRange(tiedCardsDeck);
                            tiedCardsDeck.Clear();
                        }
                        player.DiscardPile.AddRange(drawnCards.Select(card => card.Value));
                    }
                }

                return RoundStatus.WIN;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Draws the last card from the player's pile of cards
        /// </summary>
        /// <param name="player">The player whose cards should be drawn</param>
        /// <returns>Drawn card</returns>
        public Card DrawCard(Player player)
        {
            try
            {
                // If no Draw Pile Card Available - check Discard Pile , shuffle and add to the draw pile
                if (player.DrawPile.Count == 0)
                    if (player.DiscardPile.Count > 0)
                    {
                        player.DrawPile.AddRange(Shuffle(player.DiscardPile));
                        player.DiscardPile.Clear();
                    }

                // Take the last card and remove the card from draw pile
                if (player.DrawPile.Count > 0)
                {
                    var drawnCard = player.DrawPile.Last();
                    player.DrawPile.RemoveAt(player.DrawPile.Count - 1);
                    return drawnCard;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates list of players
        /// </summary>
        /// <returns>List of players</returns>
        public List<Player> IntializePlayersWithDrawDecks()
        {
            try
            {
                var players = new List<Player>();

                for (var playerCountIterator = 0; playerCountIterator < Constants.PlayerCount; playerCountIterator++)
                    players.Add(new Player(Constants.DefaultPlayerName + (playerCountIterator + 1), playerCountIterator + 1));

                CreatePlayersDrawPile(ref players);

                return players;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

