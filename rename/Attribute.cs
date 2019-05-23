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
    public partial class Attribute : Form
    {
        public Attribute()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private static long fileSize = 0;
        private static string fileType = "文件夹";
        private static bool fileIsReadOnly = false;
        private static bool fileIsHide = false;


        private void GetSize(string path, bool readOnly, bool hide)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                DirectoryInfo[] infos = info.GetDirectories();
                for (int i = 0; i < infos.Length; i++)
                {
                    GetSize(path + "\\" + infos[i].ToString(),readOnly,hide);
                }
                FileInfo[] files = info.GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    GetSize(path + "\\" + files[i].ToString(),readOnly,hide);
                }
            }
            catch
            {

                FileInfo info = new FileInfo(path);
                fileSize += info.Length;
                fileType = path.Substring(path.LastIndexOf('.') + 1).ToUpper() + "文件";
                fileIsReadOnly = info.IsReadOnly;
                if ((File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    fileIsHide = true;
                }
                if (readOnly)
                {
                    info.IsReadOnly = checkBox1.Checked;
                }
                if (hide)
                {
                    if ((File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        // Show the file.
                        File.SetAttributes(path, FileAttributes.Archive);
                    }
                    else
                    {
                        // Hide the file.
                        File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
                    }

                }
            }
        }

        private void Init(ArrayList array)
        {
            fileSize = 0;
            fileType = "文件夹";
            fileIsReadOnly = false;
            fileIsHide = false;
            try
            {
                for (int i = 0; i < array.Count; i++)
                {
                    GetSize((string)array[i],false ,false );
                }
            }
            catch
            {

            }
             if (array.Count > 1)
            {
                string path = (string)array[0];
                lbType.Text = "多种类型";
                lbPosition.Text = path.Substring(0, path.LastIndexOf('\\'));
                checkBox1.Checked= false;
                checkBox2.Checked = false;
            }
            else
            {
                lbType.Text = fileType;
                lbPosition.Text = (string)array[0];
                checkBox1.Checked = fileIsReadOnly;
                checkBox2.Checked = fileIsHide;
            }
            long length = 0;
            if (fileSize > 1024 * 1024 * 1024)
            {
                double len = (double)fileSize / (double)(1024 * 1024 * 1024);
                lbSize.Text = Math.Round(len, 2) + "G";
            }
            else
                if (fileSize > 1024 * 1024)
                {
                    double len = (double)fileSize / (double)(1024 * 1024);
                    if ((long)len > 100)
                        lbSize.Text = Math.Round(len, 0) + "M";
                    else
                        lbSize.Text = Math.Round(len, 1) + "M";
                }
                else
                    if (fileSize > 1024)
                    {
                        length = fileSize / 1024;
                        lbSize.Text = length.ToString() + "K";
                    }
                    else
                        lbSize.Text = fileSize + " 字节";
        }

        private void Attribute_Load(object sender, EventArgs e)
        {
            lbPosition.Text = lbSize.Text = lbType.Text = string.Empty;
            MainFrm frm = new MainFrm();
            ArrayList array = frm.FileList;
            Init(array);
            flagReadOnly = false;
            flagHide = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (flagReadOnly&&flagHide)
            {
                MainFrm frm = new MainFrm();
                ArrayList array = frm.FileList;
                for (int i = 0; i < array.Count; i++)
                {
                    GetSize((string)array[i], true, true);
                }
            }
            else if (flagReadOnly)
            {
                MainFrm frm = new MainFrm();
                ArrayList array = frm.FileList;
                for (int i = 0; i < array.Count; i++)
                {
                    GetSize((string)array[i], true, false);
                }
            }
            else if (flagHide)
            {
                MainFrm frm = new MainFrm();
                ArrayList array = frm.FileList;
                for (int i = 0; i < array.Count; i++)
                {
                    GetSize((string)array[i], false, true);
                }
            }
            this.Close();
        }
        private static bool flagReadOnly=false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!flagReadOnly)
                flagReadOnly = true;
            else
                flagReadOnly = false;
        }
        private static bool flagHide = false;
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!flagHide)
                flagHide = true;
            else
                flagHide = false;
        }
    }
}