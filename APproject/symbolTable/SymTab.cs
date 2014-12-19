using System;
using System.Collections.Generic;

namespace APproject
{

public class Obj {  // object describing a declared name
	public string name;		// name of the object
	public int type;			// type of the object (undef for proc)
	public Obj	next;			// to next object in same scope
	public int kind;      // var, proc, scope
	public int adr;				// address in memory or start of proc
	public int level;			// nesting level; 0=global, 1=local
	public Obj locals;		// scopes: to locally declared objects
	public int nextAdr;		// scopes: next free address in this scope

    public Queue<Obj> formalParameters { get; set; }
}

public class SymbolTable {

	const int // types
		undef = 0, integer = 1, boolean = 2, fun = 3;

	const int // object kinds
        var = 0, proc = 1, func = 2, form = 3, act = 4, scope = 5;


	public int curLevel;	// nesting level of current scope
	public Obj undefObj;	// object node for erroneous symbols
	public Obj topScope;	// topmost procedure scope
	
	Parser parser;
	
	public SymbolTable(Parser parser) {
		this.parser = parser;
		topScope = null;
		curLevel = -1;
		undefObj = new Obj();
		undefObj.name  =  "undef"; undefObj.type = undef; undefObj.kind = var;
		undefObj.adr = 0; undefObj.level = 0; undefObj.next = null;
	}


	// open a new scope and make it the current scope (topScope)
	public void OpenScope () {
		Obj scop = new Obj();
        Queue<Obj> formali = new Queue<Obj>();
		scop.name = ""; scop.kind = scope;
        scop.locals = null; scop.formalParameters = formali; 
        scop.nextAdr = 0;
		scop.next = topScope; 
        topScope = scop; 
		curLevel++;
	}


	// close the current scope
	public void CloseScope () {
		topScope = topScope.next; curLevel--;
	}


	// create a new object node in the current scope
	public Obj NewObj (string name, int kind, int type) {
        Obj p, last, obj = new Obj(); 
		obj.name = name; obj.kind = kind; obj.type = type;
		obj.level = curLevel;
		p = topScope.locals; last = null;
		while (p != null) { 
			if (p.name == name) parser.SemErr("name declared twice");
			last = p; p = p.next;
		}
		if (last == null) topScope.locals = obj; else last.next = obj;
		if (kind == var) obj.adr = topScope.nextAdr++;
     
        if (kind == form)
        {
            obj.adr = topScope.nextAdr++;
            topScope.formalParameters.Enqueue(obj);
        }
		return obj;
	}

    // set formal paramiters 
    public void setformsof(Obj obj)
    {
        obj.formalParameters = topScope.formalParameters;
    }


    public void checkActualTypes(Obj obj, Queue<int> actualTypes)
    {

        if (obj.formalParameters.Count != actualTypes.Count)
            parser.SemErr("parameter expected");
        else
        {
            // Copies the entire source Queue to a new standard array.
            Object[] ArrayofFormals = obj.formalParameters.ToArray();

            for (int i = 0; i < ArrayofFormals.Length; i++)
            {
                Obj formal = (Obj) ArrayofFormals[i];
                int actType = actualTypes.Dequeue();
                if (formal.type != actType)
                {
                    if (formal.type == boolean)
                        parser.SemErr("boolean type expected");
                    if (formal.type == integer)
                        parser.SemErr("integer type expected");
                    if (formal.type == fun)
                        parser.SemErr("fun type expected");
                }
            }

        }
    }

	// search the name in all open scopes and return its object node
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