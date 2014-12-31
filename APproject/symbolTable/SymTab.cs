using System;
using System.Collections.Generic;

namespace APproject
{
	public enum Types {undef,integer,boolean,fun}
	public enum Kinds {var,proc,act,scope}

    public class RType
    {
        public Types type;                          // Type of the function returned
        public Queue<Types> formals;                // types of formals
        public RType next;                          // the return type of the function returned
    }
    
    public class Obj {                              // object describing a declared name
		public string name;		                    // name of the object
		public Types type;          			    // type of the object (undef for proc)
		public Obj	next;	            		    // to next object in same scope
        public Obj owner;                           // the owner of the scope
		public Kinds kind;                          // var, proc, scope
		public int adr;	            			    // address in memory or start of proc
		public int level;           			    // nesting level; 0=global, 1=local
		public Obj locals;          		        // scopes: to locally declared objects
		public int nextAdr;		                    // scopes: next free address in this scope
        public RType rtype;
	    public Queue<Obj> formals { get; set; }
	}

   

	public class SymbolTable {

		public int curLevel;	// nesting level of current scope
		public Obj undefObj;	// object node for erroneous symbols
		public Obj topScope;	// topmost procedure scope
		
		Parser parser;
		
		public SymbolTable(Parser parser) {
			this.parser = parser;
			topScope = null;

			undefObj = new Obj();
			undefObj.name  =  "undef"; undefObj.type = Types.undef; undefObj.kind = Kinds.var;
			undefObj.adr = 0; undefObj.level = 0; undefObj.next = null;
		}

        ///<summary>
		/// open a new scope and make it the current scope (topScope)
		/// <summary>
        public void OpenScope () {
			Obj scop = new Obj();
			scop.name = ""; 
            scop.kind = Kinds.scope;
	        scop.locals = null; 
	        scop.nextAdr = 0;
			scop.next = topScope; 
            topScope = scop; 
			curLevel++;
		}

        ///<summary>
        /// open a new scope and make it the current scope with owner (topScope)
        /// <summary>
        /// <param name="name">owner.</param>
        public void OpenScope(Obj owner)
        {
            Obj scop = new Obj();
            scop.name = owner.name;
            scop.kind = Kinds.scope;
            scop.owner = owner;
            scop.locals = null;
            scop.nextAdr = 0;
            scop.next = topScope;
            topScope = scop;
            curLevel++;

        }

        ///<summary>
		/// close the current scope
        /// <summary>
		public void CloseScope () {
			topScope = topScope.next; curLevel--;
		}

        /// <summary>
		/// create a new object node in the current scope
        /// <summary>
        /// <param name="name">name.</param>
        /// <param name="kind">kind.</param>
        /// <param name="type">type.</param>
		public Obj NewObj (string name, Kinds kind, Types type) {
	        Obj p, last, obj = new Obj(); 
			obj.name = name; obj.kind = kind; obj.type = type;
			obj.level = curLevel;
			p = topScope.locals; last = null;
			while (p != null) { 
				if (p.name == name) parser.SemErr("name declared twice");
				last = p; p = p.next;
			}
			if (last == null) topScope.locals = obj; else last.next = obj;
			if (kind == Kinds.var) obj.adr = topScope.nextAdr++;
            if (kind == Kinds.proc) obj.formals = new Queue<Obj>();
	       
			return obj;
		}

        public void complexReturnTypeControl(Obj procedur, Obj robj)
        {
            RType procrtype = procedur.rtype;
            RType returnobj = robj.rtype;
            if (procedur.type != Types.fun)
            {
                parser.SemErr(procedur.type + " return type expected");
                return;
            }
           
            procrtype = procrtype.next;
            if (robj.formals.Count != procrtype.formals.Count)
                parser.SemErr("parameter expected return type");
            else
            {
                Obj[] aFunFormals = robj.formals.ToArray();
                Types[] rTypeformals = procrtype.formals.ToArray();
                for (int i = 0; i < aFunFormals.Length; i++)
                {
                    if (aFunFormals[i].type != rTypeformals[i])
                    {
                        parser.SemErr(rTypeformals[i] + " parameter in return type expected");
                        return;
                    }
                }

                if(procrtype.type != returnobj.type)
                {
                    parser.SemErr(procrtype.type +" return type expected");
                    return;
                }
             }
            while (procrtype.next != null)
            {
                
            }


        }

        /// <summary>
        /// return the owner of the current scope
        /// <summary>

        public Obj getOwner()
        {
            return topScope.owner;
        }

        /// <summary>
        /// Add a formal formal to procedure
        /// <summary>
        /// <param name="procedure">procedure.</param>
        /// <param name="parameter">parameter.</param>

        public void addFormal(Obj procedure, Obj parameter)
        {
            procedure.formals.Enqueue(parameter);
        }

        /// <summary>
        /// Set the return type of the procedure
        /// <summary>
        /// <param name="procedure">procedure.</param>
        /// <param name="type">type.</param>
        public void setRType(Obj procedure, RType rtype)
        {
            procedure.type = rtype.type;
            procedure.rtype = rtype;
        }

       
		/// <summary>
		/// Checks that the actual parameter type of a function call have the same type of the formal parameters.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="actualTypes">Actual types.</param>
	    public void checkActualFormalTypes(Obj obj, Queue<Types> actualTypes)
	    {
	        if (obj.formals.Count != actualTypes.Count)
	            parser.SemErr("parameter expected");
	        else
	        {
	            Obj[] ArrayofFormals = obj.formals.ToArray();
	            for (int i = 0; i < ArrayofFormals.Length; i++)
	            {
	                Obj formal = ArrayofFormals[i];
	                Types actType = actualTypes.Dequeue();
	                if (formal.type != actType)
	                {
						parser.SemErr(formal.type + "type expected");
	                }
	            }

	        }
	    }



        /// <summary>
        /// search the name in all open scopes and return its object node
        /// </summary>
        /// <param name="name">name.</param>
		public Obj Find (string name) {
            Obj obj, scope; 
			scope = topScope;
			while (scope != null) {  // for all open scopes
				obj = scope.locals;
				while (obj != null) {  // for all objects in this scope
					if (obj.name == name) return obj;
					obj = obj.next;
				}
				scope = scope.next;
			}
			parser.SemErr(name + " is undeclared");
			return undefObj;
		}

	} // end SymbolTable

} // end namespace