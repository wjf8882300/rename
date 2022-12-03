namespace rename
{
	partial class FrmScan
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.tsCmmFileSearch = new System.Windows.Forms.ComboBox();
			this.bntOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// tsCmmFileSearch
			// 
			this.tsCmmFileSearch.FormattingEnabled = true;
			this.tsCmmFileSearch.Location = new System.Drawing.Point(26, 49);
			this.tsCmmFileSearch.Name = "tsCmmFileSearch";
			this.tsCmmFileSearch.Size = new System.Drawing.Size(438, 20);
			this.tsCmmFileSearch.TabIndex = 0;
			// 
			// bntOK
			// 
			this.bntOK.Location = new System.Drawing.Point(470, 49);
			this.bntOK.Name = "bntOK";
			this.bntOK.Size = new System.Drawing.Size(75, 23);
			this.bntOK.TabIndex = 1;
			this.bntOK.Text = "确定";
			this.bntOK.UseVisualStyleBackColor = true;
			this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "文件路径";
			// 
			// FrmScan
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(560, 128);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bntOK);
			this.Controls.Add(this.tsCmmFileSearch);
			this.Name = "FrmScan";
			this.Text = "文件路径";
			this.Load += new System.EventHandler(this.FrmScan_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox tsCmmFileSearch;
		private System.Windows.Forms.Button bntOK;
		private System.Windows.Forms.Label label1;
	}
}