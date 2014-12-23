using System.Collections;
using System.Collections.Generic;



using System;

namespace APproject {



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _number = 2;
	public const int maxT = 27;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public SymbolTable   tab;
	public CodeGenerator gen;
/*--------------------------------------------------------------------------*/


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

	
	void AddOp(out Op op) {
		op = Op.ADD; 
		if (la.kind == 3) {
			Get();
		} else if (la.kind == 4) {
			Get();
			op = Op.SUB; 
		} else SynErr(28);
	}

	void Expr(out Types type) {
		Types type1; Op op; 
		SimExpr(out type);
		if (la.kind == 16 || la.kind == 17 || la.kind == 18) {
			RelOp(out op);
			SimExpr(out type1);
			if (type != type1) SemErr("incompatible types");
			type = Types.boolean; 
		}
	}

	void SimExpr(out Types type) {
		Types type1; Op op; 
		Term(out type);
		while (la.kind == 3 || la.kind == 4) {
			AddOp(out op);
			Term(out type1);
			if (type != Types.integer || type1 != Types.integer)
			 SemErr("integer type expected"); 
		}
	}

	void RelOp(out Op op) {
		op = Op.EQU; 
		if (la.kind == 16) {
			Get();
		} else if (la.kind == 17) {
			Get();
			op = Op.LSS; 
		} else if (la.kind == 18) {
			Get();
			op = Op.GTR; 
		} else SynErr(29);
	}

	void Factor(out Types type) {
		int n; Obj obj; string name; 
		type = Types.fun; 
		if (la.kind == 1) {
			Ident(out name);
			obj = tab.Find(name); type = obj.type;
			if (obj.kind != Kinds.var && obj.kind != Kinds.form) SemErr("variable expected"); 
			
		} else if (la.kind == 2) {
			Get();
			n = Convert.ToInt32(t.val);
			type = Types.integer; 
		} else if (la.kind == 4) {
			Get();
			Factor(out type);
			if (type != Types.integer) {
			 SemErr("integer type expected");
			type = Types.integer;
			}
			
		} else if (la.kind == 5) {
			Get();
			type = Types.boolean; 
		} else if (la.kind == 6) {
			Get();
			type = Types.boolean; 
		} else SynErr(30);
	}

	void Ident(out string name) {
		Expect(1);
		name = t.val; 
	}

	void MulOp(out Op op) {
		op = Op.MUL; 
		if (la.kind == 7) {
			Get();
		} else if (la.kind == 8) {
			Get();
			op = Op.DIV; 
		} else SynErr(31);
	}

	void ProcDecl() {
		Types type; string name; Obj obj; int adr; 
		Expect(9);
		if (la.kind == 1) {
			Ident(out name);
			obj = tab.NewObj(name, Kinds.proc, Types.undef); obj.adr = gen.pc;
			tab.OpenScope(); 
			Expect(10);
			while (la.kind == 1) {
				Ident(out name);
				Type(out type);
				tab.NewObj(name, Kinds.form, type); 
				while (la.kind == 11) {
					Get();
					Ident(out name);
					Type(out type);
					tab.NewObj(name, Kinds.form, type); 
				}
				tab.setformsof(obj);
				
			}
			Expect(12);
			Expect(13);
			adr = gen.pc - 2; 
			while (StartOf(1)) {
				if (la.kind == 26) {
					VarDecl();
				} else {
					Stat();
				}
			}
			Expect(14);
			tab.CloseScope(); 
		} else if (la.kind == 15) {
			Get();
			obj = tab.NewObj("Main", Kinds.proc, Types.undef); obj.adr = gen.pc;
			
			gen.progStart = gen.pc;
			tab.OpenScope(); 
			Expect(10);
			Expect(12);
			Expect(13);
			adr = gen.pc - 2; 
			while (StartOf(1)) {
				if (la.kind == 26) {
					VarDecl();
				} else {
					Stat();
				}
			}
			Expect(14);
			tab.CloseScope(); 
		} else SynErr(32);
	}

	void Type(out Types type) {
		type = Types.undef; 
		if (la.kind == 9) {
			Get();
			type = Types.fun; 
		} else if (la.kind == 24) {
			Get();
			type = Types.integer; 
		} else if (la.kind == 25) {
			Get();
			type = Types.boolean; 
		} else SynErr(33);
	}

