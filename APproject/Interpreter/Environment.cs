using System;
using System.Collections.Generic;

namespace APproject
{
	public class Environment
	{
		private List<Dictionary<string,object>> mem;
		private int lastIndex {get{return mem.Count-1;}}

		/// <summary>
		/// Initializes a new instance of the Memory class.
		/// Before use the object, it is necessary to call the addScope method, to create a inizial scope.
		/// </summary>
		public Environment (){
			mem = new List<Dictionary<string,object>>();
		}

		/// <summary>
		/// Add a new scope in the memory
		/// </summary>
		public void addScope(){
			mem.Add(new Dictionary<string,object>());
		}

		/// <summary>
		/// Remouve the last scope.
		/// </summary>
		public void removeScope(){
			mem.RemoveAt(lastIndex);
		}

		/// <summary>
		/// Add or Update an anonymus function in the actual scope
		/// </summary>
		/// <param name="var">Variable.</param>
		/// <param name="fun">Fun.</param>
		/// <param name="mem">Mem.</param>
		public void addUpdateValue (Obj var , ASTNode fun, Environment mem){
			addUpdateValue(var.name, new Tuple<ASTNode,Environment>(fun,mem));
		}

		/// <summary>
		/// Add or Update a variable in the actual scope.
		/// </summary>
		/// <param name="var">Variable.</param>
		/// <param name="value">Value.</param>
		public void addUpdateValue(Obj var, object value){
			addUpdateValue (var.name, value);
		}

		private void addUpdateValue(string var, object value){
			bool find = false;
			foreach (var scope in mem) {
				find = scope.ContainsKey (var);
				if (find) {
					scope [var] = value;
					break;
				}
			}
			if (!find)
				mem [lastIndex].Add (var, value);
		}

		/// <summary>
		/// Try to get a variable value from the memory.
		/// If the varialble value is null a VariableNotInizialized exception is throw.
		/// </summary>
		/// <returns><c>true</c>, if get value was tryed, <c>false</c> otherwise.</returns>
		/// <param name="var">Variable.</param>
		/// <param name="value">Value.</param>
		public bool TryGetValue(Obj var, out object value){
			value = null;
			for (int i = lastIndex; i >= 0; i--) {
				if (mem [i].TryGetValue (var.name, out value)) {
					if (value != null)
						return true;
					else
						throw new VariableNotInizialized (var.name);
				}
			}
			return false;
		}

		/// <summary>
		/// Get a value from the memory, if the variable is not found throw a Variable NotFoundException(),
		/// instead if the variable is not inizialized a VariableNotInizialized exception is throw. 
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="var">Variable.</param>
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

		/// <summary>
		/// Clones the actual memory in a new one.
		/// </summary>
		/// <returns>The memory.</returns>
		public Environment CloneMemory(){
			Environment funMem = new Environment ();
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