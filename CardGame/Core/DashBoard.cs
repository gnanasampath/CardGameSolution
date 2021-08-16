using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    // Singleton class to keep game level properties
    public sealed class Dashboard
    {
        private Player _gameWinner = null;
        private Player _roundWinner = null;
        private Dashboard() { }
        private static Dashboard _instance = null;
        public static Dashboard Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Dashboard();
                }
                return _instance;
            }
        }

        public Player GameWinner
        {
            get
            {
                return _gameWinner;
            }
        }

        public Player RoundWinner
        {
            get
            {
                return _roundWinner;
            }
        }
        /// <summary>
        /// Sets the winner of the game
        /// </summary>
        /// <param name="winner"></param>
        public void SetGameWinner(Player winner)
        {
            this._gameWinner = winner;
        }
        /// <summary>
        /// Clears all the winners
        /// </summary>
        public void Clear()
        {
            this._gameWinner = null;
            this._roundWinner = null;
        }
        /// <summary>
        /// Sets the winner of the round
        /// </summary>
        /// <param name="winner"></param>
        public void SetRoundWinner(Player winner)
        {
            this._roundWinner = winner;
        }
    }
}
