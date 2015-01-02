using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace APproject
{
	public static class TestJson
	{
		public static void Start ()
		{
			var setting = new JsonSerializerSettings () {
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore
			};

			var json = JsonConvert.SerializeObject(InterpreterTest.testAFun (),setting);
			//Console.WriteLine(json);

			dynamic ret = JsonConvert.DeserializeObject<dynamic>(json);
			InterpreterTest.printAST(deserialize (ret, new Dictionary<string, Obj>(), new Dictionary<string, Obj>()));
			new Interpreter (deserialize (ret, new Dictionary<string, Obj>(), new Dictionary<string, Obj>())).Start ();

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

