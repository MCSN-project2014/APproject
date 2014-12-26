using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace APproject.FSCodeGenerator
{
    /**
     * This class converts an AST of funW@P into the corresponding F#
     * code, simply by visiting the tree.
     * Output is written into a outputFileName.fs file, 
     * result of the translation.
     * */
    public class FSCodeGen
    {
        private StreamWriter fileWriter;

        public FSCodeGen(string outputFileName)
        {
            if (outputFileName == string.Empty)
            {
                Console.WriteLine("Invalid File Name");
            }
            else
            {
                try
                {
                    FileStream output = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fileWriter = new StreamWriter(outputFileName + ".fs");
                }
                catch (IOException)
                {
                    Console.WriteLine("Error while creating the new file " + outputFileName + ".fs");
                }
            }
        }

     

        /**
         * This method calls the necessary ones in order to translate 
         * the AST into F# code.
         * It is recursively called by all other methods.
         * Just a big switch case.
         **/
        public void translate(Node n) {
            switch (n.label)
            {
                case Labels.Main: 
                    translateMain(n);
                    break;
                case Labels.FunDecl: 
                    translateFunDecl(n);
                    break;
                case Labels.Fun: 
                    translateFun(n);
                    break;
                case Labels.If:
                    translateIf(n);
                    break;
                case Labels.While: 
                    translateWhile(n);
                    break;
                case Labels.Return: 
                    translateReturn(n);
                    break;
                case Labels.Assig: 
                    translateAssig(n);
                    break;
                case Labels.Decl: 
                    translateDecl(n);
                    break;
                case Labels.AssigDecl: 
                    translateAssigDecl(n);
                    break;
                case Labels.Print: 
                    translatePrint(n);
                    break;
                case Labels.For: 
                    translateFor(n);
                    break;
                case Labels.Async: 
                    translateAsync(n);
                    break;
                case Labels.Afun: 
                    translateAfun(n);
                    break;
                case Labels.Plus: 
                    translatePlus(n);
                    break;
                case Labels.Mul: 
                    translateMul(n);
                    break;
                case Labels.Minus: 
                    translateMinus(n);
                    break;
                case Labels.Div: 
                    translateDiv(n);
                    break;
                case Labels.Gt: 
                    translateGt(n);
                    break;
                case Labels.Gte: 
                    translateGte(n);
                    break;
                case Labels.Lt: 
                    translateLt(n);
                    break;
                case Labels.Lte:
                    translateLte(n);
                    break;
                case Labels.Eq:
                    translateEq(n);
                    break;
                default : 
                    break;
            }
            fileWriter.Close();
        }

        /**
         * This is a helper method which calls the method translate 
         * if needed or prints the name of a node if it's a terminal
         * one.
         **/
        private void translateRecursive (Node n){
             if (n.isTerminal())
                fileWriter.Write(n.term.name);
            else translate(n);
        }

        public void translateMain(Node n)
        {
            fileWriter.WriteLine(" il main non esiste");
            foreach ( Node c in n.getChildren() )
            {
                translate(c);
            }
            
        }

        public void translateFunDecl(Node n)
        {

        }

        public void translateFun(Node n)
        {

        }
    
        public void translateIf(Node n)
        {
            fileWriter.WriteLine(" if in f# ");

        }

        public void translateWhile(Node n)
        {

        }

        public void translateReturn(Node n)
        {

        }

        public void translateAssig(Node n)
        {

        }

        public void translateDecl(Node n)
        {

        }

        public void translateAssigDecl(Node n)
        {

        }

        public void translatePrint(Node n)
        {

        }

        public void translateFor(Node n)
        {

        }

        public void translateAsync(Node n)
        {
            fileWriter.WriteLine("async {");
            fileWriter.Write("\t");
            translate(n.getChildren().ElementAt(0));
            fileWriter.WriteLine("}");
        }

        public void translateAfun(Node n)
        {

        }

        public void translatePlus(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" + ");

            translateRecursive(second);

        }

        public void translateMinus(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" - ");

            translateRecursive(second);
        }

        public void translateMul(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" * ");

            translateRecursive(second);

        }

        public void translateDiv(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" / ");

            translateRecursive(second);

        }

        public void translateGt(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" > ");

            translateRecursive(second);
        }

        public void translateGte(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" >= ");

            translateRecursive(second);
        }

        public void translateLt(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" < ");

            translateRecursive(second);
        }

        public void translateLte(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" <= ");

            translateRecursive(second);
        }

        public void translateEq(Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            fileWriter.Write(" == ");

            translateRecursive(second);
        }
    }
}
