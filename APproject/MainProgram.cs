using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                
                //String fileName = "traslated_file";   //can be args[1]  argument with some parameter ( e.g -t filename) 
                //FSCodeGen genFsharp = new FSCodeGen(fileName);
                //genFsharp.translate(parser.gen.getRoot());
               
                //Console.WriteLine(parser.errors.count + " errors detected");
				if (parser.errors.count == 0){
					InterpreterTest.printAST(parser.gen.getRoot());
					Interpreter inter = new Interpreter (parser.gen.getRoot());
					inter.Start ();
				}
                Console.Read();
            }
            else
            {
                Console.Write("-- No source file specified");
            }

			/*
			//ASTNode tesNode = InterpreterTest.factorialRecursive ();
			//InterpreterTest.printAST (tesNode);
			//InterpreterTest.Start (InterpreterTest.testReturnAFun);

			var setting = new JsonSerializerSettings () {
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore
			};

			var json = JsonConvert.SerializeObject(InterpreterTest.testReturnAFun (),setting);
			//Console.WriteLine(json);

			dynamic ret = JsonConvert.DeserializeObject<dynamic>(json);
			InterpreterTest.printAST(deserialize (ret, new Dictionary<string, Obj>(), new Dictionary<string, Obj>()));
			new Interpreter (deserialize (ret, new Dictionary<string, Obj>(), new Dictionary<string, Obj>())).Start ();

			//InterpreterTest.Start (InterpreterTest.testAsync);
			*/
		}



		private static ASTNode deserialize(dynamic ret, Dictionary<string,Obj> variables, Dictionary<string,Obj> functions){
			if (ret.children == null) {
				//Console.WriteLine (ret.value.GetType ());
				if (ret.value is JObject) {
					Obj value;
					var namestr = Convert.ToString (ret.value.name);
					if (variables.TryGetValue (namestr, out value))
						return new Term (value);
					else {
						var tmp = new Obj{ name = namestr };
						variables.Add (namestr, tmp);
						return new Term (tmp);
					}
				} else {
					if (ret.value.Type == JTokenType.Boolean)
						return new Term (ret.value.ToObject<bool> ());
					else
						return new Term (ret.value.ToObject<int> ());
				}
			}else{
				Node n;
				if (ret.value != null){ 
					Obj value;
					var namestr = Convert.ToString (ret.value.name);
					if (functions.TryGetValue (namestr, out value))
						n = new Node ((Labels)ret.label, value);
					else{
						var tmp = new Obj{ name = namestr };
						functions.Add (namestr, tmp);
						n = new Node ((Labels)ret.label, tmp);
					}
				}else
					n = new Node ((Labels)ret.label);

				foreach (dynamic p in ret.children)
					n.addChildren(deserialize (p, variables,functions));

				return n;
			}

		}
	}
}
