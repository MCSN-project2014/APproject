using System;

namespace APproject
{
	public class InterpreterException : Exception
	{
		public InterpreterException(){
		}

		public override string Message {
			get {
				return "FanW@p RUNTIME EXCEPTION:\n";
			}
		}
	}

	public class VariableNotInizialized : InterpreterException
	{
		private string var;

		public VariableNotInizialized(string var){
			this.var = var;
		}

		public override string Message {
			get {
				return base.Message + "Access variable '"+ var +"' before inizialized";
			}
		}
	}

	public class ReadNotIntegerValue : InterpreterException
	{
		public ReadNotIntegerValue()
		{
		}

		public override string Message {
			get {
				return base.Message + "The read input is not a number";
			}
		}
	}
}

