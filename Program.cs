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
            if (args.Length == 0) return;
            string fileName = args[1];
            string text = File.ReadAllText(fileName);
            OwlGenerator generator = new OwlGenerator(args[0], args[2]);
            ExpressLexer lexer = new ExpressLexer(text);
            ExpressParser parser = new ExpressParser(lexer, generator);
            //parser.Parse();
            ParseResult res = parser.Parse();
            generator.Close();
            //Print(res.Root, new bool[] { });
            //Console.ReadKey();
        }

        private static void Print(ASTNode node, bool[] crossings)
        {
            for (int i = 0; i < crossings.Length - 1; i++)
                Console.Write(crossings[i] ? "|   " : "    ");
            if (crossings.Length > 0)
                Console.Write("+-> ");
            Console.WriteLine(node.ToString());
            for (int i = 0; i != node.Children.Count; i++)
            {
                bool[] childCrossings = new bool[crossings.Length + 1];
                Array.Copy(crossings, childCrossings, crossings.Length);
                childCrossings[childCrossings.Length - 1] = (i < node.Children.Count - 1);
                Print(node.Children[i], childCrossings);
            }
        }
    }
}
