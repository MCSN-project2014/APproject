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
			if (TryGetValue (var, out result))
				if (result != null)
					return result;
				else
					throw new VariableNotInizialized (var.name);
			else
				throw new VariableNotFoundException();
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

	public class VariableNotFoundException: Exception
	{
		public VariableNotFoundException()
		{
		}

		public VariableNotFoundException(string message)
			: base(message)
		{
		}

		public VariableNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}