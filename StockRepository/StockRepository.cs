using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Project.Models;

namespace Project.Repository
{
    public class StockRepository
    {
        ConcurrentDictionary<string, StockPrice> Stocks { get; set; }

        public StockRepository()
        {
            Stocks = new ConcurrentDictionary<string, StockPrice>();
        }

        /// <summary>
        /// Update stock price information (Adds if previously not saved)
        /// </summary>
        /// <param name="stock">Stock being updated/added</param>
        public void UpdateStock(StockPrice stock)
        {
            // Validation
            if (stock.Price < 0)
                throw new ArgumentException("Price Invalid");
            if (Stocks.ContainsKey(stock.StockTicker) && Stocks[stock.StockTicker].Timestamp > stock.Timestamp)
                throw new ArgumentException("Timestamp Invalid");

            Stocks[stock.StockTicker] = stock;
        }

        /// <summary>
        /// Returns list of all stocks saved in memory
        /// </summary>
        /// <returns></returns>
        public List<StockPrice> GetStocks()
        {
            return Stocks.Values.ToList();
        }
    }
}
