using System;
using System.IO;
using System.Text;

namespace AlfredManager
{
    public static class Commands
    {
        private static string path = "";

        // Cвойство класса - метод доступа к полю path класса Commands.
        public static string NewPath
        {
            get => path;
            set => path = value;
        }

        /// <summary>
        /// Метод для обработки команды.
        /// </summary>
        /// <param name="input"> Строка с командой и ее параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        /// <returns></returns>
        public static bool СommandProcessing(string input, string path)
        {
            if (NewPath == "")
            {
                NewPath = Directory.GetCurrentDirectory();
            }

            // Удаление всех первых и последних пробелов.
            input = input.Trim(' ');
            string[] command = input.Split(' ');
            bool flag = Command(command, path);
            return flag;
        }

        /// <summary>
        /// Метод вызова команд.
        /// </summary>
        /// <param name="command"> Массив строк команды и ее параметров. </param>
        /// <returns> Для обновления переменной flag в методе Launch. </returns>
        public static bool Command(string[] command, string path)
        {
            if (command.Length == 1)
            {
                return EasyCommands(command, path);
            }
            else if (command.Length == 2)
            {
                AvarageCommands(command, path);
                return true;
            }
            else if (command.Length >= 3)
            {
                HardCommands(command, path);
                return true;
            }

            return true;
        }

        public static void HardCommands(string[] command, string path)
        {
            switch (command[0])
            {
                case "cp":
                    FileWCopy(command, path);
                    return;
                case "cat":
                    ReadFileEn(command, path);
                    return;
                case "move":
                    FileMoveTo(command, path);
                    return;
                case "touch":
                    if (command.Length == 3)
                    {
                        CreateFile(command, path);
                    }
                    return;
                case "unif":
                    Concatenation(command, path);
                    return;
            }
        }

        /// <summary>
        /// Метод для команд с одним параметром.
        /// </summary>
        /// <param name="command"> Массив строк с командой и параметром. </param>
        /// <param name="path"> Текущий путь. </param>
        public static void AvarageCommands(string[] command, string path)
        {
            switch (command[0])
            {
                case "cd":
                    if (command[1] == "..")
                    {
                        MoveWOPath(command, path);
                    }
                    else
                    {
                        MoveWPath(command, path);
                    }
                    return;
                case "cp":
                    FileWOCopy(command, path);
                    return;
                case "cat":
                    ReadFileWO(command, path);
                    return;
                case "rm":
                    FileRemove(command, path);
                    return;
                case "touch":
                    CreateFile(command, path);
                    return;
            }
        }

