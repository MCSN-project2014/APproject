using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using APproject;
using System.Collections.Generic;

namespace APproject
{
	public static class JsonSerializer
	{
		public static string serialize(List<Dictionary<string,object>> parameters, ASTNode node){
			var setting = new JsonSerializerSettings () {
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore
			};
			var jsonNode = JsonConvert.SerializeObject (node, setting);
			var parameter = JsonConvert.SerializeObject (parameters, setting);

			return "{parameter: " + parameter + ", block: " + jsonNode + "}";
		}


		public static void Start ()
		{
			var setting = new JsonSerializerSettings () {
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore
			};

			var json = JsonConvert.SerializeObject(InterpreterTest.testAFun (),setting);
			//Console.WriteLine(json);

			dynamic ret = JsonConvert.DeserializeObject<dynamic>(json);
			InterpreterTest.printAST(deserialize (ret));
			new Interpreter (deserialize (ret)).Start ();

		}


		public static ASTNode deserialize(dynamic ret){
			if (ret.children == null) {
				//Console.WriteLine (ret.value.GetType ());
				if (ret.value is JObject) {
					return new Term (new Obj{ name = Convert.ToString(ret.value.name) });
				} else {
					if (ret.value.Type == JTokenType.Boolean)
						return new Term (ret.value.ToObject<bool> ());
					else
						return new Term (ret.value.ToObject<int> ());
				}
			}else{
				Node n;
				if (ret.value != null)
					n = new Node ((Labels)ret.label, new Obj{name = Convert.ToString (ret.value.name)});
				else
					n = new Node ((Labels)ret.label);

				foreach (dynamic p in ret.children)
					n.addChildren(deserialize (p));

				return n;
			}

		}
	}
}

