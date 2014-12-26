using System;
using System.Collections.Generic;

namespace APproject
{
	public static class Interpreter
	{
		public static void tryInterpreter(){
			Obj o = new Obj ();
			o.kind = Kinds.func;
			Node main = new Node (new Term(o, terminalType.variable));
			Node If = new Node ( Labels.If);
			main.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Obj term1 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			Obj term2 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			condition.addChildren (new Node(new Term(term1, terminalType.variable)));
			condition.addChildren (new Node(new Term(term2, terminalType.variable)));
			Node ret = new Node (Labels.Return);
			If.addChildren (ret);
			Obj term3 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			ret.addChildren (new Node(new Term(term3, terminalType.variable)));

			printAST (main);
			//Interpret (main);

			Console.ReadKey();
		}
			
		/*
		public static void Interpret (Node node){
			if (node != null) {
				if (node.term != null)
					Console.WriteLine ("term");
				else{
					List<Node> children = node.getChildren ();
					bool condition;
					switch (node.label) {
					case Labels.If:
						condition = Interpret (children [0]);
						if (condition)
							Interpret (children [1]);
						else if (children.Count > 2)
							Interpret (children [2]);
						break;
					case Labels.While:
						condition = Interpret (children [0]);
						while (condition) {
							Interpret (children [1]);
							condition = Interpret (children [0]);
						}
						break;
					case Labels.Print:
						Console.WriteLine ("FUNW@P console: " + Interpret (children [0]));
						break;
				}

			}
		}
		}
*/
		public static void printAST(Node node){
			if (node != null){
				if (node.term != null)
					Console.WriteLine (node.term);
				else
					Console.WriteLine (node.label);
				foreach (Node n in node.getChildren()) {
					printAST (n);
				}
			}
		}
	}
}

