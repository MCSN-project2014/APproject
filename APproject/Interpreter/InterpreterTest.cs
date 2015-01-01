using System;

namespace APproject
{
	public delegate ASTNode test ();

	public static class InterpreterTest
	{
		public static void Start (test t)
		{
			ASTNode program = t ();
			printAST (program);
			Interpreter inter = new Interpreter (program);
			inter.Start ();
			Console.ReadKey ();
		}

		/// <summary>
		/// fun test(a int, b int){
		/// 	println(a-b);
		/// }
		/// fun main(){
		/// 	test(7,3);
		/// }
		/// </summary>
		/// <returns>The dec fun.</returns>
		public static ASTNode testDecFun ()
		{
			Node program = new Node (Labels.Program);

			Obj fun = new Obj{ name = "test" };
			Node funDec = new Node (Labels.FunDecl, fun);
			program.addChildren (funDec);
			Node block1 = new Node (Labels.Block);
			funDec.addChildren (block1);
			Obj varA = new Obj{ name = "a" };
			Obj varB = new Obj{ name = "b" };
			funDec.addChildren (new Term (varA));
			funDec.addChildren (new Term (varB));
			Node print = new Node (Labels.Print);
			block1.addChildren (print);
			Node minus = new Node (Labels.Minus);
			minus.addChildren (new Term (varA));
			minus.addChildren (new Term (varB));
			print.addChildren (minus);

			Node main = new Node (Labels.Main);
			program.addChildren (main);
			Node assDec = new Node (Labels.AssigDecl);
			main.addChildren (assDec);
			assDec.addChildren (new Term (new Obj{ name = "c" }));
			Node call = new Node (Labels.FunCall, fun);
			call.addChildren (new Term (7));
			call.addChildren (new Term (3));
			assDec.addChildren (call);

			return program;
		}

		/// <summary>
		/// fun main(){
		/// 	var a int = 5;
		/// 	var f fun = fun()int{
		/// 				return a;
		/// 				}
		/// 	print(f());
		/// 	a = 10;
		/// 	print(f());
		/// }
		/// </summary>
		/// <returns>The dec fun.</returns>
		public static ASTNode testAFun ()
		{
			Node program = new Node (Labels.Program);
			Node main = new Node (Labels.Main);
			program.addChildren (main);

			Node assDec1 = new Node (Labels.AssigDecl);
			main.addChildren (assDec1);
			var varA = new Obj{ name = "a" };
			assDec1.addChildren (new Term (varA));
			assDec1.addChildren (new Term (5));

			Node assDec2 = new Node (Labels.AssigDecl);
			main.addChildren (assDec2);
			var varF = new Obj{ name = "f" };
			assDec2.addChildren (new Term (varF));
			Node afun = new Node (Labels.Afun);
			assDec2.addChildren (afun);
			Node block = new Node (Labels.Block);
			afun.addChildren (block);
			Node ret = new Node (Labels.Return);
			block.addChildren (ret);
			ret.addChildren (new Term (varA));

			Node print1 = new Node (Labels.Print);
			main.addChildren (print1);
			Node call1 = new Node (Labels.FunCall, varF);
			print1.addChildren (call1);

			Node ass = new Node (Labels.Assig);
			main.addChildren (ass);
			ass.addChildren (new Term (varA));
			ass.addChildren (new Term (10));

			Node print2 = new Node (Labels.Print);
			main.addChildren (print2);
			Node call2 = new Node (Labels.FunCall, varF);
			print2.addChildren (call2);

			return program;
		}

