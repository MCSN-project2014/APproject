using System;
using System.Collections.Generic;

namespace APproject
{
	public class Interpreter
	{
		MemoryFunction mem;
		Obj main;
		ASTNode startNode;

		public Interpreter( ASTNode node){
			startNode = node;
			main = new Obj{ name = "main" };
			mem = new MemoryFunction ();
		}
			
		public void Start (){
			Console.WriteLine ("INTERPRETER START:");
			foreach (ASTNode node in startNode.children) {
				if (node.label == Labels.FunDecl)
					mem.addFunction ((Obj)node.value, node);
				if (node.label == Labels.Main)
					mem.addNameSpace (main);
					Interpret (node, main);
			}
		}

		private object Interpret (ASTNode node, Obj now){
			if (!node.isTerminal ()) {
				List<ASTNode> children = node.children;
				bool condition;
				switch (node.label) {
				case Labels.Main:
					foreach (Node n in children)
						Interpret (n, now);
					break;
				case Labels.Block:
					mem.nameSpace [now].addScope ();
					object ret = null;
					foreach (Node n in children) {
						ret = Interpret (n, now);
						if (ret != null)
							break;
					}
					mem.nameSpace [now].removeScope ();
					return ret;
				case Labels.If:
					condition = InterpretCondition (children [0], now);
					if (condition) {
						Interpret (children [1], now);
					} else if (children.Count > 2)
						Interpret (children [2], now);
					break;
				case Labels.While:
					condition = InterpretCondition (children [0], now);
					while (condition) {
						Interpret (children [1], now);
						condition = InterpretCondition (children [0], now);
					}
					break;
				case Labels.Decl:
					foreach (ASTNode n in children)
						mem.nameSpace [now].addUpdateValue ((Obj)n.value, null);
					break;
				case Labels.AssigDecl:
					mem.nameSpace [now].addUpdateValue ((Obj)children [0].value, InterpretExp (children [1], now));
					break;
				case Labels.Assig:
					mem.nameSpace [now].addUpdateValue ((Obj)children [0].value, InterpretExp (children [1], now));
					break;
				case Labels.Print:
					if (children [0].isTerminal () && children [0].value is string)
						Console.WriteLine ("FUNW@P console: " + children [0].value);
					else
						Console.WriteLine ("FUNW@P console: " + Convert.ToString (InterpretExp (children [0], now)));
					break;
				case Labels.Return:
					return InterpretExp (children [0], now);
				}
			} 
			return null;
		}

		private bool InterpretCondition (ASTNode node, Obj now)
		{
			return (bool) InterpretExp(node,now);
		}

		private int InterpretExpInt (ASTNode node, Obj now){
			return (int) InterpretExp(node,now);
		}

		private object InterpretExp (ASTNode node, Obj now)
		{
			if (node.isTerminal ()) {
				if (node.value is Obj)
					return mem.nameSpace[now].getValue((Obj)node.value);
				else
					return node.value;
			} else {
				List<ASTNode> children = node.children;
				switch (node.label) {
				case Labels.Plus:
					return (int) InterpretExp (children [0],now) + (int) InterpretExp (children [1],now);
				case Labels.Minus:
					return (int) InterpretExp (children [0],now) - (int) InterpretExp (children [1],now);
				case Labels.Mul:
					return (int) InterpretExp (children [0],now) * (int) InterpretExp (children [1],now);
				case Labels.Div:
					return (int) InterpretExp (children [0],now) / (int) InterpretExp (children [1],now);
				case Labels.Eq:
					var tmp = InterpretExp (children [0],now);
					if (tmp is bool)
						return (bool) tmp == (bool) InterpretExp (children [1],now);
					else
						return (int) InterpretExp (children [0],now) == (int) InterpretExp (children [1],now);
				case Labels.Gt:
					return (int) InterpretExp (children [0],now) > (int) InterpretExp (children [1],now);
				case Labels.Gte:
					return (int) InterpretExp (children [0],now) >= (int) InterpretExp (children [1],now);
				case Labels.Lt:
					return (int) InterpretExp (children [0],now) < (int) InterpretExp (children [1],now);
				case Labels.Lte:
					return (int) InterpretExp (children [0],now) <= (int) InterpretExp (children [1],now);
				case Labels.FunCall:
					Obj funName = (Obj)node.value;
					mem.addNameSpace (funName);
					ASTNode funNode = mem.getFunction (funName);
					int i = 1;
					foreach (ASTNode actual in children) {
						mem.nameSpace [funName].addUpdateValue ((Obj)funNode.children [i].value, InterpretExp (actual, now));
						i++;
					}
					object ret = Interpret (funNode.children [0], funName);
					mem.removeNameSpae (funName);
					return ret;
				default:
					return null;
				}
			}
		}
	}
}