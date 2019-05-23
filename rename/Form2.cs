using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace rename
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private static ArrayList array;
        private void InitListView()
        {
            listView1.BeginUpdate();
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.View = View.Details;
            listView1.Scrollable = true;
            listView1.MultiSelect = true;

            listView1.Columns.Add("原文件名", 300, HorizontalAlignment.Left);
            listView1.Columns.Add("新文件名", 300, HorizontalAlignment.Left);

            listView1.EndUpdate();
        }
        private void DisplayPathandName()
        {
            listView1.Clear();
            listView1.Columns.Add("原文件", 300, HorizontalAlignment.Left);
            listView1.Columns.Add("新文件", 300, HorizontalAlignment.Left);

            for (int i = 0; i < array.Count; i++)
            {
                MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                string[] files = new string[2];
                files[0] = node.oldName;
                files[1] = node.newName;
                ListViewItem item = new ListViewItem(files, i);
                listView1.Items.Insert(i, item);
            }
        }
        private void DisplayName()
        {
            listView1.Clear();
            listView1.Columns.Add("原文件名", 300, HorizontalAlignment.Left);
            listView1.Columns.Add("新文件名", 300, HorizontalAlignment.Left);

            for (int i = 0; i < array.Count; i++)
            {
                MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                string[] files = new string[2];
                files[0] = node.oldName.Substring(node.oldName.LastIndexOf('\\')+1);
                files[1] = node.newName.Substring(node.newName.LastIndexOf('\\')+1);
                ListViewItem item = new ListViewItem(files, i);
                listView1.Items.Insert(i, item);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            //listBox1.Items.Clear();
            InitListView();
            
            MainFrm frm = new MainFrm();
            array = new ArrayList();
            array = frm.Array;
            if (array == null) return;
            DisplayPathandName();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
               if (checkBox1.Checked)
            {
                DisplayName();
            }
            else
            {
                DisplayPathandName();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < array.Count; i++)
            {
                MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                //node.newName = node.newName.Insert(node.newName.LastIndexOf('\\')+1, textBox1.Text);
                string old=node.newName.Substring(node.newName.LastIndexOf('\\')+1);
                string name = "";
                int j = i + 1;
                if (j < 10)
                    name = "0" + j;
                else
                    name = j.ToString();

				string before = node.newName.Substring(0, node.newName.LastIndexOf('\\') + 1);

				if (old.LastIndexOf('.') != -1)
				{
					string New = textBox1.Text + name + old.Substring(old.LastIndexOf('.'));
					node.newName = before + New;
				}
				else
				{
					string New = textBox1.Text + name;
					node.newName = before + New;
				}
                array[i] = node;
            }
            if (checkBox1.Checked)
            {
                DisplayName();
            }
            else
            {
                DisplayPathandName();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                for (int i = 0; i < array.Count; i++)
                {
                    MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                    string[] files = new string[2];
                    files[0] = node.oldName;
                    files[1] = node.newName;

					if (files[0] == files[1]) continue;

					if(File.Exists(files[0]))
						File.Move(@files[0], @files[1]);
					else
						if(Directory.Exists(files[0]))
							Directory.Move(@files[0], @files[1]);
                    count++;
                }
                MessageBox.Show("成功更改文件" + count.ToString() + "个！");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch(Exception ex)
            {
                string mes = ex.Message;
                MessageBox.Show("当前文件名已经存在，请重新命名！","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }

        private void cbNewExt_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewExt.Text)) txtNewExt.Text = ".txt";
            if (cbNewExt.Checked)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    string ext = ((MainFrm.MyNode)array[i]).newName.Substring(((MainFrm.MyNode)array[i]).newName.LastIndexOf('.'));
                    string newname = ((MainFrm.MyNode)array[i]).newName.Replace(ext, txtNewExt.Text);
                    ((MainFrm.MyNode)array[i]).newName = newname;
                }
            }

            if (checkBox1.Checked)
            {
                DisplayName();
            }
            else
            {
                DisplayPathandName();
            }
        }
    }
}