using System;
using System.Text.RegularExpressions;
using System.IO;
using Serilog;


namespace ConsoleOKBSAPR
{
    class Program
    {


        public static void PrintMenu()
        {
            string one = "\n1) Вывести в консоль стихотворение 'Наша Таня громко плачет' ";
            string two = "2) Произвести сложение заданных чисел";
            string three = "3) Открыть файл с заданным именем и вывести в консоль его содержимое";
            string four = "4) Завершить исполнение";

            Console.WriteLine(one);
            Console.WriteLine(two);
            Console.WriteLine(three);
            Console.WriteLine(four);
        }

        public static void DoOne() //вывод стихотворения
        {
            Console.Write("\n\tНаша Таня громко плачет: \n " +
                          "\tУронила в речку мячик. \n" +
                          "\t-Тише, Танечка, не плачь: \n" +
                          "\tНе утонет в речке мяч\n");
        }


        public static void DoTwo()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log/LOG.txt")
                .WriteTo.Console()
                .CreateLogger();
            bool ok = true;
            int sum = 0;
            Console.WriteLine("Введите 2 целых числа: ");
            string str = Console.ReadLine();
            str = new Regex(@"\s+").Replace(str, " "); //по маске @"\s+" заменяет все пробельные символы 1 пробелом(можно сделать чтобы вводилось типа 3 + 3)
            string[] str_num = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            while (str_num.Length != 2)
            {
                Log.Error("Input number {ex}", "incorect count number");
                Console.WriteLine("Введите корректно 2 числа через пробел: ");
                str = Console.ReadLine();
                str = new Regex(@"\s+").Replace(str, " ");
                str_num = str.Split(' ');
            }



            while (ok)
            {
                try
                {
                    for (int i = 0; i < 2; i++)
                    {
                        sum = sum + Convert.ToInt32(str_num[i]);
                    }
                    Console.WriteLine(sum);

                    ok = false; //всё выполнилось верно выходим из круга ада
                }
                catch (FormatException)
                {
                    Log.Error("Input number {ex}", "FormatException");
                    sum = 0;
                    Console.WriteLine("Введите корректно 2 числа через пробел: ");
                    str = Console.ReadLine();
                    str = new Regex(@"\s+").Replace(str, " ");
                    str_num = str.Split(' ');

                    while (str_num.Length != 2)
                    {
                        Log.Error("Input number {ex}", "incorect count number");
                        Console.WriteLine("Введите корректно 2 числа через пробел: ");
                        str = Console.ReadLine();
                        str = new Regex(@"\s+").Replace(str, " ");
                        str_num = str.Split(' ');
                    }
                }
                catch (OverflowException)
                {
                    Log.Error("Input number {ex}", "OverflowException");
                    sum = 0;
                    Console.WriteLine("Слишком большое одно из чисел Введите корректно 2 числа через пробел: ");
                    str = Console.ReadLine();
                    str = new Regex(@"\s+").Replace(str, " ");
                    str_num = str.Split(' ');

                    while (str_num.Length != 2)
                    {
                        Log.Error("Input number {ex}", "incorect count number");
                        Console.WriteLine("Слишком большое одно из чисел Введите корректно 2 числа через пробел: ");
                        str = Console.ReadLine();
                        str = new Regex(@"\s+").Replace(str, " ");
                        str_num = str.Split(' ');
                    }
                }
            }
        }

        public static void DoThree()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log/LOG.txt")
                .WriteTo.Console()
                .CreateLogger();
            bool ok = true;
            string path = @"C:\Users\tiuki\source\repos\ConsoleOKBSAPR\ConsoleOKBSAPR\text_file\";


            string[] files = Directory.GetFiles(path);
            Console.WriteLine("Список файлов: ");
            foreach (string s in files)
            {
                string str = s.Substring(path.Length);
                Console.WriteLine(str);
            }
            Console.WriteLine("Введите имя файла из предложенного списка");
            string name_file = Console.ReadLine();
            path = path + name_file;

            while (ok)
            {
                try
                {
                    using (FileStream fstream = File.OpenRead(path))
                    {
                        // преобразуем строку в байты
                        byte[] array = new byte[fstream.Length];
                        // считываем данные
                        fstream.Read(array, 0, array.Length);
                        // декодируем байты в строку
                        string textFromFile = System.Text.Encoding.Default.GetString(array);
                        Console.WriteLine($"Текст из файла:\n\t {textFromFile}");
                    }
                    ok = false;
                }
                catch (Exception e)
                {
                    Log.Error("Exception {ex}", Convert.ToString(e));
                    Console.WriteLine("Введите корректное имя файла: ");
                    path = @"C:\Users\tiuki\source\repos\ConsoleOKBSAPR\ConsoleOKBSAPR\text_file\";
                    name_file = Console.ReadLine();
                    path = path + name_file;
                }
            }
        }


        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log/LOG.txt")
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Start Programm");

            int input = 0;
            while (input != 4)
            {
                PrintMenu();
                Console.Write("Введите номер пунктa из меню: ");
                bool check = true;
                //FormatException
                while (check)
                {
                    try
                    {
                        input = Convert.ToInt32(Console.ReadLine());
                        if (input >= 1 && input <= 4)
                        {
                            check = false;
                        }
                        else
                        {
                            Console.Write("Введите правильный номер: ");
                        }
                    }
                    catch (FormatException)
                    {
                        Log.Error("Input point menu {ex},{count}", "FormatException", 0);
                        Console.Write("Введите корректное значение: ");
                    }
                    catch (StackOverflowException)
                    {
                        Log.Error("Input point menu {ex},{count}", "StackOverflowException", 0);
                        Console.Write("Введите корректное значение: ");
                    }
                }

                switch (input)
                {
                    case 1:
                        Log.Information("Input point menu {count}", 1);
                        DoOne();
                        break;
                    case 2:
                        Log.Information("Input point menu {count}", 2);
                        DoTwo();
                        break;
                    case 3:
                        Log.Information("Input point menu {count}", 3);
                        DoThree();
                        break;
                }
            }

            Log.Information("Finish program");
        }
    }
}
