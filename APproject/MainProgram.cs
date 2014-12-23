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
			Obj o = new Obj ();
			o.kind = Kinds.func;
			Node main = new Node (o);
			Node If = new Node ( Statement.If);
			main.addChildren (If);
			Node condition = new Node (Statement.geq);
			If.addChildren (condition);
			Obj term1 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			Obj term2 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			condition.addChildren (new Node(term1));
			condition.addChildren (new Node(term2));
			Node ret = new Node (Statement.Return);
			If.addChildren (ret);
			Obj term3 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			ret.addChildren (new Node(term3));
			printAST (main);

			Console.ReadKey();
        }

		public static void printAST(Node node){
			if (node != null){
				if (node.term != null)
					Console.WriteLine (node.term);
				else
					Console.WriteLine (node.stmn);
				foreach (Node n in node.getChildren()) {
					printAST (n);
				}
			}
		}
    }
}
