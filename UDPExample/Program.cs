using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPExample
{
    class Program
    {
        /// <summary>
        ///     Локальний порт, який вводиться динамічно в роботі програми
        /// </summary>
        public static int localPort;
        /// <summary>
        ///     Віддалений порт, який вводиться динамічно у роботі програми
        /// </summary>
        public static int remotePort;
        /// <summary>
        ///     Сокет, який виконує роль сервера і клієнта одночасно
        /// </summary>
        public static Socket singleSocket;
        static void Main()
        {
            Console.OutputEncoding = Encoding.Default;
            Console.InputEncoding = Encoding.Default;
            /// Надання значень для портів
                Console.Write("Введіть локальний порт: ");
                localPort = int.Parse(Console.ReadLine());
                Console.Write("Введіть віддалений порт: ");
                remotePort = int.Parse(Console.ReadLine());
            try
            {
                /// Ініціалізація сокета
                singleSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                /// Запуск метода, який у потоці виконує роботу сервера
                Task task = new Task(Listen);
                task.Start();


                Console.WriteLine("Щоб відправити повідомлення введіть товідомлення та нажміть Enter:");
                Console.WriteLine();

                while (true)
                {
                    string message = Console.ReadLine();
                    byte[] bytes = Encoding.UTF8.GetBytes(message);
                    /// Створення кінцевої точки, якій будуть відправлятися дані
                    /// У точці має занчення (в даному випадку) тільки порт оскільки ip-адреса однакова для усіх
                    EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), remotePort);
                    /// Метод, який відправляє дані байтового масиву по кінцевій точці
                    singleSocket.SendTo(bytes, endPoint);
                }
            }
            catch (Exception ex)
            {
                /// Виведення повідомлення у разі виникнення виключення у роботі програми
                Console.WriteLine("\n" + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                /// У випадку помилки видаляється сокет
                /// А саме перериваються звязки
                /// і закривається сокет
                Close();
            }

        }
        static void Listen()
        {
            try
            {
                /// Створення точки, яка позначає локальний (для цієї програми) сервер
                EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), localPort);
                /// Привязування сокета до точки сервера
                singleSocket.Bind(endPoint);
                while (true)
                {
                    /// Ініціалізація StringBuilder, який дозволяє зручно маніпулювати строковими данними
                    StringBuilder builder = new StringBuilder();
                    /// Поле позначає кількість данних, які передаються від клієнта
                    int countBytes = 0;
                    /// Поле позначає байтовий масив, у якого записуються дані, які отримані від відаленого сервера
                    byte[] bytes = new byte[256];
                    /// Ініціалізація точки тестовими даними
                    /// Вона міститиме дані про кінцеву точку, яка відправила дані
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                    do
                    {
                        /// Метод, який отримує дані і додає їх у байтовий масив
                        /// А також ініціалізує точку з якої прийшли дані. Точка ініціалізується 
                        /// при передачі другим параметром силки на віддалену точку
                        countBytes = singleSocket.ReceiveFrom(bytes, ref point);
                        /// Формування кінцевої строки
                        builder.Append(Encoding.UTF8.GetString(bytes));
                    } while (singleSocket.Available > 0);
                    /// Виведення кінцевої строки на консоль
                    Console.WriteLine("{0} - {1}", point, builder.ToString());
                }
            }
            catch (Exception ex)
            {
                /// Виведення повідомлення у разі виникнення виключення у роботі програми
                Console.WriteLine("\n" + ex.Message + "\n" + ex.StackTrace + "\n" + ex.Source);
            }
            finally
            {
                /// У випадку помилки видаляється сокет
                /// А саме перериваються звязки
                /// і закривається сокет
                Close();
            }
        }

        // Метод, який закриває сокет
        private static void Close()
        {
            /// Перевірка чи сокет не пустий
            if (singleSocket != null)
            {
                /// Метод Shutdown гарантує, що будуть перервані як передача так і відпрвка данних
                singleSocket.Shutdown(SocketShutdown.Both);
                /// Закриває сокет
                singleSocket.Close();
                /// Присвоює сокету нульове значення
                singleSocket = null;
            }
        }
    }
}

