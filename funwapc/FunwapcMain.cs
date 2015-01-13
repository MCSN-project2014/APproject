using System;
using APproject;
using System.Linq;
using System.Collections.Generic;
using NDesk.Options;

namespace  funwapc
{
    class MainClass
    {
        
        private bool  show_help = true;
        private string outputFileName = "a", sourceFile = null, directoryOutput = "";
        private OptionSet p;

        public void parseOption(string [] args)
        {
            p = new OptionSet() {
            // options
            { "o=", "Specify the output file name.", v => {outputFileName = v; show_help = false;} },
            { "d=", "The path directory of the output file.",  v => {directoryOutput =  v ; show_help = false;} },
            { "h|help",  "show this message and exit", 
              v => show_help = v != null },
            { "<>", "name of source file",   //default: string without options 
                v => sourceFile = v}
            };

             try
            {
               p.Parse(args);

                 
            }
            catch (OptionException e)
            {
                Console.Write("bundling: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `greet --help' for more information.\n");
                return;
            }

             if (!show_help)
             {
                 Console.WriteLine("Options:");
                 Console.WriteLine("\t Input File: {0}", sourceFile);
                 Console.WriteLine("\t Ouptut File: {0}", outputFileName);
                 Console.WriteLine("\t Ouptut directory: {0}", directoryOutput);
             }
        }

        public void ShowHelp()
        {
            Console.WriteLine("Usage: funwapc [SOURCEFILE] [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        

        public static void Main(string[] args)
        {
            MainClass mc = new MainClass();
            Console.WriteLine("APproject - funwapc");
            mc.parseOption( args );
            if (mc.show_help)
            {
                mc.ShowHelp();
                return;
            }
            ASTNode root;
            if (HelperParser.TryParse(mc.sourceFile, out root))
            {
                String pathOutputname = mc.directoryOutput + mc.outputFileName;
                FSCodeGen genFsharp = new FSCodeGen(pathOutputname);
                genFsharp.translate(root);
            }

            }
           

    }

}
