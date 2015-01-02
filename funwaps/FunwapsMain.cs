using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using APproject;
using System.Collections.Generic;

namespace funwaps
{
	class FunwapsMain
	{
		public static void Main (string[] args)
		{
			var setting = new JsonSerializerSettings () {
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore
			};
			var json = JsonConvert.SerializeObject(InterpreterTest.testAFun (),setting);
			Console.WriteLine (json);

			//args = new string[1];
			//args [0] = json;

			if (args.Length == 1) {
				dynamic ret = JsonConvert.DeserializeObject<dynamic> (args [0]);
				var root = TestJson.deserialize (ret);
				InterpreterTest.printAST (root);
				new Interpreter (root).Start ();
			} else
				Console.WriteLine ("no input");

		}
	}
}
