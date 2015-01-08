using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using APproject;
using System.Collections.Generic;

namespace APproject
{
	public static class HelperJson
	{
		private static JsonSerializerSettings setting = new JsonSerializerSettings () {
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			NullValueHandling = NullValueHandling.Ignore,
			//Formatting = Formatting.Indented,
			//StringEscapeHandling = StringEscapeHandling.EscapeHtml

		};

		public static string serialize(List<Dictionary<string,object>> parameters, ASTNode node){
			var parameter = JsonConvert.SerializeObject (parameters, setting);
			var jsonNode = JsonConvert.SerializeObject (node, setting);
			return "{\"parameter\": " + parameter + ", \"block\": " + jsonNode + "}";
		}

		public static string serialize(List<string> actual, List<string> formal, ASTNode node){
		
			string jsonPar = "[";
			for (int i=0; i < actual.Count; i++) {
				jsonPar += "{\\\""+formal[i]+"\\\" : \"+"+ actual[i] + "+\"}" +
					(i<actual.Count-1 ? ",":""); 
			}
			jsonPar+="]";

			var jsonNode = JsonConvert.SerializeObject (node, setting);
			jsonNode = jsonNode.Replace ("\"", "\\\"");
			var tmp =  "\"{\\\"parameter\\\" : " + jsonPar + ", \\\"block\\\" : " + jsonNode + "}\"";
			Console.WriteLine (tmp);
			return tmp;
		}
		/*
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
		*/

		public static List<Dictionary<string,object>> DeserializeParameter (string json){
			var ret = JsonConvert.DeserializeObject<List<Dictionary<string,object>>> (json);
			foreach (var item in ret) {
				var keys = item.Keys.GetEnumerator();
				keys.MoveNext();
				var key = keys.Current;
				if (item[key] is Int64)
					item[key] = Convert.ToInt32(item[key]);
			}
			return ret;
		}

		public static ASTNode DeserializeAST (string json){
			dynamic ret = JsonConvert.DeserializeObject<dynamic>(json);
			return _Deserialize(ret); 
		}

		private static ASTNode _Deserialize(dynamic ret){
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
					n.addChildren(_Deserialize (p));

				return n;
			}

		}
	}
}

