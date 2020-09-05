using Hime.Redist;
using System;
using Express;
using System.Text;
using System.IO;

namespace Express_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: express2owl base_iri input_directory [debug]");
                Environment.Exit(-1);
            }
            string dir = args[1];
            FileAttributes att = File.GetAttributes(dir);
            if (! Directory.Exists(dir))
            {
                Environment.Exit(-1);
            }
            ExpressModelGenerator generator;
            int errors = 0;
            int successes = 0;
            foreach (string inputFile in Directory.GetFiles(dir))
            {
                if (!inputFile.EndsWith(".exp")) continue;
                string text = File.ReadAllText(inputFile);
                ExpressLexer lexer = new ExpressLexer(text);
                ExpressParser parser = new ExpressParser(lexer);
                ParseResult res = parser.Parse();
                if (res.Errors.Count > 0)
                {
                    Console.WriteLine($"[ERROR] Failed Processing {inputFile}");
                    foreach(ParseError err in res.Errors)
                    {
                        Console.WriteLine($"***** {err.Message}");
                    }
                    errors++;
                    continue;
                }
                string outputFile = inputFile.Replace(".exp", ".owl");
                bool debug = false;
                if (args.Length == 3)
                {
                    debug = (args[2].Equals("debug")) ? true : false;
                }
                generator = new ExpressModelGenerator(res.Root, args[0], inputFile, outputFile, debug);
                generator.GenerateSchema();
                generator.GenerateOWl();
                Console.WriteLine($"[INFO] Successfully Processed {inputFile}");
                successes++;
            }
            Console.WriteLine($"[INFO] {errors} errors, {successes} successes");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
