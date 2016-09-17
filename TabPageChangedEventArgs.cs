using System;
using System.Runtime.CompilerServices;

namespace BnsModTool
{
	public class TabPageChangedEventArgs : EventArgs
	{
		public int NewIndex
		{
			get;
			private set;
		}

		public int OldIndex
		{
			get;
			private set;
		}

		public TabPageChangedEventArgs(int oldIndex, int newIndex)
		{
			this.OldIndex = oldIndex;
			this.NewIndex = newIndex;
		}
	}
}