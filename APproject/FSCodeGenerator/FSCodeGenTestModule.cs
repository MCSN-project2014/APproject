using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace APproject.FSCodeGenerator
{
    class FSCodeGenTestModule
    {

        static public Node createAST()
        {    /** create a sample AST for:
              * main-> if -> Cond  -> term1 
              *                    -> term2
              *           -> Return -> term 3
              * */
      
			Node main = new Node (Labels.Main);
			Node If = new Node ( Labels.If);
			main.addChildren (If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Obj term1 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			Obj term2 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			condition.addChildren (new Node(new Term(term1)));
			condition.addChildren (new Node(new Term(term2)));
			Node ret = new Node (Labels.Return);
			If.addChildren (ret);
			Obj term3 = new Obj ();
			term1.type = Types.integer;
			term1.kind = Kinds.var;
			ret.addChildren (new Node(new Term (term3)));
		 
            return main;
        }

        static public Node createAST1()
        {
            Term t1 = new Term(42);
            Term t2 = new Term(10);
            Node min = new Node(Labels.Minus);
            Node plus = new Node(Labels.Plus);
            Node NodeT1 = new Node(t1);
            Node NodeT2 = new Node(t2);

            plus.addChildren(NodeT1);
            plus.addChildren(min);
            min.addChildren(NodeT1);
            min.addChildren(NodeT2);

            return plus;

        }

        static public Node createAST2()
        {
            Term t1 = new Term(42);
            Term t2 = new Term(10);
            Node min = new Node(Labels.Minus);
            Node async = new Node(Labels.Async);
            Node NodeT1 = new Node(t1);
            Node NodeT2 = new Node(t2);

            async.addChildren(min);
            min.addChildren(NodeT1);
            min.addChildren(NodeT2);

            return async;

        }
        public void printAST(Node node)
        {
            if (node != null)
            {
                if (node.term != null)
                    Console.WriteLine(node.term);
                else
                    Console.WriteLine(node.label);
                foreach (Node n in node.getChildren())
                {
                    printAST(n);
                }
            }
        }

        //static void Main(string[] args)
        //{
        //    String fileName = "traslated_file";
        //    FSCodeGen gen = new FSCodeGen(fileName);
        //    Node root = createAST2();
        //    gen.translate(root);


        //}
    }
}
