using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Runtime.InteropServices;

namespace rename
{
	public delegate void ProgressBarChange(int i);
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        MainFrm frm;
		bool isStop;
		string searchTxt;
		DevSoft.CommonApp.Util.ExcelHelper ex = new DevSoft.CommonApp.Util.ExcelHelper();
        private void InitListView()
        {
            lvFilePath.BeginUpdate();
            lvFilePath.GridLines = true;
            lvFilePath.FullRowSelect = true;
            lvFilePath.View = View.Details;
            lvFilePath.Scrollable = true;
            lvFilePath.MultiSelect = false;
            lvFilePath.Clear();
            lvFilePath.Columns.Add("文件名", 300, HorizontalAlignment.Left);
            lvFilePath.Columns.Add("关键字位置", 80, HorizontalAlignment.Left);
            lvFilePath.Columns.Add("出现次数", 80, HorizontalAlignment.Left);
            lvFilePath.EndUpdate();
			
        }
        private void Form4_Load(object sender, EventArgs e)
        {
			lbPgbPercent.Text = lbFile.Text = "";
            frm = this.Owner as MainFrm;
            label10.Text = frm.StrForm4;
            InitListView();
			txtKey.Focus();
			cbOnlyTxt_CheckedChanged(sender, e);
			cbCase_CheckedChanged(sender, e);
			Application.DoEvents();

            RegisterHotKey(Handle, 100, KeyModifiers.Control, Keys.Up);//注册
            RegisterHotKey(Handle, 101, KeyModifiers.Control, Keys.Down);//注册
        }

		private void ScanDir(string dir,ref List<string> ls)
		{
			if (isStop) return;
			string[] dirs = Directory.GetDirectories(dir);
			foreach (string tdir in dirs)
			{
				ScanDir(tdir, ref ls);
			}
			string[] files = Directory.GetFiles(dir);

			foreach (string file in files)
			{
				ls.Add(file);
			}
			
		}

		private void InitProgressBar(object sender, EventArgs e)
		{
			ProgressBar tmpPgb = sender as ProgressBar;
			ControlEventArgs tmpCea = e as ControlEventArgs;
			tmpPgb.Maximum = (int)tmpCea.param;
			tmpPgb.Minimum = 0;
			tmpPgb.Value = 0;
			Application.DoEvents();
		}
		private void ChnageProgressBar(object sender, EventArgs e)
		{
			ProgressBar tmpPgb = sender as ProgressBar;
			ControlEventArgs tmpCea = e as ControlEventArgs;
			if (tmpPgb.Value != tmpPgb.Maximum)
				tmpPgb.Value = (int)tmpCea.param + 1;
		}

		private void InitLable1(object sender, EventArgs e)
		{
			lbResult.Text = sender.ToString();
		}
		private void InitLable2(object sender, EventArgs e)
		{
			lbFile.Text = sender.ToString();
		}
		private void InitLable3(object sender, EventArgs e)
		{
			lbPgbPercent.Text = sender.ToString();
		}

        private void InitLable4(object sender, EventArgs e)
        {
            statusStrip1.Items["tssStautus"].Text = sender.ToString();
        }