		/// <summary>
		/// fun test(){
		/// 	var a int = 7;
		/// 	return fun()int{
		/// 		return a;
		/// 	}
		/// }
		/// fun main(){
		/// 	var f fun = test();
		/// 	print(f());
		/// }
		/// </summary>
		/// <returns>The dec fun.</returns>
		public static ASTNode testReturnAFun ()
		{
			Node program = new Node (Labels.Program);

			Obj testfun = new Obj{ name = "test" }; 
			Node test = new Node (Labels.FunDecl, testfun);
			program.addChildren (test);
			Node blocktest = new Node (Labels.Block);
			test.addChildren (blocktest);
			Node assDec1 = new Node (Labels.AssigDecl);
			blocktest.addChildren (assDec1);
			Obj varA = new Obj (){ name = "a" };
			assDec1.addChildren (new Term (varA));
			assDec1.addChildren (new Term (7));
			Node ret = new Node (Labels.Return);
			blocktest.addChildren (ret);
			Node afun = new Node (Labels.Afun);
			ret.addChildren (afun);
			Node blockAfun = new Node (Labels.Block);
			afun.addChildren (blockAfun);
			Node retAfun = new Node (Labels.Return);
			blockAfun.addChildren (retAfun);
			retAfun.addChildren (new Term (varA));

			Node main = new Node (Labels.Main);
			program.addChildren (main);
			Node assDec2 = new Node (Labels.AssigDecl);
			main.addChildren (assDec2);
			var varF = new Obj (){ name = "f" };
			assDec2.addChildren (new Term (varF));
			Node call1 = new Node (Labels.FunCall, testfun);
			assDec2.addChildren (call1);

			Node print1 = new Node (Labels.Print);
			main.addChildren (print1);
			Node call2 = new Node (Labels.FunCall, varF);
			print1.addChildren (call2);
			//print1.addChildren (new Term("ciao"));

			return program;
		}

		/// <summary>
		/// fun test(a int, b int){
		/// 	return a-b;
		/// }
		/// fun main(){
		/// 	var c int = async{return test(7,3)};
		/// 	print(c);
		/// }
		/// </summary>
		/// <returns>The dec fun.</returns>
		public static ASTNode testAsync ()
		{
			Node program = new Node (Labels.Program);

			Obj fun = new Obj{ name = "test" };
			Node funDec = new Node (Labels.FunDecl, fun);
			program.addChildren (funDec);
			Node block1 = new Node (Labels.Block);
			funDec.addChildren (block1);
			Obj varA = new Obj{ name = "a" };
			Obj varB = new Obj{ name = "b" };
			funDec.addChildren (new Term (varA));
			funDec.addChildren (new Term (varB));
			Node ret1 = new Node (Labels.Return);
			block1.addChildren (ret1);
			Node minus = new Node (Labels.Minus);
			ret1.addChildren (minus);
			minus.addChildren(new Term(varA));
			minus.addChildren(new Term(varB));

			Node main = new Node (Labels.Main);
			program.addChildren (main);
			Node assDec = new Node (Labels.AssigDecl);
			main.addChildren (assDec);
			var varC = new Obj{ name = "c" };
			assDec.addChildren (new Term (varC));
			Node async = new Node (Labels.Async);
			assDec.addChildren (async);
			Node call = new Node (Labels.FunCall, fun);
			async.addChildren (call);
			call.addChildren (new Term (7));
			call.addChildren (new Term (3));
			Node print = new Node (Labels.Print);
			main.addChildren (print);
			print.addChildren (new Term (varC));

			return program;
		}

		/// <summary>
		/// fun test(a int){
		/// 	if a>10{
		/// 		return a;
		/// 	}else
		/// 		return test(a+1);
		/// }
		/// fun main(){
		/// 	print(test(0));
		/// }
		/// </summary>
		/// <returns>The dec fun.</returns>
		public static ASTNode testRecursive ()
		{
			Node program = new Node (Labels.Program);

			Obj fun = new Obj{ name = "test" };
			Node funDec = new Node (Labels.FunDecl, fun);
			program.addChildren (funDec);
			Node blockfun = new Node (Labels.Block);
			funDec.addChildren (blockfun);
			Obj varA = new Obj{ name = "a" };
			funDec.addChildren (new Term (varA));
			Node If = new Node (Labels.If);
			blockfun.addChildren (If);
			Node condition = new Node (Labels.Gt);
			If.addChildren (condition);
			condition.addChildren (new Term (varA));
			condition.addChildren (new Term (10));
			Node blockTrue = new Node (Labels.Block);
			If.addChildren (blockTrue);
			Node blockFalse = new Node (Labels.Block);
			If.addChildren (blockFalse);
			Node ret1 = new Node (Labels.Return);
			blockTrue.addChildren (ret1);
			ret1.addChildren (new Term (varA));

			Node ret2 = new Node (Labels.Return);
			blockFalse.addChildren (ret2);
			Node call1 = new Node (Labels.FunCall, fun);
			ret2.addChildren (call1);
			Node plus = new Node (Labels.Plus);
			call1.addChildren (plus);
			plus.addChildren (new Term (varA));
			plus.addChildren (new Term (1));

			Node main = new Node (Labels.Main);
			program.addChildren (main);
			Node assDec = new Node (Labels.AssigDecl);
			main.addChildren (assDec);
			var varC = new Obj{ name = "c" };
			assDec.addChildren (new Term (varC));
			Node call = new Node (Labels.FunCall, fun);
			call.addChildren (new Term (0));
			assDec.addChildren (call);
			Node print = new Node (Labels.Print);
			main.addChildren (print);
			print.addChildren (new Term (varC));
			return program;
		}

