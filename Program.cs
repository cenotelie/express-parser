﻿using Hime.Redist;
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
            if (args.Length == 0) return;
            string fileName = args[1];
            string text = File.ReadAllText(fileName);
            ExpressLexer lexer = new ExpressLexer(text);
            ExpressParser parser = new ExpressParser(lexer);
            ParseResult res = parser.Parse();
            TextGenerator generator = new TextGenerator(res.Root, args[0], args[2], false);
            generator.GenerateSchema();
            generator.GenerateOWl();
        }
    }
}
