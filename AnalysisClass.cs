using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BnsModTool
{
	internal static class AnalysisClass
	{
        private static MainForm mainForm;
        private static List<Control> defaultControls;
        private static BackgroundWorker analysisWorker;
        private static BackgroundWorker refreshWorker;
        private static System.Windows.Forms.Timer timer;
        private static UEViewer ue;
        private static List<IntPtr> lostHandles;
        private static int interval;
        private static string outputModelFolderName;
        private static bool isSingleAnalysis;
        private static BackgroundWorker singleAnalysisWorker;
        private static bool userCancelAnalysis;
        private static bool userCancelRefresh;

		internal static IDataObject DropItem
		{
			get;
			set;
		}

		static AnalysisClass()
		{
			AnalysisClass.lostHandles = new List<IntPtr>();
		}

		private static bool AnalysisLog(string fileFullname, string log, out string childFolderName, out string objectID, bool[] filter)
		{
			bool flag;
			childFolderName = string.Empty;
			objectID = string.Empty;
			string str = string.Concat(Path.GetFileNameWithoutExtension(fileFullname), ".upk");
			string str1 = "Loading SkeletalMesh3 ";
			string str2 = string.Format(" from package {0}", str);
			List<int> nums = new List<int>();
			int length = 0;
			while (length < log.Length)
			{
				int num = log.IndexOf(str1, length);
				if (num < 0)
				{
					break;
				}
				length = num + str1.Length;
				nums.Add(length);
			}
			List<int>.Enumerator enumerator = nums.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = current; i < log.Length && log[i] != ' '; i++)
					{
						stringBuilder.Append(log[i]);
					}
					objectID = stringBuilder.ToString();
					if (str2 != log.Substring(current + objectID.Length, str2.Length))
					{
						continue;
					}
					string lower = objectID.ToLower();
					if (lower.Contains("jinm"))
					{
						if (filter[0])
						{
							childFolderName = "JinM";
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					else if (lower.Contains("jinf"))
					{
						if (filter[1])
						{
							childFolderName = "JinF";
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					else if (lower.Contains("gonm"))
					{
						if (filter[2])
						{
							childFolderName = "GonM";
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					else if (lower.Contains("gonf"))
					{
						if (filter[3])
						{
							childFolderName = "GonF";
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					else if (lower.Contains("lynm"))
					{
						if (filter[4])
						{
							childFolderName = "LynM";
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					else if (lower.Contains("lynf"))
					{
						if (filter[5])
						{
							childFolderName = "LynF";
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					else if (lower.Contains("kunn"))
					{
						if (filter[6])
						{
							childFolderName = "Yun";
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					else if (filter[7])
					{
						childFolderName = "Misc";
					}
					else
					{
						flag = false;
						return flag;
					}
					flag = true;
					return flag;
				}
				flag = false;
				return flag;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		private static void analysisWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string argument = (string)((object[])e.Argument)[0];
			bool flag = (bool)((object[])e.Argument)[1];
			string argument1 = (string)((object[])e.Argument)[2];
			string str1 = (string)((object[])e.Argument)[3];
			string argument2 = (string)((object[])e.Argument)[4];
			ListBox.ObjectCollection objectCollections = (ListBox.ObjectCollection)((object[])e.Argument)[5];
			bool[] flagArray = (bool[])((object[])e.Argument)[6];
			AnalysisClass.analysisWorker.ReportProgress(0, new Report((object x) => {
				AnalysisClass.mainForm.pbAnalysis.Value = 0;
				AnalysisClass.mainForm.lblCurrentAnalysisFileName.Text = string.Empty;
				AnalysisClass.mainForm.lblAnalysisProgress.Text = "0/0";
				LogClass.AppendLine(string.Concat("批量解析Start于", DateTime.Now.ToString()), true);
			}, null));
			if (!Directory.Exists(argument1))
			{
				throw new Exception(string.Concat("Export folder\"", argument1, "\"does not exist，Please re-set the correct folder."));
			}
			string str2 = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "umodel.exe");
			if (!File.Exists(str2))
			{
				throw new Exception(string.Concat("\"", str2, "\"does not exist，请自行从\"http://www.gildor.org/en/projects/umodel\"网站下载UEViewer程序并将所有文件到本程序所在文件夹。"));
			}
			AnalysisClass.TryCreateOutputAnalysisFolder(argument1);
			List<string> strs = new List<string>();
			if (!objectCollections.Contains(str1))
			{
				throw new Exception("解析Start项does not exist，不能进行解析。");
			}
			if (!objectCollections.Contains(argument2))
			{
				throw new Exception("解析结束项does not exist，不能进行解析。");
			}
			int num1 = objectCollections.IndexOf(str1);
			int num2 = objectCollections.IndexOf(argument2);
			if (num1 > num2)
			{
				throw new Exception("解析Start项在结束项之后，请重新设置解析范围。");
			}
			for (int i = num1; i < num2 + 1; i++)
			{
				string fileFullNameFromItemList = AnalysisClass.GetFileFullNameFromItemList(objectCollections[i].ToString(), argument, flag);
				strs.Add(fileFullNameFromItemList);
			}
			AnalysisClass.analysisWorker.ReportProgress(0, new Report((object x) => {
				if (x != null)
				{
					AnalysisClass.mainForm.pbAnalysis.Maximum = (int)x;
					AnalysisClass.mainForm.lblAnalysisProgress.Text = string.Concat("0/", x.ToString());
				}
			}, (object)strs.Count));
			int num3 = 0;
			while (true)
			{
				if (num3 < strs.Count)
				{
					AnalysisClass.analysisWorker.ReportProgress(0, new Report((object x) => {
						if (x != null)
						{
							int num = (int)x;
							AnalysisClass.mainForm.lblCurrentAnalysisFileName.Text = strs[num];
							AnalysisClass.mainForm.pbAnalysis.Value = num + 1;
							Label label = AnalysisClass.mainForm.lblAnalysisProgress;
							int value = AnalysisClass.mainForm.pbAnalysis.Value;
							string str = value.ToString();
							value = AnalysisClass.mainForm.pbAnalysis.Maximum;
							label.Text = string.Concat(str, "/", value.ToString());
							LogClass.AppendLine(string.Concat("正在解析文件:", strs[num3]), false);
						}
					}, (object)num3));
					DateTime now = DateTime.Now;
					AnalysisClass.ue = new UEViewer(strs[num3], argument1, Application.StartupPath);
					string imageReturnLog = null;
					string tempFileName = null;
					bool hasImage = false;
					try
					{
						imageReturnLog = AnalysisClass.ue.AnalysisToImageReturnLog();
						tempFileName = AnalysisClass.ue.TempFileName;
						hasImage = AnalysisClass.ue.HasImage;
					}
					finally
					{
						AnalysisClass.ue.Dispose();
					}
					if (hasImage)
					{
						AnalysisClass.MoveImageFile(argument1, imageReturnLog, tempFileName, strs[num3], flagArray);
					}
					if (AnalysisClass.userCancelAnalysis)
					{
						AnalysisClass.ue = null;
						AnalysisClass.lostHandles.Clear();
						break;
					}
					else
					{
						num3++;
					}
				}
				else
				{
					break;
				}
			}
		}

		private static void analysisWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState is Report)
			{
				Report userState = (Report)e.UserState;
				userState.Action(userState.Argument);
				userState.Dispose();
			}
		}

		private static void analysisWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			AnalysisClass.timer.Stop();
			DateTime now = DateTime.Now;
            LogClass.AppendLine(string.Concat("Bulk parse finished", now.ToString()), false);
			if (e.Error == null)
			{
				string str = "Finished do you want to open Export folder?";
				if (AnalysisClass.userCancelAnalysis)
				{
                    str = "Parse Stoped, do you want to open Export folder?";
				}
				if (MessageBox.Show(str, string.Empty, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					Process process = new Process();
					process.StartInfo.FileName = "explorer.exe";
					process.StartInfo.Arguments = Path.Combine(AnalysisClass.mainForm.txtOutputFolder.Text, "ImgModelLib");
					process.Start();
				}
			}
			else
			{
				MessageBox.Show(e.Error.Message);
                LogClass.AppendLine("Tracking the following error:", false);
				LogClass.AppendLine(e.Error.ToString(), false);
			}
			AnalysisClass.mainForm.OnDoOperation(Operations.None);
			AnalysisClass.userCancelAnalysis = false;
		}

		private static void BeginAnalysis()
		{
			try
			{
				AnalysisClass.isSingleAnalysis = false;
				AnalysisClass.interval = Convert.ToInt32(AnalysisClass.mainForm.txtAnalysisInterval.Text);
				AnalysisClass.outputModelFolderName = AnalysisClass.mainForm.txtOutputFolder.Text;
			}
			catch
			{
				throw new Exception(string.Concat("解析频率只能是正整数，当前值\"", AnalysisClass.mainForm.txtAnalysisInterval.Text, "\"不合法。"));
			}
			AnalysisClass.timer.Start();
			BackgroundWorker backgroundWorker = AnalysisClass.analysisWorker;
			object[] text = new object[] { AnalysisClass.mainForm.txtModelFolder.Text, AnalysisClass.mainForm.rbSetup.Checked, AnalysisClass.mainForm.txtOutputFolder.Text, AnalysisClass.mainForm.txtAnalysisBeginName.Text, AnalysisClass.mainForm.txtAnalysisEndName.Text, AnalysisClass.mainForm.lstAnalysisFileName.Items, null };
			bool[] @checked = new bool[] { AnalysisClass.mainForm.chkAnalysisJinM.Checked, AnalysisClass.mainForm.chkAnalysisJinF.Checked, AnalysisClass.mainForm.chkAnalysisGonM.Checked, AnalysisClass.mainForm.chkAnalysisGonF.Checked, AnalysisClass.mainForm.chkAnalysisLynM.Checked, AnalysisClass.mainForm.chkAnalysisLynF.Checked, AnalysisClass.mainForm.chkAnalysisKunN.Checked, AnalysisClass.mainForm.chkAnalysisOther.Checked };
			text[6] = @checked;
			backgroundWorker.RunWorkerAsync(text);
		}

		private static void BeginRefresh()
		{
			if (!AnalysisClass.refreshWorker.IsBusy)
			{
                AnalysisClass.mainForm.btnRefreshAnalysisFileName.Text = "Refresh list";
				BackgroundWorker backgroundWorker = AnalysisClass.refreshWorker;
				object[] text = new object[] { AnalysisClass.mainForm.txtModelFolder.Text, AnalysisClass.mainForm.rbSetup.Checked };
				backgroundWorker.RunWorkerAsync(text);
			}
		}

		private static void CurrentSelectedItemAnalysis()
		{
			string str;
			if (AnalysisClass.mainForm.lstAnalysisFileName.SelectedIndex >= 0)
			{
				string str1 = null;
				if (!AnalysisClass.mainForm.rbSetup.Checked)
				{
					str = AnalysisClass.mainForm.lstAnalysisFileName.SelectedItem.ToString();
					str1 = Path.Combine(AnalysisClass.mainForm.txtModelFolder.Text, string.Concat(str, ".upk"));
				}
				else
				{
					str = AnalysisClass.mainForm.lstAnalysisFileName.SelectedItem.ToString();
					if (!str.StartsWith(".\\bns\\"))
					{
						str1 = Path.Combine(AnalysisClass.mainForm.txtModelFolder.Text, 
                            //"contents\\Local\\TENCENT\\CHINESES\\CookedPC");
                            "contents\\local\\NCWEST\\ENGLISH\\CookedPC");
						str1 = Path.Combine(str1, string.Concat(str.Substring(".\\local\\".Length), ".upk"));
					}
					else
					{
						str1 = Path.Combine(AnalysisClass.mainForm.txtModelFolder.Text, "contents\\bns\\CookedPC");
						str1 = Path.Combine(str1, string.Concat(str.Substring(".\\bns\\".Length), ".upk"));
					}
				}
				if (!File.Exists(str1))
				{
					MessageBox.Show(string.Concat("文件\"", str1, "\"does not exist。"));
				}
				else
				{
					AnalysisClass.SingleAnalysis(str1);
				}
			}
			else
			{
				MessageBox.Show("没有在列表中选择项。");
			}
		}

		private static void DragDropAnalysis()
		{
			string str = ((Array)AnalysisClass.DropItem.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
			AnalysisClass.SingleAnalysis(str);
		}

		private static string GetFileFullNameFromItemList(string itemName, string rootFolder, bool isGameFolder)
		{
			string str;
			if (!isGameFolder)
			{
				str = Path.Combine(rootFolder, string.Concat(itemName, ".upk"));
			}
			else
			{
				string[] strArrays = itemName.Split(new char[] { '\\' });
				string str1 = strArrays[(int)strArrays.Length - 1];
				str = (!itemName.StartsWith(".\\bns\\") ? Path.Combine(rootFolder, string.Concat(
                    //"contents\\Local\\TENCENT\\CHINESES\\CookedPC\\", 
                    "contents\\local\\NCWEST\\ENGLISH\\CookedPC\\",
                    str1, ".upk")) : Path.Combine(rootFolder, string.Concat("contents\\bns\\CookedPC\\", str1, ".upk")));
			}
			return str;
		}

		internal static void Initialize(MainForm form)
		{
			AnalysisClass.mainForm = form;
			AnalysisClass.defaultControls = new List<Control>()
			{
				form.btnSearchFileNameWithoutExtension,
				form.txtSearchFileNameWithoutExtension,
				form.lstAnalysisFileName,
				form.txtAnalysisInterval,
				form.btnBeginAnalysis,
				form.btnAnalysisCurrentItem,
				form.chkAnalysisGonF,
				form.chkAnalysisGonM,
				form.chkAnalysisJinF,
				form.chkAnalysisJinM,
				form.chkAnalysisKunN,
				form.chkAnalysisLynF,
				form.chkAnalysisLynM,
				form.chkAnalysisOther,
				form.lblAutoAnalysisArea,
				form.txtAnalysisBeginName,
				form.txtAnalysisEndName
			};
			AnalysisClass.mainForm.DoOperation += new EventHandler<OperationEventArgs>(AnalysisClass.mainForm_DoOperation);
			AnalysisClass.analysisWorker = new BackgroundWorker()
			{
				WorkerSupportsCancellation = true,
				WorkerReportsProgress = true
			};
			AnalysisClass.analysisWorker.DoWork += new DoWorkEventHandler(AnalysisClass.analysisWorker_DoWork);
			AnalysisClass.analysisWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AnalysisClass.analysisWorker_RunWorkerCompleted);
			AnalysisClass.analysisWorker.ProgressChanged += new ProgressChangedEventHandler(AnalysisClass.analysisWorker_ProgressChanged);
			AnalysisClass.refreshWorker = new BackgroundWorker()
			{
				WorkerSupportsCancellation = true,
				WorkerReportsProgress = true
			};
			AnalysisClass.refreshWorker.DoWork += new DoWorkEventHandler(AnalysisClass.refreshWorker_DoWork);
			AnalysisClass.refreshWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AnalysisClass.refreshWorker_RunWorkerCompleted);
			AnalysisClass.refreshWorker.ProgressChanged += new ProgressChangedEventHandler(AnalysisClass.refreshWorker_ProgressChanged);
			AnalysisClass.timer = new System.Windows.Forms.Timer()
			{
				Interval = 100
			};
			AnalysisClass.timer.Tick += new EventHandler(AnalysisClass.timer_Tick);
			AnalysisClass.singleAnalysisWorker = new BackgroundWorker()
			{
				WorkerReportsProgress = true
			};
			AnalysisClass.singleAnalysisWorker.DoWork += new DoWorkEventHandler(AnalysisClass.singleAnalysisWorker_DoWork);
			AnalysisClass.singleAnalysisWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AnalysisClass.singleAnalysisWorker_RunWorkerCompleted);
			AnalysisClass.singleAnalysisWorker.ProgressChanged += new ProgressChangedEventHandler(AnalysisClass.singleAnalysisWorker_ProgressChanged);
		}

		private static void LookSelectedModel()
		{
			string str;
			if (AnalysisClass.mainForm.lstAnalysisFileName.SelectedIndex >= 0)
			{
				string str1 = null;
				if (!AnalysisClass.mainForm.rbSetup.Checked)
				{
					str = AnalysisClass.mainForm.lstAnalysisFileName.SelectedItem.ToString();
					str1 = Path.Combine(AnalysisClass.mainForm.txtModelFolder.Text, string.Concat(str, ".upk"));
				}
				else
				{
					str = AnalysisClass.mainForm.lstAnalysisFileName.SelectedItem.ToString();
					if (!str.StartsWith(".\\bns\\"))
					{
						str1 = Path.Combine(AnalysisClass.mainForm.txtModelFolder.Text, 
                            //"contents\\Local\\TENCENT\\CHINESES\\CookedPC");
                            "contents\\local\\NCWEST\\ENGLISH\\CookedPC");
						str1 = Path.Combine(str1, string.Concat(str.Substring(".\\local\\".Length), ".upk"));
					}
					else
					{
						str1 = Path.Combine(AnalysisClass.mainForm.txtModelFolder.Text, "contents\\bns\\CookedPC");
						str1 = Path.Combine(str1, string.Concat(str.Substring(".\\bns\\".Length), ".upk"));
					}
				}
				UEViewer.QuickLookModel(Application.StartupPath, str1);
			}
		}

		private static void mainForm_DoOperation(object sender, OperationEventArgs e)
		{
			if ((e.Operation != Operations.LeaveSettings ? true : Directory.Exists(AnalysisClass.mainForm.txtModelFolder.Text)))
			{
				switch (e.Operation)
				{
					case Operations.LeaveSettings:
					{
						if (AnalysisClass.analysisWorker.IsBusy)
						{
							break;
						}
						AnalysisClass.SetControlState(AnalysisStates.Refresh);
						AnalysisClass.TryCancelAnalysis();
						AnalysisClass.TryCancelRefresh();
						AnalysisClass.BeginRefresh();
						break;
					}
					case Operations.RefreshAnalysisList:
					{
						AnalysisClass.SetControlState(AnalysisStates.Refresh);
						AnalysisClass.TryCancelAnalysis();
						AnalysisClass.TryCancelRefresh();
						AnalysisClass.BeginRefresh();
						break;
					}
					case Operations.CancelRefreshanalysisList:
					{
						AnalysisClass.TryCancelRefresh();
						break;
					}
					case Operations.LookModel:
					{
						AnalysisClass.LookSelectedModel();
						break;
					}
					case Operations.BeginAnalysis:
					{
						AnalysisClass.SetControlState(AnalysisStates.Analysis);
						AnalysisClass.TryCancelAnalysis();
						AnalysisClass.TryCancelRefresh();
						AnalysisClass.BeginAnalysis();
						break;
					}
					case Operations.CancelAnalysis:
					{
						AnalysisClass.TryCancelAnalysis();
						break;
					}
					case Operations.DragAnalysis:
					{
						AnalysisClass.SetControlState(AnalysisStates.OtherWork);
						AnalysisClass.DragDropAnalysis();
						break;
					}
					case Operations.CurrentAnalysis:
					{
						AnalysisClass.SetControlState(AnalysisStates.OtherWork);
						AnalysisClass.CurrentSelectedItemAnalysis();
						break;
					}
					case Operations.BeginReplace:
					{
						AnalysisClass.SetControlState(AnalysisStates.OtherWork);
						break;
					}
					case Operations.ManualAction:
					{
						AnalysisClass.SetControlState(AnalysisStates.OtherWork);
						break;
					}
					case Operations.None:
					{
						AnalysisClass.SetControlState(AnalysisStates.None);
						break;
					}
				}
			}
		}

		private static string MoveImageFile(string outputFolderName, string log, string tempFileName, string modelFileName, bool[] filter)
		{
			string str;
			string str1;
			string str2;
			if (!AnalysisClass.AnalysisLog(modelFileName, log, out str, out str1, filter))
			{
				File.Delete(tempFileName);
				str2 = null;
			}
			else
			{
				string str3 = Path.Combine(outputFolderName, "ImgModelLib");
				string str4 = Path.Combine(str3, str);
				string str5 = string.Format("{0}+{1}.jpg", Path.GetFileNameWithoutExtension(modelFileName), str1);
				string str6 = Path.Combine(str4, Path.GetFileName(str5));
				if (File.Exists(str6))
				{
					File.Delete(str6);
				}
				File.Move(tempFileName, str6);
				str2 = str6;
			}
			return str2;
		}

		private static List<string> ReadItemListFromCustom(string folderName)
		{
			List<string> strs;
			if (!Directory.Exists(folderName))
			{
				throw new Exception(string.Concat("Model folder\"", folderName, "\"does not exist，Please re-set the correct folder."));
			}
			string[] files = Directory.GetFiles(folderName, "*.upk");
			List<string> strs1 = new List<string>();
			string[] strArrays = files;
			int num = 0;
			while (true)
			{
				if (num < (int)strArrays.Length)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(strArrays[num]);
					if (!strs1.Contains(fileNameWithoutExtension))
					{
						strs1.Add(fileNameWithoutExtension);
					}
					if (AnalysisClass.userCancelRefresh)
					{
						strs = strs1;
						break;
					}
					else
					{
						num++;
					}
				}
				else
				{
					strs = strs1;
					break;
				}
			}
			return strs;
		}

		private static List<string> ReadItemListFromGame(string folderName)
		{
			string fileNameWithoutExtension;
			List<string> strs;
			if (!Directory.Exists(folderName))
			{
				throw new Exception(string.Concat("Model folder\"", folderName, "\"does not exist，Please re-set the correct folder."));
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(folderName, "contents\\bns\\CookedPC"));
			if (!directoryInfo.Exists)
			{
				throw new Exception(string.Concat("Model folder\"", directoryInfo.FullName, "\"does not exist，Please re-set the correct folder."));
			}
			DirectoryInfo directoryInfo1 = new DirectoryInfo(Path.Combine(folderName, 
                //"contents\\Local\\TENCENT\\CHINESES\\CookedPC"));
                "contents\\local\\NCWEST\\ENGLISH\\CookedPC"));
			if (!directoryInfo1.Exists)
			{
				throw new Exception(string.Concat("Model folder\"", directoryInfo1.FullName, "\"does not exist，Please re-set the correct folder."));
			}
			FileInfo[] files = directoryInfo.GetFiles("*.upk");
			FileInfo[] fileInfoArray = directoryInfo1.GetFiles("*.upk");
			List<string> strs1 = new List<string>();
			FileInfo[] fileInfoArray1 = files;
			int num = 0;
			while (true)
			{
				if (num < (int)fileInfoArray1.Length)
				{
					fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfoArray1[num].Name);
					fileNameWithoutExtension = string.Concat(".\\bns\\", fileNameWithoutExtension);
					if (!strs1.Contains(fileNameWithoutExtension))
					{
						strs1.Add(fileNameWithoutExtension);
					}
					if (AnalysisClass.userCancelRefresh)
					{
						strs = strs1;
						break;
					}
					else
					{
						num++;
					}
				}
				else
				{
					fileInfoArray1 = fileInfoArray;
					num = 0;
					while (num < (int)fileInfoArray1.Length)
					{
						fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfoArray1[num].Name);
						fileNameWithoutExtension = string.Concat(".\\local\\", fileNameWithoutExtension);
						if (!strs1.Contains(fileNameWithoutExtension))
						{
							strs1.Add(fileNameWithoutExtension);
						}
						if (AnalysisClass.userCancelRefresh)
						{
							strs = strs1;
							return strs;
						}
						else
						{
							num++;
						}
					}
					strs = strs1;
					break;
				}
			}
			return strs;
		}

		private static void refreshWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			AnalysisClass.refreshWorker.ReportProgress(0, new Report((object x) => {
				AnalysisClass.mainForm.lstAnalysisFileName.Items.Clear();
				AnalysisClass.mainForm.panelLoadingAnalysisFileName.Visible = true;
			}, null));
			if (!(bool)((object[])e.Argument)[1])
			{
				e.Result = AnalysisClass.ReadItemListFromCustom((string)((object[])e.Argument)[0]);
			}
			else
			{
				e.Result = AnalysisClass.ReadItemListFromGame((string)((object[])e.Argument)[0]);
			}
		}

		private static void refreshWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState is Report)
			{
				Report userState = (Report)e.UserState;
				userState.Action(userState.Argument);
				userState.Dispose();
			}
		}

		private static void refreshWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			AnalysisClass.mainForm.btnRefreshAnalysisFileName.Text = "Refresh list";
			if (e.Error == null)
			{
				if (!AnalysisClass.userCancelRefresh)
				{
					foreach (string result in (List<string>)e.Result)
					{
						AnalysisClass.mainForm.lstAnalysisFileName.Items.Add(result);
					}
				}
				if (AnalysisClass.mainForm.lstAnalysisFileName.Items.Count <= 0)
				{
					TextBox textBox = AnalysisClass.mainForm.txtAnalysisBeginName;
					TextBox textBox1 = AnalysisClass.mainForm.txtAnalysisEndName;
					string empty = string.Empty;
					string str = empty;
					textBox1.Text = empty;
					textBox.Text = str;
				}
				else
				{
					AnalysisClass.mainForm.txtAnalysisBeginName.Text = AnalysisClass.mainForm.lstAnalysisFileName.Items[0].ToString();
					AnalysisClass.mainForm.txtAnalysisEndName.Text = AnalysisClass.mainForm.lstAnalysisFileName.Items[AnalysisClass.mainForm.lstAnalysisFileName.Items.Count - 1].ToString();
				}
			}
			else
			{
				MessageBox.Show(e.Error.Message);
			}
			AnalysisClass.mainForm.panelLoadingAnalysisFileName.Visible = false;
			AnalysisClass.mainForm.OnDoOperation(Operations.None);
			AnalysisClass.userCancelRefresh = false;
		}

		private static void SetControlState(AnalysisStates state)
		{
			bool flag = false;
			bool flag1 = false;
			bool flag2 = false;
			switch (state)
			{
				case AnalysisStates.Refresh:
				{
					flag1 = false;
					flag = false;
					flag2 = true;
					break;
				}
				case AnalysisStates.Analysis:
				{
					flag = false;
					flag1 = true;
					flag2 = false;
					break;
				}
				case AnalysisStates.OtherWork:
				{
					flag1 = false;
					flag = false;
					flag2 = false;
					break;
				}
				case AnalysisStates.None:
				{
					flag = true;
					flag1 = false;
					flag2 = true;
					break;
				}
			}
			foreach (Control defaultControl in AnalysisClass.defaultControls)
			{
				defaultControl.Enabled = flag;
			}
			AnalysisClass.mainForm.btnCancelAnalysis.Enabled = flag1;
			AnalysisClass.mainForm.btnRefreshAnalysisFileName.Enabled = flag2;
		}

		private static void SingleAnalysis(string fileFullName)
		{
			if (Directory.Exists(AnalysisClass.mainForm.txtOutputFolder.Text))
			{
				AnalysisClass.TryCreateOutputAnalysisFolder(AnalysisClass.mainForm.txtOutputFolder.Text);
				AnalysisClass.interval = Convert.ToInt32(AnalysisClass.mainForm.txtAnalysisInterval.Text);
				AnalysisClass.outputModelFolderName = AnalysisClass.mainForm.txtOutputFolder.Text;
				AnalysisClass.isSingleAnalysis = true;
				BackgroundWorker backgroundWorker = AnalysisClass.singleAnalysisWorker;
				object[] objArray = new object[] { fileFullName, AnalysisClass.mainForm.txtOutputFolder.Text, Application.StartupPath, null };
				bool[] @checked = new bool[] { AnalysisClass.mainForm.chkAnalysisJinM.Checked, AnalysisClass.mainForm.chkAnalysisJinF.Checked, AnalysisClass.mainForm.chkAnalysisGonM.Checked, AnalysisClass.mainForm.chkAnalysisGonF.Checked, AnalysisClass.mainForm.chkAnalysisLynM.Checked, AnalysisClass.mainForm.chkAnalysisLynF.Checked, AnalysisClass.mainForm.chkAnalysisKunN.Checked, AnalysisClass.mainForm.chkAnalysisOther.Checked };
				objArray[3] = @checked;
				backgroundWorker.RunWorkerAsync(objArray);
			}
			else
			{
				MessageBox.Show(string.Concat("Export folder\"", AnalysisClass.mainForm.txtOutputFolder.Text, "\"does not exist，Please re-set the correct folder."));
				AnalysisClass.mainForm.OnDoOperation(Operations.None);
			}
		}

		private static void singleAnalysisWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string argument = (string)((object[])e.Argument)[0];
			string str = (string)((object[])e.Argument)[1];
			string argument1 = (string)((object[])e.Argument)[2];
			bool[] flagArray = (bool[])((object[])e.Argument)[3];
			DateTime now = DateTime.Now;
			LogClass.AppendLine(string.Concat("Start single parse ", now.ToString()), true);
			AnalysisClass.ue = new UEViewer(argument, str, argument1);
			AnalysisClass.singleAnalysisWorker.ReportProgress(0, new Report((object x) => AnalysisClass.timer.Start(), null));
			string imageReturnLog = AnalysisClass.ue.AnalysisToImageReturnLog();
			object[] hasImage = new object[] { imageReturnLog, AnalysisClass.ue.HasImage, AnalysisClass.ue.TempFileName, argument, flagArray };
			e.Result = hasImage;
		}

		private static void singleAnalysisWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState is Report)
			{
				Report userState = (Report)e.UserState;
				userState.Action(userState.Argument);
			}
		}

		private static void singleAnalysisWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			AnalysisClass.timer.Stop();
			AnalysisClass.isSingleAnalysis = false;
			AnalysisClass.ue.Dispose();
			AnalysisClass.ue = null;
			AnalysisClass.SetControlState(AnalysisStates.None);
			AnalysisClass.mainForm.OnDoOperation(Operations.None);
			string result = (string)((object[])e.Result)[0];
			bool flag = (bool)((object[])e.Result)[1];
			string str = (string)((object[])e.Result)[2];
			string result1 = (string)((object[])e.Result)[3];
			bool[] flagArray = (bool[])((object[])e.Result)[4];
			LogClass.AppendLine(result, false);
			DateTime now = DateTime.Now;
			LogClass.AppendLine(string.Concat("Single parse ending", now.ToString()), false);
			if (e.Error != null)
			{
				MessageBox.Show(e.Error.Message);
				LogClass.AppendLine(string.Concat("Tracking error:", e.Error.ToString()), false);
			}
			else if (!flag)
			{
                MessageBox.Show("Failed to parse model image, The file should not be a valid model file.");
			}
			else if (MessageBox.Show("Want to save the model image?", string.Empty, MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				File.Delete(str);
			}
			else
			{
				string str1 = AnalysisClass.MoveImageFile(AnalysisClass.mainForm.txtOutputFolder.Text, result, str, result1, flagArray);
				DialogResult dialogResult = MessageBox.Show("Click 'Yes' to open the folder where the image, click 'No' to open the image.", string.Empty, MessageBoxButtons.YesNoCancel);
				if (dialogResult == DialogResult.Yes)
				{
					Process process = new Process();
					process.StartInfo.FileName = "explorer.exe";
					process.StartInfo.Arguments = Path.GetDirectoryName(str1);
					process.Start();
				}
				else if (dialogResult == DialogResult.No)
				{
					Process process1 = new Process();
					process1.StartInfo.FileName = str1;
					process1.Start();
				}
			}
		}

		private static void timer_Tick(object sender, EventArgs e)
		{
			if (AnalysisClass.ue != null)
			{
				if ((!AnalysisClass.isSingleAnalysis ? true : !AnalysisClass.ue.HasImage))
				{
					IntPtr intPtr = new IntPtr(UEViewer.FindWindow(null, "UE Viewer"));
					if ((intPtr == new IntPtr(0) ? false : !AnalysisClass.lostHandles.Contains(intPtr)))
					{
						AnalysisClass.lostHandles.Add(intPtr);
						Thread.Sleep(AnalysisClass.interval);
						Graphics graphic = Graphics.FromHwnd(intPtr);
						UEViewer.RECT rECT = new UEViewer.RECT();
						UEViewer.GetWindowRect(intPtr, ref rECT);
						int right = rECT.Right - rECT.Left;
						int bottom = rECT.Bottom - rECT.Top;
						Bitmap bitmap = new Bitmap(right - 16, bottom - 38, graphic);
						Graphics graphic1 = Graphics.FromImage(bitmap);
						IntPtr hdc = graphic.GetHdc();
						IntPtr hdc1 = graphic1.GetHdc();
						UEViewer.BitBlt(hdc1, 0, 0, right - 16, bottom - 38, hdc, 0, 0, 13369376);
						graphic.ReleaseHdc(hdc);
						graphic1.ReleaseHdc(hdc1);
						Bitmap bitmap1 = new Bitmap(bitmap, right - 16, bottom - 38);
						UEViewer uEViewer = AnalysisClass.ue;
						string str = AnalysisClass.outputModelFolderName;
						Guid guid = Guid.NewGuid();
						uEViewer.TempFileName = Path.Combine(str, string.Concat(guid.ToString(), ".jpg"));
						bitmap1.Save(AnalysisClass.ue.TempFileName, ImageFormat.Jpeg);
						AnalysisClass.ue.HasImage = true;
						if (!AnalysisClass.isSingleAnalysis)
						{
							UEViewer.SendMessageA(intPtr, 16, 0, 0);
						}
					}
				}
				else
				{
					AnalysisClass.timer.Stop();
				}
			}
		}

		private static void TryCancelAnalysis()
		{
			if (AnalysisClass.analysisWorker.IsBusy)
			{
				AnalysisClass.timer.Stop();
				AnalysisClass.analysisWorker.CancelAsync();
				AnalysisClass.userCancelAnalysis = true;
				IntPtr intPtr = new IntPtr(UEViewer.FindWindow(null, "UE Viewer"));
				if ((intPtr == new IntPtr(0) ? false : !AnalysisClass.lostHandles.Contains(intPtr)))
				{
					UEViewer.SendMessageA(intPtr, 16, 0, 0);
				}
			}
		}

		private static void TryCancelRefresh()
		{
			if (AnalysisClass.refreshWorker.IsBusy)
			{
				AnalysisClass.refreshWorker.CancelAsync();
				AnalysisClass.userCancelRefresh = true;
			}
		}

		private static void TryCreateOutputAnalysisFolder(string root)
		{
			string str = Path.Combine(root, "ImgModelLib\\JinM");
			string str1 = Path.Combine(root, "ImgModelLib\\JinF");
			string str2 = Path.Combine(root, "ImgModelLib\\GonM");
			string str3 = Path.Combine(root, "ImgModelLib\\GonF");
			string str4 = Path.Combine(root, "ImgModelLib\\LynM");
			string str5 = Path.Combine(root, "ImgModelLib\\LynF");
			string str6 = Path.Combine(root, "ImgModelLib\\Yun");
			string str7 = Path.Combine(root, "ImgModelLib\\Misc");
			string[] strArrays = new string[] { str, str1, str2, str3, str4, str5, str6, str7 };
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				string str8 = strArrays1[i];
				if (!Directory.Exists(str8))
				{
					Directory.CreateDirectory(str8);
				}
			}
		}
	}
}