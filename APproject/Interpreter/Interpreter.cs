using System;
using System.Collections.Generic;

namespace APproject
{
	public class Interpreter
	{
		//MemoryFunction funMem;
		private Dictionary<Obj,ASTNode> function;
		//Obj main;
		ASTNode startNode;

		public Interpreter( ASTNode node){
			startNode = node;
			//main = new Obj{ name = "main" };
			function = new Dictionary<Obj, ASTNode> ();
		}
			
		public void Start (){
			Console.WriteLine ("\nINTERPRETER START:");
			foreach (ASTNode node in startNode.children) {
				if (node.label == Labels.FunDecl)
					function.Add ((Obj)node.value, node);
				if (node.label == Labels.Main) {
					//funMem.addNameSpace (main);
					var mem = new Memory ();
					mem.addScope ();
					Interpret (node, mem);
					mem.removeScope ();
				} else {
					var mem = new Memory ();
					mem.addScope ();
					Interpret (node, mem);
					mem.removeScope ();
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
					//if (!(ret is  Tuple<ASTNode,Memory>))
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
				case Labels.For:
					Interpret (children [0], actualMemory);
					condition = InterpretCondition (children [1], actualMemory);
					while (condition) {
						ret = Interpret (children [3], actualMemory);
						if (ret != null)
							return ret;
						Interpret (children [2], actualMemory);
						condition = InterpretCondition (children [1], actualMemory);
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
					object tmp = InterpretExp (children [0], actualMemory);
					if (tmp is Tuple<ASTNode,Memory>){
						var afunTuple = (Tuple<ASTNode,Memory>) tmp;
						return new Tuple<ASTNode,Memory>(afunTuple.Item1,afunTuple.Item2.CloneMemory());
					}
					return tmp;
				}
			} 
			return null;
		}

		private void Assignment(List<ASTNode> children, Memory actualMemory){
			object value = InterpretExp (children [1], actualMemory);
			Obj variable = (Obj) children[0].value;
			if (value is Tuple<ASTNode,Memory>) {
				var tuple = (Tuple<ASTNode,Memory>)value;
				actualMemory.addUpdateValue (variable, tuple.Item1, tuple.Item2);
			}else
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
					return actualMemory.GetValue ((Obj)node.value);
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
					ASTNode funNode;
					Memory funMem;
					if (function.TryGetValue (funName, out funNode)) {
						funMem = new Memory ();
					} else {
						var afun = (Tuple<ASTNode,Memory>) actualMemory.GetValue (funName); 
						funNode = afun.Item1;
						funMem = afun.Item2;
					}
					funMem.addScope ();
					int i = 1;
					foreach (ASTNode actual in children) {
						funMem.addUpdateValue ((Obj)funNode.children [i].value, InterpretExp (actual, actualMemory));
						i++;
					}
					object ret = Interpret (funNode.children [0], funMem);
					//if (!(ret is Tuple<ASTNode,Memory>))
						funMem.removeScope ();
					return ret;
				case Labels.Afun:
					return new Tuple<ASTNode,Memory> (node,actualMemory);
				case Labels.Read:
					string read = Console.ReadLine ();
					try{
						return Convert.ToInt32 (read);
					}catch(FormatException){
						return null;
					}
				default:
					return null;
				}
			}
		}
	}
}