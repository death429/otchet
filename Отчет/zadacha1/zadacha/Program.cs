using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace zadacha
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя файла:");
            string fileName = Console.ReadLine();

            Console.WriteLine("Введите целое число N (0 < N < 27):");
            int N = int.Parse(Console.ReadLine());

            if (N <= 0 || N >= 27)
            {
                Console.WriteLine("Ошибка: N должно быть больше 0 и меньше 27.");
                return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    for (int i = 1; i <= N; i++)
                    {
                        string line = GetAlphabeticString(i);
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine($"Файл \"{fileName}\" успешно создан и заполнен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при создании файла: {ex.Message}");
            }
        }

        static string GetAlphabeticString(int length)
        {
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += (char)('a' + i);
            }
            return result;
        }
    }
}
