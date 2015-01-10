using System;
using System.Collections.Generic;

namespace APproject
{
	public enum Types {undef,integer,boolean,fun,url}
	public enum Kinds {var,proc,scope}

    public class FRType
    {
        public Types type;                           // Type of the function returned
        public Queue<Types> formals;                 // types of formals
        public FRType next;                          // the return type of the function returned
    }
    
    public class Obj {                              // object describing a declared name
        protected string _name;                     // name of the object
        protected Types _type;                      // type of the object 
        protected Obj _next;                        // to next object in same scope
        protected Obj _owner;                       // the owner of the scope
        protected Kinds _kind;                      // var, proc, scope
        protected Obj _locals;                      // scopes: to locally declared objects
        protected bool _isUsedFromAfun;             // var : indicates if the variable is used in afun but is declared external
        protected bool _isUsedInAsync;              // var : indicates if the variable is used in async
        protected bool _isUsedInDasync;             // var : indicates if the variable is used in dasunc
        protected bool _recursive;                  // indicates whether a function is recursive or not
        protected bool _asyncControl;               // proc : indicates if a function contains or not some println or readln 
        protected bool _returnIsSet;                // proc : indicates if the return statement is set
        protected FRType _rtype;                    // proc : the return the of the function

        public string name { get { return _name; } set { _name = value; } }
        public Types type { get { return _type;} set { _type = value;} }
        public Obj next { get { return _next; } set { _next = value; } }
        public Obj owner { get { return _owner; } set { _owner = value; } }
        public Kinds kind { get { return _kind; } set { _kind = value; } }
        public Obj locals { get { return _locals; } set { _locals = value; } }          		        
        public bool isUsedFromAfun { get { return _isUsedFromAfun; } set { _isUsedFromAfun = value; } }
        public bool isUsedInAsync { get { return _isUsedInAsync; } set { _isUsedInAsync = value; } }
        public bool isUsedInDasync { get { return _isUsedInDasync; } set { _isUsedInDasync = value; } }
        public bool recursive { get { return _recursive; } set { _recursive = value; } }
        public bool asyncControl { get { return _asyncControl; } set { _asyncControl = value; } }
        public bool returnIsSet { get { return _returnIsSet; } set { _returnIsSet = value; } }
        
        public FRType rtype;     
	    public Queue<Obj> formals { get; set; }
	}

   

	public class SymbolTable {

		public int curLevel;	// nesting level of current scope
        public int ifNesting;   // nesting level of if used
        public bool dasyncused; // indicates if the dasync is used somewhere in the code
		public Obj undefObj;	// object node for erroneous symbols
		public Obj topScope;	// topmost procedure scope
		
		Parser parser;
		
		public SymbolTable(Parser parser) {
			this.parser = parser;
			topScope = null;
            curLevel = 0;
            ifNesting = 0;
            dasyncused = false;
			undefObj = new Obj();
			undefObj.name  =  "undef"; 
            undefObj.type = Types.undef;
            undefObj.kind = Kinds.var; 
            undefObj.next = null;
		}

        ///<summary>
		/// open a new scope and make it the current scope (topScope)
		/// <summary>
        public void OpenScope () {
			Obj scop = new Obj();
			scop.name = ""; 
            scop.kind = Kinds.scope;
	        scop.locals = null; 
			scop.next = topScope;
            scop.owner = null;
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
            scop.asyncControl = false;
            owner.returnIsSet = false;
            scop.kind = Kinds.scope;
            scop.owner = owner;
            scop.locals = null;
            scop.next = topScope;
            topScope = scop;
            curLevel++;


        }

        ///<summary>
		/// close the current scope
        /// <summary>
		public void CloseScope () {


            if (topScope.owner != null && !topScope.owner.returnIsSet)
            {
                parser.SemErr("return expected");
            }
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
			p = topScope.locals; last = null;
			while (p != null) { 
				if (p.name == name) parser.SemErr("name declared twice");
				last = p; p = p.next;
			}
			if (last == null) topScope.locals = obj; else last.next = obj;
            if (kind == Kinds.var)
            {
                obj.isUsedFromAfun = false;
                obj.isUsedInAsync = false;
                obj.isUsedInDasync = false;
            }
            if (kind == Kinds.proc)
            {
                obj.formals = new Queue<Obj>();
                obj.asyncControl = false;
                obj.recursive = false;
            }
			return obj;
		}

        /// <summary>
        /// search the name in all open scopes and return its object node
        /// </summary>
        /// <param name="name">name.</param>
        public Obj Find(string name)
        {
            Obj obj, scope, owner;
            owner = getOwner();
            scope = topScope;
            while (scope != null)
            {  // for all open scopes
                obj = scope.locals;
                while (obj != null)
                {  // for all objects in this scope
                    if (obj.name == name)
                        return obj;

                    obj = obj.next;
                }
                scope = scope.next;
            }
            parser.SemErr(name + " is undeclared");
            return undefObj;
        }

