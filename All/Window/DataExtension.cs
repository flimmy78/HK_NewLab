using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace System
{
    public static partial class DataExtension
    {
        #region//DataGridView
        /// <summary>
        /// 将行指定列的数据转化为Bool类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static bool ToBool(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToBool(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return false;
        }
        /// <summary>
        /// 将行指定列的数据转化为Bool类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static bool ToBool(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToBool(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return false;
        }
        /// <summary>
        /// 将行指定列的数据转化为float类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static float ToFloat(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToSingle(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为float类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static float ToFloat(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToSingle(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为double类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static double ToDouble(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToDouble(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为double类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static double ToDouble(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToDouble(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为byte类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static byte ToByte(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToByte(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Byte类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static byte ToByte(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToByte(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为int类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static int ToInt(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToInt(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为int类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static int ToInt(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToInt(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为ushort类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static ushort ToUshort(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToUshort(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为ushort类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static ushort ToUshort(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToUshort(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Long类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static long ToLong(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToLong(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Long类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static long ToLong(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToLong(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为string类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static string ToString(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToString(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return "";
        }
        /// <summary>
        /// 将行指定列的数据转化为String类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static string ToString(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToString(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return "";
        }
        /// <summary>
        /// 将行指定列数据转化为DateTime类型
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DataGridView dg, int rowIndex, int colIndex)
        {
            if (dg.Rows[rowIndex].Cells[colIndex].Value != DBNull.Value)
            {
                return All.Class.Num.ToDateTime(dg.Rows[rowIndex].Cells[colIndex].Value);
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 将行指定列数据转化为DateTime类型
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DataGridView dg, int rowIndex, string colName)
        {
            if (dg.Rows[rowIndex].Cells[colName].Value != DBNull.Value)
            {
                return All.Class.Num.ToDateTime(dg.Rows[rowIndex].Cells[colName].Value);
            }
            return DateTime.Now;
        }
        #endregion
        #region//DataGridViewRow
        /// <summary>
        /// 将行指定列的数据转化为Bool类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static bool ToBool(this DataGridViewRow dr, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToBoolean(dr.Cells[colIndex].Value);
            }
            return false;
        }
        /// <summary>
        /// 将行指定列的数据转化为Bool类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static bool ToBool(this DataGridViewRow dr, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToBoolean(dr.Cells[colName].Value);
            }
            return false;
        }
        /// <summary>
        /// 将行指定列的数据转化为float类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static float ToFloat(this DataGridViewRow dr, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToSingle(dr.Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为float类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static float ToFloat(this DataGridViewRow dr, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToSingle(dr.Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为double类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static double ToDouble(this DataGridViewRow dr, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToDouble(dr.Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为double类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static double ToDouble(this DataGridViewRow dr, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToDouble(dr.Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为byte类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static byte ToByte(this DataGridViewRow dr, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToByte(dr.Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Byte类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static byte ToByte(this DataGridViewRow dr, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToByte(dr.Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为int类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static int ToInt(this DataGridViewRow dr, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToInt32(dr.Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为int类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static int ToInt(this DataGridViewRow dr, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToInt32(dr.Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为ushort类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static ushort ToUshort(this DataGridViewRow dr, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToUInt16(dr.Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为ushort类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static ushort ToUshort(this DataGridViewRow dr, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToUInt16(dr.Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Long类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static long ToLong(this DataGridViewRow dr, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToInt64(dr.Cells[colIndex].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Long类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static long ToLong(this DataGridViewRow dr, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToInt64(dr.Cells[colName].Value);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为string类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static string ToString(this DataGridViewRow dr, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToString(dr.Cells[colIndex].Value);
            }
            return "";
        }
        /// <summary>
        /// 将行指定列的数据转化为String类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static string ToString(this DataGridViewRow dr, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToString(dr.Cells[colName].Value);
            }
            return "";
        }
        /// <summary>
        /// 将行指定列数据转化为DateTime类型
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DataGridViewRow dr, int rowIndex, int colIndex)
        {
            if (dr.Cells[colIndex].Value != DBNull.Value)
            {
                return Convert.ToDateTime(dr.Cells[colIndex].Value);
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 将行指定列数据转化为DateTime类型
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DataGridViewRow dr, int rowIndex, string colName)
        {
            if (dr.Cells[colName].Value != DBNull.Value)
            {
                return Convert.ToDateTime(dr.Cells[colName].Value);
            }
            return DateTime.Now;
        }
        #endregion
    }
}
