using BnsModTool.Properties;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BnsModTool
{
    public class MainForm : Form
    {
        private int oldSelectedCurrentIndex = 0;
        internal Operations currentOperation = Operations.None;
        private IContainer components = null;
        private TabPage tabPageSettings;
        private TabPage tabPageAnalysis;
        private TabPage tabPageAutoReplace;
        private GroupBox groupBox1;
        private Label label1;
        private GroupBox groupBox2;
        private Label label2;
        private SplitContainer splitContainer1;
        private GroupBox groupBox3;
        private Label label3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label13;
        private TabPage tabPageLog;
        private Label label16;
        private Label label15;
        private Label label17;
        private Label label19;
        private Label label20;
        private FolderBrowserDialog dlgOutputFolder;
        private FolderBrowserDialog dlgModelFolder;
        private ProgressBar pbLoadingAnalysisFileName;
        private Label label8;
        internal RadioButton rbSetup;
        internal Button btnSearchModelFolder;
        internal RadioButton rbCustom;
        internal TextBox txtModelFolder;
        internal Button btnSearchOutputFolder;
        internal TextBox txtOutputFolder;
        internal TabControl tcApp;
        internal ListBox lstAnalysisFileName;
        internal Button btnSearchFileNameWithoutExtension;
        internal TextBox txtSearchFileNameWithoutExtension;
        internal CheckBox chkAnalysisJinF;
        internal CheckBox chkAnalysisJinM;
        internal CheckBox chkAnalysisKunN;
        internal CheckBox chkAnalysisLynF;
        internal CheckBox chkAnalysisLynM;
        internal CheckBox chkAnalysisGonF;
        internal CheckBox chkAnalysisGonM;
        internal Button btnBeginAnalysis;
        internal Button btnCancelAnalysis;
        internal ProgressBar pbAnalysis;
        internal Label lblCurrentAnalysisFileName;
        internal Label lblAnalysisProgress;
        internal Button btnAnalysisCurrentItem;
        internal Label lblAutoAnalysisArea;
        internal TextBox txtAnalysisInterval;
        internal PictureBox picSourceImage;
        internal PictureBox picTargetImage;
        internal PictureBox picViewerImage;
        internal ListBox lstReplaceModelSource;
        internal Button btnSetTargetModel;
        internal Button btnSetSourceModel;
        internal CheckBox chkReplaceKunN;
        internal CheckBox chkReplaceLynF;
        internal CheckBox chkReplaceLynM;
        internal CheckBox chkReplaceGonF;
        internal CheckBox chkReplaceGonM;
        internal CheckBox chkReplaceJinF;
        internal CheckBox chkReplaceJinM;
        internal Button btnReplaceModel;
        internal RichTextBox txtLog;
        internal Button btnRefreshAnalysisFileName;
        private Button btnClearLog;
        internal Panel panelLoadingAnalysisFileName;
        private Label label22;
        private Label label21;
        private Label label14;
        private Label label12;
        internal TextBox txtAnalysisEndName;
        internal TextBox txtAnalysisBeginName;
        private System.Windows.Forms.ContextMenuStrip cmsAnalysisFileName;
        private ToolStripMenuItem btnSetAnalysisBeginItem;
        private ToolStripMenuItem btnSetAnalysisEndItem;
        private TabPage tabPageAbout;
        internal CheckBox chkAnalysisOther;
        internal Panel panelLoadingReplaceModelSource;
        private ProgressBar pbLoadingModelSource;
        private Label label23;
        internal Button btnRenameImage;
        internal Button btnRefreshReplaceModelSource;
        internal CheckBox chkReplaceOther;
        private GroupBox groupBox6;
        internal Button btnSearchImageNameWithoutExtension;
        internal TextBox txtSearchImageNameWithoutExtension;
        private Label label24;
        internal Button btnImageTargetPicture;
        internal Button btnImageSourcePicture;
        internal Button btnImageViewerPicture;
        internal ProgressBar pbReplace;
        internal Label lblTargetImage;
        internal Label lblSourceImage;
        private TabPage tabPageManualReplace;
        private Button btnOpenImageDirectory;
        private Button btnGotoManualReplace;
        private GroupBox groupBox8;
        private Label label28;
        private Label label29;
        private Label label30;
        private GroupBox groupBox7;
        private Label label27;
        private Label label26;
        private Label label25;
        private Label label40;
        private Label label39;
        internal ProgressBar pbManual;
        internal TextBox txtManualTargetFileNameZ;
        internal TextBox txtManualTargetFileNameY;
        internal TextBox txtManualTargetFileNameX;
        internal TextBox txtManualSourceFileNameZ;
        internal TextBox txtManualSourceFileNameY;
        internal TextBox txtManualSourceFileNameX;
        internal TextBox txtManualExportFolderName;
        private FolderBrowserDialog dlgManualExportFolder;
        internal TextBox txtManualTargetObjectID;
        private Label label43;
        internal TextBox txtManualSourceObjectID;
        private Label label41;
        internal Button btnManualSearchTargetZ;
        internal Button btnManualSearchTargetY;
        internal Button btnManualSearchTargetX;
        internal Button btnManualSearchSourceZ;
        internal Button btnManualSearchSourceY;
        internal Button btnManualSearchSourceX;
        internal Button btnManualExportTargetZ;
        internal Button btnManualExportTargetY;
        internal Button btnManualExportTargetX;
        internal Button btnManualExportSourceZ;
        internal Button btnManualExportSourceY;
        internal Button btnManualExportSourceX;
        internal Button btnManualSearchExportFolderName;
        internal Button btnManualBuild;
        internal Button btnManualOpenExportFolder;
        internal RichTextBox txtAbout;
        private GroupBox groupBox9;
        private GroupBox groupBox10;

        private string lastSearchText;

        public MainForm()
        {
            this.InitializeComponent();
            this.InitializeOnLoaded();
        }

        private void btnAnalysisCurrentItem_Click(object sender, EventArgs e)
        {
            this.OnDoOperation(Operations.CurrentAnalysis);
        }

        private void btnBeginAnalysis_Click(object sender, EventArgs e)
        {
            this.OnDoOperation(Operations.BeginAnalysis);
        }

        private void btnCancelAnalysis_Click(object sender, EventArgs e)
        {
            this.OnDoOperation(Operations.CancelAnalysis);
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            this.txtLog.Text = string.Empty;
        }

        private void btnGotoManualReplace_Click(object sender, EventArgs e)
        {
            string str;
            string str1;
            try
            {
                if (this.picSourceImage.Tag == null)
                {
                    MessageBox.Show("Set image source model.");
                }
                else if (this.picTargetImage.Tag != null)
                {
                    ReplaceClass.GetGotoManualFiles(this.picSourceImage.Tag.ToString(), this.picTargetImage.Tag.ToString(), this.txtModelFolder.Text, this.rbSetup.Checked, out str, out str1);
                    this.txtManualExportFolderName.Text = Path.Combine(Path.Combine(this.txtOutputFolder.Text, "ModsOutput"), string.Format("{0} to {1}", Path.GetFileNameWithoutExtension(this.picSourceImage.Tag.ToString()), Path.GetFileNameWithoutExtension(this.picTargetImage.Tag.ToString())));
                    if (!Directory.Exists(this.txtManualExportFolderName.Text))
                    {
                        Directory.CreateDirectory(this.txtManualExportFolderName.Text);
                    }
                    this.txtManualSourceFileNameX.Text = str;
                    this.txtManualTargetFileNameX.Text = str1;
                    this.txtManualSourceObjectID.Text = this.GetObjectIDFromImageFileName(this.picSourceImage.Tag.ToString());
                    this.txtManualTargetObjectID.Text = this.GetObjectIDFromImageFileName(this.picTargetImage.Tag.ToString());
                    this.tcApp.SelectedTab = this.tabPageManualReplace;
                }
                else
                {
                    MessageBox.Show("Set target model image.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnImageSourcePicture_Click(object sender, EventArgs e)
        {
            if (this.picSourceImage.Tag != null)
            {
                Process process = new Process();
                process.StartInfo.FileName = this.picSourceImage.Tag.ToString();
                process.Start();
            }
            else
            {
                MessageBox.Show(
                    //"没有要打开的原图"
                    "You do not have to open the original image");
            }
        }

        private void btnImageTargetPicture_Click(object sender, EventArgs e)
        {
            if (this.picTargetImage.Tag != null)
            {
                Process process = new Process();
                process.StartInfo.FileName = this.picTargetImage.Tag.ToString();
                process.Start();
            }
            else
            {
                MessageBox.Show(
                    //"没有要打开的原图"
                    "You do not have to open the original image");
            }
        }

        private void btnImageViewerPicture_Click(object sender, EventArgs e)
        {
            if (this.picViewerImage.Tag != null)
            {
                Process process = new Process();
                process.StartInfo.FileName = this.picViewerImage.Tag.ToString();
                process.Start();
            }
            else
            {
                MessageBox.Show(
                    //"没有要打开的原图"
                    "You do not have to open the original image");
            }
        }

        private void btnManualBuild_Click(object sender, EventArgs e)
        {
            ManualClass.BuildAsync(this.txtManualExportFolderName.Text, this.txtManualSourceFileNameX.Text, this.txtManualSourceFileNameY.Text, this.txtManualSourceFileNameZ.Text, this.txtManualTargetFileNameX.Text, this.txtManualTargetFileNameY.Text, this.txtManualTargetFileNameZ.Text, this.txtManualSourceObjectID.Text, this.txtManualTargetObjectID.Text);
        }

        private void btnManualExportSourceX_Click(object sender, EventArgs e)
        {
            ManualClass.ExportFileAsync(this.txtManualExportFolderName.Text, this.txtManualSourceFileNameX.Text, true);
        }

        private void btnManualExportSourceY_Click(object sender, EventArgs e)
        {
            ManualClass.ExportFileAsync(this.txtManualExportFolderName.Text, this.txtManualSourceFileNameY.Text, true);
        }

        private void btnManualExportSourceZ_Click(object sender, EventArgs e)
        {
            ManualClass.ExportFileAsync(this.txtManualExportFolderName.Text, this.txtManualSourceFileNameZ.Text, true);
        }

        private void btnManualExportTargetX_Click(object sender, EventArgs e)
        {
            ManualClass.ExportFileAsync(this.txtManualExportFolderName.Text, this.txtManualTargetFileNameX.Text, false);
        }

        private void btnManualExportTargetY_Click(object sender, EventArgs e)
        {
            ManualClass.ExportFileAsync(this.txtManualExportFolderName.Text, this.txtManualTargetFileNameY.Text, false);
        }

        private void btnManualExportTargetZ_Click(object sender, EventArgs e)
        {
            ManualClass.ExportFileAsync(this.txtManualExportFolderName.Text, this.txtManualTargetFileNameZ.Text, false);
        }

        private void btnManualOpenExportFolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(this.txtManualExportFolderName.Text))
            {
                MessageBox.Show(string.Concat("folder\"", this.txtManualExportFolderName.Text, "\"does not exist."));
            }
            else
            {
                Process process = new Process();
                process.StartInfo.FileName = "explorer.exe";
                process.StartInfo.Arguments = this.txtManualExportFolderName.Text;
                process.Start();
            }
        }

        private void btnManualSearchExportFolderName_Click(object sender, EventArgs e)
        {
            this.dlgManualExportFolder.SelectedPath = this.txtManualExportFolderName.Text;
            if (this.dlgManualExportFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtManualExportFolderName.Text = this.dlgManualExportFolder.SelectedPath;
            }
        }

        private void btnManualSearchSourceX_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Model file|*.upk"
            };
            if (File.Exists(this.txtManualSourceFileNameX.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(this.txtManualSourceFileNameX.Text);
                openFileDialog.FileName = this.txtManualSourceFileNameX.Text;
            }
            else if (!string.IsNullOrEmpty(this.txtManualSourceFileNameX.Text))
            {
                openFileDialog.InitialDirectory = this.txtManualSourceFileNameX.Text;
            }
            else
            {
                openFileDialog.InitialDirectory = this.txtModelFolder.Text;
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtManualSourceFileNameX.Text = openFileDialog.FileName;
            }
        }

        private void btnManualSearchSourceY_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Model file|*.upk"
            };
            if (File.Exists(this.txtManualSourceFileNameY.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(this.txtManualSourceFileNameY.Text);
                openFileDialog.FileName = this.txtManualSourceFileNameY.Text;
            }
            else if (!string.IsNullOrEmpty(this.txtManualSourceFileNameY.Text))
            {
                openFileDialog.InitialDirectory = this.txtManualSourceFileNameY.Text;
            }
            else
            {
                openFileDialog.InitialDirectory = this.txtModelFolder.Text;
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtManualSourceFileNameY.Text = openFileDialog.FileName;
            }
        }

        private void btnManualSearchSourceZ_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Model file|*.upk"
            };
            if (File.Exists(this.txtManualSourceFileNameZ.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(this.txtManualSourceFileNameZ.Text);
                openFileDialog.FileName = this.txtManualSourceFileNameZ.Text;
            }
            else if (!string.IsNullOrEmpty(this.txtManualSourceFileNameZ.Text))
            {
                openFileDialog.InitialDirectory = this.txtManualSourceFileNameZ.Text;
            }
            else
            {
                openFileDialog.InitialDirectory = this.txtModelFolder.Text;
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtManualSourceFileNameZ.Text = openFileDialog.FileName;
            }
        }

        private void btnManualSearchTargetX_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Model file|*.upk"
            };
            if (File.Exists(this.txtManualTargetFileNameX.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(this.txtManualTargetFileNameX.Text);
                openFileDialog.FileName = this.txtManualTargetFileNameX.Text;
            }
            else if (!string.IsNullOrEmpty(this.txtManualTargetFileNameX.Text))
            {
                openFileDialog.InitialDirectory = this.txtManualTargetFileNameX.Text;
            }
            else
            {
                openFileDialog.InitialDirectory = this.txtModelFolder.Text;
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtManualTargetFileNameX.Text = openFileDialog.FileName;
            }
        }

        private void btnManualSearchTargetY_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Model file|*.upk"
            };
            if (File.Exists(this.txtManualTargetFileNameY.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(this.txtManualTargetFileNameY.Text);
                openFileDialog.FileName = this.txtManualTargetFileNameY.Text;
            }
            else if (!string.IsNullOrEmpty(this.txtManualTargetFileNameY.Text))
            {
                openFileDialog.InitialDirectory = this.txtManualTargetFileNameY.Text;
            }
            else
            {
                openFileDialog.InitialDirectory = this.txtModelFolder.Text;
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtManualTargetFileNameY.Text = openFileDialog.FileName;
            }
        }

        private void btnManualSearchTargetZ_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Model file|*.upk"
            };
            if (File.Exists(this.txtManualTargetFileNameZ.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(this.txtManualTargetFileNameZ.Text);
                openFileDialog.FileName = this.txtManualTargetFileNameZ.Text;
            }
            else if (!string.IsNullOrEmpty(this.txtManualTargetFileNameZ.Text))
            {
                openFileDialog.InitialDirectory = this.txtManualTargetFileNameZ.Text;
            }
            else
            {
                openFileDialog.InitialDirectory = this.txtModelFolder.Text;
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtManualTargetFileNameZ.Text = openFileDialog.FileName;
            }
        }

        private void btnOpenImageDirectory_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "explorer.exe";
            process.StartInfo.Arguments = Path.Combine(this.txtOutputFolder.Text, "ImgModelLib");
            process.Start();
        }

        private void btnRefreshAnalysisFileName_Click(object sender, EventArgs e)
        {
            if (this.btnRefreshAnalysisFileName.Text != "Refresh list")
            {
                this.OnDoOperation(Operations.CancelRefreshanalysisList);
            }
            else
            {
                this.OnDoOperation(Operations.RefreshAnalysisList);
            }
        }

        private void btnRefreshReplaceModelSource_Click(object sender, EventArgs e)
        {
            this.OnDoOperation(Operations.RefreshReplaceList);
        }

        private void btnRenameImage_Click(object sender, EventArgs e)
        {
            if (this.lstReplaceModelSource.SelectedIndex >= 0)
            {
                string str = this.lstReplaceModelSource.SelectedItem.ToString();
                string imageFileFullNameFromListCurrentItem = this.GetImageFileFullNameFromListCurrentItem();
                RenameForm renameForm = new RenameForm(imageFileFullNameFromListCurrentItem);
                if (renameForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileInfo fileInfo = new FileInfo(imageFileFullNameFromListCurrentItem);
                    string str1 = Path.Combine(Path.GetDirectoryName(imageFileFullNameFromListCurrentItem), renameForm.NewImageName);
                    fileInfo.MoveTo(str1);
                    int num = this.lstReplaceModelSource.Items.IndexOf(str);
                    this.lstReplaceModelSource.Items.RemoveAt(num);
                    ListBox.ObjectCollection items = this.lstReplaceModelSource.Items;
                    char[] chrArray = new char[] { '\\' };
                    items.Insert(num, string.Concat(str.Split(chrArray)[0], "\\", Path.GetFileNameWithoutExtension(renameForm.NewImageName)));
                    this.lstReplaceModelSource.SelectedIndex = num;
                }
            }
        }

        private void btnReplaceModel_Click(object sender, EventArgs e)
        {
            if (this.btnReplaceModel.Text != "Generate New Model")
            {
                this.OnDoOperation(Operations.CancelReplace);
            }
            else
            {
                this.OnDoOperation(Operations.BeginReplace);
            }
        }

        private void btnSearchFileNameWithoutExtension_Click(object sender, EventArgs e)
        {
            string lower = this.txtSearchFileNameWithoutExtension.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(lower))
            {
                int selectedIndex = 0;
                if (this.lastSearchText == lower)
                {
                    if ((this.lstAnalysisFileName.SelectedItem == null ? false : this.lstAnalysisFileName.SelectedItem.ToString().ToLower().Contains(lower)))
                    {
                        selectedIndex = this.lstAnalysisFileName.SelectedIndex + 1;
                    }
                }
                this.lastSearchText = lower;
                bool flag = false;
                int num = selectedIndex;
                while (true)
                {
                    if (num >= this.lstAnalysisFileName.Items.Count)
                    {
                        break;
                    }
                    else if (this.lstAnalysisFileName.Items[num].ToString().ToLower().Contains(lower))
                    {
                        this.lstAnalysisFileName.SelectedIndex = num;
                        flag = true;
                        break;
                    }
                    else
                    {
                        num++;
                    }
                }
                if (!flag)
                {
                    num = 0;
                    while (num < this.lstAnalysisFileName.Items.Count)
                    {
                        if (this.lstAnalysisFileName.Items[num].ToString().ToLower().Contains(lower))
                        {
                            this.lstAnalysisFileName.SelectedIndex = num;
                            return;
                        }
                        else
                        {
                            num++;
                        }
                    }
                }
            }
        }

        private void btnSearchImageNameWithoutExtension_Click(object sender, EventArgs e)
        {
            string lower = this.txtSearchImageNameWithoutExtension.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(lower))
            {
                int selectedIndex = 0;
                if (this.lastSearchText == lower)
                {
                    if ((this.lstReplaceModelSource.SelectedItem == null ? false : this.lstReplaceModelSource.SelectedItem.ToString().ToLower().Contains(lower)))
                    {
                        selectedIndex = this.lstReplaceModelSource.SelectedIndex + 1;
                    }
                }
                this.lastSearchText = lower;
                bool flag = false;
                int num = selectedIndex;
                while (true)
                {
                    if (num >= this.lstReplaceModelSource.Items.Count)
                    {
                        break;
                    }
                    else if (this.lstReplaceModelSource.Items[num].ToString().ToLower().Contains(lower))
                    {
                        this.lstReplaceModelSource.SelectedIndex = num;
                        flag = true;
                        break;
                    }
                    else
                    {
                        num++;
                    }
                }
                if (!flag)
                {
                    num = 0;
                    while (num < this.lstReplaceModelSource.Items.Count)
                    {
                        if (this.lstReplaceModelSource.Items[num].ToString().ToLower().Contains(lower))
                        {
                            this.lstReplaceModelSource.SelectedIndex = num;
                            return;
                        }
                        else
                        {
                            num++;
                        }
                    }
                }
            }
        }

        private void btnSearchModelFolder_Click(object sender, EventArgs e)
        {
            this.dlgModelFolder.SelectedPath = this.txtModelFolder.Text;
            if (this.dlgModelFolder.ShowDialog() == DialogResult.OK)
            {
                this.txtModelFolder.Text = this.dlgModelFolder.SelectedPath;
            }
        }

        private void btnSearchOutputFolder_Click(object sender, EventArgs e)
        {
            this.dlgOutputFolder.SelectedPath = this.txtOutputFolder.Text;
            if (this.dlgOutputFolder.ShowDialog() == DialogResult.OK)
            {
                this.txtOutputFolder.Text = this.dlgOutputFolder.SelectedPath;
            }
        }

        private void btnSetAnalysisBeginItem_Click(object sender, EventArgs e)
        {
            if (this.lstAnalysisFileName.SelectedIndex < 0)
            {
                this.txtAnalysisBeginName.Text = string.Empty;
            }
            else
            {
                this.txtAnalysisBeginName.Text = this.lstAnalysisFileName.Items[this.lstAnalysisFileName.SelectedIndex].ToString();
            }
        }

        private void btnSetAnalysisEndItem_Click(object sender, EventArgs e)
        {
            if (this.lstAnalysisFileName.SelectedIndex < 0)
            {
                this.txtAnalysisEndName.Text = string.Empty;
            }
            else
            {
                this.txtAnalysisEndName.Text = this.lstAnalysisFileName.Items[this.lstAnalysisFileName.SelectedIndex].ToString();
            }
        }

        private void btnSetSourceModel_Click(object sender, EventArgs e)
        {
            if (this.lstReplaceModelSource.SelectedIndex >= 0)
            {
                string imageFileFullNameFromListCurrentItem = this.GetImageFileFullNameFromListCurrentItem();
                this.picSourceImage.Tag = imageFileFullNameFromListCurrentItem;
                this.picSourceImage.ImageLocation = imageFileFullNameFromListCurrentItem;
                this.lblSourceImage.Text = Path.GetFileNameWithoutExtension(imageFileFullNameFromListCurrentItem);
            }
            else
            {
                MessageBox.Show("Please select model in the list.");
            }
        }

        private void btnSetTargetModel_Click(object sender, EventArgs e)
        {
            if (this.lstReplaceModelSource.SelectedIndex >= 0)
            {
                string imageFileFullNameFromListCurrentItem = this.GetImageFileFullNameFromListCurrentItem();
                this.picTargetImage.Tag = imageFileFullNameFromListCurrentItem;
                this.picTargetImage.ImageLocation = imageFileFullNameFromListCurrentItem;
                this.lblTargetImage.Text = Path.GetFileNameWithoutExtension(imageFileFullNameFromListCurrentItem);
            }
            else
            {
                MessageBox.Show("Please select model in the list.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetImageFileFullNameFromListCurrentItem()
        {
            string[] strArrays = this.lstReplaceModelSource.SelectedItem.ToString().Split(new char[] { '\\' });
            string str = Path.Combine(Path.Combine(Path.Combine(this.txtOutputFolder.Text, "ImgModelLib"), strArrays[0]), string.Concat(strArrays[1], ".jpg"));
            return str;
        }

        private string GetObjectIDFromImageFileName(string fileName)
        {
            string str;
            string[] strArrays = Path.GetFileNameWithoutExtension(fileName).Split(new char[] { '+' });
            str = ((int)strArrays.Length <= 1 ? string.Empty : strArrays[1]);
            return str;
        }

        private void InitializeAbout()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("本程序作者:纯粹·C·草虫");
            stringBuilder.AppendLine("本程序当前版本:1.0.13.1226");
            stringBuilder.AppendLine("本程序支持的功能:");
            stringBuilder.AppendLine("\u3000\u30001、批量Or个别导出模型图片（格式为jpg、尺寸为800*600）。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30001.1、批量导出可设定要解析文件的范围，最少为1个upk文件，最多为所有upk文件。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30001.2、个别导出支持从指定的upk文件导出和从to本程序拖放的upk文件导出。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30001.3、支持只导出特定种族和性别的图片。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30001.4、不支持识别宠物猫类别，宠物猫模型归入Misc类别。");
            stringBuilder.AppendLine("\u3000\u30002、自动Or手动转换Model file（将来源模型转换为目标模型）。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30002.1、经过转换的文件不会自动替换源文件，需要用户自己放入\"游戏目录\\contents\\Local\\TENCENT\\CHINESES\\CookedPC\"。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30002.2、转换规则是将源模型转换为目标模型，用户在游戏中穿戴源模型后显示为目标模型的样子。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30002.3、转换效果需要重新登录游戏才能生效。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30002.4、自动转换仅支持模型图片显示正常的模型，如果模型图片缺少贴图则转换有风险，如果之后进游戏发现Model conversion failed则请采用手动转换。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30002.5、手动转换支持最多3个源文件对3个目标文件、最少1个源文件对1个目标文件。");
            stringBuilder.AppendLine("\u3000\u3000\u3000\u30002.6、手动转换不做任何校验，用户需要保证文件设定完整、对应关系无误。");
            stringBuilder.AppendLine("本程序不支持的功能:");
            stringBuilder.AppendLine("\u3000\u30001、修改模型本身结构。");
            stringBuilder.AppendLine("\u3000\u30002、制作果体模型（求果体制作方法Orz）。");
            stringBuilder.AppendLine("\u3000\u30003、导入游戏中does not exist的任何模型。");
            stringBuilder.AppendLine("\u3000\u30004、让自己的模型被其他玩家看到。");
            stringBuilder.AppendLine("本程序所在文件夹的核心文件清单:");
            stringBuilder.AppendLine("\u3000\u30001、本程序:剑灵模型处理工具.exe（作者制作）");
            stringBuilder.AppendLine("\u3000\u30002、UEViewer程序:umodel.exe（俄罗斯大神 Gildor 制作）");
            stringBuilder.AppendLine("\u3000\u30003、UEViewer程序组件:SDL.dll（俄罗斯大神 Gildor 制作）");
            stringBuilder.AppendLine("维护说明:");
            stringBuilder.AppendLine("\u3000\u30001、本程序所在文件夹必须包含上述3个文件，否则将出现异常错误。");
            stringBuilder.AppendLine("\u3000\u30002、如果出现模型加密等各种导致不能导出模型图片的问题，请到 http://www.gildor.org/en/projects/umodel 下载最新的UEViewer程序并放入本程序所在文件夹。");
            stringBuilder.AppendLine("\u3000\u30003、如果没有新问题Or需求，本程序不会更新。");
            stringBuilder.AppendLine("需求、问题及BUG反馈:可以使用下列论坛站内信PM我");
            stringBuilder.AppendLine("\u3000\u30001、剑灵官方论坛，ID:capry0518");
            stringBuilder.AppendLine("\u3000\u30002、外游网论坛，ID:純粹の傷，");
            stringBuilder.AppendLine("\u3000\u30003、希望之地论坛，ID:草虫，");
            stringBuilder.AppendLine("免责声明:");
            stringBuilder.AppendLine("\u3000\u30001、本程序严禁用于商业用途，作者制作目的首先是满足自己需要（傻瓜化转换模型），其次是免费分享给广大爱好者们。");
            stringBuilder.AppendLine("\u3000\u30002、作者只会在外游网论坛和希望之地论坛发布及更新，从其他渠道获取的本程序不能保证其真实性和安全性。");
            stringBuilder.AppendLine("\u3000\u30003、本程序不含任何恶意代码、不连接互联网，本程序一旦被杀毒软件报毒，请用户立即删除。");
            stringBuilder.AppendLine("\u3000\u30003、由修改模型而导致的封号结果本作者概不负责，请用户谨慎使用。");
            stringBuilder.AppendLine("\u3000\u30003、个别玩家到处炫耀得瑟自己修改模型的行为与作者无关，得瑟是他们的本性，作者强烈谴责任何藐视官网运营商权利的不理智行为。");
            stringBuilder.AppendLine("作者鸣谢:");
            stringBuilder.AppendLine("\u3000\u30001、Gildor，没有大神做的命令行模型处理工具，就没有本程序的实现基础。");
            stringBuilder.AppendLine("\u3000\u30003、希望之地论坛，没有论坛的学习资料，作者还在数买时装的钞票够不够。");
            stringBuilder.AppendLine("\u3000\u30002、宇智波鼬鼬，没有宇先森的先驱之作，就没有作者的不爽情绪和研发动力。");
            this.txtAbout.Text = stringBuilder.ToString();
        }

        private void InitializeAnalysis()
        {
            AnalysisClass.Initialize(this);
            this.panelLoadingAnalysisFileName.VisibleChanged += new EventHandler(this.panelLoadingAnalysisFileName_VisibleChanged);
            this.panelLoadingAnalysisFileName.Visible = false;
            this.btnBeginAnalysis.Click += new EventHandler(this.btnBeginAnalysis_Click);
            this.btnCancelAnalysis.Click += new EventHandler(this.btnCancelAnalysis_Click);
            this.btnAnalysisCurrentItem.Click += new EventHandler(this.btnAnalysisCurrentItem_Click);
            this.lblAutoAnalysisArea.DragDrop += new DragEventHandler(this.lblAutoAnalysisArea_DragDrop);
            this.lblAutoAnalysisArea.DragEnter += new DragEventHandler(this.lblAutoAnalysisArea_DragEnter);
            this.btnRefreshAnalysisFileName.Click += new EventHandler(this.btnRefreshAnalysisFileName_Click);
            this.btnSearchFileNameWithoutExtension.Click += new EventHandler(this.btnSearchFileNameWithoutExtension_Click);
            this.btnSetAnalysisBeginItem.Click += new EventHandler(this.btnSetAnalysisBeginItem_Click);
            this.btnSetAnalysisEndItem.Click += new EventHandler(this.btnSetAnalysisEndItem_Click);
            this.lstAnalysisFileName.MouseDoubleClick += new MouseEventHandler(this.lstAnalysisFileName_MouseDoubleClick);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tcApp = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearchModelFolder = new System.Windows.Forms.Button();
            this.txtModelFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.rbSetup = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearchOutputFolder = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.tabPageAnalysis = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnRefreshAnalysisFileName = new System.Windows.Forms.Button();
            this.panelLoadingAnalysisFileName = new System.Windows.Forms.Panel();
            this.pbLoadingAnalysisFileName = new System.Windows.Forms.ProgressBar();
            this.label8 = new System.Windows.Forms.Label();
            this.lstAnalysisFileName = new System.Windows.Forms.ListBox();
            this.cmsAnalysisFileName = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnSetAnalysisBeginItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetAnalysisEndItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSearchFileNameWithoutExtension = new System.Windows.Forms.Button();
            this.txtSearchFileNameWithoutExtension = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chkAnalysisOther = new System.Windows.Forms.CheckBox();
            this.chkAnalysisJinM = new System.Windows.Forms.CheckBox();
            this.chkAnalysisJinF = new System.Windows.Forms.CheckBox();
            this.chkAnalysisGonM = new System.Windows.Forms.CheckBox();
            this.chkAnalysisGonF = new System.Windows.Forms.CheckBox();
            this.chkAnalysisLynM = new System.Windows.Forms.CheckBox();
            this.chkAnalysisLynF = new System.Windows.Forms.CheckBox();
            this.chkAnalysisKunN = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtAnalysisEndName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtAnalysisBeginName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblAnalysisProgress = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblCurrentAnalysisFileName = new System.Windows.Forms.Label();
            this.btnBeginAnalysis = new System.Windows.Forms.Button();
            this.btnCancelAnalysis = new System.Windows.Forms.Button();
            this.pbAnalysis = new System.Windows.Forms.ProgressBar();
            this.txtAnalysisInterval = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblAutoAnalysisArea = new System.Windows.Forms.Label();
            this.btnAnalysisCurrentItem = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageAutoReplace = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.chkReplaceGonF = new System.Windows.Forms.CheckBox();
            this.chkReplaceJinM = new System.Windows.Forms.CheckBox();
            this.chkReplaceJinF = new System.Windows.Forms.CheckBox();
            this.chkReplaceGonM = new System.Windows.Forms.CheckBox();
            this.chkReplaceLynM = new System.Windows.Forms.CheckBox();
            this.chkReplaceLynF = new System.Windows.Forms.CheckBox();
            this.chkReplaceKunN = new System.Windows.Forms.CheckBox();
            this.chkReplaceOther = new System.Windows.Forms.CheckBox();
            this.btnGotoManualReplace = new System.Windows.Forms.Button();
            this.btnOpenImageDirectory = new System.Windows.Forms.Button();
            this.lblTargetImage = new System.Windows.Forms.Label();
            this.lblSourceImage = new System.Windows.Forms.Label();
            this.btnImageTargetPicture = new System.Windows.Forms.Button();
            this.btnImageSourcePicture = new System.Windows.Forms.Button();
            this.btnImageViewerPicture = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnSearchImageNameWithoutExtension = new System.Windows.Forms.Button();
            this.txtSearchImageNameWithoutExtension = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.btnRenameImage = new System.Windows.Forms.Button();
            this.panelLoadingReplaceModelSource = new System.Windows.Forms.Panel();
            this.pbLoadingModelSource = new System.Windows.Forms.ProgressBar();
            this.label23 = new System.Windows.Forms.Label();
            this.pbReplace = new System.Windows.Forms.ProgressBar();
            this.btnRefreshReplaceModelSource = new System.Windows.Forms.Button();
            this.btnReplaceModel = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.btnSetTargetModel = new System.Windows.Forms.Button();
            this.btnSetSourceModel = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.picViewerImage = new System.Windows.Forms.PictureBox();
            this.lstReplaceModelSource = new System.Windows.Forms.ListBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.picTargetImage = new System.Windows.Forms.PictureBox();
            this.picSourceImage = new System.Windows.Forms.PictureBox();
            this.tabPageManualReplace = new System.Windows.Forms.TabPage();
            this.txtManualTargetObjectID = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.txtManualSourceObjectID = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.btnManualOpenExportFolder = new System.Windows.Forms.Button();
            this.btnManualSearchExportFolderName = new System.Windows.Forms.Button();
            this.txtManualExportFolderName = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.pbManual = new System.Windows.Forms.ProgressBar();
            this.btnManualBuild = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnManualExportTargetZ = new System.Windows.Forms.Button();
            this.btnManualExportTargetY = new System.Windows.Forms.Button();
            this.btnManualExportTargetX = new System.Windows.Forms.Button();
            this.txtManualTargetFileNameZ = new System.Windows.Forms.TextBox();
            this.txtManualTargetFileNameY = new System.Windows.Forms.TextBox();
            this.txtManualTargetFileNameX = new System.Windows.Forms.TextBox();
            this.btnManualSearchTargetZ = new System.Windows.Forms.Button();
            this.btnManualSearchTargetY = new System.Windows.Forms.Button();
            this.btnManualSearchTargetX = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnManualExportSourceZ = new System.Windows.Forms.Button();
            this.btnManualExportSourceY = new System.Windows.Forms.Button();
            this.btnManualExportSourceX = new System.Windows.Forms.Button();
            this.txtManualSourceFileNameZ = new System.Windows.Forms.TextBox();
            this.txtManualSourceFileNameY = new System.Windows.Forms.TextBox();
            this.txtManualSourceFileNameX = new System.Windows.Forms.TextBox();
            this.btnManualSearchSourceZ = new System.Windows.Forms.Button();
            this.btnManualSearchSourceY = new System.Windows.Forms.Button();
            this.btnManualSearchSourceX = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.txtAbout = new System.Windows.Forms.RichTextBox();
            this.dlgOutputFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgModelFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgManualExportFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.tcApp.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageAnalysis.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelLoadingAnalysisFileName.SuspendLayout();
            this.cmsAnalysisFileName.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPageAutoReplace.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panelLoadingReplaceModelSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picViewerImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTargetImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSourceImage)).BeginInit();
            this.tabPageManualReplace.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcApp
            // 
            this.tcApp.Controls.Add(this.tabPageSettings);
            this.tcApp.Controls.Add(this.tabPageAnalysis);
            this.tcApp.Controls.Add(this.tabPageAutoReplace);
            this.tcApp.Controls.Add(this.tabPageManualReplace);
            this.tcApp.Controls.Add(this.tabPageLog);
            this.tcApp.Controls.Add(this.tabPageAbout);
            this.tcApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcApp.Location = new System.Drawing.Point(0, 0);
            this.tcApp.Name = "tcApp";
            this.tcApp.SelectedIndex = 0;
            this.tcApp.Size = new System.Drawing.Size(991, 609);
            this.tcApp.TabIndex = 0;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.groupBox1);
            this.tabPageSettings.Controls.Add(this.groupBox2);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(983, 583);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearchModelFolder);
            this.groupBox1.Controls.Add(this.txtModelFolder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rbCustom);
            this.groupBox1.Controls.Add(this.rbSetup);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 122);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(977, 458);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Model of the folder setting";
            // 
            // btnSearchModelFolder
            // 
            this.btnSearchModelFolder.Location = new System.Drawing.Point(896, 55);
            this.btnSearchModelFolder.Name = "btnSearchModelFolder";
            this.btnSearchModelFolder.Size = new System.Drawing.Size(75, 25);
            this.btnSearchModelFolder.TabIndex = 4;
            this.btnSearchModelFolder.Text = "Browse";
            this.btnSearchModelFolder.UseVisualStyleBackColor = true;
            // 
            // txtModelFolder
            // 
            this.txtModelFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", Settings.Default, "ModelFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtModelFolder.Location = new System.Drawing.Point(6, 89);
            this.txtModelFolder.Name = "txtModelFolder";
            this.txtModelFolder.Size = new System.Drawing.Size(965, 20);
            this.txtModelFolder.TabIndex = 3;
            this.txtModelFolder.Text = Settings.Default.ModelFolder;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(968, 119);
            this.label1.TabIndex = 2;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // rbCustom
            // 
            this.rbCustom.Checked = Settings.Default.IsCustomFolder;
            this.rbCustom.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsCustomFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rbCustom.Location = new System.Drawing.Point(6, 59);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(121, 17);
            this.rbCustom.TabIndex = 1;
            this.rbCustom.TabStop = true;
            this.rbCustom.Text = "Model storage folder";
            this.rbCustom.UseVisualStyleBackColor = true;
            // 
            // rbSetup
            // 
            this.rbSetup.AutoSize = true;
            this.rbSetup.Checked = Settings.Default.IsSetupFolder;
            this.rbSetup.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsSetupFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rbSetup.Location = new System.Drawing.Point(6, 30);
            this.rbSetup.Name = "rbSetup";
            this.rbSetup.Size = new System.Drawing.Size(134, 17);
            this.rbSetup.TabIndex = 0;
            this.rbSetup.TabStop = true;
            this.rbSetup.Text = "Game installation folder";
            this.rbSetup.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnSearchOutputFolder);
            this.groupBox2.Controls.Add(this.txtOutputFolder);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(977, 119);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Model export folder Settings.Default";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(764, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "The program will create a dedicated folder on this folder.";
            // 
            // btnSearchOutputFolder
            // 
            this.btnSearchOutputFolder.Location = new System.Drawing.Point(896, 23);
            this.btnSearchOutputFolder.Name = "btnSearchOutputFolder";
            this.btnSearchOutputFolder.Size = new System.Drawing.Size(75, 25);
            this.btnSearchOutputFolder.TabIndex = 1;
            this.btnSearchOutputFolder.Text = "Browse";
            this.btnSearchOutputFolder.UseVisualStyleBackColor = true;
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", Settings.Default, "OutputFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOutputFolder.Location = new System.Drawing.Point(6, 54);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(965, 20);
            this.txtOutputFolder.TabIndex = 0;
            this.txtOutputFolder.Text = Settings.Default.OutputFolder;
            // 
            // tabPageAnalysis
            // 
            this.tabPageAnalysis.Controls.Add(this.splitContainer1);
            this.tabPageAnalysis.Controls.Add(this.label5);
            this.tabPageAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalysis.Name = "tabPageAnalysis";
            this.tabPageAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalysis.Size = new System.Drawing.Size(983, 583);
            this.tabPageAnalysis.TabIndex = 1;
            this.tabPageAnalysis.Text = "Image Parser";
            this.tabPageAnalysis.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 30);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnRefreshAnalysisFileName);
            this.splitContainer1.Panel1.Controls.Add(this.panelLoadingAnalysisFileName);
            this.splitContainer1.Panel1.Controls.Add(this.lstAnalysisFileName);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer1.Size = new System.Drawing.Size(977, 550);
            this.splitContainer1.SplitterDistance = 324;
            this.splitContainer1.TabIndex = 1;
            // 
            // btnRefreshAnalysisFileName
            // 
            this.btnRefreshAnalysisFileName.Location = new System.Drawing.Point(249, 525);
            this.btnRefreshAnalysisFileName.Name = "btnRefreshAnalysisFileName";
            this.btnRefreshAnalysisFileName.Size = new System.Drawing.Size(75, 25);
            this.btnRefreshAnalysisFileName.TabIndex = 4;
            this.btnRefreshAnalysisFileName.Text = "Refresh list";
            this.btnRefreshAnalysisFileName.UseVisualStyleBackColor = true;
            // 
            // panelLoadingAnalysisFileName
            // 
            this.panelLoadingAnalysisFileName.Controls.Add(this.pbLoadingAnalysisFileName);
            this.panelLoadingAnalysisFileName.Controls.Add(this.label8);
            this.panelLoadingAnalysisFileName.Location = new System.Drawing.Point(9, 245);
            this.panelLoadingAnalysisFileName.Name = "panelLoadingAnalysisFileName";
            this.panelLoadingAnalysisFileName.Size = new System.Drawing.Size(290, 64);
            this.panelLoadingAnalysisFileName.TabIndex = 3;
            // 
            // pbLoadingAnalysisFileName
            // 
            this.pbLoadingAnalysisFileName.Location = new System.Drawing.Point(3, 28);
            this.pbLoadingAnalysisFileName.Name = "pbLoadingAnalysisFileName";
            this.pbLoadingAnalysisFileName.Size = new System.Drawing.Size(284, 25);
            this.pbLoadingAnalysisFileName.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(109, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Reading files:";
            // 
            // lstAnalysisFileName
            // 
            this.lstAnalysisFileName.ContextMenuStrip = this.cmsAnalysisFileName;
            this.lstAnalysisFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAnalysisFileName.FormattingEnabled = true;
            this.lstAnalysisFileName.HorizontalScrollbar = true;
            this.lstAnalysisFileName.Location = new System.Drawing.Point(0, 61);
            this.lstAnalysisFileName.Name = "lstAnalysisFileName";
            this.lstAnalysisFileName.Size = new System.Drawing.Size(324, 461);
            this.lstAnalysisFileName.TabIndex = 1;
            // 
            // cmsAnalysisFileName
            // 
            this.cmsAnalysisFileName.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSetAnalysisBeginItem,
            this.btnSetAnalysisEndItem});
            this.cmsAnalysisFileName.Name = "cmsAnalysisFileName";
            this.cmsAnalysisFileName.Size = new System.Drawing.Size(194, 48);
            // 
            // btnSetAnalysisBeginItem
            // 
            this.btnSetAnalysisBeginItem.Name = "btnSetAnalysisBeginItem";
            this.btnSetAnalysisBeginItem.Size = new System.Drawing.Size(193, 22);
            this.btnSetAnalysisBeginItem.Text = "Select Parse beginning";
            // 
            // btnSetAnalysisEndItem
            // 
            this.btnSetAnalysisEndItem.Name = "btnSetAnalysisEndItem";
            this.btnSetAnalysisEndItem.Size = new System.Drawing.Size(193, 22);
            this.btnSetAnalysisEndItem.Text = "Select Parse end";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSearchFileNameWithoutExtension);
            this.groupBox3.Controls.Add(this.txtSearchFileNameWithoutExtension);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(324, 61);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Find Files";
            // 
            // btnSearchFileNameWithoutExtension
            // 
            this.btnSearchFileNameWithoutExtension.Location = new System.Drawing.Point(243, 29);
            this.btnSearchFileNameWithoutExtension.Name = "btnSearchFileNameWithoutExtension";
            this.btnSearchFileNameWithoutExtension.Size = new System.Drawing.Size(75, 25);
            this.btnSearchFileNameWithoutExtension.TabIndex = 2;
            this.btnSearchFileNameWithoutExtension.Text = "Find Next";
            this.btnSearchFileNameWithoutExtension.UseVisualStyleBackColor = true;
            // 
            // txtSearchFileNameWithoutExtension
            // 
            this.txtSearchFileNameWithoutExtension.Location = new System.Drawing.Point(9, 32);
            this.txtSearchFileNameWithoutExtension.Name = "txtSearchFileNameWithoutExtension";
            this.txtSearchFileNameWithoutExtension.Size = new System.Drawing.Size(228, 20);
            this.txtSearchFileNameWithoutExtension.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "File name (without extension)";
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(0, 522);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(324, 28);
            this.label4.TabIndex = 2;
            this.label4.Text = "Double-click the list item to open the model image\r\nRight-click to open the conte" +
    "xt menu";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox10);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.txtAnalysisEndName);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.txtAnalysisBeginName);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.lblAnalysisProgress);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.lblCurrentAnalysisFileName);
            this.groupBox4.Controls.Add(this.btnBeginAnalysis);
            this.groupBox4.Controls.Add(this.btnCancelAnalysis);
            this.groupBox4.Controls.Add(this.pbAnalysis);
            this.groupBox4.Controls.Add(this.txtAnalysisInterval);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(649, 344);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Batch parse";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.chkAnalysisOther);
            this.groupBox10.Controls.Add(this.chkAnalysisJinM);
            this.groupBox10.Controls.Add(this.chkAnalysisJinF);
            this.groupBox10.Controls.Add(this.chkAnalysisGonM);
            this.groupBox10.Controls.Add(this.chkAnalysisGonF);
            this.groupBox10.Controls.Add(this.chkAnalysisLynM);
            this.groupBox10.Controls.Add(this.chkAnalysisLynF);
            this.groupBox10.Controls.Add(this.chkAnalysisKunN);
            this.groupBox10.Location = new System.Drawing.Point(9, 51);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(128, 115);
            this.groupBox10.TabIndex = 5;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Sort by";
            // 
            // chkAnalysisOther
            // 
            this.chkAnalysisOther.AutoSize = true;
            this.chkAnalysisOther.Checked = Settings.Default.IsAnalysisOther;
            this.chkAnalysisOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnalysisOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsAnalysisOther", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAnalysisOther.Location = new System.Drawing.Point(72, 88);
            this.chkAnalysisOther.Name = "chkAnalysisOther";
            this.chkAnalysisOther.Size = new System.Drawing.Size(48, 17);
            this.chkAnalysisOther.TabIndex = 26;
            this.chkAnalysisOther.Text = "Misc";
            this.chkAnalysisOther.UseVisualStyleBackColor = true;
            // 
            // chkAnalysisJinM
            // 
            this.chkAnalysisJinM.AutoSize = true;
            this.chkAnalysisJinM.Checked = Settings.Default.IsAnalysisJinM;
            this.chkAnalysisJinM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnalysisJinM.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsAnalysisJinM", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAnalysisJinM.Location = new System.Drawing.Point(6, 19);
            this.chkAnalysisJinM.Name = "chkAnalysisJinM";
            this.chkAnalysisJinM.Size = new System.Drawing.Size(48, 17);
            this.chkAnalysisJinM.TabIndex = 4;
            this.chkAnalysisJinM.Text = "JinM";
            this.chkAnalysisJinM.UseVisualStyleBackColor = true;
            // 
            // chkAnalysisJinF
            // 
            this.chkAnalysisJinF.AutoSize = true;
            this.chkAnalysisJinF.Checked = Settings.Default.IsAnalysisJinF;
            this.chkAnalysisJinF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnalysisJinF.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsAnalysisJinF", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAnalysisJinF.Location = new System.Drawing.Point(72, 19);
            this.chkAnalysisJinF.Name = "chkAnalysisJinF";
            this.chkAnalysisJinF.Size = new System.Drawing.Size(45, 17);
            this.chkAnalysisJinF.TabIndex = 5;
            this.chkAnalysisJinF.Text = "JinF";
            this.chkAnalysisJinF.UseVisualStyleBackColor = true;
            // 
            // chkAnalysisGonM
            // 
            this.chkAnalysisGonM.AutoSize = true;
            this.chkAnalysisGonM.Checked = Settings.Default.IsAnalysisGonM;
            this.chkAnalysisGonM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnalysisGonM.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsAnalysisGonM", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAnalysisGonM.Location = new System.Drawing.Point(6, 42);
            this.chkAnalysisGonM.Name = "chkAnalysisGonM";
            this.chkAnalysisGonM.Size = new System.Drawing.Size(55, 17);
            this.chkAnalysisGonM.TabIndex = 6;
            this.chkAnalysisGonM.Text = "GonM";
            this.chkAnalysisGonM.UseVisualStyleBackColor = true;
            // 
            // chkAnalysisGonF
            // 
            this.chkAnalysisGonF.AutoSize = true;
            this.chkAnalysisGonF.Checked = Settings.Default.IsAnalysisGonF;
            this.chkAnalysisGonF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnalysisGonF.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsAnalysisGonF", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAnalysisGonF.Location = new System.Drawing.Point(72, 42);
            this.chkAnalysisGonF.Name = "chkAnalysisGonF";
            this.chkAnalysisGonF.Size = new System.Drawing.Size(52, 17);
            this.chkAnalysisGonF.TabIndex = 7;
            this.chkAnalysisGonF.Text = "GonF";
            this.chkAnalysisGonF.UseVisualStyleBackColor = true;
            // 
            // chkAnalysisLynM
            // 
            this.chkAnalysisLynM.AutoSize = true;
            this.chkAnalysisLynM.Checked = Settings.Default.IsAnalysisLynM;
            this.chkAnalysisLynM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnalysisLynM.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsAnalysisLynM", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAnalysisLynM.Location = new System.Drawing.Point(6, 65);
            this.chkAnalysisLynM.Name = "chkAnalysisLynM";
            this.chkAnalysisLynM.Size = new System.Drawing.Size(52, 17);
            this.chkAnalysisLynM.TabIndex = 8;
            this.chkAnalysisLynM.Text = "LynM";
            this.chkAnalysisLynM.UseVisualStyleBackColor = true;
            // 
            // chkAnalysisLynF
            // 
            this.chkAnalysisLynF.AutoSize = true;
            this.chkAnalysisLynF.Checked = Settings.Default.IsAnalysisLynF;
            this.chkAnalysisLynF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnalysisLynF.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsAnalysisLynF", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAnalysisLynF.Location = new System.Drawing.Point(72, 65);
            this.chkAnalysisLynF.Name = "chkAnalysisLynF";
            this.chkAnalysisLynF.Size = new System.Drawing.Size(49, 17);
            this.chkAnalysisLynF.TabIndex = 9;
            this.chkAnalysisLynF.Text = "LynF";
            this.chkAnalysisLynF.UseVisualStyleBackColor = true;
            // 
            // chkAnalysisKunN
            // 
            this.chkAnalysisKunN.AutoSize = true;
            this.chkAnalysisKunN.Checked = Settings.Default.IsAnalysisKunN;
            this.chkAnalysisKunN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnalysisKunN.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsAnalysisKunN", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAnalysisKunN.Location = new System.Drawing.Point(6, 89);
            this.chkAnalysisKunN.Name = "chkAnalysisKunN";
            this.chkAnalysisKunN.Size = new System.Drawing.Size(45, 17);
            this.chkAnalysisKunN.TabIndex = 10;
            this.chkAnalysisKunN.Text = "Yun";
            this.chkAnalysisKunN.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(449, 56);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(26, 13);
            this.label22.TabIndex = 25;
            this.label22.Text = "End";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(449, 28);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(29, 13);
            this.label21.TabIndex = 24;
            this.label21.Text = "Start";
            // 
            // txtAnalysisEndName
            // 
            this.txtAnalysisEndName.Location = new System.Drawing.Point(264, 53);
            this.txtAnalysisEndName.Name = "txtAnalysisEndName";
            this.txtAnalysisEndName.Size = new System.Drawing.Size(176, 20);
            this.txtAnalysisEndName.TabIndex = 23;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(170, 60);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "To:";
            // 
            // txtAnalysisBeginName
            // 
            this.txtAnalysisBeginName.Location = new System.Drawing.Point(264, 24);
            this.txtAnalysisBeginName.Name = "txtAnalysisBeginName";
            this.txtAnalysisBeginName.Size = new System.Drawing.Size(176, 20);
            this.txtAnalysisBeginName.TabIndex = 21;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(170, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Range from:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(143, 28);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(21, 13);
            this.label20.TabIndex = 19;
            this.label20.Text = "Ms";
            // 
            // lblAnalysisProgress
            // 
            this.lblAnalysisProgress.AutoSize = true;
            this.lblAnalysisProgress.Location = new System.Drawing.Point(231, 147);
            this.lblAnalysisProgress.Name = "lblAnalysisProgress";
            this.lblAnalysisProgress.Size = new System.Drawing.Size(24, 13);
            this.lblAnalysisProgress.TabIndex = 18;
            this.lblAnalysisProgress.Text = "0/0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(170, 147);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Schedule:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 208);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(637, 130);
            this.label10.TabIndex = 16;
            this.label10.Text = resources.GetString("label10.Text");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(170, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(150, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Current parse Model file name:";
            // 
            // lblCurrentAnalysisFileName
            // 
            this.lblCurrentAnalysisFileName.Location = new System.Drawing.Point(173, 97);
            this.lblCurrentAnalysisFileName.Name = "lblCurrentAnalysisFileName";
            this.lblCurrentAnalysisFileName.Size = new System.Drawing.Size(473, 41);
            this.lblCurrentAnalysisFileName.TabIndex = 14;
            // 
            // btnBeginAnalysis
            // 
            this.btnBeginAnalysis.Location = new System.Drawing.Point(568, 21);
            this.btnBeginAnalysis.Name = "btnBeginAnalysis";
            this.btnBeginAnalysis.Size = new System.Drawing.Size(75, 25);
            this.btnBeginAnalysis.TabIndex = 13;
            this.btnBeginAnalysis.Text = "Start";
            this.btnBeginAnalysis.UseVisualStyleBackColor = true;
            // 
            // btnCancelAnalysis
            // 
            this.btnCancelAnalysis.Enabled = false;
            this.btnCancelAnalysis.Location = new System.Drawing.Point(568, 141);
            this.btnCancelAnalysis.Name = "btnCancelAnalysis";
            this.btnCancelAnalysis.Size = new System.Drawing.Size(75, 25);
            this.btnCancelAnalysis.TabIndex = 12;
            this.btnCancelAnalysis.Text = "Stop";
            this.btnCancelAnalysis.UseVisualStyleBackColor = true;
            // 
            // pbAnalysis
            // 
            this.pbAnalysis.Location = new System.Drawing.Point(8, 170);
            this.pbAnalysis.Name = "pbAnalysis";
            this.pbAnalysis.Size = new System.Drawing.Size(635, 25);
            this.pbAnalysis.TabIndex = 11;
            // 
            // txtAnalysisInterval
            // 
            this.txtAnalysisInterval.DataBindings.Add(new System.Windows.Forms.Binding("Text", Settings.Default, "AnalysisInterval", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtAnalysisInterval.Location = new System.Drawing.Point(81, 25);
            this.txtAnalysisInterval.Name = "txtAnalysisInterval";
            this.txtAnalysisInterval.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtAnalysisInterval.Size = new System.Drawing.Size(56, 20);
            this.txtAnalysisInterval.TabIndex = 2;
            this.txtAnalysisInterval.Text = Settings.Default.AnalysisInterval;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Parse speed:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblAutoAnalysisArea);
            this.groupBox5.Controls.Add(this.btnAnalysisCurrentItem);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox5.Location = new System.Drawing.Point(0, 344);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(649, 206);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Individual parse";
            // 
            // lblAutoAnalysisArea
            // 
            this.lblAutoAnalysisArea.AllowDrop = true;
            this.lblAutoAnalysisArea.BackColor = System.Drawing.Color.LightGreen;
            this.lblAutoAnalysisArea.Location = new System.Drawing.Point(6, 107);
            this.lblAutoAnalysisArea.Name = "lblAutoAnalysisArea";
            this.lblAutoAnalysisArea.Size = new System.Drawing.Size(637, 95);
            this.lblAutoAnalysisArea.TabIndex = 2;
            this.lblAutoAnalysisArea.Text = "Parse area.\r\nDrag and drop files to automatic parse.";
            this.lblAutoAnalysisArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAnalysisCurrentItem
            // 
            this.btnAnalysisCurrentItem.Location = new System.Drawing.Point(235, 52);
            this.btnAnalysisCurrentItem.Name = "btnAnalysisCurrentItem";
            this.btnAnalysisCurrentItem.Size = new System.Drawing.Size(176, 25);
            this.btnAnalysisCurrentItem.TabIndex = 1;
            this.btnAnalysisCurrentItem.Text = "Parse currently selected item";
            this.btnAnalysisCurrentItem.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(408, 65);
            this.label13.TabIndex = 0;
            this.label13.Text = "Analytical method:\r\n\r\n1. Select the list on the left of the Model file.\r\n\r\n2, wil" +
    "l be parsed Model file drag and drop to the area below, note that only the regio" +
    "n.";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(977, 27);
            this.label5.TabIndex = 2;
            this.label5.Text = "This function will export model to image";
            // 
            // tabPageAutoReplace
            // 
            this.tabPageAutoReplace.Controls.Add(this.groupBox9);
            this.tabPageAutoReplace.Controls.Add(this.btnGotoManualReplace);
            this.tabPageAutoReplace.Controls.Add(this.btnOpenImageDirectory);
            this.tabPageAutoReplace.Controls.Add(this.lblTargetImage);
            this.tabPageAutoReplace.Controls.Add(this.lblSourceImage);
            this.tabPageAutoReplace.Controls.Add(this.btnImageTargetPicture);
            this.tabPageAutoReplace.Controls.Add(this.btnImageSourcePicture);
            this.tabPageAutoReplace.Controls.Add(this.btnImageViewerPicture);
            this.tabPageAutoReplace.Controls.Add(this.groupBox6);
            this.tabPageAutoReplace.Controls.Add(this.btnRenameImage);
            this.tabPageAutoReplace.Controls.Add(this.panelLoadingReplaceModelSource);
            this.tabPageAutoReplace.Controls.Add(this.pbReplace);
            this.tabPageAutoReplace.Controls.Add(this.btnRefreshReplaceModelSource);
            this.tabPageAutoReplace.Controls.Add(this.btnReplaceModel);
            this.tabPageAutoReplace.Controls.Add(this.label19);
            this.tabPageAutoReplace.Controls.Add(this.btnSetTargetModel);
            this.tabPageAutoReplace.Controls.Add(this.btnSetSourceModel);
            this.tabPageAutoReplace.Controls.Add(this.label17);
            this.tabPageAutoReplace.Controls.Add(this.picViewerImage);
            this.tabPageAutoReplace.Controls.Add(this.lstReplaceModelSource);
            this.tabPageAutoReplace.Controls.Add(this.label16);
            this.tabPageAutoReplace.Controls.Add(this.label15);
            this.tabPageAutoReplace.Controls.Add(this.picTargetImage);
            this.tabPageAutoReplace.Controls.Add(this.picSourceImage);
            this.tabPageAutoReplace.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutoReplace.Name = "tabPageAutoReplace";
            this.tabPageAutoReplace.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutoReplace.Size = new System.Drawing.Size(983, 583);
            this.tabPageAutoReplace.TabIndex = 2;
            this.tabPageAutoReplace.Text = "Automatic conversion";
            this.tabPageAutoReplace.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.chkReplaceGonF);
            this.groupBox9.Controls.Add(this.chkReplaceJinM);
            this.groupBox9.Controls.Add(this.chkReplaceJinF);
            this.groupBox9.Controls.Add(this.chkReplaceGonM);
            this.groupBox9.Controls.Add(this.chkReplaceLynM);
            this.groupBox9.Controls.Add(this.chkReplaceLynF);
            this.groupBox9.Controls.Add(this.chkReplaceKunN);
            this.groupBox9.Controls.Add(this.chkReplaceOther);
            this.groupBox9.Location = new System.Drawing.Point(245, 7);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(160, 118);
            this.groupBox9.TabIndex = 34;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Sort by";
            // 
            // chkReplaceGonF
            // 
            this.chkReplaceGonF.AutoSize = true;
            this.chkReplaceGonF.Checked = Settings.Default.IsReplaceGonF;
            this.chkReplaceGonF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceGonF.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsReplaceGonF", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceGonF.Location = new System.Drawing.Point(92, 41);
            this.chkReplaceGonF.Name = "chkReplaceGonF";
            this.chkReplaceGonF.Size = new System.Drawing.Size(52, 17);
            this.chkReplaceGonF.TabIndex = 15;
            this.chkReplaceGonF.Text = "GonF";
            this.chkReplaceGonF.UseVisualStyleBackColor = true;
            // 
            // chkReplaceJinM
            // 
            this.chkReplaceJinM.AutoSize = true;
            this.chkReplaceJinM.Checked = Settings.Default.IsReplaceJinM;
            this.chkReplaceJinM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceJinM.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsReplaceJinM", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceJinM.Location = new System.Drawing.Point(26, 17);
            this.chkReplaceJinM.Name = "chkReplaceJinM";
            this.chkReplaceJinM.Size = new System.Drawing.Size(48, 17);
            this.chkReplaceJinM.TabIndex = 12;
            this.chkReplaceJinM.Text = "JinM";
            this.chkReplaceJinM.UseVisualStyleBackColor = true;
            // 
            // chkReplaceJinF
            // 
            this.chkReplaceJinF.AutoSize = true;
            this.chkReplaceJinF.Checked = Settings.Default.IsReplaceJinF;
            this.chkReplaceJinF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceJinF.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsReplaceJinF", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceJinF.Location = new System.Drawing.Point(92, 17);
            this.chkReplaceJinF.Name = "chkReplaceJinF";
            this.chkReplaceJinF.Size = new System.Drawing.Size(45, 17);
            this.chkReplaceJinF.TabIndex = 13;
            this.chkReplaceJinF.Text = "JinF";
            this.chkReplaceJinF.UseVisualStyleBackColor = true;
            // 
            // chkReplaceGonM
            // 
            this.chkReplaceGonM.AutoSize = true;
            this.chkReplaceGonM.Checked = Settings.Default.IsReplaceGonM;
            this.chkReplaceGonM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceGonM.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsReplaceGonM", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceGonM.Location = new System.Drawing.Point(26, 41);
            this.chkReplaceGonM.Name = "chkReplaceGonM";
            this.chkReplaceGonM.Size = new System.Drawing.Size(55, 17);
            this.chkReplaceGonM.TabIndex = 14;
            this.chkReplaceGonM.Text = "GonM";
            this.chkReplaceGonM.UseVisualStyleBackColor = true;
            // 
            // chkReplaceLynM
            // 
            this.chkReplaceLynM.AutoSize = true;
            this.chkReplaceLynM.Checked = Settings.Default.IsReplaceLynM;
            this.chkReplaceLynM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceLynM.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsReplaceLynM", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceLynM.Location = new System.Drawing.Point(26, 64);
            this.chkReplaceLynM.Name = "chkReplaceLynM";
            this.chkReplaceLynM.Size = new System.Drawing.Size(52, 17);
            this.chkReplaceLynM.TabIndex = 16;
            this.chkReplaceLynM.Text = "LynM";
            this.chkReplaceLynM.UseVisualStyleBackColor = true;
            // 
            // chkReplaceLynF
            // 
            this.chkReplaceLynF.AutoSize = true;
            this.chkReplaceLynF.Checked = Settings.Default.IsReplaceLynF;
            this.chkReplaceLynF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceLynF.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsReplaceLynF", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceLynF.Location = new System.Drawing.Point(92, 64);
            this.chkReplaceLynF.Name = "chkReplaceLynF";
            this.chkReplaceLynF.Size = new System.Drawing.Size(49, 17);
            this.chkReplaceLynF.TabIndex = 17;
            this.chkReplaceLynF.Text = "LynF";
            this.chkReplaceLynF.UseVisualStyleBackColor = true;
            // 
            // chkReplaceKunN
            // 
            this.chkReplaceKunN.AutoSize = true;
            this.chkReplaceKunN.Checked = Settings.Default.IsReplaceKunN;
            this.chkReplaceKunN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceKunN.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsReplaceKunN", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceKunN.Location = new System.Drawing.Point(26, 88);
            this.chkReplaceKunN.Name = "chkReplaceKunN";
            this.chkReplaceKunN.Size = new System.Drawing.Size(45, 17);
            this.chkReplaceKunN.TabIndex = 18;
            this.chkReplaceKunN.Text = "Yun";
            this.chkReplaceKunN.UseVisualStyleBackColor = true;
            // 
            // chkReplaceOther
            // 
            this.chkReplaceOther.AutoSize = true;
            this.chkReplaceOther.Checked = Settings.Default.IsReplaceOther;
            this.chkReplaceOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", Settings.Default, "IsReplaceOther", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceOther.Location = new System.Drawing.Point(92, 89);
            this.chkReplaceOther.Name = "chkReplaceOther";
            this.chkReplaceOther.Size = new System.Drawing.Size(48, 17);
            this.chkReplaceOther.TabIndex = 23;
            this.chkReplaceOther.Text = "Misc";
            this.chkReplaceOther.UseVisualStyleBackColor = true;
            // 
            // btnGotoManualReplace
            // 
            this.btnGotoManualReplace.Location = new System.Drawing.Point(482, 519);
            this.btnGotoManualReplace.Name = "btnGotoManualReplace";
            this.btnGotoManualReplace.Size = new System.Drawing.Size(123, 25);
            this.btnGotoManualReplace.TabIndex = 33;
            this.btnGotoManualReplace.Text = "Manual conversion";
            this.btnGotoManualReplace.UseVisualStyleBackColor = true;
            // 
            // btnOpenImageDirectory
            // 
            this.btnOpenImageDirectory.Location = new System.Drawing.Point(3, 549);
            this.btnOpenImageDirectory.Name = "btnOpenImageDirectory";
            this.btnOpenImageDirectory.Size = new System.Drawing.Size(75, 25);
            this.btnOpenImageDirectory.TabIndex = 32;
            this.btnOpenImageDirectory.Text = "Open gallery";
            this.btnOpenImageDirectory.UseVisualStyleBackColor = true;
            // 
            // lblTargetImage
            // 
            this.lblTargetImage.ForeColor = System.Drawing.Color.Black;
            this.lblTargetImage.Location = new System.Drawing.Point(616, 294);
            this.lblTargetImage.Name = "lblTargetImage";
            this.lblTargetImage.Size = new System.Drawing.Size(296, 10);
            this.lblTargetImage.TabIndex = 31;
            // 
            // lblSourceImage
            // 
            this.lblSourceImage.Location = new System.Drawing.Point(616, 7);
            this.lblSourceImage.Name = "lblSourceImage";
            this.lblSourceImage.Size = new System.Drawing.Size(296, 13);
            this.lblSourceImage.TabIndex = 30;
            // 
            // btnImageTargetPicture
            // 
            this.btnImageTargetPicture.Location = new System.Drawing.Point(918, 292);
            this.btnImageTargetPicture.Name = "btnImageTargetPicture";
            this.btnImageTargetPicture.Size = new System.Drawing.Size(57, 25);
            this.btnImageTargetPicture.TabIndex = 29;
            this.btnImageTargetPicture.Text = "Picture";
            this.btnImageTargetPicture.UseVisualStyleBackColor = true;
            // 
            // btnImageSourcePicture
            // 
            this.btnImageSourcePicture.Location = new System.Drawing.Point(918, 6);
            this.btnImageSourcePicture.Name = "btnImageSourcePicture";
            this.btnImageSourcePicture.Size = new System.Drawing.Size(57, 25);
            this.btnImageSourcePicture.TabIndex = 28;
            this.btnImageSourcePicture.Text = "Picture";
            this.btnImageSourcePicture.UseVisualStyleBackColor = true;
            // 
            // btnImageViewerPicture
            // 
            this.btnImageViewerPicture.Location = new System.Drawing.Point(556, 151);
            this.btnImageViewerPicture.Name = "btnImageViewerPicture";
            this.btnImageViewerPicture.Size = new System.Drawing.Size(49, 25);
            this.btnImageViewerPicture.TabIndex = 27;
            this.btnImageViewerPicture.Text = "Picture";
            this.btnImageViewerPicture.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnSearchImageNameWithoutExtension);
            this.groupBox6.Controls.Add(this.txtSearchImageNameWithoutExtension);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Location = new System.Drawing.Point(6, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(230, 81);
            this.groupBox6.TabIndex = 26;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Find Files";
            // 
            // btnSearchImageNameWithoutExtension
            // 
            this.btnSearchImageNameWithoutExtension.Location = new System.Drawing.Point(155, 21);
            this.btnSearchImageNameWithoutExtension.Name = "btnSearchImageNameWithoutExtension";
            this.btnSearchImageNameWithoutExtension.Size = new System.Drawing.Size(69, 25);
            this.btnSearchImageNameWithoutExtension.TabIndex = 2;
            this.btnSearchImageNameWithoutExtension.Text = "Find Next";
            this.btnSearchImageNameWithoutExtension.UseVisualStyleBackColor = true;
            // 
            // txtSearchImageNameWithoutExtension
            // 
            this.txtSearchImageNameWithoutExtension.Location = new System.Drawing.Point(8, 49);
            this.txtSearchImageNameWithoutExtension.Name = "txtSearchImageNameWithoutExtension";
            this.txtSearchImageNameWithoutExtension.Size = new System.Drawing.Size(216, 20);
            this.txtSearchImageNameWithoutExtension.TabIndex = 1;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 27);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(146, 13);
            this.label24.TabIndex = 0;
            this.label24.Text = "File name (Without extension)";
            // 
            // btnRenameImage
            // 
            this.btnRenameImage.Location = new System.Drawing.Point(242, 439);
            this.btnRenameImage.Name = "btnRenameImage";
            this.btnRenameImage.Size = new System.Drawing.Size(123, 25);
            this.btnRenameImage.TabIndex = 25;
            this.btnRenameImage.Text = "Add a note";
            this.btnRenameImage.UseVisualStyleBackColor = true;
            // 
            // panelLoadingReplaceModelSource
            // 
            this.panelLoadingReplaceModelSource.Controls.Add(this.pbLoadingModelSource);
            this.panelLoadingReplaceModelSource.Controls.Add(this.label23);
            this.panelLoadingReplaceModelSource.Location = new System.Drawing.Point(28, 275);
            this.panelLoadingReplaceModelSource.Name = "panelLoadingReplaceModelSource";
            this.panelLoadingReplaceModelSource.Size = new System.Drawing.Size(183, 64);
            this.panelLoadingReplaceModelSource.TabIndex = 24;
            // 
            // pbLoadingModelSource
            // 
            this.pbLoadingModelSource.Location = new System.Drawing.Point(2, 28);
            this.pbLoadingModelSource.Name = "pbLoadingModelSource";
            this.pbLoadingModelSource.Size = new System.Drawing.Size(179, 25);
            this.pbLoadingModelSource.TabIndex = 1;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(59, 12);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(68, 13);
            this.label23.TabIndex = 0;
            this.label23.Text = "Reading files";
            // 
            // pbReplace
            // 
            this.pbReplace.Location = new System.Drawing.Point(245, 553);
            this.pbReplace.Name = "pbReplace";
            this.pbReplace.Size = new System.Drawing.Size(228, 17);
            this.pbReplace.TabIndex = 22;
            // 
            // btnRefreshReplaceModelSource
            // 
            this.btnRefreshReplaceModelSource.Location = new System.Drawing.Point(161, 549);
            this.btnRefreshReplaceModelSource.Name = "btnRefreshReplaceModelSource";
            this.btnRefreshReplaceModelSource.Size = new System.Drawing.Size(75, 25);
            this.btnRefreshReplaceModelSource.TabIndex = 21;
            this.btnRefreshReplaceModelSource.Text = "Refresh list";
            this.btnRefreshReplaceModelSource.UseVisualStyleBackColor = true;
            // 
            // btnReplaceModel
            // 
            this.btnReplaceModel.Location = new System.Drawing.Point(244, 521);
            this.btnReplaceModel.Name = "btnReplaceModel";
            this.btnReplaceModel.Size = new System.Drawing.Size(121, 25);
            this.btnReplaceModel.TabIndex = 20;
            this.btnReplaceModel.Text = "Generate New Model";
            this.btnReplaceModel.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(242, 470);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(363, 48);
            this.label19.TabIndex = 19;
            this.label19.Text = "This function will replace Origin model to Target model to a new file.\r\nYou can b" +
    "rowse all models trhoug gallery.\r\nIf not work you can try manual conversion.";
            // 
            // btnSetTargetModel
            // 
            this.btnSetTargetModel.Location = new System.Drawing.Point(482, 439);
            this.btnSetTargetModel.Name = "btnSetTargetModel";
            this.btnSetTargetModel.Size = new System.Drawing.Size(123, 25);
            this.btnSetTargetModel.TabIndex = 7;
            this.btnSetTargetModel.Text = "Target Model";
            this.btnSetTargetModel.UseVisualStyleBackColor = true;
            // 
            // btnSetSourceModel
            // 
            this.btnSetSourceModel.Location = new System.Drawing.Point(510, 120);
            this.btnSetSourceModel.Name = "btnSetSourceModel";
            this.btnSetSourceModel.Size = new System.Drawing.Size(95, 25);
            this.btnSetSourceModel.TabIndex = 0;
            this.btnSetSourceModel.Text = "Source model";
            this.btnSetSourceModel.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(242, 135);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "Preview Model:";
            // 
            // picViewerImage
            // 
            this.picViewerImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picViewerImage.Location = new System.Drawing.Point(245, 151);
            this.picViewerImage.Name = "picViewerImage";
            this.picViewerImage.Size = new System.Drawing.Size(360, 282);
            this.picViewerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picViewerImage.TabIndex = 5;
            this.picViewerImage.TabStop = false;
            // 
            // lstReplaceModelSource
            // 
            this.lstReplaceModelSource.FormattingEnabled = true;
            this.lstReplaceModelSource.HorizontalScrollbar = true;
            this.lstReplaceModelSource.Location = new System.Drawing.Point(6, 98);
            this.lstReplaceModelSource.Name = "lstReplaceModelSource";
            this.lstReplaceModelSource.Size = new System.Drawing.Size(230, 446);
            this.lstReplaceModelSource.TabIndex = 4;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(536, 561);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(73, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "Target Model:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(534, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Source Model:";
            // 
            // picTargetImage
            // 
            this.picTargetImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picTargetImage.Location = new System.Drawing.Point(615, 292);
            this.picTargetImage.Name = "picTargetImage";
            this.picTargetImage.Size = new System.Drawing.Size(360, 282);
            this.picTargetImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTargetImage.TabIndex = 1;
            this.picTargetImage.TabStop = false;
            // 
            // picSourceImage
            // 
            this.picSourceImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSourceImage.Location = new System.Drawing.Point(615, 6);
            this.picSourceImage.Name = "picSourceImage";
            this.picSourceImage.Size = new System.Drawing.Size(360, 282);
            this.picSourceImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSourceImage.TabIndex = 0;
            this.picSourceImage.TabStop = false;
            // 
            // tabPageManualReplace
            // 
            this.tabPageManualReplace.Controls.Add(this.txtManualTargetObjectID);
            this.tabPageManualReplace.Controls.Add(this.label43);
            this.tabPageManualReplace.Controls.Add(this.txtManualSourceObjectID);
            this.tabPageManualReplace.Controls.Add(this.label41);
            this.tabPageManualReplace.Controls.Add(this.btnManualOpenExportFolder);
            this.tabPageManualReplace.Controls.Add(this.btnManualSearchExportFolderName);
            this.tabPageManualReplace.Controls.Add(this.txtManualExportFolderName);
            this.tabPageManualReplace.Controls.Add(this.label40);
            this.tabPageManualReplace.Controls.Add(this.label39);
            this.tabPageManualReplace.Controls.Add(this.pbManual);
            this.tabPageManualReplace.Controls.Add(this.btnManualBuild);
            this.tabPageManualReplace.Controls.Add(this.groupBox8);
            this.tabPageManualReplace.Controls.Add(this.groupBox7);
            this.tabPageManualReplace.Location = new System.Drawing.Point(4, 22);
            this.tabPageManualReplace.Name = "tabPageManualReplace";
            this.tabPageManualReplace.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageManualReplace.Size = new System.Drawing.Size(983, 583);
            this.tabPageManualReplace.TabIndex = 5;
            this.tabPageManualReplace.Text = "Manual Conversion";
            this.tabPageManualReplace.UseVisualStyleBackColor = true;
            // 
            // txtManualTargetObjectID
            // 
            this.txtManualTargetObjectID.Location = new System.Drawing.Point(714, 292);
            this.txtManualTargetObjectID.Name = "txtManualTargetObjectID";
            this.txtManualTargetObjectID.Size = new System.Drawing.Size(261, 20);
            this.txtManualTargetObjectID.TabIndex = 34;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(598, 295);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(110, 13);
            this.label43.TabIndex = 33;
            this.label43.Text = "Target model number:";
            // 
            // txtManualSourceObjectID
            // 
            this.txtManualSourceObjectID.Location = new System.Drawing.Point(130, 291);
            this.txtManualSourceObjectID.Name = "txtManualSourceObjectID";
            this.txtManualSourceObjectID.Size = new System.Drawing.Size(246, 20);
            this.txtManualSourceObjectID.TabIndex = 31;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(11, 295);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(113, 13);
            this.label41.TabIndex = 29;
            this.label41.Text = "Source model number:";
            // 
            // btnManualOpenExportFolder
            // 
            this.btnManualOpenExportFolder.Location = new System.Drawing.Point(855, 318);
            this.btnManualOpenExportFolder.Name = "btnManualOpenExportFolder";
            this.btnManualOpenExportFolder.Size = new System.Drawing.Size(120, 25);
            this.btnManualOpenExportFolder.TabIndex = 28;
            this.btnManualOpenExportFolder.Text = "Open the folder";
            this.btnManualOpenExportFolder.UseVisualStyleBackColor = true;
            // 
            // btnManualSearchExportFolderName
            // 
            this.btnManualSearchExportFolderName.Location = new System.Drawing.Point(859, 12);
            this.btnManualSearchExportFolderName.Name = "btnManualSearchExportFolderName";
            this.btnManualSearchExportFolderName.Size = new System.Drawing.Size(110, 25);
            this.btnManualSearchExportFolderName.TabIndex = 27;
            this.btnManualSearchExportFolderName.Text = "Browse";
            this.btnManualSearchExportFolderName.UseVisualStyleBackColor = true;
            // 
            // txtManualExportFolderName
            // 
            this.txtManualExportFolderName.Location = new System.Drawing.Point(127, 15);
            this.txtManualExportFolderName.Name = "txtManualExportFolderName";
            this.txtManualExportFolderName.Size = new System.Drawing.Size(726, 20);
            this.txtManualExportFolderName.TabIndex = 26;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(14, 18);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(112, 13);
            this.label40.TabIndex = 25;
            this.label40.Text = "Export folder Full path:";
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(9, 361);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(966, 215);
            this.label39.TabIndex = 24;
            this.label39.Text = resources.GetString("label39.Text");
            // 
            // pbManual
            // 
            this.pbManual.Location = new System.Drawing.Point(138, 318);
            this.pbManual.Name = "pbManual";
            this.pbManual.Size = new System.Drawing.Size(711, 24);
            this.pbManual.TabIndex = 23;
            // 
            // btnManualBuild
            // 
            this.btnManualBuild.Location = new System.Drawing.Point(12, 317);
            this.btnManualBuild.Name = "btnManualBuild";
            this.btnManualBuild.Size = new System.Drawing.Size(120, 25);
            this.btnManualBuild.TabIndex = 3;
            this.btnManualBuild.Text = "Generate New Model";
            this.btnManualBuild.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnManualExportTargetZ);
            this.groupBox8.Controls.Add(this.btnManualExportTargetY);
            this.groupBox8.Controls.Add(this.btnManualExportTargetX);
            this.groupBox8.Controls.Add(this.txtManualTargetFileNameZ);
            this.groupBox8.Controls.Add(this.txtManualTargetFileNameY);
            this.groupBox8.Controls.Add(this.txtManualTargetFileNameX);
            this.groupBox8.Controls.Add(this.btnManualSearchTargetZ);
            this.groupBox8.Controls.Add(this.btnManualSearchTargetY);
            this.groupBox8.Controls.Add(this.btnManualSearchTargetX);
            this.groupBox8.Controls.Add(this.label28);
            this.groupBox8.Controls.Add(this.label29);
            this.groupBox8.Controls.Add(this.label30);
            this.groupBox8.Location = new System.Drawing.Point(8, 167);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(967, 114);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Target file model ";
            // 
            // btnManualExportTargetZ
            // 
            this.btnManualExportTargetZ.Location = new System.Drawing.Point(907, 77);
            this.btnManualExportTargetZ.Name = "btnManualExportTargetZ";
            this.btnManualExportTargetZ.Size = new System.Drawing.Size(54, 25);
            this.btnManualExportTargetZ.TabIndex = 11;
            this.btnManualExportTargetZ.Text = "Unpack";
            this.btnManualExportTargetZ.UseVisualStyleBackColor = true;
            // 
            // btnManualExportTargetY
            // 
            this.btnManualExportTargetY.Location = new System.Drawing.Point(907, 48);
            this.btnManualExportTargetY.Name = "btnManualExportTargetY";
            this.btnManualExportTargetY.Size = new System.Drawing.Size(54, 25);
            this.btnManualExportTargetY.TabIndex = 10;
            this.btnManualExportTargetY.Text = "Unpack";
            this.btnManualExportTargetY.UseVisualStyleBackColor = true;
            // 
            // btnManualExportTargetX
            // 
            this.btnManualExportTargetX.Location = new System.Drawing.Point(907, 19);
            this.btnManualExportTargetX.Name = "btnManualExportTargetX";
            this.btnManualExportTargetX.Size = new System.Drawing.Size(54, 25);
            this.btnManualExportTargetX.TabIndex = 9;
            this.btnManualExportTargetX.Text = "Unpack";
            this.btnManualExportTargetX.UseVisualStyleBackColor = true;
            // 
            // txtManualTargetFileNameZ
            // 
            this.txtManualTargetFileNameZ.Location = new System.Drawing.Point(120, 80);
            this.txtManualTargetFileNameZ.Name = "txtManualTargetFileNameZ";
            this.txtManualTargetFileNameZ.Size = new System.Drawing.Size(725, 20);
            this.txtManualTargetFileNameZ.TabIndex = 8;
            // 
            // txtManualTargetFileNameY
            // 
            this.txtManualTargetFileNameY.Location = new System.Drawing.Point(120, 51);
            this.txtManualTargetFileNameY.Name = "txtManualTargetFileNameY";
            this.txtManualTargetFileNameY.Size = new System.Drawing.Size(725, 20);
            this.txtManualTargetFileNameY.TabIndex = 7;
            // 
            // txtManualTargetFileNameX
            // 
            this.txtManualTargetFileNameX.Location = new System.Drawing.Point(120, 22);
            this.txtManualTargetFileNameX.Name = "txtManualTargetFileNameX";
            this.txtManualTargetFileNameX.Size = new System.Drawing.Size(725, 20);
            this.txtManualTargetFileNameX.TabIndex = 6;
            // 
            // btnManualSearchTargetZ
            // 
            this.btnManualSearchTargetZ.Location = new System.Drawing.Point(851, 77);
            this.btnManualSearchTargetZ.Name = "btnManualSearchTargetZ";
            this.btnManualSearchTargetZ.Size = new System.Drawing.Size(50, 25);
            this.btnManualSearchTargetZ.TabIndex = 5;
            this.btnManualSearchTargetZ.Text = "Browse";
            this.btnManualSearchTargetZ.UseVisualStyleBackColor = true;
            // 
            // btnManualSearchTargetY
            // 
            this.btnManualSearchTargetY.Location = new System.Drawing.Point(851, 48);
            this.btnManualSearchTargetY.Name = "btnManualSearchTargetY";
            this.btnManualSearchTargetY.Size = new System.Drawing.Size(50, 25);
            this.btnManualSearchTargetY.TabIndex = 4;
            this.btnManualSearchTargetY.Text = "Browse";
            this.btnManualSearchTargetY.UseVisualStyleBackColor = true;
            // 
            // btnManualSearchTargetX
            // 
            this.btnManualSearchTargetX.Location = new System.Drawing.Point(851, 19);
            this.btnManualSearchTargetX.Name = "btnManualSearchTargetX";
            this.btnManualSearchTargetX.Size = new System.Drawing.Size(50, 25);
            this.btnManualSearchTargetX.TabIndex = 3;
            this.btnManualSearchTargetX.Text = "Browse";
            this.btnManualSearchTargetX.UseVisualStyleBackColor = true;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 83);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(108, 13);
            this.label28.TabIndex = 2;
            this.label28.Text = "Model file Z Full path:";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(6, 54);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(108, 13);
            this.label29.TabIndex = 1;
            this.label29.Text = "Model file Y Full path:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(6, 25);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(108, 13);
            this.label30.TabIndex = 0;
            this.label30.Text = "Model file X Full path:";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnManualExportSourceZ);
            this.groupBox7.Controls.Add(this.btnManualExportSourceY);
            this.groupBox7.Controls.Add(this.btnManualExportSourceX);
            this.groupBox7.Controls.Add(this.txtManualSourceFileNameZ);
            this.groupBox7.Controls.Add(this.txtManualSourceFileNameY);
            this.groupBox7.Controls.Add(this.txtManualSourceFileNameX);
            this.groupBox7.Controls.Add(this.btnManualSearchSourceZ);
            this.groupBox7.Controls.Add(this.btnManualSearchSourceY);
            this.groupBox7.Controls.Add(this.btnManualSearchSourceX);
            this.groupBox7.Controls.Add(this.label27);
            this.groupBox7.Controls.Add(this.label26);
            this.groupBox7.Controls.Add(this.label25);
            this.groupBox7.Location = new System.Drawing.Point(8, 47);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(967, 114);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Source file model";
            // 
            // btnManualExportSourceZ
            // 
            this.btnManualExportSourceZ.Location = new System.Drawing.Point(907, 76);
            this.btnManualExportSourceZ.Name = "btnManualExportSourceZ";
            this.btnManualExportSourceZ.Size = new System.Drawing.Size(54, 25);
            this.btnManualExportSourceZ.TabIndex = 13;
            this.btnManualExportSourceZ.Text = "Unpack";
            this.btnManualExportSourceZ.UseVisualStyleBackColor = true;
            // 
            // btnManualExportSourceY
            // 
            this.btnManualExportSourceY.Location = new System.Drawing.Point(907, 49);
            this.btnManualExportSourceY.Name = "btnManualExportSourceY";
            this.btnManualExportSourceY.Size = new System.Drawing.Size(54, 25);
            this.btnManualExportSourceY.TabIndex = 11;
            this.btnManualExportSourceY.Text = "Unpack";
            this.btnManualExportSourceY.UseVisualStyleBackColor = true;
            // 
            // btnManualExportSourceX
            // 
            this.btnManualExportSourceX.Location = new System.Drawing.Point(907, 19);
            this.btnManualExportSourceX.Name = "btnManualExportSourceX";
            this.btnManualExportSourceX.Size = new System.Drawing.Size(54, 25);
            this.btnManualExportSourceX.TabIndex = 9;
            this.btnManualExportSourceX.Text = "Unpack";
            this.btnManualExportSourceX.UseVisualStyleBackColor = true;
            // 
            // txtManualSourceFileNameZ
            // 
            this.txtManualSourceFileNameZ.Location = new System.Drawing.Point(120, 80);
            this.txtManualSourceFileNameZ.Name = "txtManualSourceFileNameZ";
            this.txtManualSourceFileNameZ.Size = new System.Drawing.Size(725, 20);
            this.txtManualSourceFileNameZ.TabIndex = 8;
            // 
            // txtManualSourceFileNameY
            // 
            this.txtManualSourceFileNameY.Location = new System.Drawing.Point(120, 51);
            this.txtManualSourceFileNameY.Name = "txtManualSourceFileNameY";
            this.txtManualSourceFileNameY.Size = new System.Drawing.Size(725, 20);
            this.txtManualSourceFileNameY.TabIndex = 7;
            // 
            // txtManualSourceFileNameX
            // 
            this.txtManualSourceFileNameX.Location = new System.Drawing.Point(120, 22);
            this.txtManualSourceFileNameX.Name = "txtManualSourceFileNameX";
            this.txtManualSourceFileNameX.Size = new System.Drawing.Size(725, 20);
            this.txtManualSourceFileNameX.TabIndex = 6;
            // 
            // btnManualSearchSourceZ
            // 
            this.btnManualSearchSourceZ.Location = new System.Drawing.Point(851, 76);
            this.btnManualSearchSourceZ.Name = "btnManualSearchSourceZ";
            this.btnManualSearchSourceZ.Size = new System.Drawing.Size(50, 25);
            this.btnManualSearchSourceZ.TabIndex = 5;
            this.btnManualSearchSourceZ.Text = "Browse";
            this.btnManualSearchSourceZ.UseVisualStyleBackColor = true;
            // 
            // btnManualSearchSourceY
            // 
            this.btnManualSearchSourceY.Location = new System.Drawing.Point(851, 48);
            this.btnManualSearchSourceY.Name = "btnManualSearchSourceY";
            this.btnManualSearchSourceY.Size = new System.Drawing.Size(50, 25);
            this.btnManualSearchSourceY.TabIndex = 4;
            this.btnManualSearchSourceY.Text = "Browse";
            this.btnManualSearchSourceY.UseVisualStyleBackColor = true;
            // 
            // btnManualSearchSourceX
            // 
            this.btnManualSearchSourceX.Location = new System.Drawing.Point(851, 19);
            this.btnManualSearchSourceX.Name = "btnManualSearchSourceX";
            this.btnManualSearchSourceX.Size = new System.Drawing.Size(50, 25);
            this.btnManualSearchSourceX.TabIndex = 3;
            this.btnManualSearchSourceX.Text = "Browse";
            this.btnManualSearchSourceX.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 82);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(108, 13);
            this.label27.TabIndex = 2;
            this.label27.Text = "Model file Z Full path:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(6, 54);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(108, 13);
            this.label26.TabIndex = 1;
            this.label26.Text = "Model file Y Full path:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(6, 25);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(108, 13);
            this.label25.TabIndex = 0;
            this.label25.Text = "Model file X Full path:";
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.btnClearLog);
            this.tabPageLog.Controls.Add(this.txtLog);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(983, 583);
            this.tabPageLog.TabIndex = 3;
            this.tabPageLog.Text = "Logs";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(900, 7);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 25);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "Clear";
            this.btnClearLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.AcceptsTab = true;
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Location = new System.Drawing.Point(3, 40);
            this.txtLog.Margin = new System.Windows.Forms.Padding(5);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(975, 537);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.Controls.Add(this.txtAbout);
            this.tabPageAbout.Location = new System.Drawing.Point(4, 22);
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAbout.Size = new System.Drawing.Size(983, 583);
            this.tabPageAbout.TabIndex = 4;
            this.tabPageAbout.Text = "about";
            this.tabPageAbout.UseVisualStyleBackColor = true;
            // 
            // txtAbout
            // 
            this.txtAbout.AcceptsTab = true;
            this.txtAbout.BackColor = System.Drawing.Color.White;
            this.txtAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAbout.Location = new System.Drawing.Point(10, 9);
            this.txtAbout.Margin = new System.Windows.Forms.Padding(5);
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.ReadOnly = true;
            this.txtAbout.Size = new System.Drawing.Size(963, 561);
            this.txtAbout.TabIndex = 1;
            this.txtAbout.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 609);
            this.Controls.Add(this.tcApp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BnS UPK model manipulation tools V1.0.13.1226 by 纯粹之伤";
            this.tcApp.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPageAnalysis.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelLoadingAnalysisFileName.ResumeLayout(false);
            this.panelLoadingAnalysisFileName.PerformLayout();
            this.cmsAnalysisFileName.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPageAutoReplace.ResumeLayout(false);
            this.tabPageAutoReplace.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panelLoadingReplaceModelSource.ResumeLayout(false);
            this.panelLoadingReplaceModelSource.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picViewerImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTargetImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSourceImage)).EndInit();
            this.tabPageManualReplace.ResumeLayout(false);
            this.tabPageManualReplace.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabPageLog.ResumeLayout(false);
            this.tabPageAbout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void InitializeLog()
        {
            LogClass.Initialize(this);
            this.btnClearLog.Click += new EventHandler(this.btnClearLog_Click);
        }

        private void InitializeManual()
        {
            ManualClass.Initialize(this);
            this.btnManualSearchExportFolderName.Click += new EventHandler(this.btnManualSearchExportFolderName_Click);
            this.btnManualSearchSourceX.Click += new EventHandler(this.btnManualSearchSourceX_Click);
            this.btnManualSearchSourceY.Click += new EventHandler(this.btnManualSearchSourceY_Click);
            this.btnManualSearchSourceZ.Click += new EventHandler(this.btnManualSearchSourceZ_Click);
            this.btnManualSearchTargetX.Click += new EventHandler(this.btnManualSearchTargetX_Click);
            this.btnManualSearchTargetY.Click += new EventHandler(this.btnManualSearchTargetY_Click);
            this.btnManualSearchTargetZ.Click += new EventHandler(this.btnManualSearchTargetZ_Click);
            this.btnManualExportSourceX.Click += new EventHandler(this.btnManualExportSourceX_Click);
            this.btnManualExportSourceY.Click += new EventHandler(this.btnManualExportSourceY_Click);
            this.btnManualExportSourceZ.Click += new EventHandler(this.btnManualExportSourceZ_Click);
            this.btnManualExportTargetX.Click += new EventHandler(this.btnManualExportTargetX_Click);
            this.btnManualExportTargetY.Click += new EventHandler(this.btnManualExportTargetY_Click);
            this.btnManualExportTargetZ.Click += new EventHandler(this.btnManualExportTargetZ_Click);
            this.btnManualOpenExportFolder.Click += new EventHandler(this.btnManualOpenExportFolder_Click);
            this.btnManualBuild.Click += new EventHandler(this.btnManualBuild_Click);
        }

        private void InitializeOnLoaded()
        {
            this.InitializeSettings();
            this.InitializeAnalysis();
            this.InitializeReplace();
            this.InitializeLog();
            this.InitializeManual();
            this.InitializeAbout();
            this.tcApp.SelectedIndexChanged += new EventHandler(this.tcApp_SelectedIndexChanged);
            base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
        }

        private void InitializeReplace()
        {
            ReplaceClass.Initialize(this);
            this.panelLoadingReplaceModelSource.VisibleChanged += new EventHandler(this.panelLoadingReplaceModelSource_VisibleChanged);
            this.panelLoadingReplaceModelSource.Visible = false;
            this.btnSetSourceModel.Click += new EventHandler(this.btnSetSourceModel_Click);
            this.btnSetTargetModel.Click += new EventHandler(this.btnSetTargetModel_Click);
            this.btnSearchImageNameWithoutExtension.Click += new EventHandler(this.btnSearchImageNameWithoutExtension_Click);
            this.lstReplaceModelSource.SelectedIndexChanged += new EventHandler(this.lstReplaceModelSource_SelectedIndexChanged);
            this.btnRenameImage.Click += new EventHandler(this.btnRenameImage_Click);
            this.btnImageViewerPicture.Click += new EventHandler(this.btnImageViewerPicture_Click);
            this.btnImageSourcePicture.Click += new EventHandler(this.btnImageSourcePicture_Click);
            this.btnImageTargetPicture.Click += new EventHandler(this.btnImageTargetPicture_Click);
            this.btnRefreshReplaceModelSource.Click += new EventHandler(this.btnRefreshReplaceModelSource_Click);
            this.btnReplaceModel.Click += new EventHandler(this.btnReplaceModel_Click);
            this.btnOpenImageDirectory.Click += new EventHandler(this.btnOpenImageDirectory_Click);
            this.btnGotoManualReplace.Click += new EventHandler(this.btnGotoManualReplace_Click);
        }

        private void InitializeSettings()
        {
            SettingsClass.Initialize(this);
            this.btnSearchOutputFolder.Click += new EventHandler(this.btnSearchOutputFolder_Click);
            this.btnSearchModelFolder.Click += new EventHandler(this.btnSearchModelFolder_Click);
            this.rbSetup.CheckedChanged += new EventHandler(this.rbSetup_CheckedChanged);
            this.rbCustom.CheckedChanged += new EventHandler(this.rbCustom_CheckedChanged);
        }

        private void lblAutoAnalysisArea_DragDrop(object sender, DragEventArgs e)
        {
            AnalysisClass.DropItem = e.Data;
            this.OnDoOperation(Operations.DragAnalysis);
        }

        private void lblAutoAnalysisArea_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        private void lstAnalysisFileName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.OnDoOperation(Operations.LookModel);
        }

        private void lstReplaceModelSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstReplaceModelSource.SelectedIndex >= 0)
            {
                string imageFileFullNameFromListCurrentItem = this.GetImageFileFullNameFromListCurrentItem();
                this.picViewerImage.Tag = imageFileFullNameFromListCurrentItem;
                this.picViewerImage.ImageLocation = imageFileFullNameFromListCurrentItem;
            }
            else
            {
                this.picViewerImage.ImageLocation = null;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        internal void OnDoOperation(Operations operation)
        {
            if (this.DoOperation != null)
            {
                this.DoOperation(this, new OperationEventArgs(operation));
            }
            this.currentOperation = operation;
        }

        private void panelLoadingAnalysisFileName_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.panelLoadingAnalysisFileName.Visible)
            {
                this.pbLoadingAnalysisFileName.Style = ProgressBarStyle.Blocks;
            }
            else
            {
                this.pbLoadingAnalysisFileName.Style = ProgressBarStyle.Marquee;
            }
        }

        private void panelLoadingReplaceModelSource_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.panelLoadingReplaceModelSource.Visible)
            {
                this.pbLoadingModelSource.Style = ProgressBarStyle.Blocks;
            }
            else
            {
                this.pbLoadingModelSource.Style = ProgressBarStyle.Marquee;
            }
        }

        private void rbCustom_CheckedChanged(object sender, EventArgs e)
        {
            this.rbSetup.Checked = !this.rbCustom.Checked;
        }

        private void rbSetup_CheckedChanged(object sender, EventArgs e)
        {
            this.rbCustom.Checked = !this.rbSetup.Checked;
        }

        private void tcApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.oldSelectedCurrentIndex != this.tcApp.SelectedIndex)
            {
                try
                {
                    if (this.tcApp.TabPages[this.oldSelectedCurrentIndex] == this.tabPageSettings)
                    {
                        this.OnDoOperation(Operations.LeaveSettings);
                    }
                }
                finally
                {
                    this.oldSelectedCurrentIndex = this.tcApp.SelectedIndex;
                }
            }
        }

        internal event EventHandler<OperationEventArgs> DoOperation;
    }
}