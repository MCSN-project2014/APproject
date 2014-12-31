using System.Collections;
using System.Collections.Generic;



using System;

namespace APproject {



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _number = 2;
	public const int _print = 3;
	public const int maxT = 38;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public SymbolTable   tab;
	public ASTGenerator  gen;

/*-------------------------------------------------------------------------------------------------------------------------------------------------------*/



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Fun() {
		Node node; 
		tab.OpenScope();  
		node = new Node(Labels.Program); 
		gen.initAST(node); 
		while (la.kind == 4) {
			ProcDecl();
		}
		tab.CloseScope(); 
	}

	void ProcDecl() {
		Types type; string name; Obj proc; Obj formal; Obj obj; 
		RType rtype; Node fundecl,block,node1; Term parameter;     
		Expect(4);
		if (la.kind == 1) {
			Ident(out name);
			proc = tab.NewObj(name, Kinds.proc, Types.undef);
			fundecl = new Node(Labels.FunDecl, proc);
			
			tab.OpenScope(proc);                             
			Expect(5);
			while (la.kind == 1) {
				Ident(out name);
				Type(out type);
				formal = tab.NewObj(name, Kinds.var, type); 
				tab.addFormal(proc,formal);
				parameter = new Term(formal);
				fundecl.addChildren(parameter);           
				while (la.kind == 6) {
					Get();
					Ident(out name);
					Type(out type);
					formal = tab.NewObj(name, Kinds.var, type); 
					tab.addFormal(proc,formal); 
					parameter = new Term(formal);
					fundecl.addChildren(parameter);               
				}
			}
			Expect(7);
			RType(out rtype);
			block = new Node(Labels.Block);  
			tab.setRType(proc,rtype);
			
			Expect(8);
			while (StartOf(1)) {
				if (la.kind == 21) {
					VarDecl(out node1);
					block.addChildren(node1); 
				} else {
					Stat();
				}
			}
			Expect(9);
			fundecl.addChildren(0,block);
			gen.addChildren(fundecl);
			tab.CloseScope(); 
		} else if (la.kind == 10) {
			Get();
			obj = tab.NewObj("Main", Kinds.proc, Types.undef);
			
			
			tab.OpenScope();	
			Expect(5);
			Expect(7);
			Expect(8);
			while (StartOf(1)) {
				if (la.kind == 21) {
					VarDecl(out node1);
				} else {
					Stat();
				}
			}
			Expect(9);
			tab.CloseScope(); 
		} else SynErr(39);
	}

	void Ident(out string name) {
		Expect(1);
		name = t.val; 
	}

	void Type(out Types type) {
		type = Types.undef; 
		if (la.kind == 4) {
			Get();
			type = Types.fun; 
		} else if (la.kind == 25) {
			Get();
			type = Types.integer; 
		} else if (la.kind == 26) {
			Get();
			type = Types.boolean; 
		} else SynErr(40);
	}

	void RType(out RType rtype) {
		Queue<Types> formals; Types type; 
		RType rtype1;	
		rtype = new RType(); 
		if (la.kind == 4) {
			Get();
			rtype.type = Types.fun;
			formals = new Queue<Types>(); 
			Expect(5);
			while (la.kind == 4 || la.kind == 25 || la.kind == 26) {
				Type(out type);
				formals.Enqueue(type); 
				while (la.kind == 6) {
					Get();
					Type(out type);
					formals.Enqueue(type); 
				}
			}
			Expect(7);
			RType(out rtype1);
			rtype1.formals = formals;
			rtype.next = rtype1;
		} else if (la.kind == 25) {
			Get();
			rtype.type = Types.integer; 
		} else if (la.kind == 26) {
			Get();
			rtype.type = Types.boolean; 
		} else SynErr(41);
	}

