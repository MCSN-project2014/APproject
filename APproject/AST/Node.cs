using System.Collections.Generic;
using System;

namespace APproject
{	
    /**
     * All possible node labels: both reserved keywords and primitive operators.
     **/
	public enum Labels {Main, FunDecl, Fun, If, While, Return, Assig, Decl, AssigDecl, Print,
                            For, Async, Afun, Plus, Mul, Minus, Div, Gt, Gte, Lt, Lte, Eq};

	public class Node
	{
		private Node parent;
		private List<Node> children;
		public Labels label;
		//public Operation op;
		public Obj term;

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

        /**
         * This method determines whether the node is a leaf or not
         * (i.e. if the node represents a terminal symbol).
        **/
        public Boolean isTerminal()
        {
            if (term.type == Types.boolean || term.type == Types.integer)
                return true;
            else return false;
        }
	}
}

