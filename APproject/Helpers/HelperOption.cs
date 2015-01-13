using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;

namespace APproject
{
    public static class HelperOption
    {
        public static bool verbose = false;
        public static string outputFileName = "a.fs";
        public static string inputFileName = null;
        private static bool compiler = false;
 
        private static OptionSet option = new OptionSet() {
            { "h|help",  "show this message and exit",  v => ShowHelp() },
            { "v|verbose", "Verbose, prints the abstract syntax tree of the program", v => verbose = true },
            //{ "<>", "Name of source file",  v => sourceFile = v }
            };
        
        private static OptionSet output = new OptionSet(){
                //{ "<>", "Name of source file",  v => sourceFile = v },
                 { "o=", "Specify the output file name.", v => ceckOutputFile(v) }
                 
            };

        public static bool ParseCompiler(string[] args)
        {
            compiler = true;
            try
            {
                if (args.Length == 0)
                {
                    ShowHelp();
                    return false;
                }
                else
                {
                    var tmp = output.Parse(option.Parse(args));
                    if (tmp.Count == 1){
                        inputFileName = tmp[0];
                        return true;
                    }else
                        return false;
                }
            }
            catch (OptionException e)
            {
                Console.Write("Error: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `greet --help' for more information.\n");
                return false;
            }
        }

        public static bool ParseInterpreter(string[] args)
        {
            compiler = false;
            try
            {
                if (args.Length == 0)
                {
                    ShowHelp();
                    return false;
                }
                else
                {
                    var tmp = option.Parse(args);
                    if (tmp.Count == 1)
                    {
                        inputFileName = tmp[0];
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (OptionException e)
            {
                Console.Write("Error: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `greet --help' for more information.\n");
                return false;
            }
        }

        public static void printInputValues()
        {
            Console.WriteLine("funwapc::");

            Console.WriteLine("\t Input File: {0}", inputFileName);

            Console.WriteLine("\t Ouptut File: {0} \n", outputFileName);

        }

        private static void ceckOutputFile(string v)
        {
            if (v.Contains(".fs"))
            {
                outputFileName = v;
            }
            else { outputFileName = v + ".fs"; }
        }


        private static void ShowHelp()
        {
            if (compiler)
                Console.WriteLine("Usage: funwapc [options] sourceFile.fun [-o VALUE]");
            else
                Console.WriteLine("Usage: funwapi [options] sourceFile.fun");
            Console.WriteLine();
            Console.WriteLine("Options:");
            option.WriteOptionDescriptions(Console.Out);
        }
    }
}
