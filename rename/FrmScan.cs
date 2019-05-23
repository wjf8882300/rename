using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace rename
{
	public partial class FrmScan : Form
	{
		public FrmScan()
		{
			InitializeComponent();
		}

		void InitCombox()
		{
			string fileSearch = ConfigurationManager.AppSettings["FileSearch"];
			if (fileSearch.Trim() != "")
			{
				string[] paths = fileSearch.Split(',');
				foreach (string path in paths)
				{
					tsCmmFileSearch.Items.Add(path);
				}
				tsCmmFileSearch.SelectedIndex = 0;
			}
		}

		private void bntOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			MainFrm mfrm= this.Owner as MainFrm;
			mfrm.scanPath = tsCmmFileSearch.Text;
			
		}

		private void FrmScan_Load(object sender, EventArgs e)
		{
			InitCombox();
		}
	}
}