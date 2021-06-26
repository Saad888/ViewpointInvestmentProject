using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Project.Recording;
using Project.Logging;
using Project.Repository;
using Project.Models;

namespace Project.Trading
{
    public class TradingService
    {
        #region Properties
        Logger Logger { get; set; }
        StockRepository Repo { get; set; }
        RecordingService Record { get; set; }

        Dictionary<string, double> PreviousOrders { get; set; }

        const double ORDER_THRESHOLD = 5;
        const int DELAY = 1000;
        #endregion

        #region Constructor
        public TradingService(StockRepository repo)
        {
            Repo = repo;
            Logger = new Logger("Trading Service");
            PreviousOrders = new Dictionary<string, double>();
            Record = new RecordingService();
        }
        #endregion

        #region Methods
        public void Run()
        {
            while(true)
            {
                Thread.Sleep(DELAY);

                Logger.Log("Getting Stock Prices");
                var stockPrices = Repo.GetStocks();

                // Generate Orders
                var orders = GenerateOrders(stockPrices);

                // Submit Orders
                var transactions = SubmitOrders(orders);

                // Log transactions
                Record.RecordTransaction(transactions);
            }
            throw new NotImplementedException();
        }


        /// <summary>
        /// Generate orders from stocks
        /// </summary>
        /// <param name="stockPrices">Existing stock prices</param>
        /// <returns>List of orders from stocks</returns>
        public List<Order> GenerateOrders(List<StockPrice> stockPrices)
        {
            var orders = new List<Order>();
            foreach(var stock in stockPrices)
            {
                // Get previous price and compare change
                var previousPrice = PreviousOrders.ContainsKey(stock.StockTicker) ? PreviousOrders[stock.StockTicker] : -1;
                var change = ChangePercentage(previousPrice, stock.Price);

                // If change < 5%, continue
                if (Math.Abs(change) < ORDER_THRESHOLD)
                    continue;

                // Generate order
                var side = change > 0 ? OrderTypes.BUY : OrderTypes.SELL;
                var order = new Order() { StockTicker = stock.StockTicker, Price = stock.Price, Quantity = 10, Side = side };
                orders.Add(order);

                // Update previous order price
                PreviousOrders[stock.StockTicker] = stock.Price;
            }
            return orders;
        }

        private double ChangePercentage(double previous, double next) => (100 * (next - previous) / Math.Abs(previous));


        /// <summary>
        /// Submits list of order and returns list of transactions
        /// </summary>
        /// <param name="orders">List of stock orders</param>
        /// <returns>List of transactions</returns>
        public List<Transaction> SubmitOrders(List<Order> orders) => orders.Select(o => SubmitOrder(o)).ToList();


        /// <summary>
        /// Submits the order
        /// </summary>
        /// <param name="order">Stock order request</param>
        /// <returns>Transaction</returns>
        public Transaction SubmitOrder(Order order)
        {
            // Validate order
            if (order.Side != OrderTypes.BUY && order.Side != OrderTypes.SELL)
                throw new ArgumentException("Invalid Order Types");

            // Submit order
            var color = order.Side == OrderTypes.BUY ? ConsoleColor.Green : ConsoleColor.Red;
            Logger.Log($"{order.Side} {order.Quantity} orders of {order.StockTicker} at ${order.Price.ToString("0.00")}", color);

            // Create transaction
            var transaction = new Transaction() { StockTicker = order.StockTicker, Price = order.Price, Quantity = order.Quantity, Side = order.Side, ExecutionDate = DateTime.Now };
            return transaction;
        }
        #endregion
    }
}
