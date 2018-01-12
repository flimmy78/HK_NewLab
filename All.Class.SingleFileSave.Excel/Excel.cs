using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace All.Class.SingleFileSave
{
    public static class Excel
    {
        [Serializable]
        public class Style:All.Class.Clone.IDeepClone<Style>
        {
            /// <summary>
            /// 是否有边框
            /// </summary>
            public bool Border
            { get; set; }
            /// <summary>
            /// 文本对齐方式
            /// </summary>
            public System.Drawing.ContentAlignment TextAlign
            { get; set; }
            /// <summary>
            /// 背景颜色
            /// </summary>
            public Color BackColor
            { get; set; }
            /// <summary>
            /// 文本颜色
            /// </summary>
            public Color ForeColor
            { get; set; }
            /// <summary>
            /// 字体名称
            /// </summary>
            public System.Drawing.Font Font
            { get; set; }
            public Style()
            {
                Font =new System.Drawing.Font( "宋体",10);
                Border = true;
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
            /// <summary>
            /// 克隆
            /// </summary>
            /// <returns></returns>
            public Style DeepClone()
            {
                return All.Class.Clone.DeepClone<Style>(this);
            }
        }
        #region//颜色
        /// <summary>
        /// Excel只支持以下颜色
        /// </summary>
        public enum Color : short
        {
            Black = 0,
            White = 9,
            Red,
            BrightGreen,
            Blue,
            Yellow,
            Pink,
            Turquoise,
            DarkRed,
            Green,
            DarkBlue,
            DarkYellow,
            Violet,
            Teal,
            Grey25Percent,
            Grey50Percent,
            CornflowerBlue,
            Maroon,
            LemonChiffon,
            Orchid = 28,
            Coral,
            RoyalBlue,
            LightCornflowerBlue,
            SkyBlue = 40,
            LightTurquoise,
            LightGreen,
            LightYellow,
            PaleBlue,
            Rose,
            Lavender,
            Tan,
            LightBlue,
            Aqua,
            Lime,
            Gold,
            LightOrange,
            Orange,
            BlueGrey,
            Grey40Percent,
            DarkTeal,
            SeaGreen,
            DarkGreen,
            OliveGreen,
            Brown,
            Plum,
            Indigo,
            Grey80Percent,
            Automatic
        }
        #endregion
        /// <summary>
        /// 提供按行和列的方法来写入数据
        /// </summary>
        public class WriteExcel
        {
            string fileName = string.Format("{0}\\{1:yyyyMMdd}.xls", All.Class.FileIO.NowPath, DateTime.Now);
            string sheetName = string.Format("{0:HHmmss}", DateTime.Now);
            FileStream fs;

            NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet table;

            private List<Style> style = new List<Style>();
            private Dictionary<int, NPOI.SS.UserModel.ICellStyle> excelStyle = new Dictionary<int, NPOI.SS.UserModel.ICellStyle>();
            public WriteExcel()
            {
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fileName">保存文件路径</param>
            /// <param name="sheetName">保存表格名称</param>
            public WriteExcel(string fileName,string sheetName)
            {
                this.fileName = fileName;
                this.sheetName = sheetName;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fileName">保存文件路径</param>
            public WriteExcel(string fileName)
                : this(fileName, string.Format("{0:HHmmss}", DateTime.Now))
            {
            }
            /// <summary>
            /// 将入的数据保存到文件,并关闭流
            /// </summary>
            public void Save()
            {
                if (fs == null)
                {
                    return;
                }
                if (workBook != null)
                {
                    workBook.Write(fs);
                }
                fs.Flush();
                fs.Close();
                fs.Dispose();
                table = null;
                workBook = null;
            }
            private bool Open()
            {
                if (fs != null && table != null)
                {
                    return fs.CanWrite;
                }
                try
                {
                    fs = new FileStream(fileName, FileMode.Create);
                    table = workBook.CreateSheet(sheetName);
                    table.CreateRow(0);
                    return fs.CanWrite && table != null;
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                }
                return false;
            }
            /// <summary>
            /// 将布尔值写入到单元格
            /// </summary>
            /// <param name="row"></param>
            /// <param name="column"></param>
            /// <param name="value"></param>
            public void Write(int row, int column, bool value)
            {
                Write<string>(row, column, value.ToString());
            }
            /// <summary>
            /// 将数字写入到单元格
            /// </summary>
            /// <param name="row"></param>
            /// <param name="column"></param>
            /// <param name="value"></param>
            public void Write(int row, int column, bool value, Style style)
            {
                Write<string>(row, row, column, column, value.ToString(), style);
            }
            /// <summary>
            /// 将数字写入到单元格
            /// </summary>
            /// <param name="rowStart"></param>
            /// <param name="rowEnd"></param>
            /// <param name="columnStart"></param>
            /// <param name="columnEnd"></param>
            /// <param name="value"></param>
            public void Write(int rowStart, int rowEnd, int columnStart, int columnEnd, bool value)
            {
                Write<string>(rowStart, rowEnd, columnStart, columnEnd, value.ToString());
            }
            /// <summary>
            /// 将布尔值写入到单元格
            /// </summary>
            /// <param name="rowStart"></param>
            /// <param name="rowEnd"></param>
            /// <param name="columnStart"></param>
            /// <param name="columnEnd"></param>
            /// <param name="value"></param>
            public void Write(int rowStart, int rowEnd, int columnStart, int columnEnd, bool value, Style style)
            {
                Write<string>(rowStart, rowEnd, columnStart, columnEnd, value.ToString(),style);
            }
            /// <summary>
            /// 将数字写入到单元格
            /// </summary>
            /// <param name="row"></param>
            /// <param name="column"></param>
            /// <param name="value"></param>
            public void Write(int row, int column, double value)
            {
                Write<double>(row, column, value);
            }
            /// <summary>
            /// 将数字写入到单元格
            /// </summary>
            /// <param name="rowStart"></param>
            /// <param name="rowEnd"></param>
            /// <param name="columnStart"></param>
            /// <param name="columnEnd"></param>
            /// <param name="value"></param>
            public void Write(int rowStart, int rowEnd, int columnStart, int columnEnd, double value)
            {
                Write<double>(rowStart, rowEnd, columnStart, columnEnd, value);
            }
            /// <summary>
            /// 将数字写入到单元格
            /// </summary>
            /// <param name="row"></param>
            /// <param name="column"></param>
            /// <param name="value"></param>
            public void Write(int row, int column, double value, Style style)
            {
                Write<double>(row, row, column, column, value, style);
            }
            /// <summary>
            /// 将数字写入到单元格
            /// </summary>
            /// <param name="rowStart"></param>
            /// <param name="rowEnd"></param>
            /// <param name="columnStart"></param>
            /// <param name="columnEnd"></param>
            /// <param name="value"></param>
            public void Write(int rowStart, int rowEnd, int columnStart, int columnEnd, double value, Style style)
            {
                Write<double>(rowStart, rowEnd, columnStart, columnEnd, value,style);
            }
            /// <summary>
            /// 将字符串写入指定单元格
            /// </summary>
            /// <param name="row"></param>
            /// <param name="column"></param>
            /// <param name="value"></param>
            public void Write(int row, int column, string value)
            {
                Write<string>(row, column, value);
            }
            /// <summary>
            /// 将字符串写入指定单元格
            /// </summary>
            /// <param name="row"></param>
            /// <param name="column"></param>
            /// <param name="value"></param>
            public void Write(int row, int column, string value, Style style)
            {
                Write<string>(row, row, column, column, value, style);
            }
            /// <summary>
            /// 将字符串写入指定单元格
            /// </summary>
            /// <param name="rowStart"></param>
            /// <param name="rowEnd"></param>
            /// <param name="columnStart"></param>
            /// <param name="columnEnd"></param>
            /// <param name="value"></param>
            public void Write(int rowStart, int rowEnd, int columnStart, int columnEnd, string value)
            {
                Write<string>(rowStart, rowEnd, columnStart, columnEnd, value);
            }
            /// <summary>
            /// 将字符串写入指定单元格
            /// </summary>
            /// <param name="rowStart"></param>
            /// <param name="rowEnd"></param>
            /// <param name="columnStart"></param>
            /// <param name="columnEnd"></param>
            /// <param name="value"></param>
            public void Write(int rowStart, int rowEnd, int columnStart, int columnEnd, string value,  Style style)
            {
                Write<string>(rowStart, rowEnd, columnStart, columnEnd, value, style);
            }
            /// <summary>
            /// 将数值写入指定单元格
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="row"></param>
            /// <param name="column"></param>
            /// <param name="value"></param>
            public void Write<T>(int row, int column, T value)
            {
                Write<T>(row, row, column, column, value);
            }
            /// <summary>
            /// 将指定数据写入到Excel表格
            /// </summary>
            /// <typeparam name="T">数据类型</typeparam>
            /// <param name="rowStart">行开始序号,从0开始</param>
            /// <param name="columnStart">列开始序号,从0开始</param>
            /// <param name="rowEnd">行结束序号,从0开始</param>
            /// <param name="columnEnd">列结束序号,从0开始</param>
            /// <param name="value">写入内容</param>
            public void Write<T>(int rowStart, int rowEnd, int columnStart, int columnEnd, T value)
            {
                Write<T>(rowStart, rowEnd, columnStart, columnEnd, value, new  Style());
            }
            /// <summary>
            /// 将指定数据写入到Excel表格
            /// </summary>
            /// <typeparam name="T">数据类型</typeparam>
            /// <param name="rowStart">行开始序号,从0开始</param>
            /// <param name="columnStart">列开始序号,从0开始</param>
            /// <param name="rowEnd">行结束序号,从0开始</param>
            /// <param name="columnEnd">列结束序号,从0开始</param>
            /// <param name="value">写入内容</param>
            /// <param name="style">单元格样式</param>
            public void Write<T>(int rowStart, int rowEnd, int columnStart, int columnEnd, T value, Style style)
            {

                rowEnd = Math.Max(rowStart, rowEnd);
                columnEnd = Math.Max(columnStart, columnEnd);
                if (!Open())
                {
                    All.Class.Log.Add("当前Excel文件流打开失败,不能进行写入");
                    return;
                }
                NPOI.SS.UserModel.IRow row;
                NPOI.SS.UserModel.ICell cell;
                if (table.LastRowNum < rowEnd)
                {
                    for (int i = table.LastRowNum + 1; i <= rowEnd; i++)
                    {
                        row = table.CreateRow(i);
                    }
                }
                for (int i = rowStart; i <= rowEnd; i++)
                {
                    for (int j = columnStart; j <= columnEnd; j++)
                    {
                        row = table.GetRow(i);
                        cell = row.CreateCell(j);
                        cell.CellStyle = GetExcelStyle(style);
                        if (i == rowStart && j == columnStart)
                        {
                            switch (All.Class.TypeUse.GetType<T>())
                            {
                                case TypeUse.TypeList.Boolean:
                                    cell.SetCellValue((bool)(object)value);
                                    break;
                                case TypeUse.TypeList.DateTime:
                                    cell.SetCellValue((DateTime)(object)value);
                                    break;
                                case TypeUse.TypeList.Byte:
                                    cell.SetCellValue((byte)(object)value);
                                    break;
                                case TypeUse.TypeList.Float:
                                    cell.SetCellValue((float)(object)value);
                                    break;
                                case TypeUse.TypeList.UShort:
                                    cell.SetCellValue((ushort)(object)value);
                                    break;
                                case TypeUse.TypeList.Int:
                                    cell.SetCellValue((int)(object)value);
                                    break;
                                case TypeUse.TypeList.Long:
                                    cell.SetCellValue((long)(object)value);
                                    break;
                                case TypeUse.TypeList.Double:
                                    cell.SetCellValue((double)(object)value);
                                    break;
                                case TypeUse.TypeList.String:
                                    cell.SetCellValue(value.ToString());
                                    break;
                                default:
                                    cell.SetCellValue("");
                                    break;
                            }
                            //区域
                            NPOI.SS.Util.CellRangeAddress cra = new NPOI.SS.Util.CellRangeAddress(rowStart, rowEnd, columnStart, columnEnd);
                            table.AddMergedRegion(cra);
                        }
                    }
                }
            }
            /// <summary>
            /// 根据用户设置的Style来决定是否要新建ExcelStyle
            /// </summary>
            /// <param name="style"></param>
            /// <returns></returns>
            private NPOI.SS.UserModel.ICellStyle GetExcelStyle(Style style)
            {
                int index = this.style.FindIndex(s =>
                    s.BackColor == style.BackColor && s.Border == style.Border &&
                    s.Font.FontFamily.Name == style.Font.FontFamily.Name &&
                    s.Font.Size == style.Font.Size && s.Font.Bold == style.Font.Bold &&
                    s.ForeColor == style.ForeColor && s.TextAlign == style.TextAlign);
                if (index < 0)
                {
                    this.style.Add(style.DeepClone());
                    NPOI.SS.UserModel.ICellStyle cellStyle = workBook.CreateCellStyle();
                    excelStyle.Add(excelStyle.Count, cellStyle);

                    //背景色
                    cellStyle.FillForegroundColor = (short)style.BackColor;
                    cellStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    //文本色
                    NPOI.SS.UserModel.IFont fontStyle = workBook.CreateFont();
                    fontStyle.Color = (short)style.ForeColor;
                    cellStyle.SetFont(fontStyle);
                    //字体
                    fontStyle.FontName = style.Font.FontFamily.Name;
                    fontStyle.FontHeightInPoints = (short)style.Font.Size;
                    fontStyle.IsBold = style.Font.Bold;
                    //fontStyle.
                    //文本位置
                    switch (style.TextAlign)
                    {
                        case System.Drawing.ContentAlignment.TopCenter:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
                            break;
                        case System.Drawing.ContentAlignment.TopLeft:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
                            break;
                        case System.Drawing.ContentAlignment.TopRight:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
                            break;
                        case System.Drawing.ContentAlignment.BottomCenter:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
                            break;
                        case System.Drawing.ContentAlignment.BottomLeft:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
                            break;
                        case System.Drawing.ContentAlignment.BottomRight:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
                            break;
                        case System.Drawing.ContentAlignment.MiddleCenter:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                            break;
                        case System.Drawing.ContentAlignment.MiddleLeft:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                            break;
                        case System.Drawing.ContentAlignment.MiddleRight:
                            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                            break;
                    }
                    //边框
                    if (style.Border)
                    {
                        cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    }
                    //((NPOI.HSSF.UserModel.HSSFSheet)table).SetEnclosedBorderOfRegion(cra,
                    //    style.Border ? NPOI.SS.UserModel.BorderStyle.Thin : NPOI.SS.UserModel.BorderStyle.None, (short)Color.Black);
                    return cellStyle;
                }
                return excelStyle[index];
            }
        //    #region//设置边框
        //    /// <summary>
        //    /// 设置单元格颜色
        //    /// </summary>
        //    /// <param name="row"></param>
        //    /// <param name="column"></param>
        //    public void SetBackColor(int row, int column,Color color)
        //    {
        //        SetBackColor(row, row, column, column, color);
        //    }
        //    /// <summary>
        //    /// 设置单元格颜色
        //    /// </summary>
        //    /// <param name="rowStart"></param>
        //    /// <param name="rowEnd"></param>
        //    /// <param name="columnStart"></param>
        //    /// <param name="columnEnd"></param>
        //    public void SetBackColor(int rowStart, int rowEnd, int columnStart, int columnEnd,Color color)
        //    {
        //        rowEnd = Math.Max(rowStart, rowEnd);
        //        columnEnd = Math.Max(columnStart, columnEnd);
        //        if (!Open())
        //        {
        //            All.Class.Log.Add("当前Excel文件流打开失败,不能进行写入");
        //            return;
        //        }
        //        NPOI.SS.UserModel.IRow row;
        //        NPOI.SS.UserModel.ICell cell;
        //        if (table.LastRowNum < rowEnd)
        //        {
        //            for (int i = table.LastRowNum + 1; i <= rowEnd; i++)
        //            {
        //                table.CreateRow(i);
        //            }
        //        }
        //        for (int i = rowStart; i <= rowEnd; i++)
        //        {
        //            for (int j = columnStart; j <= columnEnd; j++)
        //            {
        //                row = table.GetRow(i);
        //                cell =  row.GetCell(j);
        //                if (cell == null || (cell.IsMergedCell && cell.StringCellValue == ""))
        //                {
        //                    continue;
        //                }
        //                cell.CellStyle.FillForegroundColor = (short)color;
        //                cell.CellStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
        //            }
        //        }
        //    }
        //    #endregion
        //    #region//设置文本位置
        //    /// <summary>
        //    /// 设置指定行列的数据文本位置
        //    /// </summary>
        //    /// <param name="rowStart">行开始序号,从0开始</param>
        //    /// <param name="columnStart">列开始序号,从0开始</param>
        //    /// <param name="rowEnd">行结束序号,从0开始</param>
        //    /// <param name="columnEnd">列结束序号,从0开始</param>
        //    /// <param name="sf">文本对齐方式</param>
        //    public void SetStringFormat(int row, int column, System.Drawing.ContentAlignment sf)
        //    {
        //        SetStringFormat(row, row, column, column, sf);
        //    }
        //    /// <summary>
        //    /// 设置指定行列的数据文本位置
        //    /// </summary>
        //    /// <param name="rowStart">行开始序号,从0开始</param>
        //    /// <param name="columnStart">列开始序号,从0开始</param>
        //    /// <param name="rowEnd">行结束序号,从0开始</param>
        //    /// <param name="columnEnd">列结束序号,从0开始</param>
        //    /// <param name="sf">文本对齐方式</param>
        //    public void SetStringFormat(int rowStart, int rowEnd, int columnStart, int columnEnd, System.Drawing.ContentAlignment sf)
        //    {
        //        rowEnd = Math.Max(rowStart, rowEnd);
        //        columnEnd = Math.Max(columnStart, columnEnd);
        //        if (!Open())
        //        {
        //            All.Class.Log.Add("当前Excel文件流打开失败,不能进行写入");
        //            return;
        //        }
        //        NPOI.SS.UserModel.IRow row;
        //        NPOI.SS.UserModel.ICell cell;
        //        if (table.LastRowNum < rowEnd)
        //        {
        //            for (int i = table.LastRowNum + 1; i <= rowEnd; i++)
        //            {
        //                table.CreateRow(i);
        //            }
        //        }
        //        NPOI.SS.UserModel.ICellStyle style = null;
        //        for (int i = rowStart; i <= rowEnd; i++)
        //        {
        //            for (int j = columnStart; j <= columnEnd; j++)
        //            {
        //                row = table.GetRow(i);
        //                cell = row.GetCell(j);
        //                if (cell == null)
        //                {
        //                    continue;
        //                }
        //                style = cell.CellStyle;
        //                switch (sf)
        //                {
        //                    case System.Drawing.ContentAlignment.TopCenter:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
        //                        break;
        //                    case System.Drawing.ContentAlignment.TopLeft:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
        //                        break;
        //                    case System.Drawing.ContentAlignment.TopRight:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
        //                        break;
        //                    case System.Drawing.ContentAlignment.BottomCenter:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
        //                        break;
        //                    case System.Drawing.ContentAlignment.BottomLeft:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
        //                        break;
        //                    case System.Drawing.ContentAlignment.BottomRight:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
        //                        break;
        //                    case System.Drawing.ContentAlignment.MiddleCenter:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
        //                        break;
        //                    case System.Drawing.ContentAlignment.MiddleLeft:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
        //                        break;
        //                    case System.Drawing.ContentAlignment.MiddleRight:
        //                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
        //                        style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
        //                        break;
        //                }
        //            }
        //        }
        //    }
        }
        //    #endregion
        /// <summary>
        /// 将指定表格写入到Excel表格
        /// </summary>
        /// <param name="fileName">写入的文件名称</param>
        /// <param name="dt">表格</param>
        public static bool Write(string fileName, System.Data.DataTable dt)
        {
            return Write(fileName, dt, string.Format("{0:yyyyMMddHHmmss}", DateTime.Now));
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
                            if(dt.Rows[i][j].ToString() == "")
                            {
                                continue;
                            }
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
                    if (fileName.EndsWith("xlsx") || fileName.EndsWith("xlsm"))
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
