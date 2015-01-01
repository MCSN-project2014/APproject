using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
                parser.gen = new ASTGenerator();
                parser.Parse();
                InterpreterTest.printAST(parser.gen.getRoot());

                //String fileName = "traslated_file";   //can be args[1]  argument with some parameter ( e.g -t filename) 
                //FSCodeGen genFsharp = new FSCodeGen(fileName);
                //genFsharp.translate(parser.gen.getRoot());
               
                //Console.WriteLine(parser.errors.count + " errors detected");
				if (parser.errors.count == 0){
					
					Interpreter inter = new Interpreter (parser.gen.getRoot());
					inter.Start ();
				}
                Console.Read();
            }
            else
            {
                Console.Write("-- No source file specified");
            }

			var json = JsonConvert.SerializeObject(InterpreterTest.test1(),
				Formatting.Indented,
				new JsonSerializerSettings() {
					ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
					NullValueHandling = NullValueHandling.Ignore
				});
			//Console.WriteLine(json);

			dynamic ret = JsonConvert.DeserializeObject(json);
			InterpreterTest.printAST(deserialize(ret));

			//InterpreterTest.Start (InterpreterTest.testAsync);

		}

		private static ASTNode deserialize(dynamic ret){
			if (ret.children == null) {
				//if (ret.value is dynamic)
				//	return new Term (new Obj{ name = ret.value.name });
				//else
					return new Term (ret.value);
			}else{
				Node n = new Node((Labels)ret.label);
				foreach (dynamic p in ret.children) {
					n.addChildren(deserialize (p));
				}
				return n;
			}

		}
	}
}
