using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Repository;
using Project.Models;
using Project.Trading;

namespace UnitTests
{
    [TestClass]
    public class TradingServiceTests
    {
        #region Template Stocks
        StockPrice stockA_1 = new StockPrice() { StockTicker = "A", Timestamp = DateTime.Now, Price = 100 };
        StockPrice stockA_2 = new StockPrice() { StockTicker = "A", Timestamp = DateTime.Now, Price = 150 };
        StockPrice stockA_3 = new StockPrice() { StockTicker = "A", Timestamp = DateTime.Now, Price = 50 };

        StockPrice stockB_1 = new StockPrice() { StockTicker = "B", Timestamp = DateTime.Now, Price = 100 };

        StockPrice stockC_1 = new StockPrice() { StockTicker = "C", Timestamp = DateTime.Now, Price = 100 };
        #endregion

        #region Template Orders
        Order orderA = new Order() { StockTicker = "A", Quantity = 10, Price = 100, Side = "SELL" };
        Order orderB = new Order() { StockTicker = "B", Quantity = 10, Price = 100, Side = "BUY" };
        Order orderC = new Order() { StockTicker = "C", Quantity = 10, Price = 100, Side = "INVALID" };
        #endregion

        [TestMethod]
        public void GenerateOrders_InitialRequest_GeneratesAllBuyOrders()
        {
            // Arrange
            var tradingService = new TradingService(null);
            var initialStocks = new List<StockPrice>() { stockA_1, stockB_1, stockC_1 };

            // Act
            var initialOrders = tradingService.GenerateOrders(initialStocks);

            // Assert
            Assert.AreEqual(3, initialOrders.Count);
            Assert.IsTrue(initialOrders.All(i => i.Side == OrderTypes.BUY));

        }

        [TestMethod]
        public void GenerateOrders_PriceRaiseBy5Percent_GeneratesBuyOrders()
        {
            // Arrange
            var tradingService = new TradingService(null);
            var initialStocks = new List<StockPrice>() { stockA_1 };
            var addStocks = new List<StockPrice>() { stockA_2 };

            // Act
            tradingService.GenerateOrders(initialStocks);
            var order = tradingService.GenerateOrders(addStocks);

            // Assert
            Assert.AreEqual(OrderTypes.BUY, order.First().Side);
        }

        [TestMethod]
        public void GenerateOrders_PriceDropBy5Percent_GeneratesSellOrders()
        {
            // Arrange
            var tradingService = new TradingService(null);
            var initialStocks = new List<StockPrice>() { stockA_1 };
            var addStocks = new List<StockPrice>() { stockA_3 };

            // Act
            tradingService.GenerateOrders(initialStocks);
            var order = tradingService.GenerateOrders(addStocks);

            // Assert
            Assert.AreEqual(OrderTypes.SELL, order.First().Side);
        }

        [TestMethod]
        public void GenerateOrders_MinimalPriceDifference_GeneratesNoOrders()
        {
            // Arrange
            var tradingService = new TradingService(null);
            var initialStocks = new List<StockPrice>() { stockA_1 };
            var addStocks = new List<StockPrice>() { stockA_3 };

            // Act
            tradingService.GenerateOrders(initialStocks);
            var order = tradingService.GenerateOrders(addStocks);

            // Assert
            Assert.AreEqual(OrderTypes.SELL, order.First().Side);
        }


        [TestMethod]
        public void SubmitOrder_OnValidOrder_ReturnsValidTransaction()
        {
            // Arrange
            var tradingService = new TradingService(null);

            // Act
            var transactionA = tradingService.SubmitOrder(orderA);
            var transactionB = tradingService.SubmitOrder(orderB);

            // Assert
            Assert.IsTrue(CompareOrderToTransaction(orderA, transactionA));
            Assert.IsTrue(CompareOrderToTransaction(orderB, transactionB));
        }

        [TestMethod]
        public void SubmitOrder_OnInvalidOrderTypes_ThrowsException()
        {
            // Arrange
            var tradingService = new TradingService(null);

            // Assert
            Assert.ThrowsException<ArgumentException>(() => tradingService.SubmitOrder(orderC));
        }


        private bool CompareOrderToTransaction(Order order, Transaction transaction)
        {
            if (order.Price != transaction.Price)
                return false;
            if (order.Quantity != transaction.Quantity)
                return false;
            if (order.Side != transaction.Side)
                return false;
            if (order.StockTicker != transaction.StockTicker)
                return false;
            return true;
        }
    }
}
