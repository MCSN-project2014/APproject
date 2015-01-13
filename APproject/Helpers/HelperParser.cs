using System;

namespace APproject
{
	public static class HelperParser
	{

		public static bool TryParse (string file, out ASTNode node)
		{
            node = null;
            try
            {
                Scanner scanner = new Scanner(file);
                Parser parser = new Parser(scanner);
                parser.tab = new SymbolTable(parser);
                parser.gen = new ASTGenerator();
                parser.Parse();
                if (parser.errors.count == 0)
                {
                    node = parser.gen.getRoot();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (FatalError)
            {
                return false;
            }
		}

        public static void printAST (ASTNode node)
		{
			Console.WriteLine ("\nAST PRINT:");
			PrintAST (node, "", true);
		}

		private static void PrintAST (ASTNode node, string indent, bool last)
		{
			Console.Write(indent);
			if (last)
			{
				Console.Write("\\-");
				indent += "  ";
			}
			else
			{
				Console.Write("|-");
				indent += "| ";
			}
			Console.WriteLine(node);

			if (!node.isTerminal())
				for (int i = 0; i < node.children.Count; i++)
					PrintAST(node.children[i], indent, i == node.children.Count - 1);
		}
	}
}

