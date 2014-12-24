using System.Collections.Generic;
using System;

namespace APproject
{	
    /**
     * All possible node labels: both reserved keywords and primitive operators.
     **/
	public enum Statement {Main, FunDecl, Fun, If, While, Return, Assig, Decl, AssigDecl, Print,
                            For, Read, Async, Afun, Plus, Mul, Minus, Div, Gt, Gte, Lt, Lte, Eq};

	public class Node
	{
		private Node parent;
		private List<Node> children;
		public Statement stmn;
		//public Operation op;
		public Obj term;

		private void init(){
			this.term = null;
			this.parent = null;
			this.children = new List<Node> ();
		}
		public Node (Statement stmn)
		{
			init ();
			this.stmn = stmn;
		}

		public Node (Obj term){
			init ();
			this.term = term;
		}

		public void addChildren (Node node){
			node.parent = this;
			children.Add (node);
		}

		public List<Node> getChildren(){
			return new List<Node>(children);
		}
	}
}

