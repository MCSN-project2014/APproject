using System;
using System.Collections.Generic;

namespace APproject
{
	public class Memory
	{
		private List<Dictionary<Obj,object>> mem;
		private int lastIndex {get{return mem.Count-1;}}

		public Memory (){
			mem = new List<Dictionary<Obj,object>>();
			//addScope();
		}

		public void addScope(){
			mem.Add(new Dictionary<Obj,object>());
		}

		public void removeScope(){
			mem.RemoveAt(lastIndex);
		}

		public void addUpdateValue (Obj var , ASTNode fun, Memory mem){
			addUpdateValue(var, new Tuple<ASTNode,Memory>(fun,mem));
		}

		public void addUpdateValue(Obj var, object value){
			bool find = false;
			foreach (Dictionary<Obj,object> scope in mem) {
				find = scope.ContainsKey (var);
				if (find) {
					scope [var] = value;
					break;
				}
			}
			if (!find)
				mem [lastIndex].Add (var, value);
		}

		public bool TryGetValue(Obj var, out object value){
			value = null;
			for (int i = lastIndex; i >= 0; i--) {
				if (mem [i].TryGetValue (var, out value))
					return true;
			}
			return false;
		}

		public object GetValue(Obj var){
			object result;
			if (TryGetValue(var, out result))
				return result;
			else
				throw new MemoryNotFoundException();
		}

		public Memory CloneMemory(){
			Memory funMem = new Memory ();
			funMem.addScope ();
			foreach (var scope in mem) {
				foreach (var dic in scope) {
					funMem.addUpdateValue (dic.Key, dic.Value);
				}
			}
			return funMem;
		}


	}

	public class MemoryNotFoundException: Exception
	{
		public MemoryNotFoundException()
		{
		}

		public MemoryNotFoundException(string message)
			: base(message)
		{
		}

		public MemoryNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	/*
	public class Memory
	{
		private Dictionary<Obj,List<Dictionary<Obj,object>>> mem;
		public Obj actualFun {
			set {
				actualFun = value;
				List<Dictionary<Obj,object>> scope;
				if (mem.TryGetValue (actualFun, out scope))
					funScope = scope;
				else funScope = null;
			}
			get { return actualFun; }
		}
		private List<Dictionary<Obj,object>> funScope;
		private int iFunScope {get{return funScope.Count-1;}}

		public Memory (Obj fun){
			mem = new Dictionary<Obj,List<Dictionary<Obj,object>>>();
			addFunction (fun);
		}

		public void addFunction (Obj fun){
			mem.Add(fun, new List<Dictionary<Obj, object>>());
			actualFun = fun;
			addScope ();
		}

		public void addScope(){
			funScope.Add(new Dictionary<Obj,object>());
		}

		public void removeScope(){
			funScope.RemoveAt(iFunScope);
		}

		public void addValue(Obj var, object value){
			funScope[iFunScope].Add (var, value);
		}

		public object getValue(Obj var){
			int count = funScope.Count;
			for (int i = count; i >= 0; i--) {
				object value;
				if (funScope[i].TryGetValue (var, out value))
					return value;
			}
			return null;
		}
	}*/
}