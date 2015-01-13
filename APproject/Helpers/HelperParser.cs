using System;

namespace APproject
{
	public static class HelperParser
	{

		public static bool TryParse (string file, out ASTNode node)
		{
			node = null;
			Console.WriteLine("parse file: " + file +"\n");
			Scanner scanner = new Scanner(file);
			Parser parser = new Parser(scanner);
			parser.tab = new SymbolTable(parser);
			parser.gen = new ASTGenerator();
			parser.Parse();
			if (parser.errors.count == 0) {
				node = parser.gen.getRoot ();
				return true;
			}
			else{
				return false;
			}
		}
	}
}

