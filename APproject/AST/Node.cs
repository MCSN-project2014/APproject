using System.Collections.Generic;
using System;

namespace APproject
{	
    /**
     * All possible node labels: both reserved keywords and primitive operators.
     **/
	public enum Labels {Main, FunDecl, Fun, If, While, Return, Assig, Decl, AssigDecl, Print, Block,
                            For, Async, Afun, Plus, Mul, Minus, Div, Gt, Gte, Lt, Lte, Eq};

	public class Node
	{
		public Node parent;
		private List<Node> children;
		public Labels label;
		public Term term;

		private void init(){
			this.term = null;
			this.parent = null;
			this.children = new List<Node> ();
		}

		public Node (Labels l)
		{
			init ();
			this.label = l;
		}

		public Node (Term term){
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

	public enum terminalType {integer, boolean, variable, functionCall}

	public class Term{
		public int integer;
		public bool boolean;
		public Obj variable;
		public terminalType type;

		public Term(int integer){
			this.integer = integer;
			type = terminalType.integer;
		}

		public Term(bool boolean){
			this.boolean = boolean;
			type = terminalType.boolean;
		}

		public Term(Obj variable, terminalType type){
			this.variable = variable;
			this.type = type;
		}
	}
}

