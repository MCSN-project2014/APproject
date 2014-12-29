using System;
using System.Collections.Generic;

namespace APproject
{
	public class MemoryFunction
	{
		private Dictionary<Obj,Memory> _nameSpace;
		private Dictionary<Obj,ASTNode> function;
		public Dictionary<Obj,Memory> nameSpace{ get { return _nameSpace; } }

		public MemoryFunction (){
			_nameSpace = new Dictionary<Obj, Memory> ();
			function = new Dictionary<Obj, ASTNode> ();
		}

		public void addFunction(Obj fun, ASTNode node){
			function.Add (fun, node);
		}

		public ASTNode getFunction(Obj fun){
			return function [fun];
		}

		public void addNameSpace(Obj fun){
			_nameSpace.Add (fun, new Memory ());
		}

		public void removeNameSpae(Obj fun){
			_nameSpace.Remove (fun);
		}

		public Memory createClosure(Obj fun){
			return _nameSpace [fun].getMemory ();
		}

		public void createNameSpace(Obj fun, Memory mem){
			_nameSpace.Add(fun, mem);
		}
	}
}