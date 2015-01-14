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
            if (HelperOption.ParseInterpreter(args))
            {
                ASTNode root;
                if (HelperParser.TryParse(HelperOption.inputFileName, out root)){
                    if (HelperOption.verbose)
                    {
                        HelperParser.printAST(root);
                    }
                    Interpreter inter = new Interpreter (root);
                    inter.Start ();
                }
            }
		}
	}
}
