using System.Collections.Generic;
using System;

namespace APproject
{	
	public enum Statement {If,While,Return,Ass,Dec,AssDec,Print,Read,Async, 
							plus,mul,minus,div,g,geq,l,leq,eq};

	public class Node
	{
		private List<Node> children;
		public Statement stmn;
		//public Operation op; 
		public Obj term;

		private void init(){
			term = null;
			children = new List<Node> ();
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
			children.Add (node);
		}

		public List<Node> getChildren(){
			return new List<Node>(children);
		}
	}
}

