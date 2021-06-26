namespace Project.Models
{
    public class Order
    {
        /// <summary>
        /// Declare if the transaction type is "BUY" or "SELL"
        /// </summary>
        public string Side { get; set; }

        /// <summary>
        /// Price of the trade
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Stock ticker
        /// </summary>
        public string StockTicker { get; set; }

        /// <summary>
        /// Amount of stocks traded
        /// </summary>
        public int Quantity { get; set; }
    }
}
