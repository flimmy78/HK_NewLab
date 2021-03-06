﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
namespace System
{
    /// <summary>
    /// 直接取各种数据结构中的数据
    /// </summary>
    public static partial class DataExtension
    {
        #region//DataRow
        /// <summary>
        /// 将行指定列的数据转化为Bool类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static bool ToBool(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToBoolean(dr[colIndex]);
            }
            return false;
        }
        /// <summary>
        /// 将行指定列的数据转化为Bool类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static bool ToBool(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToBoolean(dr[colName]);
            }
            return false;
        }
        /// <summary>
        /// 将行指定列的数据转化为float类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static float ToFloat(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToSingle(dr[colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为float类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static float ToFloat(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToSingle(dr[colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为double类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static double ToDouble(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToDouble(dr[colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为double类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static double ToDouble(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToDouble(dr[colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为byte类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static byte ToByte(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToByte(dr[colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Byte类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static byte ToByte(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToByte(dr[colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为int类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static int ToInt(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToInt32(dr[colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为int类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static int ToInt(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToInt32(dr[colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为ushort类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static ushort ToUshort(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToUInt16(dr[colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为ushort类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static ushort ToUshort(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToUInt16(dr[colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Long类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static long ToLong(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToInt64(dr[colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Long类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static long ToLong(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToInt64(dr[colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为string类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static string ToString(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToString(dr[colIndex]);
            }
            return "";
        }
        /// <summary>
        /// 将行指定列的数据转化为String类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static string ToString(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToString(dr[colName]);
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
        public static DateTime ToDateTime(this DataRow dr, int colIndex)
        {
            if (dr[colIndex] != DBNull.Value)
            {
                return Convert.ToDateTime(dr[colIndex]);
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
        public static DateTime ToDateTime(this DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
            {
                return Convert.ToDateTime(dr[colName]);
            }
            return DateTime.Now;
        }
        #endregion
        #region//DataTable
        /// <summary>
        /// 将行指定列的数据转化为Bool类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static bool ToBool(this DataTable dt,int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToBool(dt.Rows[rowIndex][colIndex]);
            }
            return false;
        }
        /// <summary>
        /// 将行指定列的数据转化为Bool类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static bool ToBool(this DataTable dt,int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToBool(dt.Rows[rowIndex][colName]);
            }
            return false;
        }
        /// <summary>
        /// 将行指定列的数据转化为float类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static float ToFloat(this DataTable dt,int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToSingle(dt.Rows[rowIndex][colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为float类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static float ToFloat(this DataTable dt,int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToSingle(dt.Rows[rowIndex][colName]);
            }
            return 0;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static short ToShort(this DataTable dt, int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToShort(dt.Rows[rowIndex][colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static short ToShort(this DataTable dt, int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToShort(dt.Rows[rowIndex][colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为double类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static double ToDouble(this DataTable dt,int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToDouble(dt.Rows[rowIndex][colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为double类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static double ToDouble(this DataTable dt,int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToDouble(dt.Rows[rowIndex][colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为byte类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static byte ToByte(this DataTable dt,int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToByte(dt.Rows[rowIndex][colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Byte类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static byte ToByte(this DataTable dt,int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToByte(dt.Rows[rowIndex][colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为int类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static int ToInt(this DataTable dt,int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToInt(dt.Rows[rowIndex][colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为int类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static int ToInt(this DataTable dt,int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToInt(dt.Rows[rowIndex][colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为ushort类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static ushort ToUshort(this DataTable dt,int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToUshort(dt.Rows[rowIndex][colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为ushort类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static ushort ToUshort(this DataTable dt, int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToUshort(dt.Rows[rowIndex][colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Long类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static long ToLong(this DataTable dt,int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToLong(dt.Rows[rowIndex][colIndex]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为Long类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static long ToLong(this DataTable dt,int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToLong(dt.Rows[rowIndex][colName]);
            }
            return 0;
        }
        /// <summary>
        /// 将行指定列的数据转化为string类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colIndex">指定列</param>
        /// <returns></returns>
        public static string ToString(this DataTable dt,int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToString(dt.Rows[rowIndex][colIndex]);
            }
            return "";
        }
        /// <summary>
        /// 将行指定列的数据转化为String类型
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="colName">指定列</param>
        /// <returns></returns>
        public static string ToString(this DataTable dt, int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToString(dt.Rows[rowIndex][colName]);
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
        public static DateTime ToDateTime(this DataTable dt, int rowIndex, int colIndex)
        {
            if (dt.Rows[rowIndex][colIndex] != DBNull.Value)
            {
                return All.Class.Num.ToDateTime(dt.Rows[rowIndex][colIndex]);
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
        public static DateTime ToDateTime(this DataTable dt, int rowIndex, string colName)
        {
            if (dt.Rows[rowIndex][colName] != DBNull.Value)
            {
                return All.Class.Num.ToDateTime(dt.Rows[rowIndex][colName]);
            }
            return DateTime.Now;
        }

        #endregion
    }
}
