using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static partial class DataExtension
    {
        /// <summary>
        /// 根据类与属性结构,自动创建数据表,类名称为表名称,属性为字段名称,字段其他功能,请用特性修饰
        /// </summary>
        /// <typeparam name="T">所有继承IAutoTable,须要自动创建数据表的类型</typeparam>
        /// <param name="value">所有继承IAutoTable,须要自动创建数据表的类</param>
        /// <param name="Conn">数据库连接</param>
        /// <returns>创建表格是否成功</returns>
        public static bool CreateTable<T>(this T value, All.Data.DataReadAndWrite Conn) where T : IAutoTable
        {
            return true;
        }
    }
    /// <summary>
    /// 自动创建表接口
    /// </summary>
    public interface IAutoTable
    {
        
    }
}
