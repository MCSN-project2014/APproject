using System;
using System.Collections.Generic;

namespace APproject
{
	public class Interpreter
	{
		MemoryFunction funMem;
		Obj main;
		ASTNode startNode;

		public Interpreter( ASTNode node){
			startNode = node;
			main = new Obj{ name = "main" };
			funMem = new MemoryFunction ();
		}
			
		public void Start (){
			Console.WriteLine ("INTERPRETER START:");
			foreach (ASTNode node in startNode.children) {
				if (node.label == Labels.FunDecl)
					funMem.addFunction ((Obj)node.value, node);
				if (node.label == Labels.Main) {
					//funMem.addNameSpace (main);
					var mem = new Memory ();
					Interpret (node, mem);
				}
			}
		}

		private object Interpret (ASTNode node, Memory actualMemory){
			if (!node.isTerminal ()) {
				List<ASTNode> children = node.children;
				bool condition;
				switch (node.label) {
				case Labels.Main:
					foreach (Node n in children)
						Interpret (n, actualMemory);
					break;
				case Labels.Block:
					actualMemory.addScope ();
					object ret = null;
					foreach (Node n in children) {
						ret = Interpret (n, actualMemory);
						if (ret != null)
							break;
					}
					actualMemory.removeScope ();
					return ret;
				case Labels.If:
					condition = InterpretCondition (children [0], actualMemory);
					if (condition) {
						return Interpret (children [1], actualMemory);
					} else if (children.Count > 2)
						return Interpret (children [2], actualMemory);
					break;
				case Labels.While:
					condition = InterpretCondition (children [0], actualMemory);
					while (condition) {
						ret = Interpret (children [1], actualMemory);
						if (ret!=null)
							return ret;
						condition = InterpretCondition (children [0], actualMemory);
					}
					break;
				case Labels.Decl:
					foreach (ASTNode n in children)
						actualMemory.addUpdateValue ((Obj)n.value, null);
					break;
				case Labels.AssigDecl:
					Assignment (children, actualMemory);
					break;
				case Labels.Assig:
					Assignment (children, actualMemory);
					break;
				case Labels.Print:
					if (children [0].isTerminal () && children [0].value is string)
						Console.WriteLine ("FUNW@P console: " + children [0].value);
					else
						Console.WriteLine ("FUNW@P console: " + Convert.ToString (InterpretExp (children [0], actualMemory)));
					break;
				case Labels.Return:
					return InterpretExp (children [0], actualMemory);
				}
			} 
			return null;
		}

		private void Assignment(List<ASTNode> children, Memory actualMemory){
			object value = InterpretExp (children [1], actualMemory);
			Obj variable = (Obj) children[0].value;
			if (value is Node)
				funMem.addFunction(variable, (Node)value, actualMemory);
			else
				actualMemory.addUpdateValue (variable, value);
		}

		private bool InterpretCondition (ASTNode node, Memory actualMemory)
		{
			return (bool) InterpretExp(node,actualMemory);
		}

		private int InterpretExpInt (ASTNode node, Memory actualMemory){
			return (int) InterpretExp(node,actualMemory);
		}

		private object InterpretExp (ASTNode node, Memory actualMemory)
		{
			if (node.isTerminal ()) {
				if (node.value is Obj)
					return actualMemory.getValue((Obj)node.value);
				else
					return node.value;
			} else {
				List<ASTNode> children = node.children;
				switch (node.label) {
				case Labels.Plus:
					return (int) InterpretExp (children [0], actualMemory) + (int) InterpretExp (children [1],actualMemory);
				case Labels.Minus:
					return (int) InterpretExp (children [0],actualMemory) - (int) InterpretExp (children [1],actualMemory);
				case Labels.Mul:
					return (int) InterpretExp (children [0],actualMemory) * (int) InterpretExp (children [1],actualMemory);
				case Labels.Div:
					return (int) InterpretExp (children [0],actualMemory) / (int) InterpretExp (children [1],actualMemory);
				case Labels.Eq:
					var tmp = InterpretExp (children [0],actualMemory);
					if (tmp is bool)
						return (bool) tmp == (bool) InterpretExp (children [1],actualMemory);
					else
						return (int) InterpretExp (children [0],actualMemory) == (int) InterpretExp (children [1],actualMemory);
				case Labels.Gt:
					return (int) InterpretExp (children [0],actualMemory) > (int) InterpretExp (children [1],actualMemory);
				case Labels.Gte:
					return (int) InterpretExp (children [0],actualMemory) >= (int) InterpretExp (children [1],actualMemory);
				case Labels.Lt:
					return (int) InterpretExp (children [0],actualMemory) < (int) InterpretExp (children [1],actualMemory);
				case Labels.Lte:
					return (int) InterpretExp (children [0],actualMemory) <= (int) InterpretExp (children [1],actualMemory);
				case Labels.FunCall:
					Obj funName = (Obj)node.value;
					var tuple = funMem.getFunction (funName);
					ASTNode funNode = tuple.Item1;
					Memory mem = tuple.Item2!=null ? tuple.Item2 : new Memory();
					int i = 1;
					foreach (ASTNode actual in children) {
						mem.addUpdateValue ((Obj)funNode.children [i].value, InterpretExp (actual, actualMemory));
						i++;
					}
					object ret = Interpret (funNode.children [0], mem);
					return ret;
				case Labels.Afun:
					return node;
				default:
					return null;
				}
			}
		}
	}
}