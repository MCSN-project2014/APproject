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


    }
}
