using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.Design; 

namespace rename
{
    class FolderDialog :FolderNameEditor
    {
        FolderNameEditor.FolderBrowser fDialog = new System.Windows.Forms.Design.FolderNameEditor.FolderBrowser();
        public FolderDialog() { }
        public DialogResult DisplayDialog()
        { return DisplayDialog("请选择一个文件夹"); }

        public DialogResult DisplayDialog(string description)
        {
            fDialog.Style = FolderBrowserStyles.BrowseForComputer;
            fDialog.Description = description;
            return fDialog.ShowDialog();
        }
        public string Path
        {
            get
            {
                return fDialog.DirectoryPath;
            }
        }
        ~FolderDialog()
        {
            fDialog.Dispose();
        }
    }
}
