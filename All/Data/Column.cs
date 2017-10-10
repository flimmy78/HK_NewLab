using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace All.Data
{
    public class ColumnType:ColumnAttribute
    {
        /// <summary>
        /// 列数据类型
        /// </summary>
        public All.Class.TypeUse.TypeList TypeValue
        { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public string Text
        { get; set; }
        public ColumnType()
        {
            TypeValue = All.Class.TypeUse.TypeList.String;
            Text = "";
        }
    }
    /// <summary>
    /// 字段特性,即修饰数据字段
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : System.Attribute
    {
        /// <summary>
        /// 文本数据长度
        /// </summary>
        public int Count
        { get; set; }
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool AutoAdd
        { get; set; }
        /// <summary>
        /// 列是否为索引
        /// </summary>
        public bool Indexed
        { get; set; }
        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool Key
        { get; set; }
        /// <summary>
        /// 是否使用,即是否创建字段
        /// </summary>
        public bool Use
        { get; set; }
        /// <summary>
        /// 当属性为数组时,用此来确定数组的长度
        /// </summary>
        public int[] Length
        { get; set; }
        public ColumnAttribute()
        {
            Key = false;
            Indexed = false;
            Count = 255;
            AutoAdd = false;
            Use = true;
        }
    }
}
