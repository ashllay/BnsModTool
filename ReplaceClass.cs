using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace BnsModTool
{
	internal static class ReplaceClass
	{
		private static MainForm mainForm;

		private static Control[] defaultControls;

		private static BackgroundWorker refreshWorker;

		private static bool userCancelRefresh;

		private static BackgroundWorker replaceWorker;

		private static bool userCancelReplace;

		private static string exportCompletedReportDir;

		private static string BackupSourceFile(string fileID, string modelFolderName, bool isGame, string exportFolderName)
		{
			string modelFileName = ReplaceClass.GetModelFileName(fileID, modelFolderName, isGame);
			string str = Path.Combine(exportFolderName, "Backup source model");
			string str1 = Path.Combine(str, string.Concat(fileID, ".upk"));
			if (File.Exists(str1))
			{
				File.Delete(str1);
			}
			File.Copy(modelFileName, str1, true);
			return str1;
		}

		private static void BeginRefresh()
		{
			if (!ReplaceClass.refreshWorker.IsBusy)
			{
				ReplaceClass.SetControlState(ReplaceStates.Refresh);
				BackgroundWorker backgroundWorker = ReplaceClass.refreshWorker;
				object[] text = new object[] { ReplaceClass.mainForm.txtOutputFolder.Text, null };
				bool[] @checked = new bool[] { ReplaceClass.mainForm.chkReplaceJinM.Checked, ReplaceClass.mainForm.chkReplaceJinF.Checked, ReplaceClass.mainForm.chkReplaceGonM.Checked, ReplaceClass.mainForm.chkReplaceGonF.Checked, ReplaceClass.mainForm.chkReplaceLynM.Checked, ReplaceClass.mainForm.chkReplaceLynF.Checked, ReplaceClass.mainForm.chkReplaceKunN.Checked, ReplaceClass.mainForm.chkReplaceOther.Checked };
				text[1] = @checked;
				backgroundWorker.RunWorkerAsync(text);
			}
		}

		private static void BeginReplace()
		{
			if (ReplaceClass.mainForm.picSourceImage.Tag == null)
			{
				MessageBox.Show("Please set the source model.");
			}
			else if (ReplaceClass.mainForm.picTargetImage.Tag != null)
			{
				string str = ReplaceClass.mainForm.picSourceImage.Tag.ToString();
				string str1 = ReplaceClass.mainForm.picTargetImage.Tag.ToString();
				if (str != str1)
				{
					string str2 = Path.Combine(Application.StartupPath, "umodel.exe");
					if (!File.Exists(str2))
					{
                        MessageBox.Show(string.Concat("\"", str2, "\"does not exist\"http://www.gildor.org/en/projects/umodel\"Download site UEViewer program and copy all the files to the Program folder."));
					}
					else if (MessageBox.Show(
                        //"是否Start自动模型转换?错误的转换可能导致下列问题:\n\n1、源模型与目标模型种类不同，进到游戏有读取人物模型时会报错退出。\n2、按照已研究出来的规律仍无法解析对照规则，需要用户自己点击“去手动转换”按钮进行手动转换。"
                        "Start automatic model transformation? Conversion error may cause the following problems:\n\n1、Source model and the target model of different types，There would be an error into the will game stop reading character models。\n2、Has come up in accordance with the law still does not parse the control rules，Require the user to click“Go to manual conversion”Button manual conversion."
                        , "Ask before convert", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
                        ReplaceClass.mainForm.btnReplaceModel.Text = "Cancel";
						ReplaceClass.SetControlState(ReplaceStates.Build);
						ReplaceClass.mainForm.pbReplace.Style = ProgressBarStyle.Marquee;
						BackgroundWorker backgroundWorker = ReplaceClass.replaceWorker;
						object[] text = new object[] { str, str1, str2, ReplaceClass.mainForm.txtOutputFolder.Text, ReplaceClass.mainForm.txtModelFolder.Text, ReplaceClass.mainForm.rbSetup.Checked };
						backgroundWorker.RunWorkerAsync(text);
					}
				}
				else
				{
                    MessageBox.Show("Source and target are the same，Can not be replaced");
				}
			}
			else
			{
                MessageBox.Show("Please set the target model.");
			}
		}

		private static string CopyTargetFile(string fileID, string modelFolderName, bool isGame, string exportFolderName)
		{
			string modelFileName = ReplaceClass.GetModelFileName(fileID, modelFolderName, isGame);
			string str = Path.Combine(exportFolderName, string.Concat(fileID, ".upk"));
			if (File.Exists(str))
			{
				File.Delete(str);
			}
			File.Copy(modelFileName, str, true);
			return str;
		}

		private static void DeleteDirAndAllFiles(string dir)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(dir);
			if (directoryInfo.Exists)
			{
				try
				{
					directoryInfo.Delete(true);
				}
				catch
				{
				}
			}
		}

		private static void DoReplace(string dirSourceName, string dirTargetName, string sourceFileID, string targetFileID, string sourceObjectID, string targetObjectID, string modelFolder, bool isGameFolder, string exportFolder)
		{
			bool flag;
			DirectoryInfo directoryInfo;
			bool flag1;
			DirectoryInfo[] directoryInfoArray;
			int num;
			DirectoryInfo directoryInfo1 = new DirectoryInfo(dirSourceName);
			DirectoryInfo directoryInfo2 = new DirectoryInfo(dirTargetName);
			string name = null;
			string str = null;
			string name1 = null;
			string str1 = null;
			string name2 = null;
			string str2 = null;
			string str3 = null;
			string str4 = null;
			DirectoryInfo[] directories = directoryInfo1.GetDirectories("SkeletalMesh3", SearchOption.AllDirectories);
			DirectoryInfo[] directories1 = directoryInfo1.GetDirectories("MaterialInstanceConstant", SearchOption.AllDirectories);
			DirectoryInfo[] directoryInfoArray1 = directoryInfo1.GetDirectories("Texture2D", SearchOption.AllDirectories);
			if ((int)directories.Length == 1)
			{
				name = directories[0].Parent.Name;
			}
			else
			{
				flag = false;
				directoryInfoArray = directories;
				num = 0;
				while (true)
				{
					if (num < (int)directoryInfoArray.Length)
					{
						directoryInfo = directoryInfoArray[num];
						if (directoryInfo.Parent.Name == sourceFileID)
						{
							name = sourceFileID;
							flag = true;
							break;
						}
						else
						{
							num++;
						}
					}
					else
					{
						break;
					}
				}
				if (!flag)
				{
					throw new Exception(
                        //"源模型在Source文件夹下的SkeletalMesh3文件夹不唯一，无法自动转换模型，请尝试手动转换。"
                        "Source Model Source folder SkeletalMesh3 folder is not unique,Can not be automatically converted model, try to manually convert.");
				}
			}
			if ((int)directories1.Length == 1)
			{
				name1 = directories1[0].Parent.Name;
			}
			else
			{
				flag = false;
				directoryInfoArray = directories1;
				num = 0;
				while (true)
				{
					if (num < (int)directoryInfoArray.Length)
					{
						directoryInfo = directoryInfoArray[num];
						if (directoryInfo.Parent.Name != sourceFileID)
						{
							name = directoryInfo.Parent.Name;
							flag = true;
							break;
						}
						else
						{
							num++;
						}
					}
					else
					{
						break;
					}
				}
				if (!flag)
				{
					throw new Exception(
                        //"源模型在Source文件夹下的MaterialInstanceConstant文件夹不唯一，无法自动转换模型，请尝试手动转换。"
                        "Source Model Source folder MaterialInstanceConstant folder is not unique, Can not be automatically converted model, try to manually convert.");
				}
			}
			if ((int)directoryInfoArray1.Length == 1)
			{
				name2 = directoryInfoArray1[0].Parent.Name;
			}
			else
			{
				flag1 = false;
				directoryInfoArray = directoryInfoArray1;
				num = 0;
				while (true)
				{
					if (num < (int)directoryInfoArray.Length)
					{
						directoryInfo = directoryInfoArray[num];
						if ((directoryInfo.Parent.Name == name ? true : directoryInfo.Parent.Name == name1) || (int)directoryInfo.GetFiles(string.Concat("*", ReplaceClass.TryGetObjectNumberWithoutRaceSex(sourceObjectID), "*.tga")).Length <= 0)
						{
							num++;
						}
						else
						{
							str2 = directoryInfo.Parent.Name;
							flag1 = true;
							break;
						}
					}
					else
					{
						break;
					}
				}
				if (!flag1)
				{
					throw new Exception(
                        //"源模型在Source文件夹下的Texture2D文件夹不唯一，无法自动转换模型，请尝试手动转换。"
                        "Source Model Source folder Texture2D folder is not unique, Can not be automatically converted model, try to manually convert.");
				}
			}
			directories = directoryInfo2.GetDirectories("SkeletalMesh3", SearchOption.AllDirectories);
			directories1 = directoryInfo2.GetDirectories("MaterialInstanceConstant", SearchOption.AllDirectories);
			directoryInfoArray1 = directoryInfo2.GetDirectories("Texture2D", SearchOption.AllDirectories);
			if ((int)directories.Length == 1)
			{
				str = directories[0].Parent.Name;
			}
			else
			{
				flag = false;
				directoryInfoArray = directories;
				num = 0;
				while (true)
				{
					if (num < (int)directoryInfoArray.Length)
					{
						directoryInfo = directoryInfoArray[num];
						if (directoryInfo.Parent.Name == sourceFileID)
						{
							name = sourceFileID;
							flag = true;
							break;
						}
						else
						{
							num++;
						}
					}
					else
					{
						break;
					}
				}
				if (!flag)
				{
					throw new Exception(
                       // "目标模型在Target文件夹下的SkeletalMesh3文件夹不唯一，无法自动转换模型，请尝试手动转换。"
                        "SkeletalMesh 3 target model file folder in Target folder is not unique, Can not be automatically converted model, try to manually convert.");
				}
			}
			if ((int)directories1.Length == 1)
			{
				str1 = directories1[0].Parent.Name;
			}
			else
			{
				flag = false;
				directoryInfoArray = directories1;
				num = 0;
				while (true)
				{
					if (num < (int)directoryInfoArray.Length)
					{
						directoryInfo = directoryInfoArray[num];
						if (directoryInfo.Parent.Name != targetFileID)
						{
							str1 = directoryInfo.Parent.Name;
							flag = true;
							break;
						}
						else
						{
							num++;
						}
					}
					else
					{
						break;
					}
				}
				if (!flag)
				{
					throw new Exception(
                        //"目标模型在Target文件夹下的MaterialInstanceConstant文件夹不唯一，无法自动转换模型，请尝试手动转换。"
                        "Target model Target folder Material Instance Constant folder is not unique, Can not be automatically converted model, try to manually convert.");
				}
			}
			if ((int)directoryInfoArray1.Length == 1)
			{
				str2 = directoryInfoArray1[0].Parent.Name;
			}
			else
			{
				flag1 = false;
				directoryInfoArray = directoryInfoArray1;
				num = 0;
				while (true)
				{
					if (num < (int)directoryInfoArray.Length)
					{
						directoryInfo = directoryInfoArray[num];
						if ((directoryInfo.Parent.Name == str ? true : directoryInfo.Parent.Name == str1) || (int)directoryInfo.GetFiles(string.Concat("*", ReplaceClass.TryGetObjectNumberWithoutRaceSex(targetObjectID), "*.tga")).Length <= 0)
						{
							num++;
						}
						else
						{
							str2 = directoryInfo.Parent.Name;
							flag1 = true;
							break;
						}
					}
					else
					{
						break;
					}
				}
				if (!flag1)
				{
					throw new Exception(
                        //"目标模型在Target文件夹下的Texture2D文件夹不唯一，无法自动转换模型，请尝试手动转换。"
                        "Target model Target folder Texture2D folder is not unique, Can not be automatically converted model, try to manually convert.");
				}
			}
			str3 = ReplaceClass.BackupSourceFile(name, modelFolder, isGameFolder, exportFolder);
			string str5 = ReplaceClass.BackupSourceFile(name1, modelFolder, isGameFolder, exportFolder);
			string str6 = ReplaceClass.BackupSourceFile(name2, modelFolder, isGameFolder, exportFolder);
			str4 = ReplaceClass.CopyTargetFile(str, modelFolder, isGameFolder, exportFolder);
			string str7 = ReplaceClass.CopyTargetFile(str1, modelFolder, isGameFolder, exportFolder);
			string str8 = ReplaceClass.CopyTargetFile(str2, modelFolder, isGameFolder, exportFolder);
			ReplaceClass.ReplaceTargetToSourceWithASCII(str3, str4, name, str, name1, str1, name2, str2, sourceObjectID, targetObjectID);
			ReplaceClass.ReplaceTargetToSourceWithASCII(str5, str7, name, str, name1, str1, name2, str2, sourceObjectID, targetObjectID);
			ReplaceClass.ReplaceTargetToSourceWithASCII(str6, str8, name, str, name1, str1, name2, str2, sourceObjectID, targetObjectID);
		}

		internal static void GetGotoManualFiles(string sourceImageName, string targetImageName, string modelFolder, bool isGame, out string sourceFileName, out string targetFileName)
		{
			string str;
			string str1;
			sourceFileName = ReplaceClass.GetModelFileName(sourceImageName, modelFolder, isGame, out str, out str1);
			targetFileName = ReplaceClass.GetModelFileName(targetImageName, modelFolder, isGame, out str, out str1);
		}

		private static string GetModelFileName(string imageTagOrJpgFileName, string modelFolder, bool isGameFolder, out string fileID, out string objectID)
		{
			string[] files;
			string str;
			string[] strArrays;
			string[] strArrays1 = Path.GetFileNameWithoutExtension(imageTagOrJpgFileName).Split(new char[] { '+' });
			if ((int)strArrays1.Length < 2)
			{
				throw new Exception(
                    //"不能通过文件名解析文件编号和模型编号，请将文件名按[文件编号]+[模型编号].jpgOr[文件编号]+[模型编号]+[备注名称].jpg命名。"
                    "File name can not parse the file number and model number, change the file name press [File No.] + [model number] .jpg or [File No.] + [model number] + [Note name] .jpg name.");
			}
			fileID = strArrays1[0];
			objectID = strArrays1[1];
			if (!isGameFolder)
			{
				files = Directory.GetFiles(modelFolder, string.Concat(fileID, ".upk"));
				if ((int)files.Length == 0)
				{
                    strArrays = new string[] { "Failure from\"", modelFolder, "\"Locate the file\"", fileID, ".upk\"，Check, please." };
					throw new Exception(string.Concat(strArrays));
				}
				str = files[0];
			}
			else
			{
				string str1 = Path.Combine(modelFolder, "contents\\bns\\CookedPC");
				string str2 = Path.Combine(modelFolder, 
                    //"contents\\Local\\TENCENT\\CHINESES\\CookedPC");
                    "contents\\local\\NCWEST\\ENGLISH\\CookedPC");
				if (!Directory.Exists(str1))
				{
                    throw new Exception(string.Concat("\"", str1, "\"does not exist，Check, please."));
				}
				if (!Directory.Exists(str2))
				{
					throw new Exception(string.Concat("\"", str2, "\"does not exist，Check, please."));
				}
				files = Directory.GetFiles(str1, string.Concat(fileID, ".upk"));
				if ((int)files.Length == 0)
				{
					files = Directory.GetFiles(str2, string.Concat(fileID, ".upk"));
				}
				if ((int)files.Length == 0)
				{
                    strArrays = new string[] { "Failure from\"", str1, "\"or\"", str2, "\"Locate the file\"", fileID, ".upk\"，Check, please." };
					throw new Exception(string.Concat(strArrays));
				}
				str = files[0];
			}
			return str;
		}

		private static string GetModelFileName(string fileID, string modelFolderName, bool isGameFolder)
		{
			string[] files;
			string str;
			string[] strArrays;
			if (!isGameFolder)
			{
				files = Directory.GetFiles(modelFolderName, string.Concat(fileID, ".upk"));
				if ((int)files.Length == 0)
				{
					strArrays = new string[] { "Failure from\"", modelFolderName, "\"Locate the file\"", fileID, ".upk\"，Check, please." };
					throw new Exception(string.Concat(strArrays));
				}
				str = files[0];
			}
			else
			{
				string str1 = Path.Combine(modelFolderName, "contents\\bns\\CookedPC");
				string str2 = Path.Combine(modelFolderName, 
                    //"contents\\Local\\TENCENT\\CHINESES\\CookedPC");
                    "contents\\local\\NCWEST\\ENGLISH\\CookedPC");
				if (!Directory.Exists(str1))
				{
					throw new Exception(string.Concat("\"", str1, "\"does not exist，Check, please."));
				}
				if (!Directory.Exists(str2))
				{
					throw new Exception(string.Concat("\"", str2, "\"does not exist，Check, please."));
				}
				files = Directory.GetFiles(str1, string.Concat(fileID, ".upk"));
				if ((int)files.Length == 0)
				{
					files = Directory.GetFiles(str2, string.Concat(fileID, ".upk"));
				}
				if ((int)files.Length == 0)
				{
					strArrays = new string[] { "Failure from\"", str1, "\"Or\"", str2, "\"Locate the file\"", fileID, ".upk\"，Check, please." };
					throw new Exception(string.Concat(strArrays));
				}
				str = files[0];
			}
			return str;
		}

		internal static void Initialize(MainForm form)
		{
			ReplaceClass.mainForm = form;
			Control[] controlArray = new Control[] { ReplaceClass.mainForm.lstReplaceModelSource, ReplaceClass.mainForm.chkReplaceGonF, ReplaceClass.mainForm.chkReplaceGonM, ReplaceClass.mainForm.chkReplaceJinF, ReplaceClass.mainForm.chkReplaceJinM, ReplaceClass.mainForm.chkReplaceKunN, ReplaceClass.mainForm.chkReplaceLynF, ReplaceClass.mainForm.chkReplaceLynM, ReplaceClass.mainForm.chkReplaceOther, ReplaceClass.mainForm.btnSetSourceModel, ReplaceClass.mainForm.btnSetTargetModel, ReplaceClass.mainForm.btnRenameImage, ReplaceClass.mainForm.btnSearchImageNameWithoutExtension, ReplaceClass.mainForm.txtSearchImageNameWithoutExtension, ReplaceClass.mainForm.btnImageViewerPicture, ReplaceClass.mainForm.btnImageSourcePicture, ReplaceClass.mainForm.btnImageTargetPicture };
			ReplaceClass.defaultControls = controlArray;
			ReplaceClass.mainForm.DoOperation += new EventHandler<OperationEventArgs>(ReplaceClass.mainForm_DoOperation);
			ReplaceClass.refreshWorker = new BackgroundWorker();
			ReplaceClass.refreshWorker.DoWork += new DoWorkEventHandler(ReplaceClass.refreshWorker_DoWork);
			ReplaceClass.refreshWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReplaceClass.refreshWorker_RunWorkerCompleted);
			ReplaceClass.replaceWorker = new BackgroundWorker()
			{
				WorkerSupportsCancellation = true,
				WorkerReportsProgress = true
			};
			ReplaceClass.replaceWorker.DoWork += new DoWorkEventHandler(ReplaceClass.replaceWorker_DoWork);
			ReplaceClass.replaceWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReplaceClass.replaceWorker_RunWorkerCompleted);
			ReplaceClass.replaceWorker.ProgressChanged += new ProgressChangedEventHandler(ReplaceClass.replaceWorker_ProgressChanged);
		}

		private static void mainForm_DoOperation(object sender, OperationEventArgs e)
		{
			if ((e.Operation != Operations.LeaveSettings ? true : Directory.Exists(ReplaceClass.mainForm.txtModelFolder.Text)))
			{
				switch (e.Operation)
				{
					case Operations.LeaveSettings:
					case Operations.RefreshReplaceList:
					{
						ReplaceClass.TryCancelRefresh();
						ReplaceClass.BeginRefresh();
						break;
					}
					case Operations.RefreshAnalysisList:
					case Operations.CancelRefreshanalysisList:
					case Operations.BeginAnalysis:
					case Operations.CancelAnalysis:
					case Operations.DragAnalysis:
					case Operations.CurrentAnalysis:
					case Operations.ManualAction:
					{
						ReplaceClass.SetControlState(ReplaceStates.OtherWork);
						break;
					}
					case Operations.BeginReplace:
					{
						ReplaceClass.BeginReplace();
						break;
					}
					case Operations.CancelReplace:
					{
						ReplaceClass.TryCancelReplace();
						break;
					}
					case Operations.None:
					{
						ReplaceClass.SetControlState(ReplaceStates.None);
						break;
					}
				}
			}
		}

		private static void refreshWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string argument = (string)((object[])e.Argument)[0];
			bool[] flagArray = (bool[])((object[])e.Argument)[1];
			string[] strArrays = new string[8];
			if (flagArray[0])
			{
				strArrays[0] = Path.Combine(argument, "ImgModelLib\\JinM");
			}
			if (flagArray[1])
			{
				strArrays[1] = Path.Combine(argument, "ImgModelLib\\JinF");
			}
			if (flagArray[2])
			{
				strArrays[2] = Path.Combine(argument, "ImgModelLib\\GonM");
			}
			if (flagArray[3])
			{
				strArrays[3] = Path.Combine(argument, "ImgModelLib\\GonF");
			}
			if (flagArray[4])
			{
				strArrays[4] = Path.Combine(argument, "ImgModelLib\\LynM");
			}
			if (flagArray[5])
			{
				strArrays[5] = Path.Combine(argument, "ImgModelLib\\LynF");
			}
			if (flagArray[6])
			{
				strArrays[6] = Path.Combine(argument, "ImgModelLib\\Yun");
			}
			if (flagArray[7])
			{
				strArrays[7] = Path.Combine(argument, "ImgModelLib\\Misc");
			}
			List<string> strs = new List<string>();
			int num = 0;
			while (true)
			{
				if (num < (int)strArrays.Length)
				{
					if (strArrays[num] != null)
					{
						string str = null;
						if (num == 0)
						{
							str = "JinM";
						}
						else if (num == 1)
						{
							str = "JinF";
						}
						else if (num == 2)
						{
							str = "GonM";
						}
						else if (num == 3)
						{
							str = "GonF";
						}
						else if (num == 4)
						{
							str = "LynM";
						}
						else if (num == 5)
						{
							str = "LynF";
						}
						else if (num == 6)
						{
							str = "Yun";
						}
						else if (num == 7)
						{
							str = "Misc";
						}
						string[] files = Directory.GetFiles(strArrays[num], "*.jpg");
						int num1 = 0;
						while (num1 < (int)files.Length)
						{
							string str1 = files[num1];
							strs.Add(string.Format("{0}\\{1}", str, Path.GetFileNameWithoutExtension(str1)));
							if (ReplaceClass.userCancelRefresh)
							{
								return;
							}
							else
							{
								num1++;
							}
						}
					}
					num++;
				}
				else
				{
					e.Result = strs;
					break;
				}
			}
		}

		private static void refreshWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				MessageBox.Show(e.Error.Message);
			}
			else if (!ReplaceClass.userCancelRefresh)
			{
				ReplaceClass.mainForm.lstReplaceModelSource.Items.Clear();
				foreach (string result in (List<string>)e.Result)
				{
					ReplaceClass.mainForm.lstReplaceModelSource.Items.Add(result);
				}
			}
			ReplaceClass.userCancelRefresh = false;
			ReplaceClass.mainForm.OnDoOperation(Operations.None);
		}

		private static void ReplaceTargetToSourceWithASCII(string sourceFileFullName, string targetFileFullName, string sourceSkeletalMeshID, string targetSkeletalMeshID, string sourceMaterialInstanceConstant, string targetMaterialInstanceConstant, string sourceTexture2D, string targetTexture2D, string sourceObjectID, string targetObjectID)
		{
			FileInfo fileInfo = new FileInfo(targetFileFullName);
			FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			byte[] numArray = new byte[checked((int)fileStream.Length)];
			fileStream.Read(numArray, 0, (int)numArray.Length);
			numArray = ReplaceClass.ReplaceWithByte(ref numArray, targetSkeletalMeshID, sourceSkeletalMeshID);
			numArray = ReplaceClass.ReplaceWithByte(ref numArray, targetMaterialInstanceConstant, sourceMaterialInstanceConstant);
			numArray = ReplaceClass.ReplaceWithByte(ref numArray, targetTexture2D, sourceTexture2D);
			numArray = ReplaceClass.ReplaceWithByte(ref numArray, targetObjectID, sourceObjectID);
			fileStream.Position = 0L;
			fileStream.Write(numArray, 0, (int)numArray.Length);
			fileStream.Flush();
			fileStream.Close();
			string str = Path.Combine(Path.GetDirectoryName(targetFileFullName), Path.GetFileName(sourceFileFullName));
			fileInfo.MoveTo(str);
		}

		internal static byte[] ReplaceWithByte(ref byte[] buffer, string source, string target)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(source);
			byte[] numArray = Encoding.ASCII.GetBytes(target);
			return ReplaceClass.ReplaceWithByte(ref buffer, bytes, numArray);
		}

		private static byte[] ReplaceWithByte(ref byte[] all, byte[] s, byte[] t)
		{
			List<byte> nums = new List<byte>();
			for (int i = 0; i < (int)all.Length; i++)
			{
				bool flag = true;
				int num = 0;
				while (true)
				{
					if (num < (int)s.Length)
					{
						int num1 = i + num;
						if (num1 >= (int)all.Length)
						{
							flag = false;
							break;
						}
						else if (all[num1] != s[num])
						{
							flag = false;
							break;
						}
						else
						{
							num++;
						}
					}
					else
					{
						break;
					}
				}
				if (flag)
				{
					i = i + ((int)s.Length - 1);
					nums.AddRange(t);
				}
				else
				{
					nums.Add(all[i]);
				}
			}
			return nums.ToArray();
		}

		private static void replaceWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string str;
			string str1;
			string str2;
			string str3;
			string str4;
			string str5;
			string argument = (string)((object[])e.Argument)[0];
			string argument1 = (string)((object[])e.Argument)[1];
			string argument2 = (string)((object[])e.Argument)[2];
			string argument3 = (string)((object[])e.Argument)[3];
			string argument4 = (string)((object[])e.Argument)[4];
			bool flag = (bool)((object[])e.Argument)[5];
			BackgroundWorker backgroundWorker = ReplaceClass.replaceWorker;
			InvokeAction invokeAction = (object x) => {
                LogClass.AppendLine(string.Concat("Start to model transformation", DateTime.Now.ToString()), true);
				LogClass.AppendLine((string)x, false);
			};
			string[] fileNameWithoutExtension = new string[] { "Start", Path.GetFileNameWithoutExtension(argument), "to", Path.GetFileNameWithoutExtension(argument1), "Model Transformation" };
			backgroundWorker.ReportProgress(0, new Report(invokeAction, string.Concat(fileNameWithoutExtension)));
			if (!ReplaceClass.userCancelReplace)
			{
				string str6 = Path.Combine(argument3, "ModsOutput");
				string str7 = Path.Combine(str6, string.Concat(Path.GetFileNameWithoutExtension(argument), " to ", Path.GetFileNameWithoutExtension(argument1)));
				str6 = str7;
				ReplaceClass.exportCompletedReportDir = str7;
				string str8 = Path.Combine(str6, "Backup source model");
				if (!ReplaceClass.userCancelReplace)
				{
					if (Directory.Exists(str6))
					{
						try
						{
							Directory.Delete(str6, true);
						}
						catch
						{
						}
					}
					if (!Directory.Exists(str6))
					{
						Directory.CreateDirectory(str6);
					}
					if (!Directory.Exists(str8))
					{
						Directory.CreateDirectory(str8);
					}
					string str9 = Path.Combine(str6, "Source");
					if (!Directory.Exists(str9))
					{
						Directory.CreateDirectory(str9);
					}
					string str10 = Path.Combine(str6, "Target");
					if (!Directory.Exists(str10))
					{
						Directory.CreateDirectory(str10);
					}
					if (!ReplaceClass.userCancelReplace)
					{
						string modelFileName = ReplaceClass.GetModelFileName(argument, argument4, flag, out str, out str1);
						string modelFileName1 = ReplaceClass.GetModelFileName(argument1, argument4, flag, out str2, out str3);
						if (!ReplaceClass.userCancelReplace)
						{
							UEViewer uEViewer = new UEViewer(modelFileName, str9, Path.GetDirectoryName(argument2));
							if (!ReplaceClass.userCancelReplace)
							{
								UEViewer uEViewer1 = new UEViewer(modelFileName1, str10, Path.GetDirectoryName(argument2));
								if (!ReplaceClass.userCancelReplace && !ReplaceClass.userCancelReplace)
								{
									bool flag1 = uEViewer.Unpack(out str4, argument4, flag);
									ReplaceClass.replaceWorker.ReportProgress(0, new Report((object x) => LogClass.AppendLine((string)x, false), str4));
									if (!ReplaceClass.userCancelReplace)
									{
										bool flag2 = uEViewer1.Unpack(out str5, argument4, flag);
										ReplaceClass.replaceWorker.ReportProgress(0, new Report((object x) => LogClass.AppendLine((string)x, false), str5));
										if (!ReplaceClass.userCancelReplace)
										{
											if (!flag1)
											{
												throw new Exception(string.Concat("error_source\\", str6));
											}
											if (!flag2)
											{
												throw new Exception(string.Concat("error_target\\", str6));
											}
											if (!ReplaceClass.userCancelReplace)
											{
                                                ReplaceClass.replaceWorker.ReportProgress(0, new Report((object x) => LogClass.AppendLine((string)x, false), "Official convert Start"));
												ReplaceClass.DoReplace(str9, str10, str, str2, str1, str3, argument4, flag, str6);
                                                ReplaceClass.replaceWorker.ReportProgress(0, new Report((object x) => LogClass.AppendLine((string)x, false), "Concluded an official convert"));
												e.Result = str6;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private static void replaceWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState is Report)
			{
				Report userState = (Report)e.UserState;
				userState.Action(userState.Argument);
			}
		}

		private static void replaceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			DialogResult dialogResult;
			Process process;
			DateTime now = DateTime.Now;
			LogClass.AppendLine(string.Concat("Model transformation ending", now.ToString()), false);
			ReplaceClass.mainForm.OnDoOperation(Operations.None);
			ReplaceClass.mainForm.pbReplace.Style = ProgressBarStyle.Blocks;
			ReplaceClass.mainForm.btnReplaceModel.Text = "Generate New Model";
			if (e.Error != null)
			{
				string str = null;
				if (e.Error.Message.StartsWith("error_source\\"))
				{
					str = e.Error.Message.Substring("error_source\\".Length);
                    dialogResult = MessageBox.Show("Parse error source model, select 'Yes' to clean up residual files, select 'No' then open the exported folder, select 'Cancel' Stop operation.", "Model conversion failed", MessageBoxButtons.YesNoCancel);
					if (dialogResult == DialogResult.Yes)
					{
						ReplaceClass.DeleteDirAndAllFiles(str);
					}
					else if (dialogResult == DialogResult.No)
					{
						process = new Process();
						process.StartInfo.FileName = "explorer.exe";
						process.StartInfo.Arguments = str;
						process.Start();
					}
				}
				else if (!e.Error.Message.StartsWith("error_target\\"))
				{
					MessageBox.Show(e.Error.Message);
                    LogClass.AppendLine(string.Concat("Tracking error:", e.Error.ToString()), false);
					str = ReplaceClass.exportCompletedReportDir;
					if (Directory.Exists(str))
					{
                        dialogResult = MessageBox.Show("Model conversion failed, select 'Yes' to clean up residual files, select 'No' then open the exported folder, select 'Cancel' Stop operation.", "Model conversion failed", MessageBoxButtons.YesNoCancel);
						if (dialogResult == DialogResult.Yes)
						{
							ReplaceClass.DeleteDirAndAllFiles(str);
						}
						else if (dialogResult == DialogResult.No)
						{
							process = new Process();
							process.StartInfo.FileName = "explorer.exe";
							process.StartInfo.Arguments = str;
							process.Start();
						}
					}
				}
				else
				{
					str = e.Error.Message.Substring("error_target\\".Length);
                    dialogResult = MessageBox.Show("Target model parsing error, select 'Yes' to clean up residual files, select 'No' then open the exported folder, select 'Cancel' Stop operation.", "Model conversion failed", MessageBoxButtons.YesNoCancel);
					if (dialogResult == DialogResult.Yes)
					{
						ReplaceClass.DeleteDirAndAllFiles(str);
					}
					else if (dialogResult == DialogResult.No)
					{
						process = new Process();
						process.StartInfo.FileName = "explorer.exe";
						process.StartInfo.Arguments = str;
						process.Start();
					}
				}
			}
            else if ((ReplaceClass.userCancelReplace ? false : MessageBox.Show("Model conversion is complete，Do you want to open export folder?", string.Empty, MessageBoxButtons.OKCancel) == DialogResult.OK))
			{
				process = new Process();
				process.StartInfo.FileName = "explorer.exe";
				process.StartInfo.Arguments = (string)e.Result;
				process.Start();
			}
			ReplaceClass.userCancelReplace = false;
		}

		private static void SetControlState(ReplaceStates state)
		{
			bool flag = true;
			bool flag1 = true;
			bool flag2 = true;
			switch (state)
			{
				case ReplaceStates.Refresh:
				{
					flag = false;
					flag1 = false;
					flag2 = true;
					ReplaceClass.mainForm.panelLoadingReplaceModelSource.Visible = true;
					break;
				}
				case ReplaceStates.Build:
				{
					flag = false;
					flag1 = true;
					flag2 = false;
					break;
				}
				case ReplaceStates.OtherWork:
				{
					flag = false;
					flag1 = false;
					flag2 = false;
					break;
				}
				case ReplaceStates.None:
				{
					flag = true;
					flag1 = true;
					flag2 = true;
					ReplaceClass.mainForm.panelLoadingReplaceModelSource.Visible = false;
					break;
				}
			}
			Control[] controlArray = ReplaceClass.defaultControls;
			for (int i = 0; i < (int)controlArray.Length; i++)
			{
				controlArray[i].Enabled = flag;
			}
			ReplaceClass.mainForm.btnRefreshReplaceModelSource.Enabled = flag2;
			ReplaceClass.mainForm.btnReplaceModel.Enabled = flag1;
		}

		private static void TryCancelRefresh()
		{
			if (ReplaceClass.refreshWorker.IsBusy)
			{
				ReplaceClass.userCancelRefresh = true;
			}
		}

		private static void TryCancelReplace()
		{
			if (ReplaceClass.replaceWorker.IsBusy)
			{
				ReplaceClass.replaceWorker.CancelAsync();
				ReplaceClass.userCancelReplace = true;
			}
		}

		private static string TryGetObjectNumberWithoutRaceSex(string objectID)
		{
			string str;
			string[] strArrays = objectID.Split(new char[] { '\u005F' });
			int num = 0;
			while (true)
			{
				if (num < (int)strArrays.Length)
				{
					try
					{
						Convert.ToInt32(strArrays[num]);
						str = strArrays[num];
						break;
					}
					catch
					{
					}
					num++;
				}
				else
				{
					str = objectID;
					break;
				}
			}
			return str;
		}
	}
}