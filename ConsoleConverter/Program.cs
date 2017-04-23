using System;
using System.IO;
namespace ConsoleConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var folderPath = @"C:\Converter\";



            var watch = System.Diagnostics.Stopwatch.StartNew();

           
           

            foreach (string file in Directory.EnumerateFiles(folderPath, "*.json"))
            {
                var fileRead = File.ReadAllText(file);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                var conv = new Converter();
                var response = conv.LoadJson(file, fileNameWithoutExtension, fileRead);
                //Console.WriteLine(response);

                System.IO.File.WriteAllText(String.Format(@"C:\Converter\{0}.txt", fileNameWithoutExtension + "_CSV"), response);


            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            TimeSpan t = TimeSpan.FromMilliseconds(elapsedMs);
            Console.WriteLine("Time Elapsed: {0} - Total Minutes: {1}", elapsedMs, t.TotalMinutes);
            Console.ReadLine();


        }
    }
}