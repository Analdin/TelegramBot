using CalculationEfficiency.Engine;
using MySql.Data.MySqlClient;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ReplyKeyboardMarkup_01
{
    internal class TelegramBotHelper
    {
        private const string TEXT_1 = "Выкупы";
        private const string TEXT_2 = "Отчет в БД";
        private const string TEXT_3 = "Позиции";
        private const string TEXT_4 = "Ключ";

        private string _token;
        TelegramBotClient _client;

        public TelegramBotHelper(string token)
        {
            _token = token;
        }

        internal void GetUpdates()
        {
            _client = new TelegramBotClient(_token);
            var me = _client.GetMeAsync().Result;
            if (me != null && !string.IsNullOrEmpty(me.Username))
            {
                int offset = 0;
                while (true)
                {
                    try
                    {
                        var updates = _client.GetUpdatesAsync(offset).Result;
                        if (updates != null && updates.Count() > 0)
                        {
                            foreach (var update in updates)
                            {
                                processUpdate(update);
                                offset = update.Id + 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Thread.Sleep(1000);
                }
            }
        }

        private async void processUpdate(Update update)
        {
            DbHelper connect = new DbHelper();
            connect.OpenConnection();

            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    var text = update.Message.Text;
                    //Console.WriteLine("text - " + text);
                    switch (text)
                    {
                        case TEXT_1:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Делаем выкупы: " + text, replyMarkup: GetButtons());

                            var cleanQuery = @"TRUNCATE TABLE `TG_response`";
                            var cmdClean = new MySqlCommand(cleanQuery, connect.Connection);
                            cmdClean.ExecuteNonQuery();

                            var query = $@"INSERT INTO `TG_response`(`Cmd_Name`, `Btn_Name`) VALUES ('Yes','Buy')";

                            var command = new MySqlCommand(query, connect.Connection);
                            command.ExecuteNonQuery();

                            break;
                        case TEXT_2:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Выгружаем отчет в БД: " + text, replyMarkup: GetButtons());

                            var cleanQuery2 = @"TRUNCATE TABLE `TG_response`";
                            var cmdClean2 = new MySqlCommand(cleanQuery2, connect.Connection);
                            cmdClean2.ExecuteNonQuery();

                            var query2 = $@"INSERT INTO `TG_response`(`Cmd_Name`, `Btn_Name`) VALUES ('Yes','Report')";

                            var command2 = new MySqlCommand(query2, connect.Connection);
                            command2.ExecuteNonQuery();

                            break;
                        case TEXT_3:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Проверяем позиции: " + text, replyMarkup: GetButtons());

                            var cleanQuery3 = @"TRUNCATE TABLE `TG_response`";
                            var cmdClean3 = new MySqlCommand(cleanQuery3, connect.Connection);
                            cmdClean3.ExecuteNonQuery();

                            var query3 = $@"INSERT INTO `TG_response`(`Cmd_Name`, `Btn_Name`) VALUES ('Yes','Checking')";

                            var command3 = new MySqlCommand(query3, connect.Connection);
                            command3.ExecuteNonQuery();

                            break;
                        case TEXT_4:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Устанавливаем ключевое слово: " + text, replyMarkup: GetButtons());

                            var cleanQuery4 = @"TRUNCATE TABLE `TG_response`";
                            var cmdClean4 = new MySqlCommand(cleanQuery4, connect.Connection);
                            cmdClean4.ExecuteNonQuery();

                            var query4 = $@"INSERT INTO `TG_response`(`Cmd_Name`, `Btn_Name`) VALUES ('Yes','KeyBtn')";

                            var command4 = new MySqlCommand(query4, connect.Connection);
                            command4.ExecuteNonQuery();

                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Введите ключевое слово:");

                            break;
                        default:
                            var query5 = $@"TRUNCATE TABLE `MyKey`";
                            var command5 = new MySqlCommand(query5, connect.Connection);
                            command5.ExecuteNonQuery();

                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Получен текст: " + text, replyMarkup: GetButtons());

                            var query6 = $@"INSERT INTO `MyKey`(`UserKeyword`) VALUES ('{text}')";
                            var command6 = new MySqlCommand(query6, connect.Connection);
                            command6.ExecuteNonQuery();

                            break;
                    }
                    _client.SendTextMessageAsync(update.Message.Chat.Id, "Нажали кнопку :" + text, replyMarkup: GetButtons());
                    break;
                default:
                    Console.WriteLine(update.Type + " Not implemented");
                    break;
            }
            connect.CloseConnection();
        }


        private IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton(TEXT_1),
                    new KeyboardButton(TEXT_2),
                },
                new[]
                {
                    new KeyboardButton(TEXT_3),
                    new KeyboardButton(TEXT_4),
                }
            });
        }
    }
}