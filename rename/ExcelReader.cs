using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace rename
{
	class ExcelReader
	{
		private const string jet4 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";";
		private const string jet12 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
		public ExcelReader()
		{
		}
		public bool SearchExcel(string fileName, string txtSearch, bool ignoreCase, rename.ProgressBarChange init, rename.ProgressBarChange change)
		{
			bool res = false;
			StringComparison sctype = ignoreCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
			string jet = Path.GetExtension(fileName).Trim(new char[] { '.' }).ToUpper().Equals("XLS") ? jet4 : jet12;
			string mConnectionString = string.Format(jet, fileName);

			OleDbConnection oleConn = new OleDbConnection(mConnectionString);
			oleConn.Open();
			DataTable dtOle = oleConn.GetSchema("Tables");
			DataTableReader dtReader = new DataTableReader(dtOle);
			List<string> lsTableName = new List<string>();
			while (dtReader.Read())
			{
				string tableName = dtReader["Table_Name"].ToString();
				if (tableName.Trim() == string.Empty) continue;
				lsTableName.Add(tableName);
			}

			dtReader = null;
			dtOle = null;
			oleConn.Close();

			init(lsTableName.Count);
			int count =0;
			foreach (string tableName in lsTableName)
			{
				change(++count);
				OleDbDataAdapter myCommand = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", tableName), oleConn);
				DataSet myDataSet = new DataSet();
				try
				{
					myCommand.Fill(myDataSet);
				}
				catch (Exception ex)
				{
					//throw new Exception("该Excel文件的工作表的名字不正确," + ex.Message);
					System.Windows.Forms.MessageBox.Show("该Excel文件的工作表的名字不正确," + ex.Message);
					continue;
				}
				if (myDataSet == null || myDataSet.Tables[0].Rows.Count == 0)continue;
				DataTable dt = myDataSet.Tables[0];
				foreach (DataRow dr in dt.Rows)
				{
					foreach (DataColumn dc in dt.Columns)
					{
						object obj = dr[dc];
						if (obj != null&&!string.IsNullOrEmpty(obj.ToString()))
						{
							if (obj.ToString().IndexOf(txtSearch, sctype) != -1)
							{
								res = true;
								change(lsTableName.Count-1);
								return true;
							}
						}
					}
				}
			}
			return res;
		}


	}
}
