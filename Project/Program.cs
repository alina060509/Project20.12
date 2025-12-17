using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project
{
    internal class Program
    {
        private static int replacementWords = 0;
        static void Main(string[] args)
        {
            string file = "C:/Users/User/Downloads/project.txt";
            string[] badWordsList = { "тварь", "гадина" };

            if (!File.Exists(file))
            {
                Console.WriteLine("документ не найден");
                return;
            }

            Thread readerThread = new Thread(() => ProcessFile(file, badWordsList));

            readerThread.Start();
            readerThread.Join();

            Console.WriteLine("Работа закончена");
        }
        static int ProcessFile(string file, string[] forbiddenWords)
        {
            try
            {
                string content = File.ReadAllText(file);
                int count = 0;

                foreach (var word in forbiddenWords)
                {
                    int index = 0;
                    while ((index = content.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
                    {
                        count++;
                        index += word.Length;
                    }
                    string stars = new string('!', word.Length);
                    content = System.Text.RegularExpressions.Regex.Replace(
                        content,
                        System.Text.RegularExpressions.Regex.Escape(word),
                        stars,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    );
                }

                if (count > 0)
                {
                    File.WriteAllText(file, content);
                    Console.WriteLine($"Файл {Path.GetFileName(file)} обработан: {count} замен.");


                }
                if (count == 0)
                {
                    Console.WriteLine("Плохих слов нет");
                }

                return count;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке файла {file}: {ex.Message}");
                return 0;
            }
        }
    }
}


