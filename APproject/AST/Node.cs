using System.Collections.Generic;
using System;

namespace APproject
{	
	public enum Statement {If,While,Return,Ass,Dec,AssDec,Print,Read,Async, 
							plus,mul,minus,div,g,geq,l,leq,eq};

	public class Node
	{
		public Node parent;
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

