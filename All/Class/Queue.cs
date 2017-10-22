using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Class
{
    /// <summary>
    /// 请求排队
    /// </summary>
    public class Queue<T>
    {
        object lockObject = new object();
        /// <summary>
        /// 队列元素改变
        /// </summary>
        public event Action QueueItemChange;
        /// <summary>
        /// 排队队列
        /// </summary>
        Dictionary<int, List<T>> allQueue = new Dictionary<int, List<T>>();
        /// <summary>
        /// 元素所在队列
        /// </summary>
        Dictionary<T, int> queue2Index = new Dictionary<T, int>();
        /// <summary>
        /// 实例化一个排队
        /// </summary>
        /// <param name="queueCount">排队的数量</param>
        public Queue(int queueCount)
        {
            allQueue.Clear();
            for (int i = 0; i < queueCount; i++)
            {
                allQueue.Add(i, new List<T>());
            }
        }
        /// <summary>
        /// 添加一个初始化数据,即添加一个要排队的数据
        /// </summary>
        /// <param name="queueIndex"></param>
        /// <param name="queueValue"></param>
        public void Init(int queueIndex, T queueValue)
        {
            if (queueIndex < 0 || queueIndex >= allQueue.Count)
            {
                throw new Exception(string.Format("当前只初始化了{0}个队列,但groupIndex:{1}不在此范围内", allQueue.Count, queueIndex));
            }
            if (queue2Index.ContainsKey(queueValue))
            {
                throw new Exception(string.Format("当前已添加过此项:{0},不能重复添加", queueValue));
            }
            queue2Index.Add(queueValue, queueIndex);
        }
        /// <summary>
        /// 获取指定队列的所有数据
        /// </summary>
        /// <param name="queueIndex"></param>
        /// <returns></returns>
        public List<T> Value(int queueIndex)
        {
            if (queueIndex < 0 || queueIndex >= allQueue.Count)
            {
                Error.Add(string.Format("当前查询的队列序号超出范围", queueIndex));
                return null;
            }
            return allQueue[queueIndex];
        }
        /// <summary>
        /// 当前元素是否已包含
        /// </summary>
        /// <param name="item"></param>
        public bool Contain(T item)
        {
            return queue2Index.ContainsKey(item);
        }
        /// <summary>
        /// 请求权限
        /// </summary>
        /// <param name="item"></param>
        public void Request(T item)
        {
            if (!queue2Index.ContainsKey(item))
            {
                Error.Add(string.Format("当前添加的元素{0},没有初始化", item));
                return;
            }
            int index = queue2Index[item];
            if (index < 0 || index >= allQueue.Count)
            {
                Error.Add(string.Format("当前添加的元素{0},超出数组界限,请重新初始化", item));
                return;
            }
            if (allQueue[index].Contains(item))
            {
                return;
            }
            lock (lockObject)
            {
                allQueue[index].Add(item);
                if (QueueItemChange != null)
                {
                    QueueItemChange();
                }
            }
        }
        /// <summary>
        /// 获取指定的项是否允许执行
        /// </summary>
        /// <param name="item">指定项</param>
        /// <returns></returns>
        public bool Allow(T item)
        {
            if (!queue2Index.ContainsKey(item))
            {
                Error.Add(string.Format("当前查询的元素{0},没有初始化", item));
                return false;
            }
            int index = queue2Index[item];
            if (index < 0 || index >= allQueue.Count)
            {
                return false;
            }
            lock (lockObject)
            {
                return allQueue[index].IndexOf(item) == 0;
            }
        }

        /// <summary>
        /// 查寻指定项的位置
        /// </summary>
        /// <param name="item">指定项</param>
        /// <returns></returns>
        public int Search(T item)
        {
            if (!queue2Index.ContainsKey(item))
            {
                Error.Add(string.Format("当前查询的元素{0},没有初始化", item));
                return -1;
            }
            int index = queue2Index[item];
            if (index < 0 || index >= allQueue.Count)
            {
                return -1;
            }
            lock (lockObject)
            {
                return allQueue[index].IndexOf(item);
            }
        }
        /// <summary>
        /// 移除指定项的请求
        /// </summary>
        /// <param name="item">指定项</param>
        public void Remove(T item)
        {
            if (!queue2Index.ContainsKey(item))
            {
                Error.Add(string.Format("当前查询的元素{0},没有初始化", item));
                return;
            }
            int index = queue2Index[item];
            if (index < 0 || index >= allQueue.Count)
            {
                return;
            }
            if (!allQueue[index].Contains(item))
            {
                return;
            }
            lock (lockObject)
            {
                allQueue[index].Remove(item);
                if (QueueItemChange != null)
                {
                    QueueItemChange();
                }
            }
        }
        /// <summary>
        /// 指定项的请求数量
        /// </summary>
        /// <param name="item"></param>
        public int GetCount(T item)
        {
            if (!queue2Index.ContainsKey(item))
            {
                Error.Add(string.Format("当前查询的元素{0},没有初始化", item));
                return 0;
            }
            int index = queue2Index[item];
            if (index < 0 || index >= allQueue.Count)
            {
                return 0;
            }
            return allQueue[index].Count;
        }
        /// <summary>
        /// 指定项的请求数量 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetCount(int index)
        {
            if (index < 0 || index >= allQueue.Count)
            {
                return 0;
            }
            return allQueue[index].Count;
 
        }
        /// <summary>
        /// 获取指定项所在组别,允许的工位号
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public T GetCurrent(T item)
        {
            if (!queue2Index.ContainsKey(item))
            {
                Error.Add(string.Format("当前查询的元素{0},没有初始化", item));
                return default(T);
            }
            int index = queue2Index[item];
            if (index < 0 || index >= allQueue.Count)
            {
                return default(T);
            }
            if (allQueue[index].Count == 0)
            {
                return default(T);
            }
            lock (lockObject)
            {
                return allQueue[index][0];
            }
        }
        /// <summary>
        /// 获取指定组别的允许工位
        /// </summary>
        /// <param name="queueIndex"></param>
        /// <returns></returns>
        public T GetCurrent(int queueIndex)
        {
            if (queueIndex < 0 || queueIndex >= allQueue.Count)
            {
                return default(T);
            }
            if (allQueue[queueIndex].Count == 0)
            {
                return default(T);
            }
            lock (lockObject)
            {
                return allQueue[queueIndex][0];
            }
        }
        /// <summary>
        /// 获取指定组别的允许工位
        /// </summary>
        /// <param name="queueIndex"></param>
        /// <returns></returns>
        public T this[int queueIndex]
        {
            get
            {
                return GetCurrent(queueIndex);
            }
        }
    }
}
