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
            Console.ReadLine();

            try
            {
                TelegramBotHelper hlp = new TelegramBotHelper(token: "5228504390:AAEJs2auWV1a4-0q02IhmHv-UPMASHSRsdA");
                hlp.GetUpdates();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}