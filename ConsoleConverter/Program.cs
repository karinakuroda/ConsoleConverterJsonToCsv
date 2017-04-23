using System;
using System.IO;
namespace ConsoleConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var folderPath = @"C:\Converter\";

            foreach (string file in Directory.EnumerateFiles(folderPath, "*.json"))
            {
                var fileRead = File.ReadAllText(file);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                var conv = new Converter();
                var response = conv.LoadJson(file, fileNameWithoutExtension, fileRead);
                Console.WriteLine(response);

                System.IO.File.WriteAllText(String.Format(@"C:\Converter\{0}.txt", fileNameWithoutExtension + "_CSV"), response);


            }
            Console.ReadLine();


        }
    }
}