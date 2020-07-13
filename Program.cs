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
                Console.WriteLine("Usage: express2owl base_iri input_file [debug]");
                return;
            }
            string inputFile = args[1];
            string text = File.ReadAllText(inputFile);
            ExpressLexer lexer = new ExpressLexer(text);
            ExpressParser parser = new ExpressParser(lexer);
            ParseResult res = parser.Parse();
            string outputFile = inputFile.Replace(".exp", ".owl");
            bool debug = false;
            if (args.Length == 3)
            {
                debug = (args[2].Equals("debug")) ? true : false;
            }
            ExpressModelGenerator generator = new ExpressModelGenerator(res.Root, inputFile, outputFile, debug);
            generator.GenerateSchema();
            generator.GenerateOWl();
        }
    }
}
