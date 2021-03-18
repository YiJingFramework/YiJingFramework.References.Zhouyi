using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TranslationFilePacker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Translation File Packer For YiJingFramework.References.Zhouyi.");
                Console.WriteLine("Package version: 2.0.0.");
                Console.WriteLine("Format: <InputFile> <OutputFile>");
                return;
            }

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
            File.WriteAllText(args[1], JsonSerializer.Serialize(translation, options));
        }
    }
}
