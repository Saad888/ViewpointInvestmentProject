using System;

namespace Project.Models
{
    public class StockPrice
    {
        /// <summary>
        /// Stock Ticker
        /// </summary>
        public string StockTicker { get; set; }

        /// <summary>
        /// Stock Price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Time of last stock price update
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
