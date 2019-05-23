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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private static ArrayList array;
        private static string[] temp;
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
                files[0] = node.oldName.Substring(node.oldName.LastIndexOf('\\') + 1);
                files[1] = node.newName.Substring(node.newName.LastIndexOf('\\') + 1);
                ListViewItem item = new ListViewItem(files, i);
                listView1.Items.Insert(i, item);
            }
        }
        private void getcommon(string str1, string str2)
        {
            //object[] items = new object[str1.Length];
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
                        //items[i] = str;
                        comboBox2.Items.Add(str);
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
        private void GetCommon()
        {
            if (array.Count > 2)
            {
                MainFrm.MyNode node1 = (MainFrm.MyNode)array[0];
                MainFrm.MyNode node2 = (MainFrm.MyNode)array[1];

                string oldname1 = node1.oldName.Substring(node1.oldName.LastIndexOf("\\") + 1);
                string oldname2 = node2.oldName.Substring(node2.oldName.LastIndexOf("\\") + 1);
                getcommon(delHouzhui(oldname1), delHouzhui(oldname2));
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            InitListView();
            MainFrm frm = new MainFrm();
            array = new ArrayList();
            array = frm.Array;
            temp = new string[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                temp[i] = node.oldName;
            }
            GetCommon();
            if (array == null) return;
            DisplayPathandName();
            label1.Text = "";
            label4.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            const string InvalidPathChars = "\\/:\"*?<>|";

            if (textBox3.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show("对不起，新文件名不能为空！");
                return;
            }
            else
            if(textBox3.Text!="")
            {
                foreach (char ch in textBox3.Text)
                {
                    foreach (char c in InvalidPathChars)
                    {
                        if (ch == c)
                        {
                            MessageBox.Show("新文件名中含有非法字符，请重新命名！","重命名");
                            return;

                        }
                    }
                }
            }

                    int count = 0;
                    for (int i = 0; i < array.Count; i++)
                    {
                        MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                        string oldname = node.oldName;
                        string newName = node.newName;
                        try
                        {
                            if (newName == string.Empty)
                                newName = node.oldName.Remove(node.oldName.LastIndexOf(textBox2.Text), textBox2.Text.ToString().Length);
							if (File.Exists(oldname))
								File.Move(@oldname, @newName);
							else
								if (Directory.Exists(oldname))
									Directory.Move(@oldname, @newName);
                            count++;
                        }
                        catch(Exception ex)
                        {
							MessageBox.Show(ex.Message);
							break;
                        }
                    }
                    if (count != 0)
                    {
                        MessageBox.Show("成功更改文件" + count.ToString() + "个！");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else MessageBox.Show("更改失败，可能是是重名！");
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == string.Empty)
            {
                label1.Text = "请保证“1、源文件名中的部分字符”不为空";
                return;
            }
            else
                label1.Text = "";
            for (int i = 0; i < array.Count; i++)
            {
                MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                try
                {
				   //
					string before = node.oldName.Substring(0, node.oldName.LastIndexOf("\\")+1);
					string end = node.oldName.Substring(node.oldName.LastIndexOf("\\") + 1);

					node.newName = before + end.Replace(textBox2.Text, textBox3.Text);
                    array[i] = node;
                }
                catch
                {
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = 0; i < array.Count; i++)
            {
                MainFrm.MyNode node = (MainFrm.MyNode)array[i];
               
                   int index= node.oldName.IndexOf(textBox2.Text);
                  if (index ==-1)
                {
                    label4.Text = "字符与某些源文件不匹配";
                    label4.ForeColor = Color.Blue;
                    count++;
                }
                else
                {
                    label4.Text = "";
                }
            }
            if (count == array.Count)
            {
                label4.Text = "没有匹配的源文件";
                label4.ForeColor = Color.Red;
            }
          
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count >= 2)
            {
                string str1;
                string str2;
                //if (checkBox2.Checked)
                //{
                //    str1 = listView1.SelectedItems[0].SubItems[1].Text;
                //    str2 = listView1.SelectedItems[1].SubItems[1].Text;
                //}
                //else
                //{
                //    str1 = listView1.SelectedItems[0].SubItems[0].Text;
                //    str2 = listView1.SelectedItems[1].SubItems[0].Text;
                //}
                str1 = listView1.SelectedItems[0].SubItems[0].Text;
                str2 = listView1.SelectedItems[1].SubItems[0].Text;
                str1 = str1.Substring(str1.LastIndexOf("\\") + 1);
                str2 = str2.Substring(str2.LastIndexOf("\\") + 1);
                getcommon(delHouzhui(str1), delHouzhui(str2));
            }
            else
            {
                MainFrm.MyNode node = (MainFrm.MyNode)array[0];
                string str1 = node.oldName.Substring(node.oldName.LastIndexOf('\\') + 1);
                textBox2.Text = delHouzhui(str1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        //private void checkBox2_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBox2.Checked)
        //    {
        //        for (int i = 0; i < array.Count; i++)
        //        {
        //            MainFrm.MyNode node = (MainFrm.MyNode)array[i];
        //            string oldname = node.oldName;
        //            string newName = node.newName;
        //            try
        //            {
        //                if (textBox3.Text == string.Empty)
        //                {
        //                    if (node.newName == string.Empty)
        //                        newName = node.oldName.Remove(node.oldName.LastIndexOf(textBox2.Text), textBox2.Text.ToString().Length);
        //                    else
        //                        newName = node.newName.Remove(node.newName.LastIndexOf(textBox2.Text), textBox2.Text.ToString().Length);
        //                    node.newName = newName;
        //                    array[i] = node;
        //                }
        //                else
        //                {
        //                    node.newName = node.oldName.Replace(textBox2.Text, textBox3.Text);
        //                    array[i] = node;
        //                }

        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    else
        //        button1_Click(sender, e);
        //    if (checkBox1.Checked)
        //    {
        //        DisplayName();
        //    }
        //    else
        //    {
        //        DisplayPathandName();
        //    }
        //}

        
        
        private static  Stack stack=new Stack();
        private void button3_Click(object sender, EventArgs e)
        {
            ArrayList myarray = new ArrayList();
            for (int i = 0; i < array.Count; i++)
            {
                MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                string oldname = temp[i];
                string newName = node.newName;
                try
                {
                    myarray.Add(oldname);
                    if (textBox3.Text == string.Empty)
                    {
                        int pos=oldname.LastIndexOf(textBox2.Text);
                        string str=textBox2.Text;
                        temp[i] = oldname.Remove(pos, str.Length);
                        node.newName = temp[i];
                        array[i] = node;
                    }
                    else
                    {
						//
						string before = node.oldName.Substring(0, node.oldName.LastIndexOf("\\") + 1);
						string end = node.oldName.Substring(node.oldName.LastIndexOf("\\") + 1);

						node.newName = before + end.Replace(textBox2.Text, textBox3.Text);
                        //node.newName = oldname.Replace(textBox2.Text, textBox3.Text);
                        array[i] = node;
                    }
                    
                }
                catch
                {
                    return;
                }
            }
            stack.Push(myarray);
            if (checkBox1.Checked)
            {
                DisplayName();
            }
            else
            {
                DisplayPathandName();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (stack.Count != 0)
            {
                ArrayList myarray = (ArrayList)stack.Pop();
                for (int i = 0; i < myarray.Count; i++)
                {
                    string str = (string)myarray[i];
                    temp[i] = str;
                    MainFrm.MyNode node = (MainFrm.MyNode)array[i];
                    node.newName = str;
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
            
        }
    }
}