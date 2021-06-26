using System.Collections.Generic;
using System.Data.SqlClient;
using Project.Models;

namespace Project.Recording
{
    public class RecordingService
    {
        private const string CONNECTION_STRING = "CONNECTION_STRING";
        private SqlConnection Connection { get; set; }

        public RecordingService()
        {
            Connection = new SqlConnection(CONNECTION_STRING);
            Connection.Open();
            ClearTable();
        }

        public void RecordTransaction(List<Transaction> transactions)
        {
            // Records transactions in database
            transactions.ForEach((t) => SaveToDatabase(t));
        }

        private void SaveToDatabase(Transaction transaction)
        {
            string sql = @"
                INSERT INTO transactions
                VALUES (@ticker, @side, @quantity, @price, @execution_date)
            ";

            var command = new SqlCommand(sql, Connection);
            command.Parameters.Add(new SqlParameter("@ticker", transaction.StockTicker));
            command.Parameters.Add(new SqlParameter("@side", transaction.Side));
            command.Parameters.Add(new SqlParameter("@quantity", transaction.Quantity));
            command.Parameters.Add(new SqlParameter("@price", transaction.Price));
            command.Parameters.Add(new SqlParameter("@execution_date", transaction.ExecutionDate));

            command.ExecuteNonQuery();
        }

        private void ClearTable()
        {
            string sql = "DELETE FROM transactions";
            var command = new SqlCommand(sql, Connection);
            command.ExecuteNonQuery();
        }
    }
}
