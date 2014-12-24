using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APproject
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("APproject");
            if (args.Length > 0)
            {
                Console.WriteLine("parse file: " + args[0]);
                Scanner scanner = new Scanner(args[0]);
                Parser parser = new Parser(scanner);
                parser.tab = new SymbolTable(parser);
                parser.gen = new CodeGenerator();
                //parser.treegen = new APproject.treeGenerator.TreeGen();
                parser.Parse();
                /*if (parser.errors.count == 0)
                {
                    parser.gen.Decode();
                    parser.gen.Interpret("Taste.IN");
                }*/
                Console.WriteLine(parser.errors.count + " errors detected");
            }
            else
            {
                Console.Write("-- No source file specified");
            }
    	}
	}
}