        /// <summary>
        /// Checks if the actual parameter type of a function call have the same type of function formal parameters.
        /// </summary>
        /// <param name="obj">The obj of the called function.</param>
        /// <param name="actualTypes">A queue of types of the actual function call.</param>
        public void checkActualFormalTypes(Obj obj, Queue<Types> actualTypes)
        {
            if (!(obj.formals == null || actualTypes == null))
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
        }

        /// <summary>
        /// Set the return type of a function
        /// <summary>
        /// <param name="procedure">obj of the function for which is needed set the return type.</param>
        /// <param name="type">the FRType of the function.</param>
        public void setFRType(Obj procedure, FRType rtype)
        {
            procedure.type = rtype.type;
            procedure.rtype = rtype;
        }


         /// <summary>
        /// Control if the return type of the function is corrected. This metod is usefull only if the return type of the function 
        /// is 'fun', otherwise it is sufficient to check whether procedure.type = return type
        /// <summary>
        /// <param name="procedure">obj of the function for which is needed control the return type.</param>
        /// <param name="type">The obj of the function for which is needed to control if it match the return type of the function</param>
        public void complexReturnTypeControl(FRType procedur, FRType robj)
        {
            FRType procrtype = procedur;
            FRType returnobj = robj;

            
            
            if (procrtype.type != returnobj.type)
            {
                parser.SemErr(procrtype.type + " return type expected");
                return;
            }

            while (procrtype.next != null)
            {
                if (returnobj.next == null)
                {
                    parser.SemErr(procrtype.type + " return type expected");
                }
                else
                {

                    procrtype = procrtype.next;
                    returnobj = returnobj.next;
                    Types[] rTypeformals = procrtype.formals.ToArray();
                    Types[] aFunFormals = returnobj.formals.ToArray();
                    if (procrtype.type != returnobj.type)
                    {
                        parser.SemErr(procrtype.type + " return type expected");
                        return;
                    }
                    if (aFunFormals.Length != rTypeformals.Length)
                    {
                        parser.SemErr("wrong parameter in return type");
                    }
                    else
                    {

                        for (int i = 0; i < aFunFormals.Length; i++)
                        {
                            if (aFunFormals[i] != rTypeformals[i])
                            {
                                parser.SemErr(rTypeformals[i] + " parameter in return type expected");
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Control if the return type of the function is corrected. This metod is usefull only if the return type of the function 
        /// is 'fun', otherwise it is sufficient to check whether procedure.type = return type
        /// <summary>
        /// <param name="procedure">obj of the function for which is needed control the return type.</param>
        /// <param name="type">The obj of the function for which is needed to control if it match the return type of the function</param>
        public void complexReturnTypeControl(Obj procedur, Obj robj)
        {


            FRType procrtype = procedur.rtype;
            FRType returnobj = robj.rtype;
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

                if (procrtype.type != returnobj.type)
                {
                    parser.SemErr(procrtype.type + " return type expected");
                    return;
                }
            }

            while (procrtype.next != null)
            {

                if (returnobj.next == null)
                {
                    parser.SemErr(procrtype.type + " return type expected");
                }
                else
                {

                    procrtype = procrtype.next;
                    returnobj = returnobj.next;
                    Types[] aFunFormals = returnobj.formals.ToArray();
                    Types[] rTypeformals = procrtype.formals.ToArray();
                    if (procrtype.type != returnobj.type)
                    {
                        parser.SemErr(procrtype.type + " return type expected");
                        return;
                    }
                    if (aFunFormals.Length != rTypeformals.Length)
                    {
                        parser.SemErr("wrong parameter in return type");
                    }
                    else
                    {

                        for (int i = 0; i < aFunFormals.Length; i++)
                        {
                            if (aFunFormals[i] != rTypeformals[i])
                            {
                                parser.SemErr(rTypeformals[i] + " parameter in return type expected");
                                return;
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// return the owner of the current scope
        /// <summary>
        public Obj getOwner()
        {
            Obj scope = topScope;
            Obj owner = null;
            if (scope.owner != null)
            {
                owner = scope.owner;
            }
            else
            {
                while (scope.next != null)
                {
                    scope = scope.next;
                     if (scope.owner != null){
                         owner = scope.owner;
                         return owner;
                    }
                }
            }

            return owner;
        }

        public void getOwner(out Obj owner, out bool control)
        {
            Obj scope;
            scope = topScope;

            owner = null;
            control = false;
           
            if (scope.owner != null)
            {
                control = true;
                owner = scope.owner;
            }
            else { 
                while (scope.next != null)
                {
                    scope = scope.next;
                    if (scope.owner != null)
                    {
                        control = false;
                        owner = scope.owner;
                        return;
                    }
                    
                }
            }
            
           
            
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

        

        public void setAsyncControl(bool value)
        {
            if(getOwner() != null)
                getOwner().asyncControl = value;
        }

        public bool getAsyncControl(Obj obj)
        {
            if (obj.asyncControl != null)
                return obj.asyncControl;
            else
                return false;
        }

       
		

	} // end SymbolTable

} // end namespace