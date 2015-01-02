using System.Collections.Generic;
using System;

namespace APproject
{	
    /// <summary>
	/// All possible node labels: both reserved keywords and primitive operators.
    /// </summary>
	public enum Labels {Program, Main, Afun, FunDecl, 
						For, If, While, Block,
						Assig, Decl, AssigDecl, 
						Return, Async, Print, Read,  
						Plus, Mul, Minus, Div, Gt, Gte, Lt, Lte, Eq, NotEq, And, Or, Negativ,
						FunCall};

	public abstract class ASTNode{
		public ASTNode parent;

		protected object _value;
		public object value{ get{return _value;}}

		protected List<ASTNode> _children;
		public List<ASTNode> children{ get { return _children; } }

		protected Labels _label;
		public Labels label{ get { return _label; } }

		protected int _line;
		protected int _column;
		public int line{ get {return _line;}}
		public int column{ get {return _column;}}

		public abstract bool isTerminal ();

		public virtual Types type { get{ return Types.undef;}}
	}

	public class Node : ASTNode
	{
		/// <summary>
		/// Given the label of the node, create a new istance of the class Node.
		/// </summary>
		/// <param name="l">L.</param>
		public Node (Labels l){
			parent = null;
			_children = new List<ASTNode> ();
			_label = l;
		}

		/// <summary>
		/// Given the label of the node and a value (use only for function declaration and function call), create a new istance of the class Node.
		/// </summary>
		/// <param name="l">L.</param>
		public Node (Labels l, object value){
			parent = null;
			_children = new List<ASTNode> ();
			_label = l;
			_value = value;
		}

		public Node (Labels l, int line, int column){
			parent = null;
			_children = new List<ASTNode> ();
			_label = l;
			_value = value;
			_line = line;
			_column = column;
		}

		public Node (Labels l, object value, int line, int column){
			parent = null;
			_children = new List<ASTNode> ();
			_label = l;
			_value = value;
			_line = line;
			_column = column;
		}

		/// <summary>
		/// Add a new children to the Node.
		/// </summary>
		/// <param name="node">Node.</param>
		public void addChildren (ASTNode node){
			if (node != null) {
				node.parent = this;
				children.Add (node);
			}
		}

		/// <summary>
		/// Add a new children at the given position.
		/// </summary>
		/// <param name="i">The index.</param>
		/// <param name="node">Node.</param>
		public void addChildren (int i, ASTNode node){
			node.parent = this;
			children.Insert (i, node);
		}
			
		public override bool isTerminal(){
			return false;
		}

		public override string ToString(){
			return Convert.ToString(_label);
		}
	}


	public class Term : ASTNode{
		/// <summary>
		/// Create a new Terminal of type int
		/// </summary>
		/// <param name="integer">Integer.</param>
		public Term(object value){
			_value = value;
		}

		public Term(object value, int line, int column){
			_value = value;
			_line = line;
			_column = column;
		}

		/// <summary>
		/// Returns a String that represents the current object.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="APproject.Term"/>.</returns>
		public override string ToString(){
			if (_value is Obj)
				return ((Obj)value).name;
			string tmp = Convert.ToString (value);
			if(_value is bool) return tmp.ToLower ();
			return tmp;
		}

		public override bool isTerminal(){
			return true;
		}

		public override Types type { 
			get { if (_value is Obj) {
					return ((Obj)_value).type;
				} else
					return Types.undef;
				}
		}
	}

	/*
	public class Node
	{
		public Node parent;
		private List<Node> children;
		public Labels label;
		public Term term;

		/// <summary>
		/// Given the label of the node, create a new istance of the class Node.
		/// </summary>
		/// <param name="l">L.</param>
		public Node (Labels l){
			this.term = null;
			this.parent = null;
			this.children = new List<Node> ();
			this.label = l;
		}

		/// <summary>
		/// Create a terminal node given a istance of the class Term.
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
		/// <returns><c>true</c>, if terminal was used, <c>false</c> otherwise.</returns>
		public bool isTerminal(){
			return label == Labels.Term;
		}
	}

	public enum terminalType {integer, boolean, String, variable}

	public class Term{
		public object value;
		public terminalType type;

		/// <summary>
		/// Create a new Terminal of type int
		/// </summary>
		/// <param name="integer">Integer.</param>
		public Term(int integer){
			this.value = integer;
			type = terminalType.integer;
		}

		/// <summary>
		/// Create a new Terminal of type bool
		/// </summary>
		/// <param name="boolean">If set to <c>true</c> boolean.</param>
		public Term(bool boolean){
			this.value = boolean;
			type = terminalType.boolean;
		}

		/// <summary>
		/// Create a new terminal of type variable or function
		/// </summary>
		/// <param name="variable">Variable.</param>
		/// <param name="type">Type.</param>
		public Term(Obj variable){
			this.value = variable;
			this.type = terminalType.variable;
		}

		/// <summary>
		/// Create a new Terminal of type String
		/// </summary>
		/// <param name="integer">Integer.</param>
		public Term(string String){
			this.value = String;
			type = terminalType.String;
		}

		/// <summary>
		/// Returns a String that represents the current object.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="APproject.Term"/>.</returns>
		public override string ToString(){
			switch (type) {
				case terminalType.variable:
					return ((Obj)value).name;
				default:
					return Convert.ToString (value);
			}
		}
	}
	*/
	/*
	public class Term{
		public int integer;
		public bool boolean;
		public Obj variable;
		public string String;
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
		/// Create a new Terminal of type String
		/// </summary>
		/// <param name="integer">Integer.</param>
		public Term(string String){
			this.String = String;
			type = terminalType.String;
		}

		/// <summary>
		/// Returns a String that represents the current object.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="APproject.Term"/>.</returns>
		public override string ToString(){
			switch (type) {
			case terminalType.String:
				return String;
			case terminalType.integer:
				return Convert.ToString (integer);
			case terminalType.boolean:
				return Convert.ToString (boolean);
			case terminalType.variable:
				return variable.name;
			default:
				return "";
			}
		}
	}
	*/
}

