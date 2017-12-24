using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Class
{
    public class Clone
    {
        /// <summary>
        /// 获取当前类型的深度副本,尝试副本为新内存区域
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeepClone<T>(IDeepClone<T> value)
        {
            try
            {
                return (T)All.Class.SingleFileSave.SSFile.Byte2Object(All.Class.SingleFileSave.SSFile.Object2Byte(value));
            }
            catch
            {
                throw new Exception("请将要克隆的类标记为[Serializable]");
            }
        }
        /// <summary>
        /// 获取类型的浅表副本,浅表副本为原内存区域
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ShallowClone<T>(IShallowClone<T> value)
        {
            return (T)(object)value;
        }
        /// <summary>
        /// 深度克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IDeepClone<T>
        {
            T DeepClone();
        }
        /// <summary>
        /// 浅表克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IShallowClone<T>
        {
            T ShallowClone();
        }
    }
}