		/// <summary>
		/// fun fat(a int){
		/// 	if a==1{
		/// 		return a;
		/// 	}else
		/// 		return a + test(a-1);
		/// }
		/// fun main(){
		/// 	print(fat(3));
		/// }
		/// </summary>
		/// <returns>The dec fun.</returns>
		public static ASTNode factorialRecursive ()
		{
			Node program = new Node (Labels.Program);

			Obj fun = new Obj{ name = "test" };
			Node funDec = new Node (Labels.FunDecl, fun);
			program.addChildren (funDec);
			Node blockfun = new Node (Labels.Block);
			funDec.addChildren (blockfun);
			Obj varA = new Obj{ name = "a" };
			funDec.addChildren (new Term (varA));
			Node If = new Node (Labels.If);
			blockfun.addChildren (If);
			Node condition = new Node (Labels.Eq);
			If.addChildren (condition);
			condition.addChildren (new Term (varA));
			condition.addChildren (new Term (1));
			Node blockTrue = new Node (Labels.Block);
			If.addChildren (blockTrue);
			Node blockFalse = new Node (Labels.Block);
			If.addChildren (blockFalse);
			Node ret1 = new Node (Labels.Return);
			blockTrue.addChildren (ret1);
			ret1.addChildren (new Term (varA));

			Node ret2 = new Node (Labels.Return);
			blockFalse.addChildren (ret2);
			Node mul = new Node (Labels.Mul);
			ret2.addChildren (mul);
			mul.addChildren (new Term (varA));
			Node call1 = new Node (Labels.FunCall, fun);
			mul.addChildren (call1);
			Node minus = new Node (Labels.Minus);
			call1.addChildren (minus);
			minus.addChildren (new Term (varA));
			minus.addChildren (new Term (1));

			Node main = new Node (Labels.Main);
			program.addChildren (main);
			Node assDec = new Node (Labels.AssigDecl);
			main.addChildren (assDec);
			var varC = new Obj{ name = "c" };
			assDec.addChildren (new Term (varC));
			Node call = new Node (Labels.FunCall, fun);
			call.addChildren (new Term (10));
			assDec.addChildren (call);
			Node print = new Node (Labels.Print);
			main.addChildren (print);
			print.addChildren (new Term (varC));
			return program;
		}

		/// <summary>
		/// if 3>=1 {
		/// 	var a int= 3 + 7;
		/// 	println (a);
		/// }
		/// </summary>
		public static ASTNode test1 ()
		{
			Node main = new Node (Labels.Main);
			Node If = new Node (Labels.If);
			main.addChildren (If);
			Node condition = new Node (Labels.Eq);
			If.addChildren (condition);
			Term term1 = new Term (true);
			Term term2 = new Term (true);
			condition.addChildren (term1);
			condition.addChildren (term2);
			Node block = new Node (Labels.Block);
			If.addChildren (block);
			Node decAss = new Node (Labels.AssigDecl);
			block.addChildren (decAss);
			Obj varA = new Obj ();
			varA.name = "a";
			Term num1 = new Term (3);
			Term num2 = new Term (7);
			Node plus = new Node (Labels.Plus);
			plus.addChildren (num1);
			plus.addChildren (num2);
			decAss.addChildren (new Term (varA));
			decAss.addChildren (plus);
			Node print = new Node (Labels.Print);
			block.addChildren (print);
			print.addChildren (new Term (varA));

			return main;
		}

