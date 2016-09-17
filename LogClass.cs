using System;
using System.Windows.Forms;

namespace BnsModTool
{
	internal static class LogClass
	{
		private static MainForm mainForm;

		internal static void AppendLine(string log, bool clearBefore)
		{
			if (clearBefore)
			{
				LogClass.mainForm.txtLog.Text = string.Empty;
			}
			if (!string.IsNullOrEmpty(LogClass.mainForm.txtLog.Text))
			{
				RichTextBox richTextBox = LogClass.mainForm.txtLog;
				richTextBox.Text = string.Concat(richTextBox.Text, "\r\n");
			}
			RichTextBox richTextBox1 = LogClass.mainForm.txtLog;
			richTextBox1.Text = string.Concat(richTextBox1.Text, log);
		}

		internal static void Initialize(MainForm form)
		{
			LogClass.mainForm = form;
		}
	}
}