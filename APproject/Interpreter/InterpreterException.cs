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

	public class VariableNotInitialized : InterpreterException
	{
		private string var;

		public VariableNotInitialized(string var){
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

	class DasyncException : InterpreterException {
		private string error;
		public DasyncException(string error){
			this.error = error;
		}

		public override string Message {
			get {
				return base.Message + "The server fail with: " + error;
			}
		}
	}
}