	void VarDecl(out Node node) {
		string name; ArrayList names = new ArrayList(); 
		Types type; Types type1; 
		Term term; Obj obj; ASTNode exprnode;
		Expect(21);
		Ident(out name);
		node =  new Node (Labels.AssigDecl);
		names.Add(name); 
		if (la.kind == 6) {
			Get();
			Ident(out name);
			names.Add(name); 
			while (la.kind == 6) {
				Get();
				Ident(out name);
				names.Add(name); 
			}
			Type(out type);
			Expect(14);
			foreach(string n in names)
			{
			obj = tab.NewObj(n, Kinds.var, type);
			} 
			
		} else if (la.kind == 4 || la.kind == 25 || la.kind == 26) {
			Type(out type);
			if (la.kind == 14) {
				Get();
				obj = tab.NewObj((string)names[0], Kinds.var, type);
				node =  new Node (Labels.Decl);
				term =  new Term (obj);
				node.addChildren(term);                             
			} else if (la.kind == 11) {
				Get();
				if (la.kind == 15) {
					Get();
					Expect(8);
					Expect(9);
					Expect(14);
					if(type != Types.integer)
					SemErr("incompatible types"); 
					obj = tab.NewObj((string)names[0], Kinds.var, type);
					node =  new Node (Labels.AssigDecl);
					term =  new Term (obj);
					Node readln = new Node(Labels.Read);
					node.addChildren(term);
					node.addChildren(readln); 
				} else if (StartOf(2)) {
					CompleteExpr(out type1, out exprnode);
					Expect(14);
					if (type != type1) 
					SemErr("incompatible types");
					obj = tab.NewObj((string)names[0], Kinds.var, type); 
					node =  new Node (Labels.Decl);
					term =  new Term (obj);
					node.addChildren(term);
					node.addChildren(exprnode); 
				} else if (la.kind == 4) {
					AProcDecl(out obj);
				} else SynErr(42);
			} else SynErr(43);
		} else SynErr(44);
	}

	void Stat() {
		Types type; string name; string name1; Obj obj, obj1, robj;
		Queue<Types> actualTypes = new Queue<Types>();
		Node node; ASTNode exprnode; 
		switch (la.kind) {
		case 1: {
			Ident(out name);
			obj = tab.Find(name); 
			if (la.kind == 11) {
				Get();
				if ( obj.kind != Kinds.var )
				SemErr("cannot assign to procedure"); 
				if (la.kind == 12) {
					Get();
					Expect(8);
					Expect(13);
					Ident(out name1);
					obj1 = tab.Find(name1); 
					Expect(5);
					while (StartOf(2)) {
						CompleteExpr(out type, out exprnode);
						actualTypes.Enqueue(type); 
						while (la.kind == 6) {
							Get();
							CompleteExpr(out type, out exprnode);
							actualTypes.Enqueue(type); 
						}
					}
					Expect(7);
					Expect(9);
					Expect(14);
					if (obj1.kind != Kinds.proc) 
					SemErr("object is not a procedure");
					if (obj1.type == Types.fun)
					SemErr("wrong return type");
					tab.checkActualFormalTypes(obj1,actualTypes); 
					if(obj.type != obj1.type) 
					SemErr("incompatible types"); 
				} else if (StartOf(2)) {
					CompleteExpr(out type, out exprnode);
					Expect(14);
					if (type != obj.type)
					SemErr("incompatible types"); 
				} else if (la.kind == 4) {
					AProcDecl(out robj);
				} else if (la.kind == 15) {
					Get();
					Expect(8);
					Expect(9);
					Expect(14);
					if(obj.type != Types.integer) 
					SemErr("incompatible types"); 
				} else SynErr(45);
			} else if (la.kind == 5) {
				Get();
				while (StartOf(2)) {
					CompleteExpr(out type, out exprnode);
					actualTypes.Enqueue(type); 
					while (la.kind == 6) {
						Get();
						CompleteExpr(out type, out exprnode);
						actualTypes.Enqueue(type); 
					}
				}
				Expect(7);
				Expect(14);
				if (obj.kind != Kinds.proc || obj.rtype == null) 
				SemErr("object is not a procedure");
				else
				tab.checkActualFormalTypes(obj,actualTypes); 
			} else SynErr(46);
			break;
		}
		case 16: {
			Get();
			CompleteExpr(out type, out exprnode);
			if (type != Types.boolean) 
			SemErr("boolean type expected"); 
			Stat();
			if (la.kind == 17) {
				Get();
				Stat();
			}
			break;
		}
		case 18: {
			Get();
			CompleteExpr(out type, out exprnode);
			if (type != Types.boolean) 
			SemErr("boolean type expected"); 
			Stat();
			break;
		}
		case 19: {
			Get();
			Ident(out name);
			Expect(11);
			CompleteExpr(out type, out exprnode);
			Expect(14);
			obj=tab.Find(name);
			if (type != obj.type)
			SemErr("incompatible types"); 
			CompleteExpr(out type, out exprnode);
			Expect(14);
			if (type != Types.boolean) 
			SemErr("boolean type expected"); 
			Ident(out name1);
			Expect(11);
			CompleteExpr(out type, out exprnode);
			obj=tab.Find(name1); 
			if (type != obj.type)
			SemErr("incompatible types");
			Stat();
			break;
		}
		case 20: {
			Get();
			Expect(8);
			while (la.kind == 3) {
				Get();
			}
			Expect(9);
			Expect(14);
			break;
		}
		case 13: {
			Get();
			if (StartOf(2)) {
				CompleteExpr(out type, out exprnode);
				Expect(14);
				obj = tab.getOwner();
				obj.returnIsSet=true;
				if( obj.type != type )
				SemErr("incompatible return type"); 
			} else if (la.kind == 4) {
				AProcDecl(out robj);
				obj = tab.getOwner(); 
				obj.returnIsSet=true;
				tab.complexReturnTypeControl(obj,robj); 
			} else SynErr(47);
			break;
		}
		case 8: {
			tab.OpenScope(); 
			Get();
			while (StartOf(1)) {
				if (StartOf(3)) {
					Stat();
				} else {
					VarDecl(out node);
				}
			}
			Expect(9);
			tab.CloseScope(); 
			break;
		}
		default: SynErr(48); break;
		}
	}

