using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClientExample
{
    class Program
    {
        /// <summary>
        ///     Властивість, яка представляє собою ip-адресу сервера до якого
        ///     буде підключатися TcpClient
        /// </summary>
        private const string address = "127.0.0.1";
        /// <summary>
        ///     Властивість, яка представляє собою порт по якому TcpClient буде 
        ///     підключатися до сервера
        /// </summary>
        private const int port = 8888;
        static void Main()
        {
            try
            {
                /// Ініціалізація обєкта TcpClient
                TcpClient client = new TcpClient();
                /// Створення підключення до віддаленого сервера
                client.Connect(address, port);

                /// Ініціалізація обєкта NetworkStream, який представляє собою
                /// потік підключення до сервера
                NetworkStream stream = client.GetStream();

                /// Створення нового обєкта StringBuilder
                /// для запису і подальшого виведення даних, які прийшли від сервера
                StringBuilder builder = new StringBuilder();
                /// Байтовий масив у нього записуватимуться
                /// дані, які прийшли від сервера
                byte[] data = new byte[256];
                /// Кількість байтів, яких вдалося зчитати
                int count = 0;
                do
                {
                    /// Метод Read зчитує дані, які прийшли від сервера
                    /// і записує їх у байтовий масив. Другим параметром приймає точку початку
                    /// запису даних, а третім розмір данних, яких необхідно зчитати
                    count = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data));
                }
                /// Властивість DataAvailable вказує чи містить потік дані. Повертає bool-значення
                while (stream.DataAvailable);
                /// Вивід фінальної строки
                Console.WriteLine(builder.ToString());
                /// Закриття потоку для роботи з сервером
                stream.Close();
                /// Закриття обєкта TcpClient
                client.Close();
            }
            /// Помилка яка може виникнути при роботі з TcpClient
            catch (SocketException socketEx)
            {
                /// Вивід помилки
                Console.WriteLine(socketEx.Message);
            }
            catch (Exception ex) 
            {
                /// Вивід помилки
                Console.WriteLine(ex.Message);
            }

            
            Console.WriteLine("Виведення завершено!");
            Console.ReadKey();
        }
    }
}
