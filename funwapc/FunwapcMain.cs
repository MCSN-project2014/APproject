using System;
using APproject;
using System.Linq;
using System.Collections.Generic;

namespace  funwapc
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("APproject - funwapc");
            if (HelperOption.ParseCompiler(args))
            {
                ASTNode root;
                if (HelperParser.TryParse(HelperOption.inputFileName, out root))
                {
                    if (HelperOption.verbose)
                    {
                        HelperParser.printAST(root);
                    }
                    HelperOption.printInputValues();
                    FSCodeGen genFsharp = new FSCodeGen(HelperOption.outputFileName);
                    genFsharp.translate(root);
                }
                else
                {
                    Console.WriteLine("ERROR: Can't open file " + HelperOption.inputFileName);
                }
            }
        }
           

    }

}
