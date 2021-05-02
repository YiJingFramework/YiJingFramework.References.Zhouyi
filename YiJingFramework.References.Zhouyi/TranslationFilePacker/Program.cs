using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TranslationFilePacker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("======================");
            Console.WriteLine("Translation File Packer For YiJingFramework.References.Zhouyi.");
            Console.WriteLine("Target translation file version: TR-1.");
            if (args.Length != 2)
            {
                Console.WriteLine("Format: <InputFile> <OutputFile>");
                Console.WriteLine("======================");
                return;
            }
            Console.WriteLine("======================");

            Console.WriteLine("Reading...");
            var inp = File.ReadAllText(args[0]);
            JsonSerializerOptions options
               = new JsonSerializerOptions() {
                   AllowTrailingCommas = true,
                   ReadCommentHandling = JsonCommentHandling.Skip,
                   WriteIndented = false,
                   Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
               };
            var translation = JsonSerializer.Deserialize<TranslationFile>(
                     inp, options);

            Console.WriteLine("Checking...");
            if (translation.CheckAndWrite())
            {
                Console.WriteLine("Writing...");
                File.WriteAllText(args[1], JsonSerializer.Serialize(translation, options));
                Console.WriteLine("Finished.");
            }

            Console.WriteLine("======================");
        }
    }
}
