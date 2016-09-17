using System;
using System.Runtime.CompilerServices;

namespace BnsModTool
{
	public class OperationEventArgs : EventArgs
	{
		public Operations Operation
		{
			get;
			private set;
		}

		public OperationEventArgs(Operations operation)
		{
			this.Operation = operation;
		}
	}
}