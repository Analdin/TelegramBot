using CalculationEfficiency.Engine;
using MySql.Data.MySqlClient;

namespace ReplyKeyboardMarkup_01
{
    class Program
    {
        static void Main(string[] args)
        {
            DbHelper connect = new DbHelper();
            var cleanQuery = @"TRUNCATE TABLE `TG_response`";
            var command = new MySqlCommand(cleanQuery, connect.Connection);
            connect.OpenConnection();
            command.ExecuteNonQuery();
            connect.CloseConnection();

            Console.WriteLine("Start!");

            try
            {
                TelegramBotHelper hlp = new TelegramBotHelper(token: "5239217434:AAF9ir9cGGM522Z4QRIGnUW6fTxnihyTESM");
                hlp.GetUpdates();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}