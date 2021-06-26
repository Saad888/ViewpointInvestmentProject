using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Repository;
using Project.Models;

namespace UnitTests
{
    [TestClass]
    public class StockRepositoryTests
    {
        [TestMethod]
        public void UpdateStock_InvalidPrice_ThrowsArgumentException()
        {
            // Arrange
            var testStock1 = new StockPrice() { Price = -1, StockTicker = "A", Timestamp = DateTime.Now };
            var repo = new StockRepository();
            // Act
            Action addStock1 = () => { repo.UpdateStock(testStock1); };
            // Assert
            Assert.ThrowsException<ArgumentException>(addStock1);
        }

        [TestMethod]
        public void UpdateStock_TimeStampOlderThanPreviousUpdate_ThrowsArgumentException()
        {
            // Arrange
            var testStock1 = new StockPrice() { Price = 1, StockTicker = "A", Timestamp = DateTime.Now };
            var testStock2 = new StockPrice() { Price = 1, StockTicker = "A", Timestamp = DateTime.Now.AddDays(-100) };
            var repo = new StockRepository();
            // Act
            repo.UpdateStock(testStock1);
            Action addStock = () => { repo.UpdateStock(testStock2); };
            // Assert
            Assert.ThrowsException<ArgumentException>(addStock);
        }

        [TestMethod]
        public void GetStocks_ReturnsValidStocks()
        {
            // Arrange
            var testStock1 = new StockPrice() { Price = 1, StockTicker = "A", Timestamp = DateTime.Now };
            var testStock2 = new StockPrice() { Price = 2, StockTicker = "B", Timestamp = DateTime.Now };
            var testStock3 = new StockPrice() { Price = 3, StockTicker = "A", Timestamp = DateTime.Now };
            var repo = new StockRepository();

            // Act
            repo.UpdateStock(testStock1);
            repo.UpdateStock(testStock2);
            repo.UpdateStock(testStock3);
            var stocks = repo.GetStocks();

            // Assert
            var stockA = stocks.First(s => s.StockTicker == "A");
            var stockB = stocks.First(s => s.StockTicker == "B");
            Assert.AreEqual(stockA.Price, 3);
            Assert.AreEqual(stockB.Price, 2);

        }
    }
}
