using System;
using System.IO;

namespace AlfredManager
{
    class Program
    {
        
        /// <summary>
        /// Метод для запуска программы.
        /// </summary>
        static void Launch()
        {
            // Путь директории, в которой находится пользователь.
            string path = Directory.GetCurrentDirectory();
            // Очищение консоли.
            Console.Clear();
            // Переменная, отвечающая за продолжение работы программы.
            bool flag = true;
            // Индекс, считающий итерации цикла.
            int i = 0;
            do
            {
                if (i == 0)
                {
                    Console.WriteLine("Для получения списка команд введите --help");
                }
                // Изменение цвета в консоли.
                Console.ForegroundColor = ConsoleColor.Green;
                if (Commands.NewPath == "")
                {
                    path = Directory.GetCurrentDirectory();
                }
                else
                {
                    path = Commands.NewPath;
                }
                Console.Write($"{path} ");
                // Выставление стандартного цвета консоли.
                Console.ResetColor();
                // Обработка команды.
                flag = Commands.СommandProcessing(Console.ReadLine(), path);
                i++;
            } while (flag);

        }
        
        static void Main(string[] args)
        {
            Launch();
        }
    }
}