using System;
using System.Collections.Generic;

namespace APproject
{
	public class Interpreter
	{
		Memory mem;
		ASTNode startNode;

		public Interpreter( ASTNode node){
			startNode = node;
			mem = new Memory ();
		}

		public static void tryInterpreter(){
			ASTNode If = test3 ();
			printAST (If);
			Interpreter inter = new Interpreter (If);
			inter.Start ();
			Console.ReadKey();
		}
			
		public void Start (){
			Console.WriteLine ("INTERPRETER STARTs:");
			Interpret (startNode);
		}

		private void Interpret (ASTNode node){
			if (node != null) {
				if (node.isTerminal())
					Console.WriteLine ("term");
				else{
					List<ASTNode> children = node.children;
					bool condition;
					switch (node.label) {
					case Labels.Block:
						mem.addScope ();
						foreach (Node n in children)
							Interpret (n);
						mem.removeScope ();
						break;
					case Labels.If:
						condition = InterpretCondition (children [0]);

						if (condition) {
							Interpret (children [1]);
						}else if (children.Count > 2)
							Interpret (children [2]);
						break;
					case Labels.While:
						condition = InterpretCondition (children [0]);
						while (condition) {
							Interpret (children [1]);
							condition = InterpretCondition (children [0]);
						}
						break;
					case Labels.Decl:
						foreach (ASTNode n in children)
							mem.addUpdateValue((Obj)n.value,null);
						break;
					case Labels.AssigDecl:
						mem.addUpdateValue ((Obj)children [0].value, InterpretExp (children [1]));
						break;
					case Labels.Assig:
						mem.addUpdateValue ((Obj)children [0].value, InterpretExp (children [1]));
						break;
					case Labels.Print:
						if (children[0].isTerminal() && children[0].value is string)
							Console.WriteLine ("FUNW@P console: " + children [0].value);
						else
							Console.WriteLine ("FUNW@P console: " + Convert.ToString(InterpretExp (children [0])));
						break;
					}
				}
			}
		}

		private bool InterpretCondition (ASTNode node)
		{
			return (bool) InterpretExp(node);
		}

		private int InterpretExpInt (ASTNode node){
			return (int) InterpretExp(node);
		}

		private object InterpretExp (ASTNode node)
		{
			if (node.isTerminal ()) {
				if (node.value is Obj)
					return mem.getValue ((Obj)node.value);
				else
					return node.value;
			} else {
				List<ASTNode> children = node.children;
				switch (node.label) {
				case Labels.Plus:
					return (int) InterpretExp (children [0]) + (int) InterpretExp (children [1]);
				case Labels.Minus:
					return (int) InterpretExp (children [0]) - (int) InterpretExp (children [1]);
				case Labels.Mul:
					return (int) InterpretExp (children [0]) * (int) InterpretExp (children [1]);
				case Labels.Div:
					return (int) InterpretExp (children [0]) / (int) InterpretExp (children [1]);
				case Labels.Eq:
					var tmp = InterpretExp (children [0]);
					if (tmp is bool)
						return (bool) tmp == (bool) InterpretExp (children [1]);
					else
						return (int) InterpretExp (children [0]) == (int) InterpretExp (children [1]);
				case Labels.Gt:
					return (int) InterpretExp (children [0]) > (int) InterpretExp (children [1]);
				case Labels.Gte:
					return (int) InterpretExp (children [0]) >= (int) InterpretExp (children [1]);
				case Labels.Lt:
					return (int) InterpretExp (children [0]) < (int) InterpretExp (children [1]);
				case Labels.Lte:
					return (int) InterpretExp (children [0]) <= (int) InterpretExp (children [1]);
				default:
					return null;
				}
			}
		}

