using System;
using System.Collections.Generic;
using System.Text;

namespace All.Communicate.Data
{
    public class DataDetial<T>
    {
        #region//属性
        All.Class.TypeUse.TypeList typeValue = All.Class.TypeUse.TypeList.UnKnow;
        /// <summary>
        /// 当前数据类型
        /// </summary>
        public All.Class.TypeUse.TypeList TypeValue
        {
            get 
            {
                if (typeValue == All.Class.TypeUse.TypeList.UnKnow)
                {
                    typeValue = All.Class.TypeUse.GetType<T>();
                }
                return typeValue;
            }
        }
        /// <summary>
        /// 当前值
        /// </summary>
        public Dictionary<int,T> Value
        { get; set; }
        /// <summary>
        /// 数据名称
        /// </summary>
        public Dictionary<int,string> Info
        { get; set; }
        #endregion
        #region//事件
        public event ValueReadHandle ValueChange;
        /// <summary>
        /// 数据读取完毕事件
        /// </summary>
        public event ValueReadHandle ValueReadOver;
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index">数据序列号</param>
        /// <param name="lastValue">前一值</param>
        /// <param name="value">当前值</param>
        public delegate void ValueReadHandle(int index, T lastValue, T value, string info);
        #endregion
        public DataDetial()
        {
            Value = new Dictionary<int, T>();
            Info = new Dictionary<int,string>();
        }
        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void Clear()
        {
            Value.Clear();
            Info.Clear();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void SetValue(int index, T value)
        {
            if (Value == null || index < 0 || index >= Value.Count)
            {
                return;
            }
            if (ValueChange != null && !Value[index].Equals(value))//读取到的值改变了
            {
                ValueChange(index, Value[index], value, Info[index]);
            }
            if (ValueReadOver != null)//读取到一次值,用于barcode等可能须要重复的循环触发
            {
                ValueReadOver(index, Value[index], value, Info[index]);
            }
            Value[index] = value;
        }
        /// <summary>
        /// 添加一组指定读取内容
        /// </summary>
        /// <param name="buff"></param>
        public void AddRange(Dictionary<int, string> buff)
        {
            if (buff == null)
            {
                return;
            }
            foreach (int index in buff.Keys)
            {
                if (Check(index))
                {
                    All.Class.Error.Add(string.Format("当前序号已存在,不能存在同序号数据,第一次出现:{0},当前出现:{1}",
                        Info[index], buff[index]));
                    continue;
                }
                if (index >= Info.Count)
                {
                    for (int i = Info.Count; i < index; i++)
                    {
                        Add(i, null);//跳跃式index,则中间直接添加null名称
                    }
                    Add(index, buff[index]);
                }
                else
                {
                    Info[index] = buff[index];
                }
            }
        }
        /// <summary>
        /// 检查指定位置是否已经有其他数据项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool Check(int index)
        {
            if (index >= Info.Count)
            {
                return false;
            }
            return Info[index] != null;
        }
        /// <summary>
        /// 添加一组数据
        /// </summary>
        /// <param name="info"></param>
        private void Add(int index, string info)
        {
            switch (this.TypeValue)
            {
                case Class.TypeUse.TypeList.Boolean:
                    Value.Add(index, (T)(object)false);
                    Info.Add(index, info);
                    break;
                case Class.TypeUse.TypeList.DateTime:
                    Value.Add(index, (T)(object)DateTime.Now);
                    Info.Add(index, info);
                    break;
                case Class.TypeUse.TypeList.Long:
                case Class.TypeUse.TypeList.Byte:
                case Class.TypeUse.TypeList.Double:
                case Class.TypeUse.TypeList.Float:
                case Class.TypeUse.TypeList.Int:
                case Class.TypeUse.TypeList.UShort:
                    Value.Add(index, default(T));
                    Info.Add(index, info);
                    break;
                case Class.TypeUse.TypeList.String:
                    Value.Add(index, (T)(object)"");
                    Info.Add(index, info);
                    break;
                case Class.TypeUse.TypeList.Bytes:
                    Value.Add(index, (T)(object)null);
                    Info.Add(index, info);
                    break;
                case Class.TypeUse.TypeList.UnKnow:
                    All.Class.Error.Add("未知类型", string.Format("DataStyle.Add出现未知数据类型,数据名为:{0}", info));
                    break;
            }
        }
    }
}
