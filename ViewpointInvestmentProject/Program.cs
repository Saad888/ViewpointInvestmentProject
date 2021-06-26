using System;
using System.Threading;
using Project.Simulator;
using Project.Repository;
using Project.Trading;

namespace ViewpointInvestmentProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var repo = new StockRepository();
            var sim = new StockSimulator(repo);
            var trader = new TradingService(repo);

            // Run simulator on new thread
            var simThread = new Thread(() => sim.Run());
            simThread.Start();

            // Run trader on new thread
            var tradeThread = new Thread(() => trader.Run());
            tradeThread.Start();

            Thread.Sleep(-1);
        }
    }
}
