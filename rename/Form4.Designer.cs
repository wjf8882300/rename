namespace rename
{
    partial class Form4
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
            this.components = new System.ComponentModel.Container();
            this.lvFilePath = new System.Windows.Forms.ListView();
            this.contextShow = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openTool = new System.Windows.Forms.ToolStripMenuItem();
            this.DirectoryTool = new System.Windows.Forms.ToolStripMenuItem();
            this.tsHidePath = new System.Windows.Forms.ToolStripMenuItem();
            this.hideTool = new System.Windows.Forms.ToolStripMenuItem();
            this.tsPrev = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNext = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.bntSearch = new System.Windows.Forms.Button();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pgbTotal = new System.Windows.Forms.ProgressBar();
            this.lbPgbPercent = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSearchAll = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbOnlyTxt = new System.Windows.Forms.CheckBox();
            this.subProgressBar = new System.Windows.Forms.ProgressBar();
            this.lbFile = new System.Windows.Forms.TextBox();
            this.cbCase = new System.Windows.Forms.CheckBox();
            this.bntStop = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.txtExt = new System.Windows.Forms.TextBox();
            this.cbAllFile = new System.Windows.Forms.CheckBox();
            this.rtxtReader = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssStautus = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextShow.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvFilePath
            // 
            this.lvFilePath.ContextMenuStrip = this.contextShow;
            this.lvFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFilePath.Location = new System.Drawing.Point(3, 3);
            this.lvFilePath.Name = "lvFilePath";
            this.lvFilePath.Size = new System.Drawing.Size(482, 318);
            this.lvFilePath.TabIndex = 6;
            this.lvFilePath.UseCompatibleStateImageBehavior = false;
            this.lvFilePath.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // contextShow
            // 
            this.contextShow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTool,
            this.DirectoryTool,
            this.tsHidePath,
            this.hideTool,
            this.tsPrev,
            this.tsNext});
            this.contextShow.Name = "contextMenuStrip1";
            this.contextShow.Size = new System.Drawing.Size(137, 136);
            this.contextShow.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextShow_ItemClicked);
            this.contextShow.Opening += new System.ComponentModel.CancelEventHandler(this.contextShow_Opening);
            // 
            // openTool
            // 
            this.openTool.Name = "openTool";
            this.openTool.Size = new System.Drawing.Size(136, 22);
            this.openTool.Text = "打开文件";
            // 
            // DirectoryTool
            // 
            this.DirectoryTool.Name = "DirectoryTool";
            this.DirectoryTool.Size = new System.Drawing.Size(136, 22);
            this.DirectoryTool.Text = "所在文件夹";
            // 
            // tsHidePath
            // 
            this.tsHidePath.Name = "tsHidePath";
            this.tsHidePath.Size = new System.Drawing.Size(136, 22);
            this.tsHidePath.Text = "隐藏路径";
            // 
            // hideTool
            // 
            this.hideTool.Name = "hideTool";
            this.hideTool.Size = new System.Drawing.Size(136, 22);
            this.hideTool.Text = "隐藏列表";
            // 
            // tsPrev
            // 
            this.tsPrev.Name = "tsPrev";
            this.tsPrev.Size = new System.Drawing.Size(136, 22);
            this.tsPrev.Text = "上一个";
            // 
            // tsNext
            // 
            this.tsNext.Name = "tsNext";
            this.tsNext.Size = new System.Drawing.Size(136, 22);
            this.tsNext.Text = "下一个";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "请输入关键字";
            // 
            // bntSearch
            // 
            this.bntSearch.Location = new System.Drawing.Point(399, 145);
            this.bntSearch.Name = "bntSearch";
            this.bntSearch.Size = new System.Drawing.Size(75, 23);
            this.bntSearch.TabIndex = 3;
            this.bntSearch.Text = "查询";
            this.bntSearch.UseVisualStyleBackColor = true;
            this.bntSearch.Click += new System.EventHandler(this.bntSearch_Click);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(89, 33);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(223, 21);
            this.txtKey.TabIndex = 1;
            this.txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "正在查询：";
            // 
            // pgbTotal
            // 
            this.pgbTotal.Location = new System.Drawing.Point(10, 173);
            this.pgbTotal.Name = "pgbTotal";
            this.pgbTotal.Size = new System.Drawing.Size(417, 23);
            this.pgbTotal.TabIndex = 6;
            // 
            // lbPgbPercent
            // 
            this.lbPgbPercent.AutoSize = true;
            this.lbPgbPercent.Location = new System.Drawing.Point(433, 184);
            this.lbPgbPercent.Name = "lbPgbPercent";
            this.lbPgbPercent.Size = new System.Drawing.Size(41, 12);
            this.lbPgbPercent.TabIndex = 7;
            this.lbPgbPercent.Text = "label4";
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Location = new System.Drawing.Point(9, 118);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(53, 12);
            this.lbResult.TabIndex = 8;
            this.lbResult.Text = "已处理：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "当前文件夹";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(87, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 13;
            this.label10.Text = "label10";
            // 
            // cbSearchAll
            // 
            this.cbSearchAll.AutoSize = true;
            this.cbSearchAll.Location = new System.Drawing.Point(385, 36);
            this.cbSearchAll.Name = "cbSearchAll";
            this.cbSearchAll.Size = new System.Drawing.Size(60, 16);
            this.cbSearchAll.TabIndex = 5;
            this.cbSearchAll.Text = "全搜索";
            this.cbSearchAll.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.5176F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.4824F));
            this.tableLayoutPanel1.Controls.Add(this.lvFilePath, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtxtReader, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(966, 540);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbOnlyTxt);
            this.panel1.Controls.Add(this.subProgressBar);
            this.panel1.Controls.Add(this.lbFile);
            this.panel1.Controls.Add(this.cbCase);
            this.panel1.Controls.Add(this.bntStop);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.txtExt);
            this.panel1.Controls.Add(this.cbAllFile);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.cbSearchAll);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.bntSearch);
            this.panel1.Controls.Add(this.txtKey);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.pgbTotal);
            this.panel1.Controls.Add(this.lbPgbPercent);
            this.panel1.Controls.Add(this.lbResult);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 327);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(482, 210);
            this.panel1.TabIndex = 1;
            // 
            // cbOnlyTxt
            // 
            this.cbOnlyTxt.AutoSize = true;
            this.cbOnlyTxt.Checked = true;
            this.cbOnlyTxt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnlyTxt.Location = new System.Drawing.Point(385, 64);
            this.cbOnlyTxt.Name = "cbOnlyTxt";
            this.cbOnlyTxt.Size = new System.Drawing.Size(60, 16);
            this.cbOnlyTxt.TabIndex = 27;
            this.cbOnlyTxt.Text = "仅文本";
            this.cbOnlyTxt.UseVisualStyleBackColor = true;
            this.cbOnlyTxt.CheckedChanged += new System.EventHandler(this.cbOnlyTxt_CheckedChanged);
            // 
            // subProgressBar
            // 
            this.subProgressBar.Location = new System.Drawing.Point(11, 145);
            this.subProgressBar.Name = "subProgressBar";
            this.subProgressBar.Size = new System.Drawing.Size(301, 23);
            this.subProgressBar.TabIndex = 26;
            // 
            // lbFile
            // 
            this.lbFile.Location = new System.Drawing.Point(89, 90);
            this.lbFile.Name = "lbFile";
            this.lbFile.ReadOnly = true;
            this.lbFile.Size = new System.Drawing.Size(385, 21);
            this.lbFile.TabIndex = 25;
            // 
            // cbCase
            // 
            this.cbCase.AutoSize = true;
            this.cbCase.Location = new System.Drawing.Point(318, 35);
            this.cbCase.Name = "cbCase";
            this.cbCase.Size = new System.Drawing.Size(60, 16);
            this.cbCase.TabIndex = 23;
            this.cbCase.Text = "大小写";
            this.cbCase.UseVisualStyleBackColor = true;
            this.cbCase.CheckedChanged += new System.EventHandler(this.cbCase_CheckedChanged);
            // 
            // bntStop
            // 
            this.bntStop.Location = new System.Drawing.Point(318, 145);
            this.bntStop.Name = "bntStop";
            this.bntStop.Size = new System.Drawing.Size(75, 23);
            this.bntStop.TabIndex = 22;
            this.bntStop.Text = "停止";
            this.bntStop.UseVisualStyleBackColor = true;
            this.bntStop.Click += new System.EventHandler(this.bntStop_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 64);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 21;
            this.label15.Text = "包含扩展名";
            // 
            // txtExt
            // 
            this.txtExt.Location = new System.Drawing.Point(89, 61);
            this.txtExt.Name = "txtExt";
            this.txtExt.Size = new System.Drawing.Size(223, 21);
            this.txtExt.TabIndex = 2;
            // 
            // cbAllFile
            // 
            this.cbAllFile.AutoSize = true;
            this.cbAllFile.Location = new System.Drawing.Point(318, 64);
            this.cbAllFile.Name = "cbAllFile";
            this.cbAllFile.Size = new System.Drawing.Size(72, 16);
            this.cbAllFile.TabIndex = 4;
            this.cbAllFile.Text = "所有文件";
            this.cbAllFile.UseVisualStyleBackColor = true;
            this.cbAllFile.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // rtxtReader
            // 
            this.rtxtReader.ContextMenuStrip = this.contextShow;
            this.rtxtReader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtReader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxtReader.Location = new System.Drawing.Point(491, 3);
            this.rtxtReader.Name = "rtxtReader";
            this.rtxtReader.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.rtxtReader, 2);
            this.rtxtReader.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtxtReader.Size = new System.Drawing.Size(472, 534);
            this.rtxtReader.TabIndex = 7;
            this.rtxtReader.Text = "";
            this.rtxtReader.VScroll += new System.EventHandler(this.rtxtReader_VScroll);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.statusStrip1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.78544F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.21456F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(972, 571);
            this.tableLayoutPanel2.TabIndex = 20;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssStautus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 549);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(972, 22);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssStautus
            // 
            this.tssStautus.Name = "tssStautus";
            this.tssStautus.Size = new System.Drawing.Size(32, 17);
            this.tssStautus.Text = "就绪";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 571);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "Form4";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "搜索";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form4_FormClosing);
            this.contextShow.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bntSearch;
        private System.Windows.Forms.TextBox txtKey;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pgbTotal;
        private System.Windows.Forms.Label lbPgbPercent;
		private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cbSearchAll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox rtxtReader;
        private System.Windows.Forms.ContextMenuStrip contextShow;
        private System.Windows.Forms.ToolStripMenuItem hideTool;
        private System.Windows.Forms.ToolStripMenuItem openTool;
        private System.Windows.Forms.CheckBox cbAllFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssStautus;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox txtExt;
		private System.Windows.Forms.ToolStripMenuItem DirectoryTool;
		private System.Windows.Forms.Button bntStop;
		private System.Windows.Forms.CheckBox cbCase;
		private System.Windows.Forms.TextBox lbFile;
		private System.Windows.Forms.ProgressBar subProgressBar;
		private System.Windows.Forms.CheckBox cbOnlyTxt;
		private System.Windows.Forms.ToolStripMenuItem tsHidePath;
        private System.Windows.Forms.ToolStripMenuItem tsPrev;
        private System.Windows.Forms.ToolStripMenuItem tsNext;
    }
}