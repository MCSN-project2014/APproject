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
			if (args.Length == 2) {
				//var param = "[{\"a\" : 3 },{\"b\" : 6 }]";
				//var block = "{\"recursive\":false,\"children\":[{\"recursive\":false,\"children\":[{\"recursive\":false,\"children\":[{\"type\":1,\"recursive\":false,\"value\":{\"name\":\"a\",\"type\":1,\"next\":{\"name\":\"b\",\"type\":1,\"kind\":0,\"isUsedFromAfun\":false,\"isUsedInAsync\":false,\"recursive\":false,\"asyncControl\":false,\"returnIsSet\":false},\"kind\":0,\"isUsedFromAfun\":false,\"isUsedInAsync\":false,\"recursive\":false,\"asyncControl\":false,\"returnIsSet\":false},\"label\":0,\"line\":0,\"column\":0},{\"type\":1,\"recursive\":false,\"value\":{\"name\":\"b\",\"type\":1,\"kind\":0,\"isUsedFromAfun\":false,\"isUsedInAsync\":false,\"recursive\":false,\"asyncControl\":false,\"returnIsSet\":false},\"label\":0,\"line\":0,\"column\":0}],\"label\":24,\"line\":0,\"column\":0,\"type\":0}],\"label\":11,\"line\":0,\"column\":0,\"type\":0}],\"label\":7,\"line\":0,\"column\":0,\"type\":0}";
				//param = param.Replace ('\"', '"');
				//block = block.Replace ('\"', '"');
				var parameter = HelperJson.DeserializeParameter (args[0]);
				var node = HelperJson.DeserializeAST (args[1]); 	
				//InterpreterTest.printAST (node);
				new Interpreter (node).Start (parameter);

			} else
				Console.WriteLine ("error");
		}
	}
}
