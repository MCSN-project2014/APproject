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
				return "funW@p RUNTIME EXCEPTION:\n";
			}
		}

		public string CodePosition{
			get{return "(line: "+row+", column: "+column+")";}
		}
	}

	/*
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
	*/

	public class CantPrintFunction : InterpreterException
	{
		public CantPrintFunction(int column, int row):base(column, row){
		}

		public override string Message {
			get {
				return base.Message + "Println of a function is not allowed. "+CodePosition;
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
				return base.Message + "The inserted input is not a number. "+CodePosition;
			}
		}
	}

    public class ParameterNumberException : InterpreterException
    {
        string fun;
        public ParameterNumberException(string fun)
        {
            this.fun = fun;
        }

        public override string Message
        {
            get
            {
                return base.Message + "Wrong number of parameters in function call '" + fun + "'";
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
				return base.Message + "The server failed with: " + error;
			}
		}
	}

	class ServerConnectionException : InterpreterException {

		private string error;

		public ServerConnectionException(string error){
			this.error = error;
		}

		public override string Message {
			get {
				return base.Message + "it's not possible to connect to the server: " + error;
			}
		}
	}
}