	void AProcDecl(out Obj robj) {
		string name; Types type; RType rtype; Obj formal;
		Node node;
		robj = tab.NewObj(null, Kinds.proc, Types.undef);		    
		tab.OpenScope(robj); 
		Expect(4);
		Expect(5);
		while (la.kind == 1) {
			Ident(out name);
			Type(out type);
			formal = tab.NewObj(name, Kinds.var, type); 
			tab.addFormal(robj,formal); 
			while (la.kind == 6) {
				Get();
				Ident(out name);
				Type(out type);
				formal = tab.NewObj(name, Kinds.var, type);
				tab.addFormal(robj,formal); 
			}
		}
		Expect(7);
		RType(out rtype);
		tab.setRType(robj,rtype); 
		Expect(8);
		while (StartOf(1)) {
			if (la.kind == 21) {
				VarDecl(out node);
			} else {
				Stat();
			}
		}
		Expect(9);
		tab.CloseScope(); 
	}

	void CompleteExpr(out Types type, out ASTNode node) {
		Types type1; 
		Expr(out type,out node);
		if (la.kind == 34 || la.kind == 35) {
			BoolOp();
			Expr(out type1,out node);
			if (type != type1)
			SemErr("incompatible types");
			type = Types.boolean; 
		}
	}

	void Expr(out Types type,out ASTNode node) {
		Types type1; 
		SimpExpr(out type, out node);
		if (StartOf(4)) {
			RelOp();
			SimpExpr(out type1, out node);
			if (type != type1)
			SemErr("incompatible types");
			type = Types.boolean; 
		}
	}

	void BoolOp() {
		if (la.kind == 34) {
			Get();
		} else if (la.kind == 35) {
			Get();
		} else SynErr(49);
	}

	void SimpExpr(out Types type, out ASTNode node) {
		Types type1; ASTNode op, firstTerm, secondTerm; 
		Term(out type, out firstTerm);
		node = firstTerm; 
		while (la.kind == 22 || la.kind == 27) {
			AddOp(out op);
			node = op; 
			Term(out type1,out secondTerm);
			if (type != Types.integer || type1 != Types.integer)
			SemErr("integer type expected"); 
			((Node)op).addChildren(firstTerm);
			((Node)op).addChildren(secondTerm); 
		}
	}

	void RelOp() {
		switch (la.kind) {
		case 28: {
			Get();
			break;
		}
		case 29: {
			Get();
			break;
		}
		case 30: {
			Get();
			break;
		}
		case 31: {
			Get();
			break;
		}
		case 32: {
			Get();
			break;
		}
		case 33: {
			Get();
			break;
		}
		default: SynErr(50); break;
		}
	}

	void Term(out Types type, out ASTNode node) {
		Types type1; ASTNode op, firstTerm, secondTerm; 
		Factor(out type, out firstTerm);
		node = firstTerm; 
		while (la.kind == 36 || la.kind == 37) {
			MulOp(out op);
			node = op; 
			Factor(out type1,out secondTerm);
			if (type != Types.integer || type1 != Types.integer)
			SemErr("integer type expected");
			((Node)op).addChildren(firstTerm);
			((Node)op).addChildren(secondTerm); 
		}
	}

	void AddOp(out ASTNode op) {
		op = new Node(Labels.Plus); 
		if (la.kind == 27) {
			Get();
		} else if (la.kind == 22) {
			Get();
			op = new Node(Labels.Minus); 
		} else SynErr(51);
	}

