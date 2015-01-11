using System;

namespace APproject
{
	public class InterpreterException : Exception
	{
		int column;
		int row;
		public InterpreterException(){
		}

		public InterpreterException(int column, int row){
			this.column = column;
			this.row = row;
		}

		public override string Message {
			get {
				return "FanW@p RUNTIME EXCEPTION:\n";
			}
		}

		public string CodePosition{
			get{return "(line: "+row+", column: "+column+")";}
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

	public class CantPrintFunction : InterpreterException
	{
		public CantPrintFunction(int column, int row):base(column, row){
		}

		public override string Message {
			get {
				return base.Message + "the value of the print is a function. "+CodePosition;
			}
		}
	}

	public class ReadNotIntegerValue : InterpreterException
	{
		public ReadNotIntegerValue (int column, int row) : base(column, row)
		{
		}

		public override string Message {
			get {
				return base.Message + "The readed input is not a number. "+CodePosition;
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

