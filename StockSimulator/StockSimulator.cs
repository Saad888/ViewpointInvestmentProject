using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Project.Repository;
using Project.Models;
using Project.Logging;

namespace Project.Simulator
{
    public class StockSimulator
    {
        #region Properties
        StockRepository Repo { get; set; }
        Random Rand { get; set; }
        Logger Logger { get; set; }

        const double PRICE_MIN = 10.00;
        const double PRICE_MAX = 100.00;
        const double ADJUST_LIMIT = 0.1;
        const int DELAY_MIN = 2000;
        const int DELAY_MAX = 15000;

        readonly List<string> STOCKS = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "J" };
        #endregion

        #region Public Method
        public StockSimulator(StockRepository _repo)
        {
            Repo = _repo;
            Rand = new Random();
            Logger = new Logger("Stock Simulator", ConsoleColor.Yellow);
        }

        /// <summary>
        /// Create multiple producer threads with various stocks
        /// </summary>
        public void Run()
        {
            // Create simulation thread per stock
            STOCKS.ForEach(s => 
                {
                    var thread = new Thread(() => { SimulateStock(s); });
                    thread.Start();
                }
            );
        }
        #endregion

        #region Private Methods
        private void SimulateStock(string stockTicker)
        {
            // Create stock and set initial price
            var stock = new StockPrice() { StockTicker = stockTicker, Price = GenerateRandomPrice() };
            Repo.UpdateStock(stock);
            Logger.Log($"Start stock {stock.StockTicker} price at ${stock.Price.ToString("0.00")}");

            while (true)
            {
                // Sleep for random amount
                Thread.Sleep(Rand.Next(DELAY_MIN, DELAY_MAX));

                // Update price randomly
                var newPrice = GenerateAdjustedPrice(stock.Price);
                Logger.Log($"Update stock {stock.StockTicker} price from ${stock.Price.ToString("0.00")} to ${newPrice.ToString("0.00")}");
                stock.Price = newPrice;
                Repo.UpdateStock(stock);
            }
        }

        private double GenerateRandomPrice()
        {
            return ((PRICE_MAX - PRICE_MIN) * Rand.NextDouble() + PRICE_MIN);
        }

        private double GenerateAdjustedPrice(double price)
        {
            var min = -1 * ADJUST_LIMIT / 2;
            var max = ADJUST_LIMIT / 2;
            var adjust = ((max - min) * Rand.NextDouble()) + min;
            return price * (1 + adjust);
        }
        #endregion
    }
}