		private StringComparison sctype;
		private void Search()
		{
			string str = frm.StrForm4;
			linList = new List<ListItemNode>();
			List<string> ls = null;
			if (frm.SelectedFiles != null && frm.SelectedFiles.Count > 0)
				ls = frm.SelectedFiles;
			else ls = new List<string>(new string[] { str });
			//else ls = new List<string>(Directory.GetFiles(str));
			isStop = false;
			ex.isStop = false;
			contextShow.Items["tsHidePath"].Text = "隐藏路径";
			bool isScaned = false;
			List<string> fls = new List<string>();
			foreach (string file in ls)
			{
				if (isStop)
				{
					break;
				}
				if (Directory.Exists(file))
				{
					if (!isScaned)
					{
						if (MessageBox.Show("是否遍历子目录？", "文件扫描", MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							isScaned = true;
							ScanDir(file, ref fls);
						}
						else
						{
							break;
						}
					}
					else
					{
						ScanDir(file, ref fls);
					}

				}
				else
				{
					fls.Add(file);
				}

			}
			string[] files = fls.ToArray();
			IAsyncResult pbaResult = pgbTotal.BeginInvoke(new EventHandler(InitProgressBar), new object[] { pgbTotal, new ControlEventArgs(files.Length) });
			try
			{
				pgbTotal.EndInvoke(pbaResult);
			}
			catch
			{

			}
			int count = 0;//匹配的数目

			string cext = txtExt.Text;
			List<string> lext = new List<string>(cext.Replace(".","").Split(','));
			for (int i = 0; i < files.Length && !isStop; i++)
			{
				string file = files[i];
				pbaResult = lbFile.BeginInvoke(new EventHandler(InitLable2), new object[] { file, EventArgs.Empty });
				try
				{
					lbFile.EndInvoke(pbaResult);
				}
				catch
				{
				}
				string ext = Path.GetExtension(file).ToUpper().Replace(".", "");
				if (!lext.Contains(ext)) continue;
				int index = 1;
				int turnup = 0;
				
				switch(ext)
				{
					case "XLS":
					case "XLSX":
						ExcelReader er = new ExcelReader();
						if (er.SearchExcel(file, searchTxt, cbCase.Checked, new ProgressBarChange(SubProgressInit), new ProgressBarChange(SubProgressChange)))
						{
							turnup++;
							count++;
						}
						//ex.pgb = subProgressBar;
						//if (ex.SearchExcel(file, searchTxt, cbCase.Checked, new ProgressBarChange(SubProgressInit), new ProgressBarChange(SubProgressChange)))
						//{
						//    turnup++;
						//    count++;
						//}
						break;
					default:
						FindTxt(file, ref count,ref turnup,ref index);
						break;
				}

				if (turnup != 0)
				{
					string[] listItem ={ file, index.ToString(), turnup.ToString() };
					lvFilePath.BeginInvoke(new EventHandler(AddListViewItem), new object[] { new ListViewItem(listItem), EventArgs.Empty });
					//listView1.Items.Add(new ListViewItem(listItem));
				}
				
				string slbResult = string.Format("已处理：{0,-10}\t待处理：{1,-10}\t匹配数：{2,-10}\t匹配率：{3}%\t", i + 1, files.Length - i - 1, count ,Math.Round((double)(count ) / (double)(i + 1), 2) * 100);
				pbaResult = lbResult.BeginInvoke(new EventHandler(InitLable1), new object[] { slbResult, EventArgs.Empty });
				try
				{
					lbResult.EndInvoke(pbaResult);
				}
				catch
				{
				}
				
				pbaResult = lbPgbPercent.BeginInvoke(new EventHandler(InitLable3), new object[] { Convert.ToString(Math.Round((double)(i + 1) / (double)files.Length, 2) * 100) + "%", EventArgs.Empty });
				try
				{
					lbPgbPercent.EndInvoke(pbaResult);
				}
				catch
				{
				}
				pbaResult = pgbTotal.BeginInvoke(new EventHandler(ChnageProgressBar), new object[] { pgbTotal, new ControlEventArgs(i) });
				try
				{
					pgbTotal.EndInvoke(pbaResult);
				}
				catch
				{
				}
				Application.DoEvents();
			}
		}

		private void FindTxt(string file, ref int count,ref int turnup,ref int index)
		{
			StreamReader sr = new StreamReader(file, Encoding.Default);
			int tmp = count;
			while (sr.Peek() >= 0)
			{
				string content = sr.ReadLine();
				if (content.IndexOf(searchTxt, sctype) != -1)
				{
					turnup++;
					if(tmp==count)count++;
					if (!cbSearchAll.Checked)
					{
						break;
					}
				}
				index++;
			}
			sr.Close();
			int tmpIndex = index > 10 ? 10 : index;
			SubProgressInit(tmpIndex);
			for (int i = 0; i <= tmpIndex; i++)
			{
				SubProgressChange(i);
			}
		}

		private void AddListViewItem(object sender, EventArgs e)
		{
			ListViewItem lvi = (ListViewItem)sender;
			lvFilePath.Items.Add(lvi);
			linList.Add(new ListItemNode(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text));
		}

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
				bntSearch_Click(sender, e);
            }
        }

		string txtExts = ".TXT,.SQL,.CS,.H,.CPP,.JSP,.JS,.JAVA,.XML";
		string activePath = "";
		string searchKey = "";

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvFilePath.FocusedItem.SubItems == null) return;
			string focusedItemText = linList[lvFilePath.FocusedItem.Index].FileName;
			this.Text = "当前正在阅读:" + Path.GetFileName(focusedItemText);
			tssStautus.Text = focusedItemText;
			if (!txtExts.Contains(Path.GetExtension(focusedItemText).ToUpper()))
			{
				return;
			}
			activePath = focusedItemText;
			searchKey = txtKey.Text;
			Thread th = new Thread(new ThreadStart(ReadListViewItem));
			th.Start();
        }

		private void ReadListViewItem()
		{
			rtxtReader.BeginInvoke(new EventHandler(ReadFile), new object[] { activePath, EventArgs.Empty });
		}
		
		private void ReadFile(object sender,EventArgs e)
		{
			string path = sender as string;             
            //rtxtReader.LoadFile(path, RichTextBoxStreamType.PlainText);
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (fs.CanRead)
            {
                //读取时加入编码信息,否则读取汉字会乱码
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string strline = sr.ReadLine();
                StringBuilder sb = new StringBuilder();
                while (strline != null)
                {
                    strline = sr.ReadLine();
                    sb = sb.Append(strline + "\n");
                }
                sr.Close();
                rtxtReader.Text = sb.ToString();
            }
			SetColor(searchKey, Color.Yellow);		
		}


		private void rtxtReader_VScroll(object sender, EventArgs e)
		{
			
		}

		const int SB_VERT = 1;
		const int EM_SETSCROLLPOS = 0x0400 + 222;
		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, POINT lParam);

		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			public int x;
			public int y;
			public POINT()
			{

			}
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

        private static List<int> posKey = new List<int>();
        private static int readerSelectedIndex;

		private void SetColor(string input, Color color)
        {
			int index = rtxtReader.Text.IndexOf(input, sctype);
            readerSelectedIndex = 0;
            posKey.Clear();
            while (index != -1 && index < rtxtReader.Text.Length)
            {
				posKey.Add(index);
                rtxtReader.Select(index, input.Length);
                rtxtReader.SelectionColor = color;
                rtxtReader.SelectionBackColor = Color.Blue;
				index = rtxtReader.Text.IndexOf(input, index + input.Length, sctype);
            }
			this.Text += string.Format("(出现次数:{0})", posKey.Count);
            rtxtReader.Refresh();

			if (posKey.Count > 0)
			{
				rtxtReader.SelectionStart = posKey[0];
				rtxtReader.Focus();

                statusStrip1.BeginInvoke(new EventHandler(InitLable4), 
                    new object[] { string.Format("共有{0}个匹配项", posKey.Count), EventArgs.Empty });
			}

            

            Application.DoEvents();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllFile.Checked)
            {
                InitListView();
                string str = frm.StrForm4;
                string searchTxt = txtKey.Text;
                string[] files = Directory.GetFiles(str);
                pgbTotal.Maximum = files.Length;
                pgbTotal.Minimum = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    string[] listItem ={ files[i] };
                    lvFilePath.Items.Add(new ListViewItem(listItem));
                    pgbTotal.Value = i + 1;
                    Application.DoEvents();
                }

            }
            else
            {
                lvFilePath.Clear();
            }
        }

		private void bntStop_Click(object sender, EventArgs e)
		{
			isStop = true;
			ex.isStop = true;
		}

		private void SubProgressInit(int columns)
		{
			IAsyncResult pbaResult = null;
			try
			{
				pbaResult = subProgressBar.BeginInvoke(new EventHandler(InitProgressBar), new object[] { subProgressBar, new ControlEventArgs(columns) });
				subProgressBar.EndInvoke(pbaResult);
			}
			catch
			{

			}
		}
		
		private void SubProgressChange(int i)
		{
			IAsyncResult pbaResult = null;
			try
			{
				pbaResult = subProgressBar.BeginInvoke(new EventHandler(ChnageProgressBar), new object[] { subProgressBar, new ControlEventArgs(i) });
				subProgressBar.EndInvoke(pbaResult);
			}
			catch
			{

			}
		}

		private void cbOnlyTxt_CheckedChanged(object sender, EventArgs e)
		{
			if (cbOnlyTxt.Checked)
			{
				txtExt.Text = txtExts;
			}
			else
			{
				txtExt.Text = ConfigurationManager.AppSettings["ext"];
			}
		}

		private void cbCase_CheckedChanged(object sender, EventArgs e)
		{
			sctype = cbCase.Checked ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
		}

		private void ShowOrHide()
		{
			bool isHide = contextShow.Items["hideTool"].Text == "隐藏列表" ? true : false;
			if (isHide)
			{
				tableLayoutPanel1.ColumnCount = 1;
				lvFilePath.Visible = false;
				panel1.Visible = false;
				hideTool.Text = "显示列表";
			}
			else
			{
				tableLayoutPanel1.ColumnCount = 3;
				lvFilePath.Visible = true;
				panel1.Visible = true;
				hideTool.Text = "隐藏列表";
			}
		}

		private void OpenFile()
		{
			if (lvFilePath.Items.Count > 0)
			{
				try
				{
					System.Diagnostics.Process.Start(linList[lvFilePath.FocusedItem.Index].FileName);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void OpenDirectory()
		{
			if (lvFilePath.Items.Count > 0)
			{
				try
				{
					System.Diagnostics.Process.Start("explorer", "/select," + linList[lvFilePath.FocusedItem.Index].FileName);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void ShowOrHidePath()
		{

			bool isHide = contextShow.Items["tsHidePath"].Text == "隐藏路径" ? true : false;

			DisplayName(isHide);

			if (contextShow.Items["tsHidePath"].Text == "隐藏路径")
			{
				contextShow.Items["tsHidePath"].Text = "显示路径";
			}
			else
			{
				contextShow.Items["tsHidePath"].Text = "隐藏路径";
			}
		}

		struct ListItemNode
		{
			public string FileName;
			public string KeyPos;
			public string FindTimes;
			public ListItemNode(string fileName, string keyPos, string findTimes)
			{
				FileName = fileName;
				KeyPos = keyPos;
				FindTimes = findTimes;
			}
		}

		private List<ListItemNode> linList;

		private void DisplayName(bool isHide)
		{
			InitListView();
			if (linList == null) return;
			for (int i = 0; i < linList.Count; i++)
			{
				ListItemNode lin= linList[i];
				string fileName = lin.FileName;
				string keyPos = lin.KeyPos;
				string findTimes = lin.FindTimes;
				if (isHide)
				{
					fileName = fileName.Substring(fileName.LastIndexOf('\\')+1);
				}
				ListViewItem item = new ListViewItem();
				System.Windows.Forms.ListViewItem.ListViewSubItem lvsi1 = new ListViewItem.ListViewSubItem(item, fileName);
				System.Windows.Forms.ListViewItem.ListViewSubItem lvsi2 = new ListViewItem.ListViewSubItem(item, keyPos);
				System.Windows.Forms.ListViewItem.ListViewSubItem lvsi3 = new ListViewItem.ListViewSubItem(item, findTimes);
				item.SubItems.Clear();
				item.SubItems.Insert(0, lvsi1);
				item.SubItems.Insert(1, lvsi2);
				item.SubItems.Insert(2, lvsi3);

				//item.SubItems.AddRange(new ListViewItem.ListViewSubItem[] { lvsi1, lvsi2, lvsi3 });
				lvFilePath.Items.Insert(i, item);
			}
		}

		private void contextShow_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			switch (e.ClickedItem.Name)
			{
				case "hideTool":
					ShowOrHide();
					break;
				case "openTool":
					OpenFile();
					break;
				case "DirectoryTool":
					OpenDirectory();
					break;
				case "tsHidePath":
					ShowOrHidePath();
					break;
                case "tsPrev":
                    if (readerSelectedIndex > 0)
                    {
                        rtxtReader.SelectionStart = posKey[--readerSelectedIndex];
                        rtxtReader.SelectionLength = txtKey.Text.Length;
                        rtxtReader.Focus();
                    }
                    
                    break;
                case "tsNext":
                    if (posKey.Count - 1 > readerSelectedIndex)
                    {
                        rtxtReader.SelectionStart = posKey[++readerSelectedIndex];
                        rtxtReader.SelectionLength = txtKey.Text.Length;
                        rtxtReader.Focus();
                    }
                    break;
			}
		}

		private void contextShow_Opening(object sender, CancelEventArgs e)
		{
			Control ctrl = contextShow.SourceControl;
			if (ctrl != null)
			{
				switch (ctrl.Name)
				{
					case "lvFilePath":
						contextShow.Items["hideTool"].Visible = false;
						contextShow.Items["openTool"].Visible = true;
						contextShow.Items["DirectoryTool"].Visible = true;
						contextShow.Items["tsHidePath"].Visible = true;
                        contextShow.Items["tsPrev"].Visible = false;
                        contextShow.Items["tsNext"].Visible = false;
						break;
					case "rtxtReader":
						contextShow.Items["hideTool"].Visible = true;
						contextShow.Items["openTool"].Visible = false;
						contextShow.Items["DirectoryTool"].Visible = false;
						contextShow.Items["tsHidePath"].Visible = false;
                        contextShow.Items["tsPrev"].Visible = true;
                        contextShow.Items["tsNext"].Visible = true;
						break;
				}
			}
		}

		private void bntSearch_Click(object sender, EventArgs e)
		{
			if (txtKey.Text.Equals(string.Empty))
			{
				return;
			}
			InitListView();
			searchTxt = txtKey.Text;
			Thread th = new Thread(new ThreadStart(Search));
			th.Start(); 
		}

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd, // handle to window     
            int id, // hot key identifier     
            KeyModifiers fsModifiers, // key-modifier options     
            Keys vk // virtual-key code     
            );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
                IntPtr hWnd, // handle to window     
                int id // hot key identifier     
            );

        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(Handle, 100);//卸载快捷键    
            UnregisterHotKey(Handle, 101);  
        }

        protected override void WndProc(ref Message m)//监视Windows消息   
        {
            const int WM_HOTKEY = 0x0312;//按快捷键    
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Up  
                            if (readerSelectedIndex > 0)
                            {
                                rtxtReader.SelectionStart = posKey[--readerSelectedIndex];
                                rtxtReader.SelectionLength = txtKey.Text.Length;
                                rtxtReader.Focus();
                            }
                            break;
                        case 101:    //按下的是Up  
                            if (posKey.Count - 1 > readerSelectedIndex)
                            {
                                rtxtReader.SelectionStart = posKey[++readerSelectedIndex];
                                rtxtReader.SelectionLength = txtKey.Text.Length;
                                rtxtReader.Focus();
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        } 



    }

}