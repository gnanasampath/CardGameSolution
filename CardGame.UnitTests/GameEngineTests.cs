using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.UnitTests
{
    [TestClass]
    public class GameEngineTests
    {
        GameEngine gameEngine = null;

        // Initial setup runs before each test
        [TestInitialize]
        public void Setup()
        {
            gameEngine = GameEngine.Instance;
        }

        //Creates a sample test Players
        private List<Player> CreateTestPlayers()
        {
            List<Player> testPlayers = new List<Player>();
            testPlayers.Add(new Player("FirstPlayer", 1));
            testPlayers.Add(new Player("SecondPlayer", 2));
            gameEngine.ClearTieDeck();

            return testPlayers;
        }

        /// <summary>
        /// Test Shuffle method if the return list of cards are at least 70% shuffled
        /// </summary>
        [TestMethod]
        public void Shuffle_InputProvided_ReturnShuffledList()
        {
            List<Card> testCards = new List<Card>();
            for (int i = 0; i <= 10; i++)
                testCards.Add(new Card(i));

            var result = gameEngine.Shuffle(testCards);

            int countOfShuffledIndex = 0;
            for (int i = 0; i <= 10; i++)
            {
                if (result[i].CardValue != i + 1)
                    countOfShuffledIndex++;
            }

            //Check if 70% of the cards are shuffled to pass the test
            Assert.AreEqual(true, countOfShuffledIndex > 7 ? true : false);

        }
        /// <summary>
        /// Tests Check Game winner for winning scenario
        /// </summary>
        [TestMethod]
        public void CheckGameWinner_WinningScenario_ReturnGameStatusWin()
        {
            var testPlayers = CreateTestPlayers();
            testPlayers[1].DrawPile.Add(new Card(1));

            var result = gameEngine.CheckGameWinner(ref testPlayers);

            Assert.AreEqual(GameStatus.WIN, result);
        }
        // Tests Game Draw Scenario
        [TestMethod]
        public void CheckGameWinner_DrawScenario_ReturnGameStatusDraw()
        {
            var testPlayers = CreateTestPlayers();
            var drawnCards = new Dictionary<int, Card>();
            drawnCards.Add(testPlayers[0].ID, new Card(5));
            drawnCards.Add(testPlayers[1].ID, new Card(5));

            for (int roundIterator = 0; roundIterator < Constants.TotalDeckSize / 2; roundIterator++)
                gameEngine.CompareDrawnCards(drawnCards, ref testPlayers);

            var result = gameEngine.CheckGameWinner(ref testPlayers);
            Assert.AreEqual(GameStatus.DRAW, result);
        }
        /// <summary>
        /// Tests total number of cards created and assigned to the players are as per the configuration
        /// </summary>
        [TestMethod]
        public void CreatePlayersDrawPile_InputProvided_ReturnsPlayersWithDrawPile()
        {
            var testPlayers = CreateTestPlayers();
            gameEngine.CreatePlayersDrawPile(ref testPlayers);

            var totalNumberOfCardsAllocated = testPlayers.SelectMany(player => player.DrawPile).Count();

            Assert.AreEqual(true, totalNumberOfCardsAllocated == Constants.TotalDeckSize ? true : false);
        }
        /// <summary>
        /// Tests a round win scenario
        /// </summary>
        [TestMethod]
        public void CompareDrawnCards_InputsProvided_PlayerWins()
        {
            var testPlayers = CreateTestPlayers();
            //List of drawn Cards
            var drawnCards = new Dictionary<int, Card>();
            drawnCards.Add(testPlayers[0].ID, new Card(10));
            drawnCards.Add(testPlayers[1].ID, new Card(5));

            var roundStatus = gameEngine.CompareDrawnCards(drawnCards, ref testPlayers);

            Assert.AreEqual(roundStatus, RoundStatus.WIN);
        }
        /// <summary>
        ///  Tests a round draw scenario
        /// </summary>
        [TestMethod]
        public void CompareDrawnCards_InputsProvided_RoundDraw()
        {
            var testPlayers = CreateTestPlayers();
            //List of drawn cards
            var drawnCards = new Dictionary<int, Card>();
            drawnCards.Add(testPlayers[0].ID, new Card(5));
            drawnCards.Add(testPlayers[1].ID, new Card(5));

            var roundStatus = gameEngine.CompareDrawnCards(drawnCards, ref testPlayers);

            Assert.AreEqual(roundStatus, RoundStatus.DRAW);
        }
        /// <summary>
        /// Tests if the previous round is draw and the current round winner gets awarded with 4 cards in the discardpile
        /// </summary>
        [TestMethod]
        public void CompareDrawnCards_InputsProvided_WinAfterRoundDraw()
        {
            var testPlayers = CreateTestPlayers();
            // List of drawn cards
            var drawnCards = new Dictionary<int, Card>();
            drawnCards.Add(testPlayers[0].ID, new Card(5));
            drawnCards.Add(testPlayers[1].ID, new Card(5));

            var roundStatus = gameEngine.CompareDrawnCards(drawnCards, ref testPlayers);

            // List of drawn cards
            drawnCards = new Dictionary<int, Card>();
            drawnCards.Add(testPlayers[0].ID, new Card(10));
            drawnCards.Add(testPlayers[1].ID, new Card(5));

            roundStatus = gameEngine.CompareDrawnCards(drawnCards, ref testPlayers);

            Assert.AreEqual(true, testPlayers[0].DiscardPile.Count == 4 ? true : false);
        }

        /// <summary>
        /// Tests if the system handles multiple draw in a row
        /// </summary>
        [TestMethod]
        public void CompareDrawnCards_InputsProvided_SubSequentRoundDraw()
        {
            var testPlayers = CreateTestPlayers();
            var drawnCards = new Dictionary<int, Card>();
            drawnCards.Add(testPlayers[0].ID, new Card(5));
            drawnCards.Add(testPlayers[1].ID, new Card(5));

            var roundStatus = gameEngine.CompareDrawnCards(drawnCards, ref testPlayers);

            roundStatus = gameEngine.CompareDrawnCards(drawnCards, ref testPlayers);

            Assert.AreEqual(roundStatus, RoundStatus.DRAW);
        }

        /// <summary>
        /// Tests if a player with draw pile draws proper card
        /// </summary>
        [TestMethod]
        public void DrawCard_PlayerWithDrawPile_ReturnCard()
        {
            var player = new Player("Player1", 1);
            player.DrawPile.Add(new Card(5));

            var result = gameEngine.DrawCard(player);

            Assert.AreEqual(true, result.CardValue == 5 ? true : false);

        }
        /// <summary>
        /// Tests if a player with only discard pile draws proper card from the shuffled discard pile
        /// </summary>
        [TestMethod]
        public void DrawCard_PlayerWithOnlyDiscardPile_ReturnCardFromDiscardPile()
        {
            var player = new Player("Player1", 1);
            player.DiscardPile.Add(new Card(5));

            var result = gameEngine.DrawCard(player);

            Assert.AreEqual(true, result.CardValue == 5 ? true : false);
        }
        /// <summary>
        /// Tests if the system creates configured number of players
        /// </summary>
        [TestMethod]
        public void CreatePlayers_NoInput_ReturnsPlayersBasedOnConfig()
        {
            var result = gameEngine.IntializePlayersWithDrawDecks();
            Assert.AreEqual(true, result.Count == Constants.PlayerCount ? true : false);
        }

    }
}