	void VarDecl() {
		string name; ArrayList names = new ArrayList(); Types type; Types type1; Obj obj; 
		Expect(26);
		Ident(out name);
		names.Add(name); 
		if (la.kind == 11) {
			Get();
			Ident(out name);
			names.Add(name); 
			while (la.kind == 11) {
				Get();
				Ident(out name);
				names.Add(name); 
			}
			Type(out type);
			Expect(20);
			foreach(string n in names)
			{ tab.NewObj(n, Kinds.var, type); }
		} else if (la.kind == 9 || la.kind == 24 || la.kind == 25) {
			Type(out type);
			if (la.kind == 20) {
				Get();
				tab.NewObj((string)names[0], Kinds.var, type); 
			} else if (la.kind == 19) {
				Get();
				Expr(out type1);
				Expect(20);
				if (type != type1) SemErr("incompatible types");
				tab.NewObj((string)names[0], Kinds.var, type);
				obj = tab.Find(name);
			} else SynErr(34);
		} else SynErr(35);
	}

	void Stat() {
		Types type; string name; Obj obj; Queue<Types> actualTypes = new Queue<Types>();
		int adr; 
		if (la.kind == 1) {
			Ident(out name);
			obj = tab.Find(name); 
			if (la.kind == 19) {
				Get();
				if ( !(obj.kind == Kinds.var || obj.kind == Kinds.form) )
				SemErr("cannot assign to procedure"); 
				Expr(out type);
				Expect(20);
				if (type != obj.type) SemErr("incompatible types"); 
			} else if (la.kind == 10) {
				Get();
				while (StartOf(2)) {
					Expr(out type);
					actualTypes.Enqueue(type); 
					while (la.kind == 11) {
						Get();
						Expr(out type);
						actualTypes.Enqueue(type); 
					}
				}
				Expect(12);
				Expect(20);
				if (obj.kind != Kinds.proc) SemErr("object is not a procedure");
				tab.checkActualFormalTypes(obj,actualTypes);
				
			} else SynErr(36);
		} else if (la.kind == 21) {
			Get();
			Expect(10);
			Expr(out type);
			Expect(12);
			if (type != Types.boolean) SemErr("boolean type expected");
			adr = gen.pc - 2; 
			Stat();
			if (la.kind == 22) {
				Get();
				Stat();
			}
		} else if (la.kind == 23) {
			Get();
			Expect(10);
			Expr(out type);
			Expect(12);
			if (type != Types.boolean) SemErr("boolean type expected"); 
			Stat();
		} else if (la.kind == 13) {
			Get();
			while (StartOf(1)) {
				if (StartOf(3)) {
					Stat();
				} else {
					VarDecl();
				}
			}
			Expect(14);
		} else SynErr(37);
	}

	void Term(out Types type) {
		Types type1; Op op; 
		Factor(out type);
		while (la.kind == 7 || la.kind == 8) {
			MulOp(out op);
			Factor(out type1);
			if (type != Types.integer || type1 != Types.integer)
			 SemErr("integer type expected"); 
		}
	}

	void Fun() {
		tab.OpenScope(); 
		ProcDecl();
		while (la.kind == 9) {
			ProcDecl();
		}
		tab.CloseScope();
		if (gen.progStart == -1) 
		SemErr("main function never defined");
		
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Fun();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x},
		{x,T,T,x, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,T,x,T, x,x,x,x, x}

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
			case 3: s = "\"+\" expected"; break;
			case 4: s = "\"-\" expected"; break;
			case 5: s = "\"true\" expected"; break;
			case 6: s = "\"false\" expected"; break;
			case 7: s = "\"*\" expected"; break;
			case 8: s = "\"/\" expected"; break;
			case 9: s = "\"fun\" expected"; break;
			case 10: s = "\"(\" expected"; break;
			case 11: s = "\",\" expected"; break;
			case 12: s = "\")\" expected"; break;
			case 13: s = "\"{\" expected"; break;
			case 14: s = "\"}\" expected"; break;
			case 15: s = "\"main\" expected"; break;
			case 16: s = "\"==\" expected"; break;
			case 17: s = "\"<\" expected"; break;
			case 18: s = "\">\" expected"; break;
			case 19: s = "\"=\" expected"; break;
			case 20: s = "\";\" expected"; break;
			case 21: s = "\"if\" expected"; break;
			case 22: s = "\"else\" expected"; break;
			case 23: s = "\"while\" expected"; break;
			case 24: s = "\"int\" expected"; break;
			case 25: s = "\"bool\" expected"; break;
			case 26: s = "\"var\" expected"; break;
			case 27: s = "??? expected"; break;
			case 28: s = "invalid AddOp"; break;
			case 29: s = "invalid RelOp"; break;
			case 30: s = "invalid Factor"; break;
			case 31: s = "invalid MulOp"; break;
			case 32: s = "invalid ProcDecl"; break;
			case 33: s = "invalid Type"; break;
			case 34: s = "invalid VarDecl"; break;
			case 35: s = "invalid VarDecl"; break;
			case 36: s = "invalid Stat"; break;
			case 37: s = "invalid Stat"; break;

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