using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BnsModTool
{
	internal class ManualClass
	{
		private static MainForm mainForm;

		private static BackgroundWorker exportWorker;

		private static BackgroundWorker buildWorker;

		public ManualClass()
		{
		}

		private static void Build(string outputFolderName, string sourceFileNameX, string sourceFileNameY, string sourceFileNameZ, string targetFileNameX, string targetFileNameY, string targetFileNameZ, string sourceObjectID, string targetObjectID)
		{
			string str;
			bool flag;
			bool flag1;
			bool flag2;
            ManualClass.buildWorker.ReportProgress(0, new Report((object x) => LogClass.AppendLine(string.Concat("Start to manual conversion", DateTime.Now.ToString()), true), null));
			if (string.IsNullOrEmpty(sourceFileNameX) && !string.IsNullOrEmpty(targetFileNameX) || !string.IsNullOrEmpty(sourceFileNameX) && string.IsNullOrEmpty(targetFileNameX))
			{
				flag = false;
			}
			else if (string.IsNullOrEmpty(sourceFileNameX) || string.IsNullOrEmpty(targetFileNameX))
			{
				flag = true;
			}
			else
			{
				flag = (File.Exists(sourceFileNameX) ? true : !File.Exists(targetFileNameX));
			}
			if (!flag)
			{
                throw new Exception("Source file X and target file X do not create the relation, can not be converted.");
			}
			if (string.IsNullOrEmpty(sourceFileNameY) && !string.IsNullOrEmpty(targetFileNameY) || !string.IsNullOrEmpty(sourceFileNameY) && string.IsNullOrEmpty(targetFileNameY))
			{
				flag1 = false;
			}
			else if (string.IsNullOrEmpty(sourceFileNameY) || string.IsNullOrEmpty(targetFileNameY))
			{
				flag1 = true;
			}
			else
			{
				flag1 = (File.Exists(sourceFileNameY) ? true : !File.Exists(targetFileNameY));
			}
			if (!flag1)
			{
                throw new Exception("Source file Y and target file Y do not create the relation, can not be converted.");
			}
			if (string.IsNullOrEmpty(sourceFileNameZ) && !string.IsNullOrEmpty(targetFileNameZ) || !string.IsNullOrEmpty(sourceFileNameZ) && string.IsNullOrEmpty(targetFileNameZ))
			{
				flag2 = false;
			}
			else if (string.IsNullOrEmpty(sourceFileNameZ) || string.IsNullOrEmpty(targetFileNameZ))
			{
				flag2 = true;
			}
			else
			{
				flag2 = (File.Exists(sourceFileNameZ) ? true : !File.Exists(targetFileNameZ));
			}
			if (!flag2)
			{
                throw new Exception("Source file Z and target file Z do not create the relation, can not be converted.");
			}
			if ((string.IsNullOrEmpty(sourceObjectID) || string.IsNullOrEmpty(targetObjectID) ? true : sourceObjectID == targetObjectID))
			{
                throw new Exception("Source model number and the model number is not set target correspondence, can not be converted.");
			}
			string str1 = Path.Combine(outputFolderName, "Backup source model");
			if (!Directory.Exists(str1))
			{
				Directory.CreateDirectory(str1);
			}
			if (!string.IsNullOrEmpty(sourceFileNameX))
			{
				File.Copy(sourceFileNameX, Path.Combine(str1, Path.GetFileName(sourceFileNameX)), true);
				str = Path.Combine(outputFolderName, Path.GetFileName(sourceFileNameX));
				if (File.Exists(str))
				{
					File.Delete(str);
				}
			}
			if (!string.IsNullOrEmpty(sourceFileNameY))
			{
				File.Copy(sourceFileNameY, Path.Combine(str1, Path.GetFileName(sourceFileNameY)), true);
				str = Path.Combine(outputFolderName, Path.GetFileName(sourceFileNameY));
				if (File.Exists(str))
				{
					File.Delete(str);
				}
			}
			if (!string.IsNullOrEmpty(sourceFileNameZ))
			{
				File.Copy(sourceFileNameZ, Path.Combine(str1, Path.GetFileName(sourceFileNameZ)), true);
				str = Path.Combine(outputFolderName, Path.GetFileName(sourceFileNameZ));
				if (File.Exists(str))
				{
					File.Delete(str);
				}
			}
			string str2 = null;
			string str3 = null;
			string str4 = null;
			if (!string.IsNullOrEmpty(targetFileNameX))
			{
				str2 = Path.Combine(outputFolderName, Path.GetFileName(targetFileNameX));
				File.Copy(targetFileNameX, str2, true);
			}
			if (!string.IsNullOrEmpty(targetFileNameY))
			{
				str3 = Path.Combine(outputFolderName, Path.GetFileName(targetFileNameY));
				File.Copy(targetFileNameY, str3, true);
			}
			if (!string.IsNullOrEmpty(targetFileNameZ))
			{
				str4 = Path.Combine(outputFolderName, Path.GetFileName(targetFileNameZ));
				File.Copy(targetFileNameZ, str4, true);
			}
			if (str2 != null)
			{
				ManualClass.Replace(str2, sourceFileNameX, sourceFileNameX, sourceFileNameY, sourceFileNameZ, targetFileNameX, targetFileNameY, targetFileNameZ, sourceObjectID, targetObjectID);
			}
			if (str3 != null)
			{
				ManualClass.Replace(str3, sourceFileNameY, sourceFileNameX, sourceFileNameY, sourceFileNameZ, targetFileNameX, targetFileNameY, targetFileNameZ, sourceObjectID, targetObjectID);
			}
			if (str4 != null)
			{
				ManualClass.Replace(str4, sourceFileNameZ, sourceFileNameX, sourceFileNameY, sourceFileNameZ, targetFileNameX, targetFileNameY, targetFileNameZ, sourceObjectID, targetObjectID);
			}
		}

		internal static void BuildAsync(string outputFolderName, string sourceFileNameX, string sourceFileNameY, string sourceFileNameZ, string targetFileNameX, string targetFileNameY, string targetFileNameZ, string sourceObjectID, string targetObjectID)
		{
            if (MessageBox.Show("Start manual model transformation? Conversion error may cause the following problems:\n\n1Source model and the target model of different kinds into the game there will not read characters models.\n2 Source file and target file relation control errors and model number error, any game will cause an exception error.", "Ask before conversion", MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				ManualClass.mainForm.OnDoOperation(Operations.ManualAction);
				BackgroundWorker backgroundWorker = ManualClass.buildWorker;
				object[] objArray = new object[] { outputFolderName, sourceFileNameX, sourceFileNameY, sourceFileNameZ, targetFileNameX, targetFileNameY, targetFileNameZ, sourceObjectID, targetObjectID };
				backgroundWorker.RunWorkerAsync(objArray);
			}
		}

		private static void buildWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string argument = ((object[])e.Argument)[0] as string;
			string str = ((object[])e.Argument)[1] as string;
			string argument1 = ((object[])e.Argument)[2] as string;
			string str1 = ((object[])e.Argument)[3] as string;
			string argument2 = ((object[])e.Argument)[4] as string;
			string str2 = ((object[])e.Argument)[5] as string;
			string argument3 = ((object[])e.Argument)[6] as string;
			string str3 = ((object[])e.Argument)[7] as string;
			string argument4 = ((object[])e.Argument)[8] as string;
			ManualClass.Build(argument, str, argument1, str1, argument2, str2, argument3, str3, argument4);
		}

		private static void buildWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState is Report)
			{
				Report userState = (Report)e.UserState;
				userState.Action(userState.Argument);
			}
		}

		private static void buildWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			ManualClass.mainForm.OnDoOperation(Operations.None);
			DateTime now = DateTime.Now;
            LogClass.AppendLine(string.Concat("Manual conversion completed", now.ToString()), false);
			if (e.Error == null)
			{
                MessageBox.Show("Conversion is complete.");
			}
			else
			{
				LogClass.AppendLine(string.Concat("Tracking the following error:\r\n", e.Error.ToString()), false);
				MessageBox.Show(e.Error.Message);
			}
		}

		private static void ExportFile(string outputFolderName, string exportFileFullName, bool isSource)
		{
			string str;
            ManualClass.exportWorker.ReportProgress(0, new Report((object x) => LogClass.AppendLine(string.Concat("Start manual Unpack", DateTime.Now.ToString()), true), null));
			if (!Directory.Exists(outputFolderName))
			{
				throw new Exception(string.Concat("Export folder\"", outputFolderName, "\"does not exist, Please fix"));
			}
			string str1 = Path.Combine(outputFolderName, (isSource ? "Source" : "Target"));
			if (!Directory.Exists(str1))
			{
				Directory.CreateDirectory(str1);
			}
			UEViewer uEViewer = new UEViewer(exportFileFullName, str1, Application.StartupPath);
			uEViewer.Unpack(out str);
			ManualClass.exportWorker.ReportProgress(0, new Report((object x) => LogClass.AppendLine((string)x, false), str));
		}

		internal static void ExportFileAsync(string outputFolderName, string exportFileFullName, bool isSource)
		{
			ManualClass.mainForm.OnDoOperation(Operations.ManualAction);
			BackgroundWorker backgroundWorker = ManualClass.exportWorker;
			object[] objArray = new object[] { outputFolderName, exportFileFullName, isSource };
			backgroundWorker.RunWorkerAsync(objArray);
		}

		private static void exportWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string argument = ((object[])e.Argument)[0] as string;
			string str = ((object[])e.Argument)[1] as string;
			bool flag = (bool)((object[])e.Argument)[2];
			ManualClass.ExportFile(argument, str, flag);
		}

		private static void exportWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState is Report)
			{
				Report userState = (Report)e.UserState;
				userState.Action(userState.Argument);
			}
		}

		private static void exportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			ManualClass.mainForm.OnDoOperation(Operations.None);
			DateTime now = DateTime.Now;
            LogClass.AppendLine(string.Concat("Unpack completed manually", now.ToString()), false);
			if (e.Error == null)
			{
                MessageBox.Show("Unpack complete.");
			}
			else
			{
                LogClass.AppendLine(string.Concat("Tracking the following error:\r\n", e.Error.ToString()), false);
				MessageBox.Show(e.Error.Message);
			}
		}

		internal static void Initialize(MainForm form)
		{
			ManualClass.mainForm = form;
			ManualClass.mainForm.DoOperation += new EventHandler<OperationEventArgs>(ManualClass.mainForm_DoOperation);
			ManualClass.exportWorker = new BackgroundWorker()
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			ManualClass.exportWorker.DoWork += new DoWorkEventHandler(ManualClass.exportWorker_DoWork);
			ManualClass.exportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ManualClass.exportWorker_RunWorkerCompleted);
			ManualClass.exportWorker.ProgressChanged += new ProgressChangedEventHandler(ManualClass.exportWorker_ProgressChanged);
			ManualClass.buildWorker = new BackgroundWorker()
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			ManualClass.buildWorker.DoWork += new DoWorkEventHandler(ManualClass.buildWorker_DoWork);
			ManualClass.buildWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ManualClass.buildWorker_RunWorkerCompleted);
			ManualClass.buildWorker.ProgressChanged += new ProgressChangedEventHandler(ManualClass.buildWorker_ProgressChanged);
		}

		private static void mainForm_DoOperation(object sender, OperationEventArgs e)
		{
			if ((e.Operation != Operations.LeaveSettings ? true : Directory.Exists(ManualClass.mainForm.txtModelFolder.Text)))
			{
				switch (e.Operation)
				{
					case Operations.LeaveSettings:
					case Operations.RefreshAnalysisList:
					case Operations.CancelRefreshanalysisList:
					case Operations.LookModel:
					case Operations.BeginAnalysis:
					case Operations.CancelAnalysis:
					case Operations.DragAnalysis:
					case Operations.CurrentAnalysis:
					case Operations.RefreshReplaceList:
					case Operations.BeginReplace:
					case Operations.CancelReplace:
					{
						ManualClass.SetControlState(true, true);
						break;
					}
					case Operations.ManualAction:
					{
						ManualClass.SetControlState(true, false);
						break;
					}
					case Operations.None:
					{
						ManualClass.SetControlState(false, true);
						break;
					}
				}
			}
		}

		private static void Replace(string fileFullName, string sourceFileFullName, string sourceFileNameX, string sourceFileNameY, string sourceFileNameZ, string targetFileNameX, string targetFileNameY, string targetFileNameZ, string sourceObjectID, string targetObjectID)
		{
			string fileNameWithoutExtension = null;
			string str = null;
			string fileNameWithoutExtension1 = null;
			string str1 = null;
			string fileNameWithoutExtension2 = null;
			string str2 = null;
			if (!string.IsNullOrEmpty(sourceFileNameX))
			{
				fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFileNameX);
			}
			if (!string.IsNullOrEmpty(sourceFileNameY))
			{
				str = Path.GetFileNameWithoutExtension(sourceFileNameY);
			}
			if (!string.IsNullOrEmpty(sourceFileNameZ))
			{
				fileNameWithoutExtension1 = Path.GetFileNameWithoutExtension(sourceFileNameZ);
			}
			if (!string.IsNullOrEmpty(targetFileNameX))
			{
				str1 = Path.GetFileNameWithoutExtension(targetFileNameX);
			}
			if (!string.IsNullOrEmpty(targetFileNameY))
			{
				fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(targetFileNameY);
			}
			if (!string.IsNullOrEmpty(targetFileNameZ))
			{
				str2 = Path.GetFileNameWithoutExtension(targetFileNameZ);
			}
			FileInfo fileInfo = new FileInfo(fileFullName);
			FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			byte[] numArray = new byte[checked((int)fileStream.Length)];
           //byte[] numArray = new byte[fileStream.Length];
			fileStream.Read(numArray, 0, (int)numArray.Length);
			if ((fileNameWithoutExtension == null ? false : str1 != null))
			{
				numArray = ReplaceClass.ReplaceWithByte(ref numArray, str1, fileNameWithoutExtension);
			}
			if ((str == null ? false : fileNameWithoutExtension2 != null))
			{
				numArray = ReplaceClass.ReplaceWithByte(ref numArray, fileNameWithoutExtension2, str);
			}
			if ((fileNameWithoutExtension1 == null ? false : str2 != null))
			{
				numArray = ReplaceClass.ReplaceWithByte(ref numArray, str2, fileNameWithoutExtension1);
			}
			if ((sourceObjectID == null ? false : targetObjectID != null))
			{
				numArray = ReplaceClass.ReplaceWithByte(ref numArray, targetObjectID, sourceObjectID);
			}
			fileStream.Position = 0L;
			fileStream.Write(numArray, 0, (int)numArray.Length);
			fileStream.Flush();
			fileStream.Close();
			string str3 = Path.Combine(Path.GetDirectoryName(fileFullName), Path.GetFileName(sourceFileFullName));
			fileInfo.MoveTo(str3);
		}

		private static void SetControlState(bool isBusy, bool isOther)
		{
			TextBox textBox = ManualClass.mainForm.txtManualExportFolderName;
			TextBox textBox1 = ManualClass.mainForm.txtManualSourceFileNameX;
			TextBox textBox2 = ManualClass.mainForm.txtManualSourceFileNameY;
			TextBox textBox3 = ManualClass.mainForm.txtManualSourceFileNameZ;
			TextBox textBox4 = ManualClass.mainForm.txtManualSourceObjectID;
			TextBox textBox5 = ManualClass.mainForm.txtManualTargetFileNameX;
			TextBox textBox6 = ManualClass.mainForm.txtManualTargetFileNameY;
			TextBox textBox7 = ManualClass.mainForm.txtManualTargetFileNameZ;
			TextBox textBox8 = ManualClass.mainForm.txtManualTargetObjectID;
			Button button = ManualClass.mainForm.btnManualBuild;
			Button button1 = ManualClass.mainForm.btnManualExportSourceX;
			Button button2 = ManualClass.mainForm.btnManualExportSourceY;
			Button button3 = ManualClass.mainForm.btnManualExportSourceZ;
			Button button4 = ManualClass.mainForm.btnManualExportTargetX;
			Button button5 = ManualClass.mainForm.btnManualExportTargetY;
			Button button6 = ManualClass.mainForm.btnManualExportTargetZ;
			Button button7 = ManualClass.mainForm.btnManualOpenExportFolder;
			Button button8 = ManualClass.mainForm.btnManualSearchExportFolderName;
			Button button9 = ManualClass.mainForm.btnManualSearchSourceX;
			Button button10 = ManualClass.mainForm.btnManualSearchSourceY;
			Button button11 = ManualClass.mainForm.btnManualSearchSourceZ;
			Button button12 = ManualClass.mainForm.btnManualSearchTargetX;
			Button button13 = ManualClass.mainForm.btnManualSearchTargetY;
			bool flag = !isBusy;
			bool flag1 = flag;
			ManualClass.mainForm.btnManualSearchTargetZ.Enabled = flag;
			bool flag2 = flag1;
			flag1 = flag2;
			button13.Enabled = flag2;
			bool flag3 = flag1;
			flag1 = flag3;
			button12.Enabled = flag3;
			bool flag4 = flag1;
			flag1 = flag4;
			button11.Enabled = flag4;
			bool flag5 = flag1;
			flag1 = flag5;
			button10.Enabled = flag5;
			bool flag6 = flag1;
			flag1 = flag6;
			button9.Enabled = flag6;
			bool flag7 = flag1;
			flag1 = flag7;
			button8.Enabled = flag7;
			bool flag8 = flag1;
			flag1 = flag8;
			button7.Enabled = flag8;
			bool flag9 = flag1;
			flag1 = flag9;
			button6.Enabled = flag9;
			bool flag10 = flag1;
			flag1 = flag10;
			button5.Enabled = flag10;
			bool flag11 = flag1;
			flag1 = flag11;
			button4.Enabled = flag11;
			bool flag12 = flag1;
			flag1 = flag12;
			button3.Enabled = flag12;
			bool flag13 = flag1;
			flag1 = flag13;
			button2.Enabled = flag13;
			bool flag14 = flag1;
			flag1 = flag14;
			button1.Enabled = flag14;
			bool flag15 = flag1;
			flag1 = flag15;
			button.Enabled = flag15;
			bool flag16 = flag1;
			flag1 = flag16;
			textBox8.Enabled = flag16;
			bool flag17 = flag1;
			flag1 = flag17;
			textBox7.Enabled = flag17;
			bool flag18 = flag1;
			flag1 = flag18;
			textBox6.Enabled = flag18;
			bool flag19 = flag1;
			flag1 = flag19;
			textBox5.Enabled = flag19;
			bool flag20 = flag1;
			flag1 = flag20;
			textBox4.Enabled = flag20;
			bool flag21 = flag1;
			flag1 = flag21;
			textBox3.Enabled = flag21;
			bool flag22 = flag1;
			flag1 = flag22;
			textBox2.Enabled = flag22;
			bool flag23 = flag1;
			flag1 = flag23;
			textBox1.Enabled = flag23;
			textBox.Enabled = flag1;
			if (isOther)
			{
				ManualClass.mainForm.pbManual.Style = ProgressBarStyle.Blocks;
			}
			else
			{
				ManualClass.mainForm.pbManual.Style = (isBusy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks);
			}
		}
	}
}