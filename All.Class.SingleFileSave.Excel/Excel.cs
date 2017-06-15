using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace All.Class.SingleFileSave
{
    public static class Excel
    {
        /// <summary>
        /// 将指定表格写入到Excel表格
        /// </summary>
        /// <param name="fileName">写入的文件名称</param>
        /// <param name="dt">表格</param>
        public static bool Write(string fileName, System.Data.DataTable dt)
        {
            return Write(fileName, dt, string.Format("{0:yyyyMMddHHmmss", DateTime.Now));
        }
        /// <summary>
        /// 将指定表格写入到Excel表格
        /// </summary>
        /// <param name="fileName">写入的文件名称</param>
        /// <param name="dt">表格</param>
        /// <param name="sheetName">Excel表sheet名称</param>
        public static bool Write(string fileName, System.Data.DataTable dt,string sheetName)
        {
            bool result = true;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    var workBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
                    var table = workBook.CreateSheet(sheetName);
                    var row = table.CreateRow(0);
                    NPOI.SS.UserModel.ICell cell;
                    row = table.CreateRow(0);
                    All.Class.TypeUse.TypeList[] columnType = new TypeUse.TypeList[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        cell = row.CreateCell(i);
                        if (dt.Columns[i].Caption == "")
                        {
                            cell.SetCellValue(dt.Columns[i].ColumnName);
                        }
                        else
                        {
                            cell.SetCellValue(dt.Columns[i].Caption);
                        }
                        columnType[i] = All.Class.TypeUse.GetType(dt.Columns[i].DataType.ToString());
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        row = table.CreateRow(i + 1);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            cell = row.CreateCell(j);
                            switch (columnType[j])
                            {
                                case TypeUse.TypeList.Boolean:
                                    cell.SetCellValue((bool)dt.Rows[i][j]);
                                    break;
                                case TypeUse.TypeList.DateTime:
                                    cell.SetCellValue((DateTime)dt.Rows[i][j]);
                                    break;
                                case TypeUse.TypeList.Byte:
                                    cell.SetCellValue((byte)dt.Rows[i][j]);
                                    break;
                                case TypeUse.TypeList.Float:
                                    cell.SetCellValue((float)dt.Rows[i][j]);
                                    break;
                                case TypeUse.TypeList.UShort:
                                    cell.SetCellValue((ushort)dt.Rows[i][j]);
                                    break;
                                case TypeUse.TypeList.Int:
                                    cell.SetCellValue((int)dt.Rows[i][j]);
                                    break;
                                case TypeUse.TypeList.Long:
                                    cell.SetCellValue((long)dt.Rows[i][j]);
                                    break;
                                case TypeUse.TypeList.Double:
                                    cell.SetCellValue((double)dt.Rows[i][j]);
                                    break;
                                case TypeUse.TypeList.String:
                                    cell.SetCellValue(dt.Rows[i][j].ToString());
                                    break;
                                default:
                                    cell.SetCellValue("");
                                    break;
                            }
                        }
                    }
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            table.SetColumnWidth(i, (dt.Rows[0][i].ToString().Length + 5) * 256);
                        }
                    }
                    workBook.Write(fs);
                    fs.Flush();
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                All.Class.Error.Add(e);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 读取指定Excel数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static System.Data.DataTable Read(string fileName)
        {
            return Read(fileName, 0);
        }
        /// <summary>
        /// 读取指定Excel指定表数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheet">表格名称</param>
        /// <returns></returns>
        public static System.Data.DataTable Read(string fileName, string sheet)
        {
            return Read(fileName, sheet, true);
        }
        /// <summary>
        /// 读取指定Excel指定表数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheet">表格序号</param>
        /// <returns></returns>
        public static System.Data.DataTable Read(string fileName, int sheet)
        {
            return Read(fileName, sheet, true);
        }
        /// <summary>
        /// 读取指定Excel指定表数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheet">表格名称</param>
        /// <param name="firstTitle">表格第一行是否做列名</param>
        /// <returns></returns>
        public static System.Data.DataTable Read(string fileName, string sheet, bool firstTitle)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    NPOI.SS.UserModel.IWorkbook work = null;
                    if (fileName.EndsWith("xlsx"))
                    {
                        work = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
                    }
                    else
                    {
                        work = new NPOI.HSSF.UserModel.HSSFWorkbook(fs, true);
                    }
                    NPOI.SS.UserModel.ISheet hs = work.GetSheet(sheet);
                    System.Collections.IEnumerator rows = hs.GetEnumerator();
                    NPOI.SS.UserModel.ICell cell;
                    while (rows.MoveNext())
                    {
                        NPOI.SS.UserModel.IRow row = (NPOI.SS.UserModel.IRow)rows.Current;
                        if (result.Columns.Count <= 0)
                        {
                            if (firstTitle)
                            {
                                for (int i = 0; i < row.LastCellNum; i++)
                                {
                                    cell = row.GetCell(i);
                                    result.Columns.Add(string.Format("column{0}", i));
                                    if (cell != null)
                                    {
                                        result.Columns[i].Caption = cell.ToString();
                                    }
                                }
                                continue;
                            }
                            else
                            {
                                for (int i = 0; i < hs.GetRow(0).LastCellNum; i++)
                                {
                                    result.Columns.Add(string.Format("column{0}", i));
                                }
                            }
                        }
                        System.Data.DataRow dr = result.NewRow();
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            cell = row.GetCell(i);
                            if (cell != null)
                            {
                                dr[i] = cell.ToString();
                            }
                            else
                            {
                                dr[i] = null;
                            }
                        }
                        result.Rows.Add(dr);
                    }
                    work = null;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 读取指定Excel指定表数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheet">表格序号</param>
        /// <param name="firstTitle">表格第一行是否做列名</param>
        /// <returns></returns>
        public static System.Data.DataTable Read(string fileName, int sheet, bool firstTitle)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    NPOI.SS.UserModel.IWorkbook work = null;
                    if (fileName.EndsWith("xlsx"))
                    {
                        work = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
                    }
                    else
                    {
                        work = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
                    }
                    NPOI.SS.UserModel.ISheet hs = work.GetSheetAt(sheet);
                    System.Collections.IEnumerator rows = hs.GetEnumerator();
                    NPOI.SS.UserModel.ICell cell;
                    while (rows.MoveNext())
                    {
                        NPOI.SS.UserModel.IRow row = (NPOI.SS.UserModel.IRow)rows.Current;
                        if (result.Columns.Count <= 0)
                        {
                            if (firstTitle)
                            {
                                for (int i = 0; i < row.LastCellNum; i++)
                                {
                                    cell = row.GetCell(i);
                                    result.Columns.Add(string.Format("column{0}", i));
                                    if (cell != null)
                                    {
                                        result.Columns[i].Caption = cell.ToString();
                                    }
                                }
                                continue;
                            }
                            else
                            {
                                for (int i = 0; i < hs.GetRow(0).LastCellNum; i++)
                                {
                                    result.Columns.Add(string.Format("column{0}", i));
                                }
                            }
                        }
                        System.Data.DataRow dr = result.NewRow();
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            cell = row.GetCell(i);
                            if (cell != null)
                            {
                                dr[i] = cell.ToString();
                            }
                            else
                            {
                                dr[i] = null;
                            }
                        }
                        result.Rows.Add(dr);
                    }
                    work = null;
                }
            }
            catch { }
            return result;
        }
    }
}
