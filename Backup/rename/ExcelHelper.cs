using System;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace DevSoft.CommonApp.Util
{
    /// <summary>
    /// ˵    ����Excel�����ӡģ��
    ///	��ʱ���ṩ����Excel������ʽ��������ʽ������Excelģ�������ú�
    /// </summary>
    public class ExcelHelper
    {
        #region ��Ա����
        private string templetFile = null;
        private string outputFile = null;
        private object missing = Missing.Value;
        private DateTime beforeTime;			//Excel����֮ǰʱ��
        private DateTime afterTime;				//Excel����֮��ʱ��
        Excel.Application app;
        Excel.Workbook workBook;
		Excel.Worksheet workSheet;
        Excel.Range range;
        Excel.Range range1;
        Excel.Range range2;
        Excel.TextBox textBox;
        private int sheetCount = 1;			//WorkSheet����
        private string sheetPrefixName = "ҳ";
        #endregion

        #region ��������
        /// <summary>
        /// WorkSheetǰ׺�������磺ǰ׺��Ϊ��ҳ������ôWorkSheet��������Ϊ��ҳ-1��ҳ-2...��
        /// </summary>
        public string SheetPrefixName
        {
            set { this.sheetPrefixName = value; }
        }

        /// <summary>
        /// WorkSheet����
        /// </summary>
        public int WorkSheetCount
        {
            get { return workBook.Sheets.Count; }
        }

        /// <summary>
        /// Excelģ���ļ�·��
        /// </summary>
        public string TempletFilePath
        {
            set { this.templetFile = value; }
        }

        /// <summary>
        /// ���Excel�ļ�·��
        /// </summary>
        public string OutputFilePath
        {
            set { this.outputFile = value; }
        }
        #endregion

        #region ��������

        #region ExcelHelper
        /// <summary>
        /// ���캯������һ������Excel��������Ϊģ�壬��ָ�����·��
        /// </summary>
        /// <param name="templetFilePath">Excelģ���ļ�·��</param>
        /// <param name="outputFilePath">���Excel�ļ�·��</param>
        public ExcelHelper(string templetFilePath, string outputFilePath)
        {
            if (templetFilePath == null)
                throw new Exception("Excelģ���ļ�·������Ϊ�գ�");

            if (outputFilePath == null)
                throw new Exception("���Excel�ļ�·������Ϊ�գ�");

            if (!File.Exists(templetFilePath))
                throw new Exception("ָ��·����Excelģ���ļ������ڣ�");

            this.templetFile = templetFilePath;
            this.outputFile = outputFilePath;

            //����һ��Application����ʹ��ɼ�
            beforeTime = DateTime.Now;
            app = new Excel.ApplicationClass();
            app.Visible = true;
            afterTime = DateTime.Now;

            //��ģ���ļ����õ�WorkBook����
            workBook = app.Workbooks.Open(templetFile, missing, missing, missing, missing, missing,
				missing, missing, missing, missing, missing, missing, missing, missing, missing);

            //�õ�WorkSheet����
            workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);

        }

        /// <summary>
        /// ���캯������һ�����еĹ�����
        /// </summary>
        /// <param name="fileName">Excel�ļ���</param>
        public ExcelHelper(string fileName)
        {
            if (!File.Exists(fileName))
                throw new Exception("ָ��·����Excel�ļ������ڣ�");

            //����һ��Application����ʹ��ɼ�
            beforeTime = DateTime.Now;
            app = new Excel.ApplicationClass();
            app.Visible = true;
            afterTime = DateTime.Now;

            //��һ��WorkBook
            workBook = app.Workbooks.Open(fileName,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
				Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            //�õ�WorkSheet����
            workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);

        }

        /// <summary>
        /// ���캯�����½�һ��������
        /// </summary>
        public ExcelHelper()
        {
            //����һ��Application����ʹ��ɼ�
            beforeTime = DateTime.Now;
            app = new Excel.ApplicationClass();
            //app.Visible = true; ���Զ���
            afterTime = DateTime.Now;

            //�½�һ��WorkBook
            workBook = app.Workbooks.Add(Type.Missing);

            //�õ�WorkSheet����
            workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);

        }
        #endregion

        #region Data Export Methods

        /// <summary>
        /// ��DataTable����д��Excel�ļ����Զ���ҳ��
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rows">ÿ��WorkSheetд�����������</param>
        /// <param name="top">���������ʼ������</param>
        /// <param name="left">���������ʼ������</param>
        public void DataTableToExcel(DataTable dt, int rows, int top, int left)
        {
            int rowCount = dt.Rows.Count;		//DataTable����
            int colCount = dt.Columns.Count;	//DataTable����
            sheetCount = this.GetSheetCount(rowCount, rows);	//WorkSheet����
            //			StringBuilder sb;

            //����sheetCount-1��WorkSheet����
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            for (int i = 1; i <= sheetCount; i++)
            {
                int startRow = (i - 1) * rows;		//��¼��ʼ������
                int endRow = i * rows;			//��¼����������

                //�������һ��WorkSheet����ô��¼����������ΪԴDataTable����
                if (i == sheetCount)
                    endRow = rowCount;

                //��ȡҪд�����ݵ�WorkSheet���󣬲�������
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //��dt�е�����д��WorkSheet
                //				for(int j=0;j<endRow-startRow;j++)
                //				{
                //					for(int k=0;k<colCount;k++)
                //					{
                //						workSheet.Cells[top + j,left + k] = dt.Rows[startRow +j][k].ToString();
                //					}
                //				}

                //���ö�ά��������д��
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = dt.Rows[startRow + j][k].ToString();
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value2 = ss;

                #region ����Windwoճ���������������ݣ���Web�����в�ͨ��
                /*sb = new StringBuilder();

				for(int j=0;j<endRow-startRow;j++)
				{
					for(int k=0;k<colCount;k++)
					{
						sb.Append( dt.Rows[startRow + j][k].ToString() );
						sb.Append("\t");
					}

					sb.Append("\n");
				}

				System.Windows.Forms.Clipboard.SetDataObject(sb.ToString());

				range = (Excel.Range)workSheet.Cells[top,left];
				workSheet.Paste(range,false);*/
                #endregion

            }
        }


        /// <summary>
        /// ��DataTable����д��Excel�ļ�������ҳ��
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="top">���������ʼ������</param>
        /// <param name="left">���������ʼ������</param>
        public void DataTableToExcel(DataTable dt, int top, int left)
        {
            int rowCount = dt.Rows.Count;		//DataTable����
            int colCount = dt.Columns.Count;	//DataTable����

            //���ö�ά��������д��
            string[,] arr = new string[rowCount + 1, colCount + 1];
            for (int i = 0; i < colCount; i++)
            {
                arr[0, i] = dt.Columns[i].ColumnName.ToString();
            }

            for (int j = 0; j < rowCount; j++)
            {
                for (int k = 0; k < colCount; k++)
                {
                    arr[j + 1, k] = dt.Rows[j][k].ToString();
                }
            }

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount + 1, colCount + 1);
            range.Value2 = arr;
        }


        /// <summary>
        /// ��DataTable����д��Excel�ļ����Զ���ҳ����ָ��Ҫ�ϲ�����������
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rows">ÿ��WorkSheetд�����������</param>
        /// <param name="top">���������ʼ������</param>
        /// <param name="left">���������ʼ������</param>
        /// <param name="mergeColumnIndex">DataTable��Ҫ�ϲ���ͬ�е�����������0��ʼ</param>
        public void DataTableToExcel(DataTable dt, int rows, int top, int left, int mergeColumnIndex)
        {
            int rowCount = dt.Rows.Count;		//ԴDataTable����
            int colCount = dt.Columns.Count;	//ԴDataTable����
            sheetCount = this.GetSheetCount(rowCount, rows);	//WorkSheet����
            //			StringBuilder sb;

            //����sheetCount-1��WorkSheet����
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            for (int i = 1; i <= sheetCount; i++)
            {
                int startRow = (i - 1) * rows;		//��¼��ʼ������
                int endRow = i * rows;			//��¼����������

                //�������һ��WorkSheet����ô��¼����������ΪԴDataTable����
                if (i == sheetCount)
                    endRow = rowCount;

                //��ȡҪд�����ݵ�WorkSheet���󣬲�������
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //��dt�е�����д��WorkSheet
                //				for(int j=0;j<endRow-startRow;j++)
                //				{
                //					for(int k=0;k<colCount;k++)
                //					{
                //						workSheet.Cells[top + j,left + k] = dt.Rows[startRow + j][k].ToString();
                //					}
                //				}

                //���ö�ά��������д��
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = dt.Rows[startRow + j][k].ToString();
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value2 = ss;

                //�ϲ���ͬ��
                this.MergeRows(workSheet, left + mergeColumnIndex, top, rows);

            }
        }


        /// <summary>
        /// ����ά��������д��Excel�ļ����Զ���ҳ��
        /// </summary>
        /// <param name="arr">��ά����</param>
        /// <param name="rows">ÿ��WorkSheetд�����������</param>
        /// <param name="top">������</param>
        /// <param name="left">������</param>
        public void ArrayToExcel(string[,] arr, int rows, int top, int left)
        {
            int rowCount = arr.GetLength(0);		//��ά����������һά���ȣ�
            int colCount = arr.GetLength(1);	//��ά������������ά���ȣ�
            sheetCount = this.GetSheetCount(rowCount, rows);	//WorkSheet����

            //����sheetCount-1��WorkSheet����
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            //����ά��������д��Excel
            for (int i = sheetCount; i >= 1; i--)
            {
                int startRow = (i - 1) * rows;		//��¼��ʼ������
                int endRow = i * rows;			//��¼����������

                //�������һ��WorkSheet����ô��¼����������ΪԴDataTable����
                if (i == sheetCount)
                    endRow = rowCount;

                //��ȡҪд�����ݵ�WorkSheet���󣬲�������
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //����ά�����е�����д��WorkSheet
                //				for(int j=0;j<endRow-startRow;j++)
                //				{
                //					for(int k=0;k<colCount;k++)
                //					{
                //						workSheet.Cells[top + j,left + k] = arr[startRow + j,k];
                //					}
                //				}

                //���ö�ά��������д��
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = arr[startRow + j, k];
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value2 = ss;
            }

        }//end ArrayToExcel


        /// <summary>
        /// ����ά��������д��Excel�ļ�������ҳ��
        /// </summary>
        /// <param name="arr">��ά����</param>
        /// <param name="top">������</param>
        /// <param name="left">������</param>
        public void ArrayToExcel(string[,] arr, int top, int left)
        {
            int rowCount = arr.GetLength(0);		//��ά����������һά���ȣ�
            int colCount = arr.GetLength(1);	//��ά������������ά���ȣ�

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);
            range.FormulaArray = arr;

        }//end ArrayToExcel

        /// <summary>
        /// ����ά��������д��Excel�ļ�������ҳ��
        /// </summary>
        /// <param name="arr">��ά����</param>
        /// <param name="top">������</param>
        /// <param name="left">������</param>
        /// <param name="isFormula">���������Ƿ���Ҫ����</param>
        public void ArrayToExcel(string[,] arr, int top, int left, bool isFormula)
        {
            int rowCount = arr.GetLength(0);		//��ά����������һά���ȣ�
            int colCount = arr.GetLength(1);	//��ά������������ά���ȣ�

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            //ע�⣺ʹ��range.FormulaArrayд�ϲ��ĵ�Ԫ��������
            if (isFormula)
                range.FormulaArray = arr;
            else
                range.Value2 = arr;

        }//end ArrayToExcel

        /// <summary>
        /// ����ά��������д��Excel�ļ�������ҳ�����ϲ�ָ���е���ͬ��
        /// </summary>
        /// <param name="arr">��ά����</param>
        /// <param name="top">������</param>
        /// <param name="left">������</param>
        /// <param name="isFormula">���������Ƿ���Ҫ����</param>
        /// <param name="mergeColumnIndex">��Ҫ�ϲ��е�������</param>
        public void ArrayToExcel(string[,] arr, int top, int left, bool isFormula, int mergeColumnIndex)
        {
            int rowCount = arr.GetLength(0);		//��ά����������һά���ȣ�
            int colCount = arr.GetLength(1);	//��ά������������ά���ȣ�

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            //ע�⣺ʹ��range.FormulaArrayд�ϲ��ĵ�Ԫ��������
            if (isFormula)
                range.FormulaArray = arr;
            else
                range.Value2 = arr;

            this.MergeRows(workSheet, mergeColumnIndex, top, rowCount);

        }//end ArrayToExcel

        /// <summary>
        /// ����ά��������д��Excel�ļ�������ҳ��
        /// </summary>
        /// <param name="sheetIndex">����������</param>
        /// <param name="arr">��ά����</param>
        /// <param name="top">������</param>
        /// <param name="left">������</param>
        public void ArrayToExcel(int sheetIndex, string[,] arr, int top, int left)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            // �ı䵱ǰ������
            this.workSheet = (Excel.Worksheet)this.workBook.Sheets.get_Item(sheetIndex);

            int rowCount = arr.GetLength(0);		//��ά����������һά���ȣ�
            int colCount = arr.GetLength(1);	//��ά������������ά���ȣ�

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            range.Value2 = arr;

        }//end ArrayToExcel

        /// <summary>
        /// ����ά��������д��Excel�ļ����Զ���ҳ����ָ��Ҫ�ϲ�����������
        /// </summary>
        /// <param name="arr">��ά����</param>
        /// <param name="rows">ÿ��WorkSheetд�����������</param>
        /// <param name="top">������</param>
        /// <param name="left">������</param>
        /// <param name="mergeColumnIndex">����Ķ�ά�������൱��DataTable����������������0��ʼ</param>
        public void ArrayToExcel(string[,] arr, int rows, int top, int left, int mergeColumnIndex)
        {
            int rowCount = arr.GetLength(0);		//��ά����������һά���ȣ�
            int colCount = arr.GetLength(1);	//��ά������������ά���ȣ�
            sheetCount = this.GetSheetCount(rowCount, rows);	//WorkSheet����

            //����sheetCount-1��WorkSheet����
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            //����ά��������д��Excel
            for (int i = sheetCount; i >= 1; i--)
            {
                int startRow = (i - 1) * rows;		//��¼��ʼ������
                int endRow = i * rows;			//��¼����������

                //�������һ��WorkSheet����ô��¼����������ΪԴDataTable����
                if (i == sheetCount)
                    endRow = rowCount;

                //��ȡҪд�����ݵ�WorkSheet���󣬲�������
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //����ά�����е�����д��WorkSheet
                for (int j = 0; j < endRow - startRow; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        workSheet.Cells[top + j, left + k] = arr[startRow + j, k];
                    }
                }

                //���ö�ά��������д��
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = arr[startRow + j, k];
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value2 = ss;

                //�ϲ���ͬ��
                this.MergeRows(workSheet, left + mergeColumnIndex, top, rows);
            }

        }//end ArrayToExcel
        #endregion

        #region WorkSheet Methods

        /// <summary>
        /// �ı䵱ǰ������
        /// </summary>
        /// <param name="sheetIndex">����������</param>
        public void ChangeCurrentWorkSheet(int sheetIndex)
        {
            //��ָ������������������Χ���򲻸ı䵱ǰ������
            if (sheetIndex < 1)
                return;

            if (sheetIndex > this.WorkSheetCount)
                return;

            this.workSheet = (Excel.Worksheet)this.workBook.Sheets.get_Item(sheetIndex);
        }
        /// <summary>
        /// ����ָ�����ƵĹ�����
        /// </summary>
        /// <param name="sheetName">����������</param>
        public void HiddenWorkSheet(string sheetName)
        {
            try
            {
                Excel.Worksheet sheet = null;

                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                        sheet = workSheet;
                }

                if (sheet != null)
                    sheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("����Ϊ\"" + sheetName + "\"�Ĺ���������");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ����ָ�������Ĺ�����
        /// </summary>
        /// <param name="sheetIndex"></param>
        public void HiddenWorkSheet(int sheetIndex)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                Excel.Worksheet sheet = null;
                sheet = (Excel.Worksheet)workBook.Sheets.get_Item(sheetIndex);

                sheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }


        /// <summary>
        /// ��ָ�����ƵĹ�������濽��ָ�������ĸù�����ĸ�������������
        /// </summary>
        /// <param name="sheetName">����������</param>
        /// <param name="sheetCount">���������</param>
        public void CopyWorkSheets(string sheetName, int sheetCount)
        {
            try
            {
                Excel.Worksheet sheet = null;
                int sheetIndex = 0;

                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                        sheetIndex = workSheet.Index;
                    }
                }

                if (sheet != null)
                {
                    for (int i = sheetCount; i >= 1; i--)
                    {
                        sheet.Copy(this.missing, sheet);
                    }

                    //������
                    for (int i = sheetIndex; i <= sheetIndex + sheetCount; i++)
                    {
                        workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);
                        workSheet.Name = sheetName + "-" + Convert.ToString(i - sheetIndex + 1);
                    }
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("����Ϊ\"" + sheetName + "\"�Ĺ���������");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ��һ��������������һ����������棬��������
        /// </summary>
        /// <param name="srcSheetIndex">����Դ����������</param>
        /// <param name="aimSheetIndex">����λ�ù������������¹��������ڸù��������</param>
        /// <param name="newSheetName"></param>
        public void CopyWorkSheet(int srcSheetIndex, int aimSheetIndex, string newSheetName)
        {
            if (srcSheetIndex > this.WorkSheetCount || aimSheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                Excel.Worksheet srcSheet = (Excel.Worksheet)workBook.Sheets.get_Item(srcSheetIndex);
                Excel.Worksheet aimSheet = (Excel.Worksheet)workBook.Sheets.get_Item(aimSheetIndex);

                srcSheet.Copy(this.missing, aimSheet);

                //������
                workSheet = (Excel.Worksheet)aimSheet.Next;		//��ȡ�¿����Ĺ�����
                workSheet.Name = newSheetName;
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }


        /// <summary>
        /// ��������ɾ��������
        /// </summary>
        /// <param name="sheetName"></param>
        public void DeleteWorkSheet(string sheetName)
        {
            try
            {
                Excel.Worksheet sheet = null;

                //�ҵ�����λsheetName�Ĺ�����
                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                    }
                }

                if (sheet != null)
                {
                    sheet.Delete();
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("����Ϊ\"" + sheetName + "\"�Ĺ���������");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ��������ɾ��������
        /// </summary>
        /// <param name="sheetIndex"></param>
        public void DeleteWorkSheet(int sheetIndex)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                Excel.Worksheet sheet = null;
                sheet = (Excel.Worksheet)workBook.Sheets.get_Item(sheetIndex);

                sheet.Delete();
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        #endregion

        #region TextBox Methods
        /// <summary>
        /// ��ָ���ı���д�����ݣ���ÿ��WorkSheet����
        /// </summary>
        /// <param name="textboxName">�ı�������</param>
        /// <param name="text">Ҫд����ı�</param>
        public void SetTextBox(string textboxName, string text)
        {
            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);


                try
                {
                    textBox = (Excel.TextBox)workSheet.TextBoxes(textboxName);
                    textBox.Text = text;
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("������IDΪ\"" + textboxName + "\"���ı���");
                }
            }
        }

        /// <summary>
        /// ��ָ���ı���д�����ݣ���ָ��WorkSheet����
        /// </summary>
        /// <param name="sheetIndex">����������</param>
        /// <param name="textboxName">�ı�������</param>
        /// <param name="text">Ҫд����ı�</param>
        public void SetTextBox(int sheetIndex, string textboxName, string text)
        {
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            try
            {
                textBox = (Excel.TextBox)workSheet.TextBoxes(textboxName);
                textBox.Text = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("������IDΪ\"" + textboxName + "\"���ı���");
            }
        }

        /// <summary>
        /// ���ı���д�����ݣ���ÿ��WorkSheet����
        /// </summary>
        /// <param name="ht">Hashtable�ļ�ֵ�Ա����ı����ID������</param>
        public void SetTextBoxes(Hashtable ht)
        {
            if (ht.Count == 0) return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                foreach (DictionaryEntry dic in ht)
                {
                    try
                    {
                        textBox = (Excel.TextBox)workSheet.TextBoxes(dic.Key);
                        textBox.Text = dic.Value.ToString();
                    }
                    catch
                    {
                        this.KillExcelProcess();
                        throw new Exception("������IDΪ\"" + dic.Key.ToString() + "\"���ı���");
                    }
                }
            }
        }

        /// <summary>
        /// ���ı���д�����ݣ���ָ��WorkSheet����
        /// </summary>
        /// <param name="ht">Hashtable�ļ�ֵ�Ա����ı����ID������</param>
        public void SetTextBoxes(int sheetIndex, Hashtable ht)
        {
            if (ht.Count == 0) return;

            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            foreach (DictionaryEntry dic in ht)
            {
                try
                {
                    textBox = (Excel.TextBox)workSheet.TextBoxes(dic.Key);
                    textBox.Text = dic.Value.ToString();
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("������IDΪ\"" + dic.Key.ToString() + "\"���ı���");
                }
            }
        }
        #endregion

        #region Cell Methods
        /// <summary>
        /// ��Ԫ��д�����ݣ��Ե�ǰWorkSheet����
        /// </summary>
        /// <param name="rowIndex">������</param>
        /// <param name="columnIndex">������</param>
        /// <param name="text">Ҫд����ı�ֵ</param>
        public void SetCells(int rowIndex, int columnIndex, string text)
        {
            try
            {
                workSheet.Cells[rowIndex, columnIndex] = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("��Ԫ��[" + rowIndex + "," + columnIndex + "]д���ݳ���");
            }
        }

        /// <summary>
        /// ��Ԫ��д�����ݣ���ָ��WorkSheet����
        /// </summary>
        /// <param name="sheetIndex">����������</param>
        /// <param name="rowIndex">������</param>
        /// <param name="columnIndex">������</param>
        /// <param name="text">Ҫд����ı�ֵ</param>
        public void SetCells(int sheetIndex, int rowIndex, int columnIndex, string text)
        {
            try
            {
                this.ChangeCurrentWorkSheet(sheetIndex);	//�ı䵱ǰ������Ϊָ��������
                workSheet.Cells[rowIndex, columnIndex] = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("��Ԫ��[" + rowIndex + "," + columnIndex + "]д���ݳ���");
            }
        }

        /// <summary>
        /// ��Ԫ��д�����ݣ���ÿ��WorkSheet����
        /// </summary>
        /// <param name="ht">Hashtable�ļ�ֵ�Ա��浥Ԫ���λ�����������������������á�,��������������</param>
        public void SetCells(Hashtable ht)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (ht.Count == 0) return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                foreach (DictionaryEntry dic in ht)
                {
                    try
                    {
                        position = dic.Key.ToString();
                        rowIndex = Convert.ToInt32(position.Split(',')[0]);
                        columnIndex = Convert.ToInt32(position.Split(',')[1]);

                        workSheet.Cells[rowIndex, columnIndex] = dic.Value;
                    }
                    catch
                    {
                        this.KillExcelProcess();
                        throw new Exception("��Ԫ��[" + dic.Key + "]д���ݳ���");
                    }
                }
            }
        }

        /// <summary>
        /// ��Ԫ��д�����ݣ���ָ��WorkSheet����
        /// </summary>
        /// <param name="ht">Hashtable�ļ�ֵ�Ա��浥Ԫ���λ�����������������������á�,��������������</param>
        public void SetCells(int sheetIndex, Hashtable ht)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            if (ht.Count == 0) return;

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            foreach (DictionaryEntry dic in ht)
            {
                try
                {
                    position = dic.Key.ToString();
                    rowIndex = Convert.ToInt32(position.Split(',')[0]);
                    columnIndex = Convert.ToInt32(position.Split(',')[1]);

                    workSheet.Cells[rowIndex, columnIndex] = dic.Value;
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("��Ԫ��[" + dic.Key + "]д���ݳ���");
                }
            }
        }

        /// <summary>
        /// ���õ�Ԫ��Ϊ�ɼ����
        /// </summary>
        /// <remarks>
        /// ���Excel�ĵ�Ԫ���ʽ����Ϊ���֣����ڻ�����������ʱ����Ҫ������Щ��Ԫ���FormulaR1C1���ԣ�
        /// ����д����Щ��Ԫ������ݽ����ᰴ��Ԥ���趨�ĸ�ʽ��ʾ
        /// </remarks>
        /// <param name="arr">���浥Ԫ���λ�����������������������á�,��������������</param>
        public void SetCells(int sheetIndex, string[] arr)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            if (arr.Length == 0) return;

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            for (int i = 0; i < arr.Length; i++)
            {
                try
                {
                    position = arr[i];
                    rowIndex = Convert.ToInt32(position.Split(',')[0]);
                    columnIndex = Convert.ToInt32(position.Split(',')[1]);

                    Excel.Range cell = (Excel.Range)workSheet.Cells[rowIndex, columnIndex];
                    cell.FormulaR1C1 = cell.Text;
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception(string.Format("���㵥Ԫ��{0}����", arr[i]));
                }
            }
        }

        /// <summary>
        /// ��Ԫ��д�����ݣ���ָ��WorkSheet����
        /// </summary>
        /// <param name="ht">Hashtable�ļ�ֵ�Ա��浥Ԫ���λ�����������������������á�,��������������</param>
        public void SetCells(string sheetName, Hashtable ht)
        {
            int rowIndex;
            int columnIndex;
            string position;
            Excel.Worksheet sheet = null;
            int sheetIndex = 0;

            if (ht.Count == 0) return;

            try
            {
                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                        sheetIndex = workSheet.Index;
                    }
                }

                if (sheet != null)
                {
                    foreach (DictionaryEntry dic in ht)
                    {
                        try
                        {
                            position = dic.Key.ToString();
                            rowIndex = Convert.ToInt32(position.Split(',')[0]);
                            columnIndex = Convert.ToInt32(position.Split(',')[1]);

                            sheet.Cells[rowIndex, columnIndex] = dic.Value;
                        }
                        catch
                        {
                            this.KillExcelProcess();
                            throw new Exception("��Ԫ��[" + dic.Key + "]д���ݳ���");
                        }
                    }
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("����Ϊ\"" + sheetName + "\"�Ĺ���������");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }


        /// <summary>
        /// �ϲ���Ԫ�񣬲���ֵ����ÿ��WorkSheet����
        /// </summary>
        /// <param name="beginRowIndex">��ʼ������</param>
        /// <param name="beginColumnIndex">��ʼ������</param>
        /// <param name="endRowIndex">����������</param>
        /// <param name="endColumnIndex">����������</param>
        /// <param name="text">�ϲ���Range��ֵ</param>
        public void MergeCells(int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);

                range.ClearContents();		//�Ȱ�Range����������ϲ��Ų������
                range.MergeCells = true;
                range.Value2 = text;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    
            }
        }

        /// <summary>
        /// �ϲ���Ԫ�񣬲���ֵ����ָ��WorkSheet����
        /// </summary>
        /// <param name="sheetIndex">WorkSheet����</param>
        /// <param name="beginRowIndex">��ʼ������</param>
        /// <param name="beginColumnIndex">��ʼ������</param>
        /// <param name="endRowIndex">����������</param>
        /// <param name="endColumnIndex">����������</param>
        /// <param name="text">�ϲ���Range��ֵ</param>
        public void MergeCells(int sheetIndex, int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);
            range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);

            range.ClearContents();		//�Ȱ�Range����������ϲ��Ų������
            range.MergeCells = true;
            range.Value2 = text;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }
        #endregion

        #region Row Methods


        /// <summary>
        /// ��ָ�������е�������ͬ���кϲ�����ÿ��WorkSheet����
        /// </summary>
        /// <param name="columnIndex">������</param>
        /// <param name="beginRowIndex">��ʼ������</param>
        /// <param name="endRowIndex">����������</param>
        public void MergeRows(int columnIndex, int beginRowIndex, int endRowIndex)
        {
            if (endRowIndex - beginRowIndex < 1)
                return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                int beginIndex = beginRowIndex;
                int count = 0;
                string text1;
                string text2;
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                for (int j = beginRowIndex; j <= endRowIndex; j++)
                {
                    range = (Excel.Range)workSheet.Cells[j, columnIndex];
                    text1 = range.Text.ToString();

                    range = (Excel.Range)workSheet.Cells[j + 1, columnIndex];
                    text2 = range.Text.ToString();

                    if (text1 == text2)
                    {
                        ++count;
                    }
                    else
                    {
                        if (count > 0)
                        {
                            this.MergeCells(workSheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                        }

                        beginIndex = j + 1;		//���ÿ�ʼ�ϲ�������
                        count = 0;		//��������0
                    }

                }

            }
        }


        /// <summary>
        /// ��ָ�������е�������ͬ���кϲ�����ָ��WorkSheet����
        /// </summary>
        /// <param name="sheetIndex">WorkSheet����</param>
        /// <param name="columnIndex">������</param>
        /// <param name="beginRowIndex">��ʼ������</param>
        /// <param name="endRowIndex">����������</param>
        public void MergeRows(int sheetIndex, int columnIndex, int beginRowIndex, int endRowIndex)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            if (endRowIndex - beginRowIndex < 1)
                return;

            int beginIndex = beginRowIndex;
            int count = 0;
            string text1;
            string text2;
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            for (int j = beginRowIndex; j <= endRowIndex; j++)
            {
                range = (Excel.Range)workSheet.Cells[j, columnIndex];
                text1 = range.Text.ToString();

                range = (Excel.Range)workSheet.Cells[j + 1, columnIndex];
                text2 = range.Text.ToString();

                if (text1 == text2)
                {
                    ++count;
                }
                else
                {
                    if (count > 0)
                    {
                        this.MergeCells(workSheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                    }

                    beginIndex = j + 1;		//���ÿ�ʼ�ϲ�������
                    count = 0;		//��������0
                }

            }

        }


        /// <summary>
        /// ���У���ָ�����������ָ�������У�
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void InsertRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 0; i < count; i++)
                    {
                        range.Insert(false,Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ���У���ָ��WorkSheetָ�����������ָ�������У�
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void InsertRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                for (int i = 0; i < count; i++)
                {
                    range.Insert(false,Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// �����У���ָ�������渴��ָ�������У�
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void CopyRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range1 = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 1; i <= count; i++)
                    {
                        range2 = (Excel.Range)workSheet.Rows[rowIndex + i, this.missing];
                        range1.Copy(range2);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// �����У���ָ��WorkSheetָ�������渴��ָ�������У�
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void CopyRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range1 = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                for (int i = 1; i <= count; i++)
                {
                    range2 = (Excel.Range)workSheet.Rows[rowIndex + i, this.missing];
                    range1.Copy(range2);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ɾ����
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void DeleteRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 0; i < count; i++)
                    {
                        range.Delete(Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ɾ����
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void DeleteRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                for (int i = 0; i < count; i++)
                {
                    range.Delete(Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        #endregion

        #region Column Methods

        /// <summary>
        /// ���У���ָ�����ұ߲���ָ�������У�
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void InsertColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                    for (int i = 0; i < count; i++)
                    {
                        range.Insert(false,Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ���У���ָ��WorkSheetָ�����ұ߲���ָ�������У�
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void InsertColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                for (int i = 0; i < count; i++)
                {
                    range.Insert(false,Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// �����У���ָ�����ұ߸���ָ�������У�
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void CopyColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    //					range1 = (Excel.Range)workSheet.Columns[columnIndex,this.missing];
                    range1 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex) + "1", this.IntToLetter(columnIndex) + "10000");

                    for (int i = 1; i <= count; i++)
                    {
                        //						range2 = (Excel.Range)workSheet.Columns[this.missing,columnIndex + i];
                        range2 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex + i) + "1", this.IntToLetter(columnIndex + i) + "10000");
                        range1.Copy(range2);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// �����У���ָ��WorkSheetָ�����ұ߸���ָ�������У�
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void CopyColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                //				range1 = (Excel.Range)workSheet.Columns[Type.Missing,columnIndex];
                range1 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex) + "1", this.IntToLetter(columnIndex) + "10000");

                for (int i = 1; i <= count; i++)
                {
                    //					range2 = (Excel.Range)workSheet.Columns[Type.Missing,columnIndex + i];
                    range2 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex + i) + "1", this.IntToLetter(columnIndex + i) + "10000");
                    range1.Copy(range2);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ɾ����
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void DeleteColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                    for (int i = 0; i < count; i++)
                    {
                        range.Delete(Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ɾ����
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void DeleteColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                for (int i = 0; i < count; i++)
                {
                    range.Delete(Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        #endregion

        #region Range Methods

        /// <summary>
        /// ��ָ����Χ���򿽱���Ŀ������
        /// </summary>
        /// <param name="sheetIndex">WorkSheet����</param>
        /// <param name="startCell">Ҫ��������Ŀ�ʼCellλ�ã����磺A10��</param>
        /// <param name="endCell">Ҫ��������Ľ���Cellλ�ã����磺F20��</param>
        /// <param name="targetCell">Ŀ������Ŀ�ʼCellλ�ã����磺H10��</param>
        public void RangeCopy(int sheetIndex, string startCell, string endCell, string targetCell)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("����������Χ��WorkSheet�������ܴ���WorkSheet������");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);
                range1 = workSheet.get_Range(startCell, endCell);
                range2 = workSheet.get_Range(targetCell, this.missing);

                range1.Copy(range2);
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// ��ָ����Χ���򿽱���Ŀ������
        /// </summary>
        /// <param name="sheetName">WorkSheet����</param>
        /// <param name="startCell">Ҫ��������Ŀ�ʼCellλ�ã����磺A10��</param>
        /// <param name="endCell">Ҫ��������Ľ���Cellλ�ã����磺F20��</param>
        /// <param name="targetCell">Ŀ������Ŀ�ʼCellλ�ã����磺H10��</param>
        public void RangeCopy(string sheetName, string startCell, string endCell, string targetCell)
        {
            try
            {
                Excel.Worksheet sheet = null;

                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                    }
                }

                if (sheet != null)
                {
                    for (int i = sheetCount; i >= 1; i--)
                    {
                        range1 = sheet.get_Range(startCell, endCell);
                        range2 = sheet.get_Range(targetCell, this.missing);

                        range1.Copy(range2);
                    }
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("����Ϊ\"" + sheetName + "\"�Ĺ���������");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// �Զ����
        /// </summary>
        public void RangAutoFill()
        {
            Excel.Range rng = workSheet.get_Range("B4", Type.Missing);
            rng.Value2 = "����һ ";
            rng.AutoFill(workSheet.get_Range("B4", "B9"),
                Excel.XlAutoFillType.xlFillWeekdays);

            rng = workSheet.get_Range("C4", Type.Missing);
            rng.Value2 = "һ��";
            rng.AutoFill(workSheet.get_Range("C4", "C9"),
                Excel.XlAutoFillType.xlFillMonths);

            rng = workSheet.get_Range("D4", Type.Missing);
            rng.Value2 = "1";
            rng.AutoFill(workSheet.get_Range("D4", "D9"),
                Excel.XlAutoFillType.xlFillSeries);

            rng = workSheet.get_Range("E4", Type.Missing);
            rng.Value2 = "3";
            rng = workSheet.get_Range("E5", Type.Missing);
            rng.Value2 = "6";
            rng = workSheet.get_Range("E4", "E5");
            rng.AutoFill(workSheet.get_Range("E4", "E9"),
                Excel.XlAutoFillType.xlFillSeries);

        }

        /// <summary>
        /// Ӧ����ʽ
        /// </summary>
        public void ApplyStyle()
        {
            object missingValue = Type.Missing;
            Excel.Range rng = workSheet.get_Range("B3", "L23");
            Excel.Style style;

            try
            {
                style = workBook.Styles["NewStyle"];
            }
            // Style doesn't exist yet.
            catch
            {
                style = workBook.Styles.Add("NewStyle", missingValue);
                style.Font.Name = "Verdana";
                style.Font.Size = 12;
                style.Font.Color = 255;
                style.Interior.Color = (200 << 16) | (200 << 8) | 200;
                style.Interior.Pattern = Excel.XlPattern.xlPatternSolid;
            }

            rng.Value2 = "'Style Test";
            rng.Style = "NewStyle";
            rng.Columns.AutoFit();
        }

        #endregion

        #region ExcelHelper Kit
        /// <summary>
        /// ��Excel�е���ĸ����ֵת������������ֵ
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public int LetterToInt(string letter)
        {
            int n = 0;

            if (letter.Trim().Length == 0)
                throw new Exception("�����ܿ��ַ�����");

            if (letter.Length >= 2)
            {
                char c1 = letter.ToCharArray(0, 2)[0];
                char c2 = letter.ToCharArray(0, 2)[1];

                if (!char.IsLetter(c1) || !char.IsLetter(c2))
                {
                    throw new Exception("��ʽ����ȷ����������ĸ��");
                }

                c1 = char.ToUpper(c1);
                c2 = char.ToUpper(c2);

                int i = Convert.ToInt32(c1) - 64;
                int j = Convert.ToInt32(c2) - 64;

                n = i * 26 + j;
            }

            if (letter.Length == 1)
            {
                char c1 = letter.ToCharArray()[0];

                if (!char.IsLetter(c1))
                {
                    throw new Exception("��ʽ����ȷ����������ĸ��");
                }

                c1 = char.ToUpper(c1);

                n = Convert.ToInt32(c1) - 64;
            }

            if (n > 256)
                throw new Exception("����������Χ��Excel�����������ܳ���256��");

            return n;
        }

        /// <summary>
        /// ��Excel�е���������ֵת��Ϊ�ַ�����ֵ
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string IntToLetter(int n)
        {
            if (n > 256)
                throw new Exception("����������Χ��Excel�����������ܳ���256��");

            int i = Convert.ToInt32(n / 26);
            int j = n % 26;

            char c1 = Convert.ToChar(i + 64);
            char c2 = Convert.ToChar(j + 64);

            if (n > 26)
                return c1.ToString() + c2.ToString();
            else if (n == 26)
                return "Z";
            else
                return c2.ToString();
        }

        #endregion

        #region Output File(ע�⣺���Ŀ���ļ��Ѵ��ڵĻ������)
        /// <summary>
        /// ���Excel�ļ����˳�
        /// </summary>
        public void OutputExcelFile()
        {
            if (this.outputFile == null)
                throw new Exception("û��ָ������ļ�·����");

            try
            {
                workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// ���ָ����ʽ���ļ���֧�ָ�ʽ��HTML��CSV��TEXT��EXCEL��
        /// </summary>
        /// <param name="format">HTML��CSV��TEXT��EXCEL��XML</param>
        public void OutputFile(string format)
        {
            if (this.outputFile == null)
                throw new Exception("û��ָ������ļ�·����");

            try
            {
                switch (format)
                {
                    case "HTML":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing,

Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing,

Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    //					case "XML":
                    //					{
                    //						workBook.SaveAs(outputFile,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,
                    //							Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                    //							Type.Missing, Type.Missing, Type.Missing, Type.Missing,	Type.Missing);
                    //						break;
                    //
                    //					}
                    default:
                        {
                            workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        public void SaveFile()
        {
            try
            {
                workBook.Save();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// ����ļ�
        /// </summary>
        public void SaveAsFile()
        {
            if (this.outputFile == null)
                throw new Exception("û��ָ������ļ�·����");

            try
            {
                workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// ��Excel�ļ����Ϊָ����ʽ
        /// </summary>
        /// <param name="format">HTML��CSV��TEXT��EXCEL��XML</param>
        public void SaveAsFile(string format)
        {
            if (this.outputFile == null)
                throw new Exception("û��ָ������ļ�·����");

            try
            {
                switch (format)
                {
                    case "HTML":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    //					case "XML":
                    //					{
                    //						workBook.SaveAs(outputFile,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,
                    //							Type.Missing, Type.Missing,Excel.XlSaveAsAccessMode.xlNoChange,
                    //							Type.Missing, Type.Missing, Type.Missing,Type.Missing,	Type.Missing);
                    //						break;
                    //					}
                    default:
                        {
                            workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// ����ļ�
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        public void SaveFile(string fileName)
        {
            try
            {
				workBook.SaveAs(fileName, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// ��Excel�ļ����Ϊָ����ʽ
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="format">HTML��CSV��TEXT��EXCEL��XML</param>
        public void SaveAsFile(string fileName, string format)
        {
            try
            {
                switch (format)
                {
                    case "HTML":
                        {
							workBook.SaveAs(fileName, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
							workBook.SaveAs(fileName, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
							workBook.SaveAs(fileName, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    //					case "XML":
                    //					{
                    //						workBook.SaveAs(fileName,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,      
                    //							Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                    //							Type.Missing, Type.Missing, Type.Missing, Type.Missing,	Type.Missing);
                    //						break;
                    //					}
                    default:
                        {
							workBook.SaveAs(fileName, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }
        #endregion

        #endregion

        #region ˽�з���

        /// <summary>
        /// �ϲ���Ԫ�񣬲���ֵ����ָ��WorkSheet����
        /// </summary>
        /// <param name="beginRowIndex">��ʼ������</param>
        /// <param name="beginColumnIndex">��ʼ������</param>
        /// <param name="endRowIndex">����������</param>
        /// <param name="endColumnIndex">����������</param>
        /// <param name="text">�ϲ���Range��ֵ</param>
        private void MergeCells(Excel.Worksheet sheet, int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            if (sheet == null)
                return;

            range = sheet.get_Range(sheet.Cells[beginRowIndex, beginColumnIndex], sheet.Cells[endRowIndex, endColumnIndex]);

            range.ClearContents();		//�Ȱ�Range����������ϲ��Ų������
            range.MergeCells = true;
            range.Value2 = text;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// ��ָ�������е�������ͬ���кϲ�����ָ��WorkSheet����
        /// </summary>
        /// <param name="columnIndex">Ҫ�ϲ���������</param>
        /// <param name="beginRowIndex">�ϲ���ʼ������</param>
        /// <param name="rows">Ҫ�ϲ�������</param>
        private void MergeRows(Excel.Worksheet sheet, int columnIndex, int beginRowIndex, int rows)
        {
            int beginIndex = beginRowIndex;
            int count = 0;
            string text1;
            string text2;

            if (sheet == null)
                return;

            for (int j = beginRowIndex; j < beginRowIndex + rows; j++)
            {
                range1 = (Excel.Range)sheet.Cells[j, columnIndex];
                range2 = (Excel.Range)sheet.Cells[j + 1, columnIndex];
                text1 = range1.Text.ToString();
                text2 = range2.Text.ToString();

                if (text1 == text2)
                {
                    ++count;
                }
                else
                {
                    if (count > 0)
                    {
                        this.MergeCells(sheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                    }

                    beginIndex = j + 1;		//���ÿ�ʼ�ϲ�������
                    count = 0;		//��������0
                }

            }

        }


        /// <summary>
        /// ����WorkSheet����
        /// </summary>
        /// <param name="rowCount">��¼������</param>
        /// <param name="rows">ÿWorkSheet����</param>
        public int GetSheetCount(int rowCount, int rows)
        {
            int n = rowCount % rows;		//����

            if (n == 0)
                return rowCount / rows;
            else
                return Convert.ToInt32(rowCount / rows) + 1;
        }

        /// <summary>
        /// ����Excel����
        /// </summary>
        public void KillExcelProcess()
        {
            Process[] myProcesses;
            DateTime startTime;
            myProcesses = Process.GetProcessesByName("Excel");

            try
            {
                //�ò���Excel����ID����ʱֻ���жϽ�������ʱ��
                foreach (Process myProcess in myProcesses)
                {
                    myProcess.Kill();
                    //startTime = myProcess.StartTime;
                    //if (startTime > beforeTime && startTime < afterTime)
                    //{
                    //    myProcess.Kill();
                    //}
                }
            }
            catch
            {

            }
        }


        private void Dispose()
        {
            workBook.Close(null, null, null);
            app.Workbooks.Close();
            app.Quit();

            if (range != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                range = null;
            }
            if (range1 != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range1);
                range1 = null;
            }
            if (range2 != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range2);
                range2 = null;
            }
            if (textBox != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(textBox);
                textBox = null;
            }
            if (workSheet != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
                workSheet = null;
            }
            if (workBook != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
                workBook = null;
            }
            if (app != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
            }

            GC.Collect();

            this.KillExcelProcess();

        }//end Dispose
        #endregion

		public bool SearchExcel(string fileName, string txtSearch, bool ignoreCase,rename.ProgressBarChange init,rename.ProgressBarChange change)
		{
			if (!File.Exists(fileName))
				throw new Exception("ָ��·����Excel�ļ������ڣ�");

			//����һ��Application����ʹ��ɼ�
			beforeTime = DateTime.Now;
			app = new Excel.ApplicationClass();
			app.Visible = false;
			afterTime = DateTime.Now;

			//��һ��WorkBook
			workBook = app.Workbooks.Open(fileName,
				Type.Missing, Type.Missing, Type.Missing, Type.Missing,
				Type.Missing, Type.Missing, Type.Missing, Type.Missing,
				Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

			StringComparison sctype = ignoreCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
			bool res = false;
			foreach (Excel.Worksheet ws in workBook.Worksheets)
			{
				if (isStop) break;
				if (Find(ws, txtSearch, sctype,init,change))
				{
					res = true;
					break;
				}
				Application.DoEvents();
			}

			Dispose();
			return res;
		}
		
		public System.Windows.Forms.ProgressBar pgb;
		public bool isStop;
		private bool Find(Excel.Worksheet ws, string txtSearch, StringComparison sctype, rename.ProgressBarChange init, rename.ProgressBarChange change)
		{
			bool res = false;
			if (ws == null) return res;
			int columns = ws.Columns.Count;
			int rows = ws.Rows.Count;
			columns = columns > 100 ? 100 : columns;
			rows = rows > 200 ? 200 : rows;
			init(columns/2);
			for (int i = 1; i < columns/2; i++)
			{
				change(i);
				for (int j = 1; j < rows/2; j++)
				{
					if (isStop) break;
					range = ws.get_Range(ws.Cells[j, i], ws.Cells[j, i]);
					if (range == null || range.Value2 ==null) continue;
					string value = range.Value2.ToString();
					if (value.Trim().Equals(string.Empty)) continue;
					if (value.IndexOf(txtSearch, sctype) != -1)
					{
						res = true;
						i = columns - 1;
						break;
					}
				}
			}
			return res;
		}
	}//end class
}