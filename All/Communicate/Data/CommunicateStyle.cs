using System;
using System.Collections.Generic;
using System.Text;

namespace All.Communicate.Data
{
    /// <summary>
    /// 通讯节点
    /// </summary>
    public class CommunicateStyle
    {
        /// <summary>
        /// 通讯方式
        /// </summary>
        public Communicate Value
        { get; set; }
        /// <summary>
        /// 通讯类下的有设备
        /// </summary>
        public MeterStyleCollection Meters
        { get; set; }
        /// <summary>
        /// 通讯节点,即一个通讯父类
        /// </summary>
        public CommunicateStyle()
        {
            this.Value = null;
            this.Meters = new MeterStyleCollection();
        }
    }
    /// <summary>
    /// 通讯节点集合
    /// </summary>
    public class CommunicateStyleCollection:List<CommunicateStyle>
    {
        Dictionary<string, CommunicateStyle> buff = new Dictionary<string, CommunicateStyle>();
        /// <summary>
        /// 根据通讯名称Text找到对应的通讯节点
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public CommunicateStyle this[string item]
        {
            get 
            {
                if (buff.ContainsKey(item))
                {
                    return buff[item];
                }
                int index = base.FindIndex(tmp => tmp.Value.Text == item);
                if (index < 0)
                {
                    throw new Exception(string.Format("没有找到名称为:{0}的通讯节点", item));
                }
                buff.Add(item, base[index]);
                return buff[item];
            }
        }
    }
}
