using System;

namespace BnsModTool
{
	internal static class SettingsClass
	{
		private static MainForm mainForm;

		internal static void Initialize(MainForm form)
		{
			SettingsClass.mainForm = form;
		}
	}
}