        /// <summary>
        /// Метод для команд без параметров.
        /// </summary>
        /// <param name="command"> Массив строк с командой. </param>
        /// <param name="path"> Нынешний путь. </param>
        /// <returns> Для обновления переменной flag в методе Launch. </returns>
        public static bool EasyCommands(string[] command, string path)
        {
            switch (command[0])
            {
                case "exit": return false;
                case "--help":
                    Help();
                    return true;
                case "ls":
                    OutputContent(path);
                    return true;
                case "lsd":
                    OutputDisk();
                    return true;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Метод для вывода списка команд.
        /// </summary>
        public static void Help()
        {
            // Выставление стандартного цвета консоли.
            Console.ResetColor();
            Console.WriteLine("--help - получение списка команд");
            Console.WriteLine("ls - просмотр списка файлов в директории");
            Console.WriteLine("lsd - просмотр списка дисков компьютера (Для Unix - бесполезно)");
            Console.WriteLine("cd .. - переход в верхнюю директорию");
            Console.WriteLine("cd <path> - переход в другую директорию по пути");
            Console.WriteLine("cp <file> - копировать файл из папки файла в нее же");
            Console.WriteLine("cp <path> <file> - копировать файл из данной папки по заданному пути");
            Console.WriteLine("cat <fileName> - чтение файла и вывод в консоль в кодировке UTF-8");
            Console.WriteLine("cat <fileName> <кодировка> - вывод файла в одной из кодировки UTF-8, ASCII, UTF-32, Default, Unicode");
            Console.WriteLine("move <fileName> <path> - перемещение файла в выбранную пользователем директорию");
            Console.WriteLine("rm <fileName> - удаление файла из папки, в которой вы находитесь");
            Console.WriteLine("touch <fileName> - создание простого текстового файла в кодировке UTF-8");
            Console.WriteLine("touch <fileName> <кодировка> - создание простого текстового файла в выбранной кодировке");
            Console.WriteLine("unif <newFileName> <file> <file> ... - конкатенация содержимого двух или более текстовых файлов");
            Console.WriteLine("exit - завершение работы файлого менеджера");
        }

        /// <summary>
        /// Метод для просмотра списка файлов и директорий в директории. 
        /// </summary>
        /// <param name="path"> Путь, где находится пользователь. </param>
        public static void OutputContent(string path)
        {
            // Создает объекта класса DirectoryInfo.
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var directory in di.GetDirectories())
            {
                Console.WriteLine($"Директория: {directory.Name} ");
            }

            foreach (var file in di.GetFiles())
            {
                Console.WriteLine($"Файл: {file.Name} ");
            }
        }

        /// <summary>
        /// Метод для просмотра списка дисков.
        /// </summary>
        public static void OutputDisk()
        {
            // Создает массив дисков.
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (var disk in allDrives)
            {
                Console.WriteLine(disk.Name);
            }
        }

        /// <summary>
        /// Метод "перехода" в верхнюю директорию.
        /// </summary>
        /// <param name="command"> Комманда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void MoveWOPath(string[] command, string path)
        {
            // Разделитель путей для Unix & Windows.
            char separator = Path.DirectorySeparatorChar;
            string[] pathArray = path.Split(separator);
            string newPath = "";
            // Узнаю операционную систему юзера.
            string[] system = Environment.OSVersion.ToString().Split(" ");
            int i = 1;
            if (system[0] == "Microsoft")
            {
            }

            for (; i < pathArray.Length - 1; i++)
            {
                newPath += separator;
                newPath += pathArray[i];
            }

            if (newPath == "")
            {
                if (system[0] == "Microsoft")
                {
                    newPath += pathArray[0];
                }
                else
                {
                    newPath += separator;
                }
            }
            
            NewPath = newPath;
        }

        /// <summary>
        /// Метод для перемещения в директорию по заданному пути.
        /// </summary>
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void MoveWPath(string[] command, string path)
        {
            // Создание полного пути.
            string newPath = Path.Combine(path, command[1]);
            try
            {
                if (Directory.Exists(command[1]))
                {
                    Directory.SetCurrentDirectory(command[1]);
                    NewPath = Directory.GetCurrentDirectory();
                }
                else if (Directory.Exists(newPath))
                {
                    Directory.SetCurrentDirectory(newPath);
                    NewPath = Directory.GetCurrentDirectory();
                }
                else
                {
                    Console.WriteLine("По заданному пути нет директории");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Недостаточно прав для доступа к директории: \n" + e.Message);
            }
        }

        /// <summary>
        /// Метод для копирования файла в текущую директорию.
        /// </summary>
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void FileWOCopy(string[] command, string path)
        {
            try
            {
                if (File.Exists(command[1]))
                {
                    string newFileName = command[1] + "_copy";
                    try
                    {
                        File.Copy(command[1], newFileName);
                        Console.WriteLine("Копирование успешно!");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Файла с введенным именем не существует");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Файл с таким именем уже существует: \n" + e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Недостаточно прав для доступа к файлу: \n" + e.Message);
            }
        }

        /// <summary>
        /// Метод для копировании файла из текущей папки по заданному пути.
        /// </summary>
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void FileWCopy(string[] command, string path)
        {
            // Создание полного пути.
            string newPath = Path.Combine(path, command[1]);
            try
            {
                if (File.Exists(command[2]))
                {
                    string sourceFile = Path.Combine(path, command[2]);
                    if (Directory.Exists(command[1]))
                    {
                        string destFile = Path.Combine(command[1], command[2]);
                        File.Copy(sourceFile, destFile, false);
                        Console.WriteLine("Копирование успешно");
                    }
                    else if (Directory.Exists(newPath))
                    {
                        string destFile = Path.Combine(newPath, command[2]);
                        File.Copy(sourceFile, destFile, false);
                        Console.WriteLine("Копирование успешно");
                    }
                    else
                    {
                        Console.WriteLine("Директории с заданным именем нет");
                    }
                }
                else
                {
                    Console.WriteLine("Файла с введенным именем не существует");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Недостаточно прав для доступа к файлу: \n" + e.Message);
            }
        }

        /// <summary>
        /// Метод для чтения файла в кодировке UTF-8.
        /// </summary>
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void ReadFileWO(string[] command,string path)
        {
            try
            {
                // Создание полного пути.
                string newPath = Path.Combine(path, command[1]);
                if (File.Exists(command[1]))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("**********Начало файла**********");
                    Console.ResetColor();
                    string text = File.ReadAllText(command[1]);
                    Console.WriteLine(text);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("**********Конец файла***********");
                }
                else if (File.Exists(newPath))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("**********Начало файла**********");
                    Console.ResetColor();
                    string text = File.ReadAllText(newPath);
                    Console.WriteLine(text);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("**********Конец файла***********");
                }
                else
                {
                    Console.WriteLine("Файла с таким именем нет в папке");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Недостаточно прав для доступа к файлу: \n" + e.Message);
            }
        }

        /// <summary>
        /// Метод для вывода файла в указанной кодировки.
        /// </summary> 
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void ReadFileEn(string[] command, string path)
        {
            try
            {
                string newPath = Path.Combine(path, command[1]);
                if ((File.Exists(command[1])) && (Path.GetExtension(command[1]) == ".txt"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("**********Начало файла**********");
                    Console.ResetColor();
                    string text = File.ReadAllText(command[1], FileEncode(command[2]));
                    Console.WriteLine(text);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("**********Конец файла***********");
                }
                else if (File.Exists(newPath) && (Path.GetExtension(command[1]) == ".txt"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("**********Начало файла**********");
                    string text = File.ReadAllText(newPath, FileEncode(command[2]));
                    Console.WriteLine(text);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("**********Конец файла***********");
                }
                else
                {
                    Console.WriteLine("Файла с таким именем нет в папке или его расширение не txt");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Недостаточно прав для доступа к файлу: \n" + e.Message);
            }
        }

        /// <summary>
        /// Метод, чтобы узнать введенную кодировку.
        /// </summary>
        /// <param name="encode"> Введенная кодировка. </param>
        /// <returns> Возвращенная кодировка. </returns>
        public static Encoding FileEncode(string encode)
        {
            Encoding encod;
            switch (encode)
            {
                case "UTF-8":
                    encod = Encoding.UTF8;
                    return encod;
                case "UTF-32":
                    encod = Encoding.UTF32;
                    return encod;
                case "Default":
                    encod = Encoding.Default;
                    return encod;
                case "ASCII":
                    encod = Encoding.ASCII;
                    return encod;
                case "Unicode":
                    encod = Encoding.Unicode;
                    return encod;
                default:
                    Console.WriteLine("Указанной кодировки не найдено. Файл выведен в кодировке UTF-8");
                    encod = Encoding.UTF8;
                    return encod;
            }
        }

        
        /// <summary>
        /// Метод для перемещения файла из данной папки по заданному пути.
        /// </summary>
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void FileMoveTo(string[] command, string path)
        {
            try
            {
                // Создание полного пути.
                string newPath = Path.Combine(path, command[2]);
                if (File.Exists(command[1]))
                {
                    string sourceFile = Path.Combine(path, command[1]);
                    if (Directory.Exists(command[2]))
                    {
                        FileInfo file = new FileInfo(sourceFile);
                        string destFile = Path.Combine(command[2], command[1]);
                        file.MoveTo(destFile);
                        Console.WriteLine("Файл перемещен успешно");
                    }
                    else if(Directory.Exists(newPath))
                    {
                        
                        FileInfo file = new FileInfo(sourceFile);
                        string destFile = Path.Combine(newPath, command[1]);
                        file.MoveTo(destFile);
                        Console.WriteLine("Файл перемещен успешно");
                    }
                    else
                    {
                        Console.WriteLine("Указанной директории не существует");
                    }
                }
                else
                {
                    Console.WriteLine("Файла с таким именем нет в папке");
                }
                
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Недостаточно прав для доступа к файлу: \n" + e.Message);
            }
        }

        /// <summary>
        /// Метод удаления любого файла из системы по заданному пути.
        /// </summary>
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void FileRemove(string[] command, string path)
        {
            try
            {
                // Создание полного пути.
                string newPath = Path.Combine(path, command[1]);
                if (File.Exists(command[1]))
                {
                    File.Delete(command[1]);
                    Console.WriteLine("Файл удален!");
                }
                else if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                    Console.WriteLine("Файл удален!");
                }
                else
                {
                    Console.WriteLine("Файла с заданным именем нет в файловой системе");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Недостаточно прав для доступа к файлу: \n" + e.Message);
            }
        }

        /// <summary>
        /// Метод создания текстового файла в заданно кодировке.
        /// </summary>
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void CreateFile(string[] command, string path)
        {
            // Массив недопустимых символов в имени файла.
            string[] wrongElem = {">", "<", "|", @"\" , "/" , "?", "*", ":", "'", "%"};
            try
            {
                if (!File.Exists(command[1]))
                {
                    string newPath = Path.Combine(path, command[1]);
                    // Идентификатор запрещенных символов в имени файла.
                    int flag = 0;
                    for (int i = 0; i < command[1].Length; i++)
                    {
                        if (Array.IndexOf(wrongElem, command[1][i].ToString()) != -1)
                        {
                            flag = 1;
                        }
                    }

                    if (flag == 1)
                    {
                        Console.WriteLine("Недопустимые символы в имени файла");
                    }
                    else
                    {
                        // Создания файла по заданному пути в кодировке UTF-8.
                        StreamWriter file = new StreamWriter(newPath, false, Encoding.UTF8);
                        if (command.Length == 3)
                        {
                            // Изменение кодировки файла.
                            file = new StreamWriter(newPath, false, FileEncode(command[2]));
                        } 
                        using (file)
                        {
                            Console.ResetColor();
                            Console.WriteLine("Введите текст в файл");
                            do
                            {
                                string input = Console.ReadLine();
                                file.WriteLine(input);
                            } while (Console.ReadKey().Key != ConsoleKey.Enter);
                        }
                        Console.WriteLine("Файл создан");
                    }
                }
                else
                {
                    Console.WriteLine("В папке уже есть файл с таким именем");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Недостаточно прав: \n" + e.Message);
            }
        }

        /// <summary>
        /// Метод конкатенации нескольких файлов.
        /// </summary>
        /// <param name="command"> Команда с параметрами. </param>
        /// <param name="path"> Нынешний путь. </param>
        public static void Concatenation(string[] command, string path)
        {
            // Массив недопустимых символов в имени файла.
            string[] wrongElem = {">", "<", "|", @"\" , "/" , "?", "*", ":", "'", "%"};
            try
            {
                string newPath = Path.Combine(path, command[1]);
                if (!File.Exists(newPath))
                {
                    // Идентификатор запрещенных символов в имени файла.
                    int flag = 0;
                    for (int i = 0; i < command[1].Length; i++)
                    {
                        if (Array.IndexOf(wrongElem, command[1][i].ToString()) != -1)
                        {
                            flag = 1;
                        }
                    }

                    if (flag == 1)
                    {
                        Console.WriteLine("Недопустимые символы в имени файла");
                    }
                    else
                    {
                        // Создания файла по заданному пути в кодировке UTF-8.
                        Console.WriteLine(newPath);

                        StreamWriter file = new StreamWriter(newPath, false, Encoding.UTF8);
                        for (int i = 2; i < command.Length; i++)
                        {
                            if (File.Exists(command[i]))
                            {
                                string text = File.ReadAllText(command[i], Encoding.UTF8);
                                using (file = new StreamWriter(newPath, true, Encoding.UTF8))
                                {
                                    // Запись нового текста в файл.
                                    file.Write(text);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Одного из файлов не существует");
                            }
                        }

                        Console.WriteLine("Файл создан");
                    }
                }
                else
                {
                    Console.WriteLine("Файл с таким именем существует в папке");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}