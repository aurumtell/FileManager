
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace File_manager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Добро пожаловать в Файловый Менеджер!");
                PrintMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                PrintMenu();
            }
        }
        /// <summary>
        /// Выводит меню в консоль.
        /// </summary>
        public static void PrintMenu()
        {
            string drive = null;
            string directory = null;
            string file = null;
            Console.WriteLine("Вам предложены несколько операций.Выберите одну из них и введите ее номер.");
            Console.WriteLine("Примечание 1: В операции 11 менеджер работает с файлами, в данной папке");
            Console.WriteLine("Примечание 2: В операциях 9-11 достаточно выбрать директорию.");
            Console.WriteLine("Примечание: Если вы хотите выбрать операции с 2-10, то сначала вы должны выбрать операцию 1.");
            Console.WriteLine("1.Просмотр списка дисков компьютера и выбор диска, переход в другую директорию (выбор папки)");
            Console.WriteLine("2.Переход в другую директорию (выбор папки)");
            Console.WriteLine("3.Просмотр списка файлов в директории и выбор файла");
            Console.WriteLine("4.Вывод содержимого текстового файла в консоль в кодировке UTF-8");
            Console.WriteLine("5.Вывод содержимого текстового файла в консоль в выбранной пользователем " +
                "кодировке(предоставляется не менее трех вариантов)");
            Console.WriteLine("6.Копирование файла(создание нового файла и копирование файла в новый");
            Console.WriteLine("7.Перемещение файла в выбранную пользователем директорию");
            Console.WriteLine("8.Удаление файла");
            Console.WriteLine("9.Создание простого текстового файла в кодировке UTF-8");
            Console.WriteLine("10.Создание простого текстового файла в выбранной пользователем кодировке" +
                "(предоставляется не менее трех вариантов)");
            Console.WriteLine("11.Конкатенация содержимого двух или более текстовых файлов и вывод " +
                "результата в консоль в кодировке UTF - 8");
            Console.WriteLine("12.Изменить директорию");
            do
            {
                Console.WriteLine("Введите номер операции:");
                Choice(ref drive, ref directory, ref file);
                Console.WriteLine("Нажмите Esc для завершения работы, в противном случае нажмите Enter.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);

        }

        /// <summary>
        /// Проверяет правильность введенного числа.
        /// </summary>
        /// <param name="begin">Начало нужного промежутка.</param>
        /// <param name="end">Конец нужного промежутка.</param>
        /// <returns>Возвращает введенное(правильное) число.</returns>
        public static int NumberCheck(int begin, int end)
        {
            int number;
            while (!int.TryParse(Console.ReadLine(), out number) || (number < begin || number > end))
            {
                Console.WriteLine("Входные данные ведены некорректно.Введите число снова.");
            }
            return number;
        }

        /// <summary>
        /// Обработка выбора пользователя.
        /// </summary>
        /// <param name="drive">Путь к диску.</param>
        /// <param name="directory">Путь к папке.</param>
        /// <param name="file">Путь к файлу.</param>
        public static void Choice(ref string drive, ref string directory, ref string file)
        {
            int number = NumberCheck(0, 12);
            //Проверка на выбранную операцию, для проверки введенных данных для полного пути.
            if (number > 8 && number < 12)
            {
                while ((drive == null) && (number != 1) || ((directory == null) && (number != 2)))
                {
                    Console.WriteLine("Вы ввели неполный путь(не выбрали диск или папку).Введите номер операции снова:");
                    number = NumberCheck(0, 12);
                }
            }
            else
            {
                //Проверка на полный путь или нет.
                while ((drive == null) && (number != 1) || ((directory == null) && (number != 1 && number != 2)))
                {
                    Console.WriteLine("Вы ввели неполный путь(не выбрали диск или папку).Введите номер операции снова:");
                    number = NumberCheck(0, 12);
                }
            }
            switch (number)
            {
                case 1:
                    PrintDrives(ref drive);
                    break;
                case 2:
                    PrintDirectories(drive, ref directory);
                    break;
                case 3:
                    PrintFiles(directory, ref file);
                    break;
                case 4:
                    PrintFile(file, false);
                    break;
                case 5:
                    PrintFile(file, true);
                    break;
                case 6:
                    CopyFile(file, directory);
                    break;
                case 7:
                    MoveFile(file);
                    break;
                case 8:
                    DeleteFile(file);
                    break;
                case 9:
                    CreateFile(directory, false);
                    break;
                case 10:
                    CreateFile(directory, true);
                    break;
                case 11:
                    ConcatenationFiles(directory);
                    break;
            }
        }

        /// <summary>
        /// Вывод дисков компьютера.
        /// </summary>
        /// <param name="drive">Путь к диску.</param>
        public static void PrintDrives(ref string drive)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            bool n = false;
            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive {0}", d.Name);
            }
            Console.WriteLine("Введите букву выбранного диска:");
            drive = Console.ReadLine().ToUpper();
            while (!n)
            {
                foreach (DriveInfo d in allDrives)
                {
                    //Проверка на существование выбранного диска.
                    if (d.Name[0].ToString() == drive)
                    {
                        Console.WriteLine($"Вы выбрали диск {drive}.");
                        n = true;
                        drive = d.Name;
                        break;
                    }
                }
                if (!n)
                {
                    Console.WriteLine("Выбранного диска не существует.Введите данные снова:");
                    drive = Console.ReadLine().ToUpper();
                }
            }

        }

        /// <summary>
        /// Вывод папок компьютера в заданном диске.
        /// </summary>
        /// <param name="drive">Путь к диску.</param>
        /// <param name="directory">Путь к папке.</param>
        public static void PrintDirectories(string drive, ref string directory)
        {
            string path = drive;
            string[] directories = null;
            do
            {
                Console.WriteLine("Выберите нужную папку и впишите ее номер");
                directories = Directory.GetDirectories(path);
                int i = 0;
                int number;
                foreach (var item in directories)
                {
                    Console.WriteLine(i + "." + item);
                    i++;
                }
                number = NumberCheck(0, i - 1);
                path = directories[number];
                Console.WriteLine("Если мы дошли до нужной папки, нажмите клавишу Enter.Иначе нажмите другую клавишу");
            } while (Console.ReadKey().Key != ConsoleKey.Enter);
            Console.WriteLine($"Вы выбрали папку {path}");
            directory = path;
        }

        /// <summary>
        /// Вывод файлов в заданной директории.
        /// </summary>
        /// <param name="directory">Путь к папке.</param>
        /// <param name="file">Путь к файлу.</param>
        public static void PrintFiles(string directory, ref string file)
        {
            string path = directory;
            string[] files = null;
            Console.WriteLine("Выберите нужный файл и впишите его номер");
            files = Directory.GetFiles(path, "*.txt");
            int i = 0;
            int number;
            foreach (var item in files)
            {
                Console.WriteLine(i + "." + item);
                i++;
            }
            number = NumberCheck(0, i - 1);
            file = files[number];
            Console.WriteLine($"Вы выбрали файл {file}.");
        }

        /// <summary>
        /// Выводит содержимое файла в консоль.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="encoding">В зависимости от операции, вывод с заданной кодировкой или же по выбору пользователя.</param>
        public static void PrintFile(string path, bool encoding)
        {
            int number;
            if (File.Exists(path))
            {
                if (encoding == false)
                    Console.WriteLine(File.ReadAllText(path, Encoding.UTF8));
                else
                {
                    Console.WriteLine("Выберите нужную кодировку и введите ее номер:");
                    Console.WriteLine("1.UTF7");
                    Console.WriteLine("2.UTF32");
                    Console.WriteLine("3.Unicode");
                    number = NumberCheck(0, 3);
                    if (number == 1)
                        Console.WriteLine(File.ReadAllText(path, Encoding.UTF7));
                    if (number == 2)
                        Console.WriteLine(File.ReadAllText(path, Encoding.UTF32));
                    if (number == 3)
                        Console.WriteLine(File.ReadAllText(path, Encoding.Unicode));
                }
            }

        }

        /// <summary>
        /// Копирование файла в выбранную директорию.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="directory">Путь к папке.</param>
        public static void CopyFile(string path, string directory)
        {
            Console.WriteLine("Напишите название файла, который будет копией выбранного файла без расширения");
            string fName = Console.ReadLine() + ".txt";
            //Проверка на существование файла.
            if (!File.Exists(Path.Combine(directory, fName)))
            {
                File.Copy(path, Path.Combine(directory, fName));
                Console.WriteLine($"Файл {path} скопирован в текущую директорию");
            }
            else
            {
                Console.WriteLine("Данный файл существует");
                CopyFile(path, directory);
            }
        }

        /// <summary>
        /// Перемещает файл в нужную директорию с измененным названием.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        public static void MoveFile(string path)
        {
            string drive1 = null;
            string directory1 = null;
            Console.WriteLine("Выберите нужный путь для перемещения файла:");
            PrintDrives(ref drive1);
            PrintDirectories(drive1, ref directory1);
            Console.WriteLine("Введите новое имя файла");
            string fName = Console.ReadLine() + ".txt";
            //Проверка на существование файла.
            if (!File.Exists(Path.Combine(directory1, fName)))
            {
                File.Move(path, Path.Combine(directory1, fName));
                Console.WriteLine($"Файл перемещен в {directory1}");
            }
            else
            {
                Console.WriteLine("Данный файл существует");
                MoveFile(path);
            }            
        }

        /// <summary>
        /// Удаление файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        public static void DeleteFile(string path)
        {
            File.Delete(path);
            Console.WriteLine($"Файл {path} удален");
            Console.WriteLine("После удаления файла вернитесь к операциям 1-3, чтобы выбрать новый файл, для работы с ним.");
            PrintMenu();
        }

        /// <summary>
        /// Создание нового файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="encoding">В зависимости от операции, создание с заданной кодировкой или же по выбору пользователя.</param>
        public static void CreateFile(string path, bool encoding)
        {
            Console.WriteLine("Введите новое название файла без расширения");
            string fName = Console.ReadLine() + ".txt";
            //Проверка на существование файла.
            if (!File.Exists(Path.Combine(path, fName)))
            {
                Console.WriteLine("Введите текст файла");
                string createText = Console.ReadLine();
                int number;
                if (encoding == false)
                    File.WriteAllText(Path.Combine(path, fName), createText, Encoding.UTF8);
                else
                {
                    Console.WriteLine("Выберите нужную кодировку и введите ее номер:");
                    Console.WriteLine("1.UTF7");
                    Console.WriteLine("2.UTF32");
                    Console.WriteLine("3.Unicode");
                    number = NumberCheck(0, 3);
                    if (number == 1)
                        File.WriteAllText(Path.Combine(path, fName), createText, Encoding.UTF7);
                    if (number == 2)
                        File.WriteAllText(Path.Combine(path, fName), createText, Encoding.UTF32);
                    if (number == 3)
                        File.WriteAllText(Path.Combine(path, fName), createText, Encoding.Unicode);
                }
                Console.WriteLine($"Файл {Path.Combine(path, fName)} создан в заданной кодировке");
            }
            else
            {
                Console.WriteLine("Данный файл существует");
                CreateFile(path, encoding);
            }
        }

        /// <summary>
        /// Конкатенация двух и более файлов.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        public static void ConcatenationFiles(string path)
        {
            string[] files = null;
            Console.WriteLine("Введите название нового файла без расширения");
            string fName = Console.ReadLine() + ".txt";
            //Проверка на существование файла.
            if (!File.Exists(Path.Combine(path, fName)))
            {
                Console.WriteLine("Выберите нужные файлы и впишите их номера.Если файлы выбраны, нажмите ПРОБЕЛ");
                files = Directory.GetFiles(path, "*.txt");
                int i = 0;
                int number;
                foreach (var item in files)
                {
                    Console.WriteLine(i + "." + item);
                    i++;
                }
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input == " ")
                        break;
                    //Конкатенация файлов.
                    while (!int.TryParse(input, out number) || (number < 0 || number > i - 1))
                    {
                        Console.WriteLine("Входные данные введены некорректно.Введите число снова.");
                    }
                    string file = files[number];
                    File.AppendAllText(Path.Combine(path, fName), File.ReadAllText(file));
                    Console.WriteLine("Для окончания ввода введите пробел");
                }
                Console.WriteLine($"Вы сконкатенировали файл {Path.Combine(path, fName)}.");
            }
            else
            {
                Console.WriteLine("Данный файл существует");
                ConcatenationFiles(path);
            }
        }

        /// <summary>
        /// Изменяет директорию.
        /// </summary>
         public static void ChangePath(){
            PrintMenu();
         }

    }
}