		/// <summary>
		/// if 3>=1 {
		/// 	var a int= 3 + 7;
		/// }
		/// var a int = 3;
		/// println (a);
		/// </summary>
		public static ASTNode test2 ()
		{

			Node blockM = new Node (Labels.Main);

			Obj varA = new Obj ();
			varA.name = "a";

			Node If = new Node (Labels.If);
			blockM.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Term term1 = new Term (3);
			Term term2 = new Term (1);
			condition.addChildren (term1);
			condition.addChildren (term2);
			Node block = new Node (Labels.Block);
			If.addChildren (block);
			Node decAss2 = new Node (Labels.AssigDecl);
			block.addChildren (decAss2);
			Term num1 = new Term (3);
			Term num2 = new Term (7);
			Node plus = new Node (Labels.Plus);
			plus.addChildren (num1);
			plus.addChildren (num2);
			decAss2.addChildren (new Term (varA));
			decAss2.addChildren (plus);

			Node decAss1 = new Node (Labels.AssigDecl);
			blockM.addChildren (decAss1);
			num1 = new Term (3);
			decAss1.addChildren (new Term (varA));
			decAss1.addChildren (num1);

			Node print = new Node (Labels.Print);
			blockM.addChildren (print);
			print.addChildren (new Term (varA));

			return blockM;
		}

		/// <summary>
		/// var a int = 3;
		/// if 3>=1 {
		/// 	a = 3 + 7;
		/// }
		/// println (a);
		/// </summary>
		public static ASTNode test3 ()
		{
			Node blockM = new Node (Labels.Main);

			Node decAss1 = new Node (Labels.AssigDecl);
			blockM.addChildren (decAss1);
			Obj varA = new Obj ();
			varA.name = "a";
			Term num1 = new Term (3);
			decAss1.addChildren (new Term (varA));
			decAss1.addChildren (num1);

			Node If = new Node (Labels.If);
			blockM.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Term term1 = new Term (3);
			Term term2 = new Term (1);
			condition.addChildren (term1);
			condition.addChildren (term2);
			Node block = new Node (Labels.Block);
			If.addChildren (block);
			Node decAss2 = new Node (Labels.AssigDecl);
			block.addChildren (decAss2);
			num1 = new Term (3);
			Term num2 = new Term (7);
			Node plus = new Node (Labels.Plus);
			plus.addChildren (num1);
			plus.addChildren (num2);
			decAss2.addChildren (new Term (varA));
			decAss2.addChildren (plus);

			Node print = new Node (Labels.Print);
			blockM.addChildren (print);
			print.addChildren (new Term (varA));

			return blockM;
		}

		/// <summary>
		/// var i int = 0;
		/// while i<=10 {
		/// 	println (i);
		/// 	i = i +1;
		/// }
		/// </summary>
		public static ASTNode testWhile ()
		{
			Node blockM = new Node (Labels.Main);

			Node decAssI = new Node (Labels.AssigDecl);
			blockM.addChildren (decAssI);
			Obj varI = new Obj ();
			varI.name = "i";
			Term initI = new Term (0);
			decAssI.addChildren (new Term (varI));
			decAssI.addChildren (initI);

			Node While = new Node (Labels.While);
			blockM.addChildren (While);

			Node condition = new Node (Labels.Lte);
			While.addChildren (condition);
			Term term1 = new Term (10);
			condition.addChildren (new Term (varI));
			condition.addChildren (term1);

			Node block = new Node (Labels.Block);
			While.addChildren (block);

			Node print = new Node (Labels.Print);
			block.addChildren (print);
			print.addChildren (new Term (varI));

			Node ass = new Node (Labels.Assig);
			block.addChildren (ass);
			Node plus = new Node (Labels.Plus);
			ass.addChildren (new Term (varI));
			plus.addChildren (new Term (varI));
			plus.addChildren (new Term (1));
			ass.addChildren (plus);

			return blockM;
		}

		public static void printAST (ASTNode node)
		{
			Console.WriteLine ("\nAST PRINT:");
			PrintAST (node, "", true);
		}

		public static void PrintAST (ASTNode node, string indent, bool last)
		{
			Console.Write(indent);
			if (last)
			{
				Console.Write("\\-");
				indent += "  ";
			}
			else
			{
				Console.Write("|-");
				indent += "| ";
			}
			Console.WriteLine(node);

			if (!node.isTerminal())
				for (int i = 0; i < node.children.Count; i++)
					PrintAST(node.children[i], indent, i == node.children.Count - 1);
		}
	}
}