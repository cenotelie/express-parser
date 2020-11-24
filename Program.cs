using Hime.Redist;
using System;
using Express;
using System.IO;
using System.Collections.Generic;
using Express_Model;

namespace Express_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: express2owl base_iri input_directory [single] [debug]");
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
            bool single = false;
            bool debug = false;
            if (args.Length > 2)
            {
                single = (args[2].Equals("true")) ? true : false;
                if (args.Length == 4)
                {
                    debug = (args[3].Equals("true")) ? true : false;
                }
            }
            List<Schema> schemas = new List<Schema>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
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
                    foreach (ParseError err in res.Errors)
                    {
                        Console.WriteLine($"***** {err.Message}");
                    }
                    errors++;
                    continue;
                }
                generator = new ExpressModelGenerator(res.Root, args[0]);
                Schema schema = generator.GenerateSchema();
                schemas.Add(schema);
                dict.Add(schema.Name, inputFile.Replace(".exp", ".owl"));
                //generator.GenerateOWl();
                Console.WriteLine($"[INFO] Successfully Processed {inputFile}");
                successes++;
            }
            if (single)
            {
                TextWriter writer;
                if (debug) writer = Console.Out;
                else writer = new StreamWriter(dir + ".owl");
                writer.WriteLine("Prefix(owl:=<http://www.w3.org/2002/07/owl#>)");
                writer.WriteLine("Prefix(rdf:=<http://www.w3.org/1999/02/22-rdf-syntax-ns#>)");
                writer.WriteLine("Prefix(rdfs:=<http://www.w3.org/2000/01/rdf-schema#>)");
                writer.WriteLine("Prefix(xml:=<http://www.w3.org/XML/1998/namespace>)");
                writer.WriteLine("Prefix(xsd:=<http://www.w3.org/2001/XMLSchema#>)");
                writer.WriteLine($"Prefix(:=<{args[0]}#>)\n");
                writer.WriteLine($"Ontology(<{args[0]}>");
                foreach (Schema s in schemas)
                {
                    s.GenerateOWL(writer, true);
                }
                writer.WriteLine(")");
                writer.Close();
                if (debug) Console.ReadKey();
            }
            else
            {
                TextWriter writer = Console.Out; ;
                string filename;
                foreach (Schema s in schemas)
                {
                    dict.TryGetValue(s.Name, out filename);
                    if (filename == null) continue;
                    if (!debug)
                    {
                        writer = new StreamWriter(filename);
                        s.GenerateOWL(writer, false);
                        writer.Close();
                    } else
                    {
                        s.GenerateOWL(writer, false);
                    }
                }
                if (debug)
                {
                    writer.Close();
                }
            }
            Console.WriteLine($"[INFO] {errors} errors, {successes} successes");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
