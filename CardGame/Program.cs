using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame
{
    class Program
    {
        /// <summary>
        /// Game Starting method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                GameEngine gameEngine = GameEngine.Instance;
                // Creates list of players.
                var players = gameEngine.IntializePlayersWithDrawDecks();

                while (true)
                {

                    var drawnCards = new Dictionary<int, Card>();

                    // Draw cards
                    for (int i = 0; i < players.Count; i++)
                    {
                        var drawnCard = gameEngine.DrawCard(players[i]);
                        drawnCards.Add(players[i].ID, drawnCard);
                        Console.WriteLine(players[i].Name + "(" + players[i].NumberOfAvailableCards +  " Cards ) : " + drawnCard.CardValue);
                    }

                    // Compare cards and get the winner or if the round is draw
                    switch (gameEngine.CompareDrawnCards(drawnCards, ref players))
                    {
                        case RoundStatus.DRAW:
                            Console.WriteLine("No Winner in this round \n");
                            break;

                        case RoundStatus.WIN:
                            Console.WriteLine(Dashboard.Instance.RoundWinner.Name + " wins this round \n");
                            Dashboard.Instance.Clear();
                            break;
                    }

                    // Check if the game has ended
                    switch (gameEngine.CheckGameWinner(ref players))
                    {
                        case GameStatus.CONTINUE:
                            break;
                        case GameStatus.DRAW:
                            Console.WriteLine(" Match is draw");
                            endGame(ref players);
                            break;
                        case GameStatus.WIN:
                            Console.WriteLine(Dashboard.Instance.GameWinner.Name + " wins the game");
                            endGame(ref players);
                            break;
                    }
                }

            }
            catch
            {
                Console.WriteLine("There is an internal error occured. Game cannot be started");
            }
        }
        /// <summary>
        /// Handles the operation after a game is completed
        /// </summary>
        /// <param name="players"></param>
        private static void endGame(ref List<Player> players)
        {
            Console.WriteLine(" -----------------------------------------------------------------------------------------\n1.Press any key for New Game \n2.Press Escape to close the game");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                Environment.Exit(0);

            Dashboard.Instance.Clear();

            players = GameEngine.Instance.IntializePlayersWithDrawDecks();
        }
    }
}
