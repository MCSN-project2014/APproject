using System;
using System.Collections.Generic;

namespace APproject
{
	public class MemoryFunction
	{
		//private List<List<Memory>> _nameSpace;
		private Dictionary<Obj,Tuple<ASTNode,Memory>> function;
		//public Dictionary<Obj,Memory> nameSpace { get { return _nameSpace; } }

		public MemoryFunction (){
			//_nameSpace = new Dictionary<Tuple<Obj,int>,Memory> ();
			function = new Dictionary<Obj, Tuple<ASTNode,Memory>> ();
		}

		public void addFunction(Obj fun, ASTNode node, Memory mem){
			function.Add (fun, new Tuple<ASTNode,Memory>(node,mem));
		}

		public void addFunction(Obj fun, ASTNode node){
			addFunction (fun, node, null);
		}

		public Tuple<ASTNode,Memory> getFunction(Obj fun){
			return function [fun];
		}
		/*
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
		*/
	}
}