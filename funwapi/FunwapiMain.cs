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

			var list = new List<Dictionary<string,object>> (new Dictionary<string,object>[]{
				new Dictionary<string,object>() {{ "cat", 2 }},
				new Dictionary<string,object>() {{ "asd", true }},
				new Dictionary<string,object>() {{ "cd", 2 }},
				new Dictionary<string,object>() {{ "ctt", 2 }}

			});

			Console.WriteLine (JsonSerializer.serialize (list, InterpreterTest.test1()));
		}
	}
}
