using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace BnsModTool
{
	public class RenameForm : Form
	{
		private string directoryName;

		private IContainer components = null;

		private Label label1;

		private Label label2;

		private TextBox txtFullName;

		private TextBox txtAbout;

		private Label label3;

		private Button btnSave;

		private Button btnCancel;

		private Label label4;

		private TextBox txtResult;

		private Label label5;

		public string NewImageName
		{
			get;
			set;
		}

		public RenameForm(string imageFullName)
		{
			this.InitializeComponent();
			this.directoryName = Path.GetDirectoryName(imageFullName);
			TextBox textBox = this.txtResult;
			TextBox textBox1 = this.txtFullName;
			string fileName = Path.GetFileName(imageFullName);
			string str = fileName;
			textBox1.Text = fileName;
			textBox.Text = str;
			string[] strArrays = Path.GetFileNameWithoutExtension(imageFullName).Split(new char[] { '+' });
			if ((int)strArrays.Length > 2)
			{
				this.txtAbout.Text = strArrays[(int)strArrays.Length - 1];
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			this.Rename();
			string str = Path.Combine(this.directoryName, this.NewImageName);
			if (!File.Exists(str))
			{
				base.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			else
			{
				MessageBox.Show(string.Concat("更改不能保存，文件名\"", str, "\"已经存在。\n\n如果实际上没有发生更改请直接关闭窗口。"));
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

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtAbout = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Original Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name Changed to:";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(115, 30);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.ReadOnly = true;
            this.txtFullName.Size = new System.Drawing.Size(339, 20);
            this.txtFullName.TabIndex = 2;
            // 
            // txtAbout
            // 
            this.txtAbout.Location = new System.Drawing.Point(115, 78);
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.Size = new System.Drawing.Size(339, 20);
            this.txtAbout.TabIndex = 3;
            this.txtAbout.TextChanged += new System.EventHandler(this.txtAbout_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(442, 115);
            this.label3.TabIndex = 4;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(283, 298);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(379, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(113, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(317, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Simply fill in the Note name, you do not need to fill in the full name.";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(115, 140);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(338, 20);
            this.txtResult.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Edited Name:";
            // 
            // RenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 336);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAbout);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add a note name for the model image";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private void Rename()
		{
			string[] strArrays = Path.GetFileNameWithoutExtension(this.txtFullName.Text).Split(new char[] { '+' });
			StringBuilder stringBuilder = new StringBuilder();
			if ((int)strArrays.Length <= 2)
			{
				string[] strArrays1 = strArrays;
				for (int i = 0; i < (int)strArrays1.Length; i++)
				{
					stringBuilder.Append(strArrays1[i]);
					stringBuilder.Append('+');
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				if (!string.IsNullOrEmpty(this.txtAbout.Text))
				{
					stringBuilder.Append("+");
					stringBuilder.Append(this.txtAbout.Text);
				}
			}
			else
			{
				for (int j = 0; j < (int)strArrays.Length - 1; j++)
				{
					stringBuilder.Append(strArrays[j]);
					stringBuilder.Append('+');
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				if (!string.IsNullOrEmpty(this.txtAbout.Text))
				{
					stringBuilder.Append("+");
					stringBuilder.Append(this.txtAbout.Text);
				}
			}
			stringBuilder.Append(".jpg");
			TextBox textBox = this.txtResult;
			string str = stringBuilder.ToString();
			string str1 = str;
			this.NewImageName = str;
			textBox.Text = str1;
		}

		private void txtAbout_TextChanged(object sender, EventArgs e)
		{
			this.Rename();
		}
	}
}