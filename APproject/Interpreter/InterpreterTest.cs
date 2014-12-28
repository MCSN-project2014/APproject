using System;

namespace APproject
{
	public delegate ASTNode test ();

	public static class InterpreterTest
	{
		public static void Start(test t){
			ASTNode If = t ();
			printAST (If);
			Interpreter inter = new Interpreter (If);
			inter.Start ();
			Console.ReadKey();
		}

		/// <summary>
		/// if 3>=1 {
		/// 	var a int= 3 + 7;
		/// 	println (a);
		/// }
		/// </summary>
		public static ASTNode test1 (){
			//Obj o = new Obj ();
			//o.kind = Kinds.func;
			Node main = new Node (Labels.Main);
			Node If = new Node ( Labels.If);
			main.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Term term1 = new Term(3);
			Term term2 = new Term(1);
			condition.addChildren (term1);
			condition.addChildren (term2);
			Node block = new Node (Labels.Block);
			If.addChildren (block);
			Node decAss = new Node (Labels.AssigDecl);
			block.addChildren (decAss);
			Obj varA = new Obj();
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

			return If;
		}

		/// <summary>
		/// if 3>=1 {
		/// 	var a int= 3 + 7;
		/// }
		/// var a int = 3;
		/// println (a);
		/// </summary>
		public static ASTNode test2 (){
			Node blockM = new Node (Labels.Block);

			Obj varA = new Obj();
			varA.name = "a";

			Node If = new Node ( Labels.If);
			blockM.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Term term1 = new Term(3);
			Term term2 = new Term(1);
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
		public static ASTNode test3 (){
			Node blockM = new Node (Labels.Block);

			Node decAss1 = new Node (Labels.AssigDecl);
			blockM.addChildren (decAss1);
			Obj varA = new Obj();
			varA.name = "a";
			Term num1 = new Term (3);
			decAss1.addChildren (new Term (varA));
			decAss1.addChildren (num1);

			Node If = new Node (Labels.If);
			blockM.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Term term1 = new Term(3);
			Term term2 = new Term(1);
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
		public static ASTNode testWhile (){
			Node blockM = new Node (Labels.Block);

			Node decAssI = new Node (Labels.AssigDecl);
			blockM.addChildren (decAssI);
			Obj varI = new Obj();
			varI.name = "i";
			Term initI = new Term(0);
			decAssI.addChildren (new Term(varI));
			decAssI.addChildren (initI);

			Node While = new Node (Labels.While);
			blockM.addChildren (While);

			Node condition = new Node (Labels.Lte);
			While.addChildren (condition);
			Term term1 = new Term(10);
			condition.addChildren (new Term(varI));
			condition.addChildren (term1);

			Node block = new Node(Labels.Block);
			While.addChildren(block);

			Node print = new Node (Labels.Print);
			block.addChildren (print);
			print.addChildren (new Term (varI));

			Node ass = new Node (Labels.Assig);
			block.addChildren (ass);
			Node plus = new Node (Labels.Plus);
			ass.addChildren (new Term(varI));
			plus.addChildren (new Term (varI));
			plus.addChildren (new Term (1));
			ass.addChildren (plus);

			return blockM;
		}
		public static void printAST(ASTNode node){
			if (node != null){
				if (node.isTerminal())
					Console.WriteLine (node);
				else{
					Console.WriteLine (node);
					foreach (ASTNode n in node.children) 
						printAST (n);
				}
			}
		}
	}
}

