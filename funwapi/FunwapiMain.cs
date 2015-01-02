using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APproject;

namespace funwapi
{
    class FunwapiMain
    {
        static void Main(string[] args)
        {
            Console.WriteLine("APproject - funwapi");
            if (args.Length > 0)
            {
				ASTNode root;
				if (HelperParser.TryParse(args[0], out root)){
                    InterpreterTest.printAST(root);
                    Interpreter inter = new Interpreter (root);
                    inter.Start ();
                }
                Console.Read();
            }
            else
                Console.Write("-- No source file specified");

			//InterpreterTest.Start (InterpreterTest.testAsync);
		}
	}
}
