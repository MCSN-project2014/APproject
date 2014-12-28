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
					case Labels.Main:
						foreach (Node n in children)
							Interpret (n);
						break;
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
	}
}