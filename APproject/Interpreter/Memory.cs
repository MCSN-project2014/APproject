using System;
using System.Collections.Generic;

namespace APproject
{
	public class Memory
	{
		private List<Dictionary<Obj,object>> scope;

		public Memory ()
		{
			scope = new List<Dictionary<Obj,object>>();
		}
	}
}

