using System.Collections.Generic;
using System;

namespace APproject
{	
    /**
     * All possible node labels: both reserved keywords and primitive operators.
     **/
	public enum Labels {Main, FunDecl, If, While, Return, Assig, Decl, AssigDecl, Print, Block,
                        For, Async, Afun, Plus, Mul, Minus, Div, Gt, Gte, Lt, Lte, Eq,
						Term, FunCall};

	public class Node
	{
		public Node parent;
		private List<Node> children;
		public Labels label;
		public Term term;

		/// <summary>
		/// Given only the label of the node create a new istance of the class Node.
		/// </summary>
		/// <param name="l">L.</param>
		public Node (Labels l){
			this.term = null;
			this.parent = null;
			this.children = new List<Node> ();
			this.label = l;
		}

		/// <summary>
		/// Create a terminal node given a istance of the class Term
		/// </summary>
		/// <param name="term">Term.</param>
		public Node (Term term){
			this.parent = null;
			this.label = Labels.Term;
			this.children = null;
			this.term = term;
		}

		/// <summary>
		/// Add a new children to the Node.
		/// </summary>
		/// <param name="node">Node.</param>
		public void addChildren (Node node){
			node.parent = this;
			children.Add (node);
		}

		/// <summary>
		/// Return a List of children 
		/// </summary>
		/// <returns>The children.</returns>
		public List<Node> getChildren(){
			return new List<Node>(children);
		}

		/// <summary>
		/// Check if a node is a leaf. 
		/// </summary>
		/// <returns><c>true</c>, if terminal was ised, <c>false</c> otherwise.</returns>
		public bool isTerminal(){
			return label == Labels.Term;
		}
	}

	public enum terminalType {integer, boolean, variable}

	public class Term{
		public int integer;
		public bool boolean;
		public Obj variable;
		public terminalType type;

		/// <summary>
		/// Create a new Terminal of type int
		/// </summary>
		/// <param name="integer">Integer.</param>
		public Term(int integer){
			this.integer = integer;
			type = terminalType.integer;
		}

		/// <summary>
		/// Create a new Terminal of type bool
		/// </summary>
		/// <param name="boolean">If set to <c>true</c> boolean.</param>
		public Term(bool boolean){
			this.boolean = boolean;
			type = terminalType.boolean;
		}

		/// <summary>
		/// Create a new terminal of type variable or function
		/// </summary>
		/// <param name="variable">Variable.</param>
		/// <param name="type">Type.</param>
		public Term(Obj variable){
			this.variable = variable;
			this.type = terminalType.variable;
		}

		/// <summary>
		/// Returns a String that represents the current object.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="APproject.Term"/>.</returns>
		public override string ToString(){
			switch (type) {
			case terminalType.boolean:
				return Convert.ToString (boolean);
			case terminalType.integer:
				return Convert.ToString (integer);
			case terminalType.variable:
				return variable.name;
			}
			return "";
		}
	}
}

