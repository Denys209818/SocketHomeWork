using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerExample
{
    class Program
    {
        public static int port { get; set; } = 8888;
        static void Main()
        {
            Console.OutputEncoding = Encoding.Default;
            Console.InputEncoding = Encoding.Default;

            /// Ініціалізація сервера. Конструктор приймає IPAddress і порт
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            try
            {
                /// Запуск сервера (прослуховування входячих запросів)
                server.Start();

                Console.WriteLine("Сервер запущений. Очікування підключення...");

                /// Метод AcceptTcpClient блокує основний потік, поки віддалений клієнт не підключиться
                /// Якщо віддалений хост підключився повертає TcpClient, щоб далі можна було відправити
                /// або навпаки отримати дані
                TcpClient client = server.AcceptTcpClient();

                /// Ініціалізує NetworkStream, який означає потік
                /// у якому відбуваєтсья зєднання сервера з клієнтом
                /// Унаслідований клас від Stream
                NetworkStream stream = client.GetStream();

                string message = "Hello, World!";
                /// Утворення байтового масиву з строки
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                /// Вдіпрвка данних від сервера до клієнта
                stream.Write(bytes, 0, bytes.Length);

                /// Закривання звязків з клієнтом
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                /// Виведення тексту помилки
                Console.WriteLine(ex.Message);
            }
            finally 
            {
                /// Зупинка сервера
                server.Stop();
            }
        }
    }
}
