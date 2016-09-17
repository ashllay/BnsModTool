using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace BnsModTool
{
	internal class UEViewer : IDisposable
	{
		internal const int WM_CLOSE = 16;
		private string fileFullName;
		private string outputFolderName;
		private string appStartupFolderName;

		internal bool HasImage
		{
			get;
			set;
		}

		internal string TempFileName
		{
			get;
			set;
		}

		public UEViewer(string fileFullName, string outputFolderName, string appStartupFolderName)
		{
			this.fileFullName = fileFullName;
			this.outputFolderName = outputFolderName;
			this.appStartupFolderName = appStartupFolderName;
		}

		internal string AnalysisToImageReturnLog()
		{
			string end;
			if (File.Exists(this.fileFullName))
			{
				Process process = new Process();
				process.StartInfo.FileName = Path.Combine(this.appStartupFolderName, "umodel.exe");
				process.StartInfo.Arguments = string.Format(" -game=bns -meshes -path=\"{0}\" {1}", Path.GetDirectoryName(this.fileFullName), Path.GetFileName(this.fileFullName));
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardInput = false;
				process.StartInfo.CreateNoWindow = true;
				process.Start();
				end = process.StandardOutput.ReadToEnd();
			}
			else
			{
				end = null;
			}
			return end;
		}

		[DllImport("gdi32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

		public void Dispose()
		{
			this.fileFullName = null;
			this.outputFolderName = null;
			this.appStartupFolderName = null;
			this.TempFileName = null;
			GC.Collect();
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern int FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool GetWindowRect(IntPtr hWnd, ref UEViewer.RECT lpRect);

		internal static string QuickLookModel(string appStartupFolderName, string filename)
		{
			Process process = new Process();
			process.StartInfo.FileName = Path.Combine(appStartupFolderName, "umodel.exe");
			process.StartInfo.Arguments = string.Format(" -game=bns -meshes -path=\"{0}\" {1}", Path.GetDirectoryName(filename), Path.GetFileName(filename));
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput = false;
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			return process.StandardOutput.ReadToEnd();
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern int SendMessageA(IntPtr hwnd, int wMsg, int wParam, int lParam);

		internal bool Unpack(out string log, string modelFolder, bool isGame)
		{
			bool flag;
			Process process = new Process();
			process.StartInfo.FileName = Path.Combine(this.appStartupFolderName, "umodel.exe");
			process.StartInfo.Arguments = string.Format(" -game=bns -path=\"{0}\" -export -md5 {1} -out=\"{2}\"", Path.GetDirectoryName(this.fileFullName), Path.GetFileName(this.fileFullName), this.outputFolderName);
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput = false;
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			log = process.StandardOutput.ReadToEnd();
			DirectoryInfo directoryInfo = new DirectoryInfo(this.outputFolderName);
			if (((int)directoryInfo.GetDirectories("Texture2D", SearchOption.AllDirectories).Length <= 0 || (int)directoryInfo.GetDirectories("MaterialInstanceConstant", SearchOption.AllDirectories).Length <= 0 ? true : (int)directoryInfo.GetDirectories("SkeletalMesh3", SearchOption.AllDirectories).Length <= 0))
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.fileFullName);
				if (!Regex.IsMatch(fileNameWithoutExtension, "\\d{8}"))
				{
					flag = false;
				}
				else if ((string.IsNullOrEmpty(modelFolder) ? false : Directory.Exists(modelFolder)))
				{
					int num = Convert.ToInt32(fileNameWithoutExtension) - 3;
					string str = string.Concat(num.ToString("00000000"), ".upk");
					num = Convert.ToInt32(fileNameWithoutExtension) - 2;
					string str1 = string.Concat(num.ToString("00000000"), ".upk");
					num = Convert.ToInt32(fileNameWithoutExtension) - 1;
					string str2 = string.Concat(num.ToString("00000000"), ".upk");
					num = Convert.ToInt32(fileNameWithoutExtension) + 1;
					string str3 = string.Concat(num.ToString("00000000"), ".upk");
					num = Convert.ToInt32(fileNameWithoutExtension) + 2;
					string str4 = string.Concat(num.ToString("00000000"), ".upk");
					num = Convert.ToInt32(fileNameWithoutExtension) + 3;
					string str5 = string.Concat(num.ToString("00000000"), ".upk");
					bool flag1 = false;
					if (!isGame)
					{
						DirectoryInfo directoryInfo1 = new DirectoryInfo(modelFolder);
						if ((flag1 ? false : (int)directoryInfo1.GetFiles(str).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(modelFolder, str));
						}
						if ((flag1 ? false : (int)directoryInfo1.GetFiles(str1).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(modelFolder, str1));
						}
						if ((flag1 ? false : (int)directoryInfo1.GetFiles(str2).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(modelFolder, str2));
						}
						if ((flag1 ? false : (int)directoryInfo1.GetFiles(str3).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(modelFolder, str3));
						}
						if ((flag1 ? false : (int)directoryInfo1.GetFiles(str4).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(modelFolder, str4));
						}
						if ((flag1 ? false : (int)directoryInfo1.GetFiles(str5).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(modelFolder, str5));
						}
					}
					else
					{
						DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(modelFolder, "contents\\bns\\CookedPC"));
						DirectoryInfo directoryInfo3 = new DirectoryInfo(Path.Combine(modelFolder, 
                            //"contents\\Local\\TENCENT\\CHINESES\\CookedPC"));
                         "contents\\local\\NCWEST\\ENGLISH\\CookedPC"));
						if ((flag1 ? false : (int)directoryInfo2.GetFiles(str).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo2.FullName, str));
						}
						if ((flag1 ? false : (int)directoryInfo3.GetFiles(str).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo3.FullName, str));
						}
						if ((flag1 ? false : (int)directoryInfo2.GetFiles(str1).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo2.FullName, str1));
						}
						if ((flag1 ? false : (int)directoryInfo3.GetFiles(str1).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo3.FullName, str1));
						}
						if ((flag1 ? false : (int)directoryInfo2.GetFiles(str2).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo2.FullName, str2));
						}
						if ((flag1 ? false : (int)directoryInfo3.GetFiles(str2).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo3.FullName, str2));
						}
						if ((flag1 ? false : (int)directoryInfo2.GetFiles(str3).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo2.FullName, str3));
						}
						if ((flag1 ? false : (int)directoryInfo3.GetFiles(str3).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo3.FullName, str3));
						}
						if ((flag1 ? false : (int)directoryInfo2.GetFiles(str4).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo2.FullName, str4));
						}
						if ((flag1 ? false : (int)directoryInfo3.GetFiles(str4).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo3.FullName, str4));
						}
						if ((flag1 ? false : (int)directoryInfo2.GetFiles(str5).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo2.FullName, str5));
						}
						if ((flag1 ? false : (int)directoryInfo3.GetFiles(str5).Length > 0))
						{
							flag1 = this.Unpack2(Path.Combine(directoryInfo3.FullName, str5));
						}
					}
					flag = flag1;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		internal bool Unpack(out string log)
		{
			return this.Unpack(out log, null, false);
		}

		private bool Unpack2(string searchfileFullName)
		{
			Process process = new Process();
			process.StartInfo.FileName = Path.Combine(this.appStartupFolderName, "umodel.exe");
			process.StartInfo.Arguments = string.Format(" -game=bns -path=\"{0}\" -export -md5 {1} -out=\"{2}\"", Path.GetDirectoryName(searchfileFullName), Path.GetFileName(searchfileFullName), this.outputFolderName);
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput = false;
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			process.StandardOutput.ReadToEnd();
			DirectoryInfo directoryInfo = new DirectoryInfo(this.outputFolderName);
			return (((int)directoryInfo.GetDirectories("Texture2D", SearchOption.AllDirectories).Length <= 0 || (int)directoryInfo.GetDirectories("MaterialInstanceConstant", SearchOption.AllDirectories).Length <= 0 ? true : (int)directoryInfo.GetDirectories("SkeletalMesh3", SearchOption.AllDirectories).Length <= 0) ? false : true);
		}

		internal struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}
	}
}