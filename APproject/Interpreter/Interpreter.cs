using System;
using System.Collections.Generic;

namespace APproject
{
	public static class Interpreter
	{
		public static void tryInterpreter(){
			Obj o = new Obj ();
			o.kind = Kinds.func;
			Node main = new Node (o);
			Node If = new Node ( Statement.If);
			main.addChildren (If);
			Node condition = new Node (Statement.geq);
			If.addChildren (condition);
			Obj term1 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			Obj term2 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			condition.addChildren (new Node(term1));
			condition.addChildren (new Node(term2));
			Node ret = new Node (Statement.Return);
			If.addChildren (ret);
			Obj term3 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			ret.addChildren (new Node(term3));

			printAST (main);
			Interpret (main);

			Console.ReadKey();
		}
			

		public static void Interpret (Node node){
			if (node != null) {
				if (node.term != null)
					Console.WriteLine ("term");
				else{
					List<Node> children = node.getChildren ();
					bool condition;
					switch (node.stmn) {
					case Statement.If:
						condition = Interpret (children [0]);
						if (condition)
							Interpret (children [1]);
						else if (children.Count > 2)
							Interpret (children [2]);
						break;
					case Statement.While:
						condition = Interpret (children [0]);
						while (condition) {
							Interpret (children [1]);
							condition = Interpret (children [0]);
						}
						break;
					case Statement.Print:
						Console.WriteLine ("FUNW@P console: " + Interpret (children [0]));
						break;
					case Statement.Read:
						int tmp = Convert.ToInt32 (Console.ReadLine ());
					}
				}

			}
		}

		public static void printAST(Node node){
			if (node != null){
				if (node.term != null)
					Console.WriteLine (node.term);
				else
					Console.WriteLine (node.stmn);
				foreach (Node n in node.getChildren()) {
					printAST (n);
				}
			}
		}
	}
}

