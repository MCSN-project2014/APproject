﻿using System;
using APproject;

namespace funwapc
{
	class MainClass
	{
		static void Main(string[] args)
		{
			Console.WriteLine("APproject - funwapc");
			if (args.Length > 0)
			{
				ASTNode root;
				String fileName = "translated_file";   //can be args[1]  argument with some parameter ( e.g -t filename) 
				if (HelperParser.TryParse(args[0], out root)){
					FSCodeGen genFsharp = new FSCodeGen(fileName);
					genFsharp.translate(root);
				}
				Console.Read();
			}else
				Console.Write("-- No source file specified");

			//InterpreterTest.Start (InterpreterTest.testAsync);
		}
	}
}