		/// <summary>
		/// if 3>=1 {
		/// 	var a int= 3 + 7;
		/// 	println (a);
		/// }
		/// </summary>
		public static ASTNode test1 (){
			//Obj o = new Obj ();
			//o.kind = Kinds.func;
			Node main = new Node (Labels.Main);
			Node If = new Node ( Labels.If);
			main.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Term term1 = new Term(3);
			Term term2 = new Term(1);
			condition.addChildren (term1);
			condition.addChildren (term2);
			Node block = new Node (Labels.Block);
			If.addChildren (block);
			Node decAss = new Node (Labels.AssigDecl);
			block.addChildren (decAss);
			Obj varA = new Obj();
			varA.name = "a";
			Term num1 = new Term (3);
			Term num2 = new Term (7);
			Node plus = new Node (Labels.Plus);
			plus.addChildren (num1);
			plus.addChildren (num2);
			decAss.addChildren (new Term (varA));
			decAss.addChildren (plus);
			Node print = new Node (Labels.Print);
			block.addChildren (print);
			print.addChildren (new Term (varA));

			return If;
		}

		/// <summary>
		/// if 3>=1 {
		/// 	var a int= 3 + 7;
		/// }
		/// var a int = 3;
		/// println (a);
		/// </summary>
		public static ASTNode test2 (){
			Node blockM = new Node (Labels.Block);

			Obj varA = new Obj();
			varA.name = "a";

			Node If = new Node ( Labels.If);
			blockM.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Term term1 = new Term(3);
			Term term2 = new Term(1);
			condition.addChildren (term1);
			condition.addChildren (term2);
			Node block = new Node (Labels.Block);
			If.addChildren (block);
			Node decAss2 = new Node (Labels.AssigDecl);
			block.addChildren (decAss2);
			Term num1 = new Term (3);
			Term num2 = new Term (7);
			Node plus = new Node (Labels.Plus);
			plus.addChildren (num1);
			plus.addChildren (num2);
			decAss2.addChildren (new Term (varA));
			decAss2.addChildren (plus);

			Node decAss1 = new Node (Labels.AssigDecl);
			blockM.addChildren (decAss1);
			num1 = new Term (3);
			decAss1.addChildren (new Term (varA));
			decAss1.addChildren (num1);

			Node print = new Node (Labels.Print);
			blockM.addChildren (print);
			print.addChildren (new Term (varA));

			return blockM;
		}

		/// <summary>
		/// var a int = 3;
		/// if 3>=1 {
		/// 	a = 3 + 7;
		/// }
		/// println (a);
		/// </summary>
		public static ASTNode test3 (){
			/*
			var a int = 3;
			if 3>=1 {
				a = 3 + 7;
			}
			println (a);
			*/
			Node blockM = new Node (Labels.Block);

			Node decAss1 = new Node (Labels.AssigDecl);
			blockM.addChildren (decAss1);
			Obj varA = new Obj();
			varA.name = "a";
			Term num1 = new Term (3);
			decAss1.addChildren (new Term (varA));
			decAss1.addChildren (num1);

			Node If = new Node (Labels.If);
			blockM.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Term term1 = new Term(3);
			Term term2 = new Term(1);
			condition.addChildren (term1);
			condition.addChildren (term2);
			Node block = new Node (Labels.Block);
			If.addChildren (block);
			Node decAss2 = new Node (Labels.AssigDecl);
			block.addChildren (decAss2);
			num1 = new Term (3);
			Term num2 = new Term (7);
			Node plus = new Node (Labels.Plus);
			plus.addChildren (num1);
			plus.addChildren (num2);
			decAss2.addChildren (new Term (varA));
			decAss2.addChildren (plus);

			Node print = new Node (Labels.Print);
			blockM.addChildren (print);
			print.addChildren (new Term (varA));

			return blockM;
		}
		public static void printAST(ASTNode node){
			if (node != null){
				if (node.isTerminal())
					Console.WriteLine (node);
				else{
					Console.WriteLine (node);
					foreach (ASTNode n in node.children) 
						printAST (n);
				}
			}
		}
	}
}