	void Factor(out Types type, out ASTNode node) {
		int n; Obj obj; string name; Types type1; bool control = false;
		Queue<Types> actualTypes = new Queue<Types>(); ASTNode exprnode; 
		type = Types.undef;
		node = new Node(Labels.Block);
		if (la.kind == 1) {
			Ident(out name);
			obj = tab.Find(name); 
			type = obj.type; 
			if (la.kind == 5) {
				control = true; 
				Get();
				while (StartOf(2)) {
					CompleteExpr(out type1, out exprnode);
					actualTypes.Enqueue(type1); 
					while (la.kind == 6) {
						Get();
						CompleteExpr(out type1, out exprnode);
						actualTypes.Enqueue(type1); 
					}
				}
				Expect(7);
				if (obj.kind != Kinds.proc || obj.rtype == null) 
				SemErr("object is not a procedure");
				else
				tab.checkActualFormalTypes(obj,actualTypes); 
			}
			if (!control && obj.kind != Kinds.var)
			  SemErr("variable expected"); 
		} else if (la.kind == 2) {
			Get();
			n = Convert.ToInt32(t.val);
			node =  new Term(n);
			type = Types.integer; 
		} else if (la.kind == 22) {
			Get();
			Factor(out type,out node);
			if (type != Types.integer) 
			SemErr("integer type expected");
			type = Types.integer; 
		} else if (la.kind == 23) {
			Get();
			type = Types.boolean; 
		} else if (la.kind == 24) {
			Get();
			type = Types.boolean; 
		} else SynErr(52);
	}

	void MulOp(out ASTNode op) {
		op = new Node(Labels.Mul); 
		if (la.kind == 36) {
			Get();
		} else if (la.kind == 37) {
			Get();
			op = new Node(Labels.Div); 
		} else SynErr(53);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Fun();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, T,x,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, T,T,x,x, x,x,x,x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "number expected"; break;
			case 3: s = "print expected"; break;
			case 4: s = "\"fun\" expected"; break;
			case 5: s = "\"(\" expected"; break;
			case 6: s = "\",\" expected"; break;
			case 7: s = "\")\" expected"; break;
			case 8: s = "\"{\" expected"; break;
			case 9: s = "\"}\" expected"; break;
			case 10: s = "\"main\" expected"; break;
			case 11: s = "\"=\" expected"; break;
			case 12: s = "\"async\" expected"; break;
			case 13: s = "\"return\" expected"; break;
			case 14: s = "\";\" expected"; break;
			case 15: s = "\"readln\" expected"; break;
			case 16: s = "\"if\" expected"; break;
			case 17: s = "\"else\" expected"; break;
			case 18: s = "\"While\" expected"; break;
			case 19: s = "\"for\" expected"; break;
			case 20: s = "\"println\" expected"; break;
			case 21: s = "\"var\" expected"; break;
			case 22: s = "\"-\" expected"; break;
			case 23: s = "\"true\" expected"; break;
			case 24: s = "\"false\" expected"; break;
			case 25: s = "\"int\" expected"; break;
			case 26: s = "\"bool\" expected"; break;
			case 27: s = "\"+\" expected"; break;
			case 28: s = "\"<\" expected"; break;
			case 29: s = "\">\" expected"; break;
			case 30: s = "\"==\" expected"; break;
			case 31: s = "\"!=\" expected"; break;
			case 32: s = "\"<=\" expected"; break;
			case 33: s = "\">=\" expected"; break;
			case 34: s = "\"&&\" expected"; break;
			case 35: s = "\"||\" expected"; break;
			case 36: s = "\"*\" expected"; break;
			case 37: s = "\"/\" expected"; break;
			case 38: s = "??? expected"; break;
			case 39: s = "invalid ProcDecl"; break;
			case 40: s = "invalid Type"; break;
			case 41: s = "invalid RType"; break;
			case 42: s = "invalid VarDecl"; break;
			case 43: s = "invalid VarDecl"; break;
			case 44: s = "invalid VarDecl"; break;
			case 45: s = "invalid Stat"; break;
			case 46: s = "invalid Stat"; break;
			case 47: s = "invalid Stat"; break;
			case 48: s = "invalid Stat"; break;
			case 49: s = "invalid BoolOp"; break;
			case 50: s = "invalid RelOp"; break;
			case 51: s = "invalid AddOp"; break;
			case 52: s = "invalid Factor"; break;
			case 53: s = "invalid MulOp"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}