using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Reflection;
using rename.Properties;

namespace rename
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
            public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath,
                                        uint dwFileAttributes,
                                        ref SHFILEINFO psfi,
                                        uint cbSizeFileInfo,
                                        uint uFlags);
        }

        #region 私有变量
        private static string FilePath = "";
        private static string strFilePath = "";
        private static Stack<string> stack;
        private static List<string> currFileList;
       
        #endregion
        #region 根据路径向listView添加文件信息
        /// <summary>
        /// 初始化ListView表
        /// </summary>
        private void InitListView()
        {
            listView1.BeginUpdate();
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.View = View.Details;
            listView1.Scrollable = true;
            listView1.MultiSelect = true;

            listView1.Columns.Add("文件名", 160, HorizontalAlignment.Left);
            listView1.Columns.Add("文件大小", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("创建时间", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("访问时间", 200, HorizontalAlignment.Left);

            listView1.EndUpdate();
        }


        /// <summary>
        /// ListView重载的共同部分
        /// </summary>
        /// <param name="strPath"></param>
        private void ListView(string strPath)
        {
            IntPtr hImgSmall;    //the handle to the system image list
            IntPtr hImgLarge;     //the handle to the system image list

            SHFILEINFO shinfo = new SHFILEINFO();
            ImageList imageList1 = new ImageList();
            imageList1.Images.Add(Resources._0003);
            //imageList1.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "../Images/0003.ICO"));
            listView1.SmallImageList = imageList1;
            DirectoryInfo curDir = new DirectoryInfo(strPath);//创建目录对象。
            FileInfo[] dirFiles;
            try
            {
                dirFiles = curDir.GetFiles();
            }
            catch { return; }
            string[] arrSubItem = new string[4];
            //文件的创建时间和访问时间。
            int iCount = 0;
            int index = 1;

            string path = curDir.FullName;
            if (path.LastIndexOf("\\") != path.Length - 1)
                path += "\\";

            bool isImage = (dirFiles.Length > 1000);
            currFileList = new List<string>();
            foreach (FileInfo fileInfo in dirFiles)
            {
                string strFileName = fileInfo.Name;
                currFileList.Add(strFileName);

                //如果不是文件pagefile.sys
                if (!strFileName.Equals("pagefile.sys"))
                {
                    long length = 0;
                    arrSubItem[0] = strFileName;
                    if (fileInfo.Length > 1024 * 1024 * 1024)
                    {
                        double len = (double)fileInfo.Length / (double)(1024 * 1024 * 1024);
                        arrSubItem[1] = Math.Round(len, 2) + "G";
                    }
                    else
                        if (fileInfo.Length > 1024 * 1024)
                        {
                            double len = (double)fileInfo.Length / (double)(1024 * 1024);
                            if ((long)len > 100)
                                arrSubItem[1] = Math.Round(len, 0) + "M";
                            else
                                arrSubItem[1] = Math.Round(len, 1) + "M";
                        }
                        else
                            if (fileInfo.Length > 1024)
                            {
                                length = fileInfo.Length / 1024;
                                arrSubItem[1] = length.ToString() + "K";
                            }
                            else
                                arrSubItem[1] = fileInfo.Length.ToString() + "B";
                    //arrSubItem[1] = fileInfo.Length.ToString() + " 字节";
                    arrSubItem[2] = fileInfo.CreationTime.ToString();
                    arrSubItem[3] = fileInfo.LastAccessTime.ToString();
                }

                if (!isImage)
                {
                    hImgSmall = Win32.SHGetFileInfo(path + strFileName, 0, ref shinfo,
                                                 (uint)Marshal.SizeOf(shinfo),
                                                  Win32.SHGFI_ICON |
                                                  Win32.SHGFI_SMALLICON);
                    System.Drawing.Icon myIcon =
                         System.Drawing.Icon.FromHandle(shinfo.hIcon);

                    imageList1.Images.Add(myIcon);
                }

                ListViewItem LiItem = new ListViewItem(arrSubItem, index++);
                listView1.Items.Insert(iCount, LiItem);
                iCount++;
            }

            strFilePath = strPath;
            if (strFilePath.IndexOf("\\\\") != -1)
                strFilePath = strFilePath.Remove(strFilePath.IndexOf('\\'), 1);
            this.Cursor = Cursors.Arrow;
            int iItem = 0;
            DirectoryInfo Dir = new DirectoryInfo(strPath);
            foreach (DirectoryInfo di in Dir.GetDirectories())
            {
                ListViewItem LiItem = new ListViewItem(di.Name, 0);
                listView1.Items.Insert(iItem, LiItem);
                iItem++;
            }
            
        }



        /// <summary>
        /// 根据树结点向listView添加文件信息
        /// </summary>
        /// <param name="tn">树结点</param>
        private void InitListView(TreeNode tn)
        {
            this.Cursor = Cursors.WaitCursor;
            listView1.Items.Clear();
            //设置列表框的表头

            string strPath = tn.FullPath;
            strPath = strPath.Remove(0, 5);
            //获得当前目录下的所有文件 
            ListView(strPath);
        }

        /// <summary>
        /// 根据路径向listView添加文件信息
        /// </summary>
        /// <param name="strName">路径</param>
        protected virtual void InitListView(string strName)
        {

            this.Cursor = Cursors.WaitCursor;
            listView1.Clear();
            //设置列表框的表头
            listView1.Columns.Add("文件名", 160, HorizontalAlignment.Left);
            listView1.Columns.Add("文件大小", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("创建时间", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("访问时间", 200, HorizontalAlignment.Left);


            //获得当前目录下的所有文件 
            ListView(strName);

        }
#endregion
        #region 向treeview添加文件夹信息

        /// <summary>
        /// 向treeview添加文件夹信息
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="nodetext"></param>
        private void AddDirectories(string strName, string nodetext)
        {
            TreeNode newNode = new TreeNode(nodetext, 2, 2);
            TreeNode tn = treeView1.SelectedNode;
            for (int i = 0; i < tn.Nodes.Count; i++)
                if (tn.Nodes[i].Text == nodetext)
                    tn.Nodes[i].Remove();
            tn.Nodes.Add(newNode);
            //  MessageBox.Show(treeView1.SelectedNode.Text);
            newNode.Nodes.Clear();
            string strPath = strName;
            DirectoryInfo dirinfo = new DirectoryInfo(strPath);
            DirectoryInfo[] adirinfo;
            try
            {
                adirinfo = dirinfo.GetDirectories();
            }
            catch
            { return; }
            foreach (DirectoryInfo di in adirinfo)
            {
                TreeNode tnDir = new TreeNode(di.Name, 2, 2);
                newNode.Nodes.Add(tnDir);
            }
            treeView1.SelectedNode = newNode;
            newNode.Expand();
        }

        /// <summary>
        /// 向treeview添加文件夹信息
        /// </summary>
        /// <param name="tn"></param>
        private void AddDirectories2(TreeNode tn)
        {
            tn.Nodes.Clear();
            string strPath = tn.FullPath;
            strPath = strPath.Remove(0, 5);
            DirectoryInfo dirinfo = new DirectoryInfo(strPath);
            DirectoryInfo[] adirinfo;
            try
            {
                adirinfo = dirinfo.GetDirectories();
            }
            catch
            { return; }
            foreach (DirectoryInfo di in adirinfo)
            {
                TreeNode tnDir = new TreeNode(di.Name, 2, 2);
                tn.Nodes.Add(tnDir);           
            }
        }
        /// <summary>
        /// 向treeview添加文件夹信息
        /// </summary>
        /// <param name="tn"></param>
        private void AddDirectories(TreeNode tn)
        {
            tn.Nodes.Clear();
            string strPath = tn.FullPath;
            strPath = strPath.Remove(0, 5);
            DirectoryInfo dirinfo = new DirectoryInfo(strPath);
            DirectoryInfo[] adirinfo;
            try
            {
                adirinfo = dirinfo.GetDirectories();
            }
            catch
            { return; }
            foreach (DirectoryInfo di in adirinfo)
            {
                TreeNode tnDir = new TreeNode(di.Name,2,2);
                tn.Nodes.Add(tnDir);
                AddDirectories2(tnDir);
            }
            stack.Push(strFilePath);
        }
#endregion
        #region 窗体初始化
        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void Init()
        {
            //MessageBox.Show(Directory.GetCurrentDirectory());
            ImageList imagelist = new ImageList();
            try
            {
                
                //imagelist.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "../Images/0001.ICO"));
                //imagelist.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "../Images/0002.ICO"));
                //imagelist.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "../Images/0003.ICO"));
                imagelist.Images.Add(Resources._0001);
                imagelist.Images.Add(Resources._0002);
                imagelist.Images.Add(Resources._0003);
            }
            catch
            {
            }
            treeView1.ImageList=imagelist;
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            TreeNode bootnode = new TreeNode("我的电脑",0,0);
            treeView1.Nodes.Add(bootnode);
            string[] drivers = System.IO.Directory.GetLogicalDrives();
            foreach (string str in drivers)
            {
                //comboBox1.Items.Add(str);
                TreeNode tndrivers = new TreeNode(str, 1, 1);
                treeView1.Nodes[0].Nodes.Add(tndrivers);
                AddDirectories(tndrivers);
                
            }
            treeView1.EndUpdate();
            treeView1.Nodes[0].Expand();
        }

        #endregion

        #region 窗体事件
        private void MainFrm_Load(object sender, EventArgs e)
        {
            label9.Text = "";
            stack = new Stack<string>();
            InitListView();
            Init();
			//InitCombox();
            toolStripButton4.Enabled = false;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "我的电脑")
            {
                return;
            }

                InitListView(e.Node);

        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

                if (e.Node.Text == "我的电脑")
                    return;
                AddDirectories(e.Node);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (flag)
                {
                    checkBox1.Checked = false;
                    if (e.Node.Text == "我的电脑")
                    {
                        toolStripButton4.Enabled = false;
                        return;
                    }
                    else
                    {
                        toolStripButton4.Enabled = true;
                    }
                    AddDirectories(e.Node);
                    InitListView(e.Node);
                    if (!e.Node.IsExpanded)
                        e.Node.Expand();
                    if (!e.Node.IsExpanded)
                    {
                        e.Node.Expand();
                    }
                    FilePath = e.Node.FullPath;
                    if (FilePath.IndexOf("\\\\") != -1)
                        FilePath = FilePath.Remove(FilePath.IndexOf("\\\\"), 1);
                }
                else
                    flag = true;
                label9.Text = strFilePath;
            //label9.Text = strFilePath;
            }
            catch
            {
            }
        }
      
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].Focused = true;
                    listView1.Items[i].Selected = true;

                }
            else
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].Focused = false;
                    listView1.Items[i].Selected = false;

                }
            }
            listView1.Focus();
            listView1.Refresh();
            Application.DoEvents();
        }



        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            string str = Path.Combine(strFilePath, listView1.FocusedItem.Text);
            string str2=listView1.FocusedItem.Text.ToString();
            try
            {
                if (listView1.FocusedItem.SubItems.Count > 1)
                { System.Diagnostics.Process.Start(str); }
                else
                {
                    InitListView(str);
                    AddDirectories(str, str2); 
                }
            }
            catch { return; }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
        }

        private void delTooltrip_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                bool directory = false;
                bool file = false;
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    try
                    {

                        string filepath = "";
                        filepath = strFilePath;
                        filepath = filepath + "\\" + listView1.SelectedItems[i].Text;
                        DirectoryInfo dir = new DirectoryInfo(@filepath);
                        dir.GetFiles();
                        directory = true;
                    }
                    catch
                    {
                        file = true;
                    }
                }
               try
                {
                    if (directory&&!file)
                    {
                        if (MessageBox.Show("是否真的删除所选文件夹？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int i;
                            for (i = 0; i < listView1.SelectedItems.Count; i++)
                            {
                                string filepath = "";
                                filepath = strFilePath;
                                filepath = filepath + "\\" + listView1.SelectedItems[i].Text;
                                DirectoryInfo dir = new DirectoryInfo(@filepath);
                                FileInfo[] getFiles = null;
                                getFiles = dir.GetFiles();
                                if (getFiles != null)
                                {
                                    dir.Delete(true);
                                }
                            }

                        }
                        else
                            return;
                    }
                }
                catch
                {
                }
                try
                {
                    if (file&&!directory)
                    {
                        if (MessageBox.Show("是否真的删除所选文件？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            for (int i = 0; i < listView1.SelectedItems.Count; i++)
                            {
                                string filepath = "";
                                filepath = strFilePath;
                                //filepath = filepath.Remove(2, 1);

                                File.Delete(@filepath + "\\" + listView1.SelectedItems[i].Text);


                            }
                        }
                    }
                }
                catch
                {
                }
                try
                {
                    if (file && directory)
                    {
                        if (MessageBox.Show("是否真的删除所选项？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int i;
                            for (i = 0; i < listView1.SelectedItems.Count; i++)
                            {
                                try
                                {
                                    string filepath = "";
                                    filepath = strFilePath;
                                    filepath = filepath + "\\" + listView1.SelectedItems[i].Text;
                                    DirectoryInfo dir = new DirectoryInfo(@filepath);
                                    FileInfo[] getFiles = null;
                                    getFiles = dir.GetFiles();
                                    if (getFiles != null)
                                    {
                                        dir.Delete(true);
                                    }
                                }
                                catch
                                {
                                    string filepath = "";
                                    filepath = strFilePath;
                                   // filepath = filepath.Remove(2, 1);

                                    File.Delete(@filepath + "\\" + listView1.SelectedItems[i].Text);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                TreeNode tn = treeView1.SelectedNode;
                AddDirectories(tn);
                InitListView(tn);
                tn.Expand();
                label9.Text = FilePath.Substring(5, FilePath.Length - 5);
            }
        }

        private static bool flag = true;
        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            flag = false;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Selected = true;
                listView1.Items[i].Focused = true;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            if (listView1.SelectedItems.Count != 1)
                label9.Text = strFilePath;
            else
            {
                string str=strFilePath + "\\" + listView1.SelectedItems[0].Text;
                if(str.IndexOf("\\\\")!=-1)
                    str=str.Remove(str.IndexOf("\\\\"),1);
                label9.Text = str;
            }
        }


        #endregion

        #region 重命名准备操作
        struct Node
        {
           public  int id;
           public string data;
        }
        struct PinLv
        {
            public string data;
            public int cout;
        }
        private static ArrayList common;
        private ArrayList arrayListSort(ArrayList arraylist)
        {
            for (int i = 0; i < arraylist.Count; i++)
            {

                for (int j = arraylist.Count-1; j >i; j--)
                {
                    PinLv p1 = (PinLv)arraylist[j];
                    PinLv p2 = (PinLv)arraylist[j-1];
                    if (p1.cout > p2.cout)
                    {
                        PinLv p = new PinLv();
                        p =(PinLv) arraylist[i];
                        arraylist[i] = arraylist[j];
                        arraylist[j] = p;
                    }
                }
            }
            return arraylist;
        }
        private ArrayList Compare(ArrayList array)
        {
            ArrayList newarray = new ArrayList();
            ArrayList myarray = new ArrayList();
            ArrayList temp = new ArrayList();
            for (int i = 0; i < array.Count; i++)
            {
                ListViewItem listviewItem = (ListViewItem)array[i];
                string str=listviewItem.Text;
                try
                {
					string before = "";
					string end = "";
					string filepath = strFilePath + "\\" + str;
					//文件
					if (File.Exists(filepath) && str.LastIndexOf('.') != -1)
					{
						before = str.Substring(0, str.LastIndexOf('.'));
						end = str.Substring(str.LastIndexOf('.'));
					}
					else
						if (Directory.Exists(filepath))// 目录
						{
							before = str;
						}
					myarray.Add(before);
                    
                    if (temp.Count == 0)
                    {
                        PinLv pinlv = new PinLv();
						pinlv.data = end;
                        pinlv.cout = 0;
                        temp.Add(pinlv);
                    }
                    else
                    {
                        int j;
                        for (j = 0; j < temp.Count; j++)
                        {
                            PinLv p = (PinLv)temp[j];
                            if (str.CompareTo(p.data) == 0)
                            {
                                p.cout++;
                                temp[j] = p;
                                break;
                            }
                        }
                        if (j == temp.Count)
                        {
                            PinLv pinlv = new PinLv();
                            pinlv.data = str;
                            pinlv.cout = 0;
                            temp.Add(pinlv);
                        }
                    }  
                    Node node = new Node();
                    node.id = i;
                    node.data = end;
                    newarray.Add(node);
                }
                catch
                {

                }
             
            }
			if (newarray.Count == 0)
            {
                MessageBox.Show("请确定选中两个或两个以上有相同特征的文件！", "重命名", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            string s = "";

			if (temp.Count == 0)
            {
                MessageBox.Show("不符合批处理条件！");
				return null;
            }
            temp=arrayListSort(temp);
            PinLv pin = new PinLv();
            pin = (PinLv)temp[0];
            s = pin.data;
            common=new ArrayList();
            for (int i = 0; i < newarray.Count; i++)
            {
                Node node = (Node)newarray[i];
                if (s.CompareTo(node.data) == 0)
                    common.Add(myarray[i]+s);
            }
            return common;
        }
        
        private void getcommon(string str1, string str2)
        {
            for (int i = 0; i < str1.Length; i++)
            {
                int m = i;

                int k = 0;
                for (int j = 0; j < str2.Length; j++)
                {
                    char[] c = new char[500];
                    int n = j;
                    while (m < str1.Length && n < str2.Length && str1[m] == str2[n])
                    {
                        c[k] = str1[m];
                        m++;
                        n++;
                        k++;
                    }
                    if (c[0] != '\0')
                    {
                        string str = new string(c);
                                


                        k = 0;
                    }
                    m = i;
                    //break;

                }
            }
        }
        
        private string delHouzhui(string filename)
        {
            try
            {
                return filename.Substring(0, filename.LastIndexOf('.'));
            }
            catch
            {
                return null;
            }
        }
        #endregion
        private static ArrayList array;
        public ArrayList Array
        {
            get
            {
                return array;
            }
            set
            {
                array = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        public class MyNode
        {
            public string oldName;
            public string newName;

		}

		private int CompareDinosByLength(string x, string y)
		{
			if(x.Length!=y.Length)
				return x.Length.CompareTo(y.Length);
			return x.CompareTo(y);
		}

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            //panel1.Controls.Clear();
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("必须选中至少一个文件！", "序列化", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
          
            try
            {
                string filepath = "";
                filepath = strFilePath;
                //filepath = filepath.Remove(2, 1);
                array = new ArrayList();

                //List<string> list = new List<string>();
                //for (int i = 0; i < listView1.SelectedItems.Count; i++)
                //{
                //    list.Add(listView1.SelectedItems[i].Text);
                //}

                currFileList.Sort(CompareDinosByLength);

                for (int i = 0; i < currFileList.Count; i++)
                {
                    string name = "";
                    int j = i + 1;
                    if (j < 10)
                        name = "0" + j;
                    else
                        name = j.ToString();

                    if (currFileList[i].LastIndexOf('.') != -1)
					{
                        string houzhui = currFileList[i].Substring(currFileList[i].LastIndexOf('.'));
						MyNode node = new MyNode();
                        node.oldName = filepath + "\\" + currFileList[i];
						node.newName = filepath + "\\" + name + houzhui;
						array.Add(node);
					}
					else
					{
						MyNode node = new MyNode();
                        node.oldName = filepath + "\\" + currFileList[i];
						node.newName = filepath + "\\" + name;
						array.Add(node);
					}
                }

                Form2 frm = new Form2();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    TreeNode tn = treeView1.SelectedNode;
                    AddDirectories(tn);
                    InitListView(tn);
                    tn.Expand();
                }
            }
            catch
            {
                
            }            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("必须选中至少一个文件！", "重命名",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            ArrayList myarray = new ArrayList();
			//myarray.AddRange(listView1.SelectedItems);
			//myarray = Compare(myarray);
			//if (myarray == null)
			//    return;
			myarray.AddRange(listView1.SelectedItems);
            try
            {
                string filepath = "";
                filepath = strFilePath;
                //filepath = filepath.Remove(2, 1);
                array = new ArrayList();
                for (int i = 0; i < myarray.Count; i++)
                {
                    MyNode node = new MyNode();
                    node.oldName = filepath + "\\" + ((ListViewItem)myarray[i]).Text;
                    node.newName = "";
                    array.Add(node);
                }
                Form3 frm = new Form3();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    TreeNode tn = treeView1.SelectedNode;
                    AddDirectories(tn);
                    InitListView(tn);
                    tn.Expand();
                }
            }
            catch 
            {
               
            }            
        }

        private void 序列化XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel1_Click(sender, e);
        }

        private void 重命名RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(sender, e);
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (stack.Count == 0) return;
            string path = stack.Pop();
            if (path.Equals(string.Empty)) return;
            string str = Path.GetFileNameWithoutExtension(path);
            string str2 = Path.GetDirectoryName(path);
            if (str == null || str2 == null) return;
            InitListView(str2);
            AddDirectories(str2, str);
            string strPath = strFilePath;
            if (strPath.IndexOf("\\\\") != -1)
                strPath = strPath.Remove(strPath.IndexOf("\\\\"), 1);
            label9.Text = strPath;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            
            TreeNode tn = treeView1.SelectedNode.Parent;
            if (tn.Text == "我的电脑")
            {
                toolStripButton4.Enabled = false;
                return;
            }
            else
                toolStripButton4.Enabled = true;
            AddDirectories(tn);
            InitListView(tn);
            tn.Expand();
            //label9.Text = FilePath.Substring(5, FilePath.Length - 5);
            string str = strFilePath;
            if (str.IndexOf("\\\\") != -1)
                str = str.Remove(str.IndexOf("\\\\"), 1);
            label9.Text = str;

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = treeView1.SelectedNode.Parent;
            string filepath = "";
            try
            {
                filepath = treeView1.SelectedNode.FullPath.Remove(0, 5).Replace("\\\\", "\\");
                DirectoryInfo dir = new DirectoryInfo(@filepath);
                FileInfo[] getFiles = null;
                getFiles = dir.GetFiles();
                if (getFiles == null) return;
                if (getFiles.Length != 0)
                {
                    if (MessageBox.Show("所选文件夹不为空，确定删除？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dir.Delete(true);
                    }
                    else
                        return;
                }
                else
                {
                    if (MessageBox.Show("是否真的删除所选文件夹？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dir.Delete(true);
                    }
                    else
                        return;
                }
            }
            catch
            {
            }
            AddDirectories(tn);
            InitListView(tn);
            tn.Expand();
            label9.Text = FilePath.Substring(5, FilePath.Length - 5);
        }
    
        private static ArrayList fileList;
        public ArrayList FileList
        {
            set
            {
                fileList = value;
            }
            get
            {
                return fileList;
            }
        }

        private void toolAttribute_Click(object sender, EventArgs e)
        {
            
            if (listView1.SelectedItems != null)
            {
                ArrayList array = new ArrayList();
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    array.Add(strFilePath + "\\" + listView1.SelectedItems[i].Text);
                }
                fileList = array;
                Attribute frm = new Attribute();
                frm.ShowDialog();
            }
        }

        private void 退出QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolSort_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                try
                {
                    listView1.ListViewItemSorter = new ListViewItemComparer(0);
                }
                catch
                {
                }
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                if(e.Column!=1)
                    listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);               
            }
            catch
            {
            }
        }

        private void getFiles(string sourcepath,string destpath)
        {
            string[] directorys = Directory.GetDirectories(sourcepath);
            if (directorys.Length != 0)
            {
                for (int i = 0; i < directorys.Length; i++)
                    getFiles(directorys[i],destpath);
            }
            else
            {
                try
                {
                    string[] files = Directory.GetFiles(sourcepath);
                    for (int j = 0; j < files.Length; j++)
                    {
                        if (File.Exists(Path.Combine(destpath, Path.GetFileName(files[j]))))
                            File.Move(files[j], Path.Combine(destpath, "new_" + Path.GetFileName(files[j])));
                        else
                            File.Move(files[j], Path.Combine(destpath, Path.GetFileName(files[j])));
                    }
                    Directory.Delete(sourcepath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void getFileTool_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("必须选中一个文件夹！", "序列化", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                string str = strFilePath + "\\" + listView1.SelectedItems[0].Text;
                if (str.IndexOf("\\\\") != -1)
                    str = str.Remove(str.IndexOf("\\\\"), 1);
                if (Directory.Exists(str))
                {
                    FolderDialog folderdialog = new FolderDialog();//文件夹对话框
                    if (folderdialog.DisplayDialog() == DialogResult.OK)
                    {
                        getFiles(str, folderdialog.Path);

                    }
                }
            }
        }

        private static string strForm4;
        public string StrForm4
        {
            get
            {
                return strForm4;
            }
            set
            {
                strForm4 = value;
            }
        }

        private List<string> selectedFiles;
        public List<string> SelectedFiles
        {
            get
            {
                return selectedFiles;
            }
            set
            {
                selectedFiles = value;
            }
        }

        private void getTextTool_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("必须选中一个文件夹！", "序列化", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                selectedFiles = new List<string>();
                strForm4 = "";
                if (listView1.SelectedItems.Count > 1)
                {
                    for (int i = 0; i < listView1.SelectedItems.Count; i++)
                    {
                        string str = strFilePath + "\\" + listView1.SelectedItems[i].Text;
                        if (str.IndexOf("\\\\") != -1)
                        str = str.Remove(str.IndexOf("\\\\"), 1);
						//if(File.Exists(str))
						//{
						//    selectedFiles.Add(str);
						//}
						selectedFiles.Add(str);
                        
                    }
                    strForm4 = strFilePath;
                }
                else
                {
                    string str = strFilePath + "\\" + listView1.SelectedItems[0].Text;
                    if (str.IndexOf("\\\\") != -1)
                        str = str.Remove(str.IndexOf("\\\\"), 1);
					
					//if (Directory.Exists(str) && Directory.GetFiles(str) != null)
					//{
					//    strForm4 = str;
					//}
					if (Directory.Exists(str))
					{
						selectedFiles.AddRange(Directory.GetFiles(str));
						selectedFiles.AddRange(Directory.GetDirectories(str));
					}
					else
						selectedFiles.Add(str);
                }
				strForm4 = strFilePath;
                Form4 form4 = new Form4();
                form4.Show(this);
            }
            
        }

		public string scanPath = "";

		private void OpenDirectory()
		{
			string strPath = scanPath;
			string[] paths = strPath.Split('\\');
			TreeNode root = treeView1.Nodes[0];
			foreach (string path in paths)
			{
				if (path.Trim() == "") continue;
				TreeNode node = FindNode(root, path);
				if (node == null)
				{
					MessageBox.Show("输入的目录有误！", "编辑", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}
				treeView1_NodeMouseClick(treeView1, new TreeNodeMouseClickEventArgs(node, MouseButtons.Middle, 1, 0, 0));
				root = node;
			}
			treeView1.SelectedNode = root;
		}

		private TreeNode FindNode(TreeNode root,string displayText)
		{
			foreach (TreeNode node in root.Nodes)
			{
				if (node.Text.Replace("\\","") == displayText)
				{
					return node;
				}
			}
			return null;
		}

		private void tsCmmFileSearch_SelectedIndexChanged(object sender, EventArgs e)
		{
			OpenDirectory();
        }

		private void MainFrm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control&&e.KeyCode == Keys.F )
			{
				MessageBox.Show("aaa");
			}
		}

		private void scanToolStrip_Click(object sender, EventArgs e)
		{
			FrmScan frmScan = new FrmScan();
			if (frmScan.ShowDialog(this) == DialogResult.OK)
			{
				OpenDirectory();
			}
		}


		private void getCurrentDirToolStrip_Click(object sender, EventArgs e)
		{
			if (listView1.Items.Count > 0)
			{
				try
				{
					 string str=strFilePath + "\\" + listView1.SelectedItems[0].Text;
					 if(str.IndexOf("\\\\")!=-1)
						str=str.Remove(str.IndexOf("\\\\"),1);
					System.Diagnostics.Process.Start("explorer", "/select," + str);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

	




    }
}