using System;
using System.Runtime.CompilerServices;

namespace BnsModTool
{
	internal struct Report : IDisposable
	{
		internal InvokeAction Action
		{
			get;
			private set;
		}

		internal object Argument
		{
			get;
			private set;
		}

		public Report(InvokeAction action, object argument)
		{
			this = new Report()
			{
				Action = action,
				Argument = argument
			};
		}

		public void Dispose()
		{
			this.Action = null;
			this.Argument = null;
		}
	}
}