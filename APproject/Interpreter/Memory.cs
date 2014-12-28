using System;
using System.Collections.Generic;

namespace APproject
{
	public class Memory
	{
		private List<Dictionary<Obj,object>> mem;
		private Dictionary<Obj,Memory> function;

		private int lastIndex {get{return mem.Count-1;}}

		public Memory (){
			mem = new List<Dictionary<Obj,object>>();
			addScope();
		}

		public void addScope(){
			mem.Add(new Dictionary<Obj,object>());
		}

		public void removeScope(){
			mem.RemoveAt(lastIndex);
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

		public object getValue(Obj var){
			int count = lastIndex;
			for (int i = count; i >= 0; i--) {
				object value;
				if (mem[i].TryGetValue (var, out value))
					return value;
			}
			return null;
		}

		public void createClousure(Obj fun){
			Memory funMem = new Memory ();
			foreach (var scope in mem) {
				foreach (var dic in scope){
					funMem.addUpdateValue (dic.Key, dic.Value);
				}
			}
			function.Add (fun, funMem);
		}

		public void addUpdateValue(Obj var, object value, Obj fun){
			bool find = false;
			foreach (var scope in mem) {
				find = scope.ContainsKey (var);
				if (find) {
					scope [var] = value;
					break;
				}
			}
			if (!find)
				function [fun].addUpdateValue (var, value);
		}

		public object getValue(Obj var, Obj fun){
			object value = getValue (var);
			if (value == null) {
				Memory funmen;
				if (function.TryGetValue (fun, out funmen)) {
					return funmen.getValue (var);
				}
			} else
				return value;
			return null;
		}

		public void addScope(Obj fun){
			function [fun].addScope ();
		}

		public void removeScope(Obj fun){
			function [fun].removeScope ();
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