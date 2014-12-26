using System;
using System.Collections.Generic;

namespace APproject
{
	public static class Interpreter
	{
		public static void tryInterpreter(){
			Obj o = new Obj ();
			o.kind = Kinds.func;
			Node main = new Node (new Term(o));
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
				condition.addChildren (new Node(new Term(term1)));
			condition.addChildren (new Node(new Term(term2)));
			Node ret = new Node (Labels.Return);
			If.addChildren (ret);
			Obj term3 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			ret.addChildren (new Node(new Term(term3)));

			printAST (main);
			//Interpret (main);

			Console.ReadKey();
		}
			
		public static void Interpret (Node node){
			if (node != null) {
				if (node.term != null)
					Console.WriteLine ("term");
				else{
					List<Node> children = node.getChildren ();
					bool condition;
					switch (node.label) {
					case Labels.If:
						condition = InterpretCondition (children [0]);
						if (condition)
							Interpret (children [1]);
						else if (children.Count > 2)
							Interpret (children [2]);
						break;
					case Labels.While:
						condition = InterpretCondition (children [0]);
						while (condition) {
							Interpret (children [1]);
							condition = InterpretCondition (children [0]);
						}
						break;
					case Labels.Print:
						Console.WriteLine ("FUNW@P console: " + Convert.ToString(InterpretExp (children [0])));
						break;
					}
				}
			}
		}

		static bool InterpretCondition (Node node)
		{
			return (bool) InterpretExp(node);
		}

		static int InterpretExpInt (Node node){
			return (Int32) InterpretExp(node);
		}

		static Object InterpretExp (Node node)
		{
			if (node.isTerminal ()) {
				return node.term.boolean;
			} else
				return null;
		}

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

