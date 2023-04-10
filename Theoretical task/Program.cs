using System.Data.SqlClient;
//Приложение имеет возможность : создать таблицу, добавить новую строку, удалить таблицу, отсортировать уникальные значени по ФИО+ДАТА РОЖДЕНИЯ  

namespace Theoretical_task
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adont;Trusted_Connection=True;";
            Console.WriteLine("Выбери вариант");
            int choice ;
            while ((choice= Convert.ToInt32(Console.ReadLine()))!=0)
            {

                switch (choice)
                {
                    case 1:
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            await connection.OpenAsync();

                            SqlCommand command = new SqlCommand();
                            command.CommandText =
                                "CREATE TABLE Users ( FullName NVARCHAR(100) NOT NULL, Birthday DATE NOT NULL, Sex NVARCHAR(2) NOT NULL )";
                            command.Connection = connection;
                            await command.ExecuteNonQueryAsync();
                            Console.WriteLine("Таблица Users создана");
                        }
                        break;
                    case 2:
                        Console.WriteLine("Введите имя:");
                        string fio = Console.ReadLine();
                        Console.WriteLine("Введите день, год, месяц:");
                        int day = Convert.ToInt32(Console.ReadLine());
                        int year = Convert.ToInt32(Console.ReadLine());
                        int month = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Введите пол");
                        string sex = Console.ReadLine();
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                        string sqlExpression =
                            $"INSERT INTO Users (FullName,Birthday,Sex) VALUES ('{fio}','{year}-{month}-{day}','{sex}')";
                            await connection.OpenAsync();
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            await command.ExecuteNonQueryAsync();
                            Console.WriteLine($"Добавлен");
                        }
                        break;
                    case 3:
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            await connection.OpenAsync();
                            SqlCommand command = new SqlCommand();
                            command.CommandText = "DROP TABLE Users";
                            command.Connection = connection;
                            await command.ExecuteNonQueryAsync();
                            Console.WriteLine("Таблица Users удалена");
                        }
                        break;
                    case 4:
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            await connection.OpenAsync();
                            string sqlExpression = "SELECT FullName,Birthday , Sex, DATEDIFF(YEAR, [Birthday], GETDATE()) AS Возраст FROM Users GROUP BY FullName, [Birthday], Sex HAVING COUNT(*) = 1 ORDER BY FullName";
                            SqlCommand command = new SqlCommand(sqlExpression, connection);    
                            SqlDataReader reader = await command.ExecuteReaderAsync();
                            
                            while (reader.Read())
                            {
                                Console.WriteLine("Full Name: {0}, Birthday: {1}, SEX : {2}", reader.GetString(0),
                                    reader.GetDateTime(1), reader.GetString(2));
                            }
                        }
                        break;
                }
            }
        }
    }
}
