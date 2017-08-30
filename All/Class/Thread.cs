using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Class
{
    public class Thread
    {
        /// <summary>
        /// 所有多线程管理
        /// </summary>
        public Dictionary<string, System.Threading.Thread> AllThread
        { get; set; }
        public Thread()
        {
            AllThread = new Dictionary<string, System.Threading.Thread>();
        }
        /// <summary>
        /// 关闭所有线程
        /// </summary>
        public void Close()
        {
            lock (AllThread)
            {
                AllThread.Values.ToList().ForEach(thread =>
                    {
                        thread.Abort();
                    });
            }
        }
        /// <summary>
        /// 关闭一个线程
        /// </summary>
        /// <param name="id"></param>
        public void Close(string id)
        {
            lock (AllThread)
            {
                if (AllThread.ContainsKey(id))
                {
                    AllThread[id].Abort();
                    AllThread.Remove(id);
                }
            }
        }
        /// <summary>
        /// 新建一个线程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public void Create(string id, Action action, int delay)
        {
            lock (AllThread)
            {
                if (!AllThread.ContainsKey(id))
                {
                    System.Threading.Thread t = new System.Threading.Thread(() => Flush(action, delay));
                    t.IsBackground = true;
                    t.Start();
                    AllThread.Add(id, t);
                }
            }
        }
        ~Thread()
        {
            Close();
        }
        private void Flush(Action action,int sleep)
        {
            int start = System.Environment.TickCount;
            while (true)
            {
                if (sleep > 50)//超过50ms的刷新,将休眠时间打散,加快退出时的响应时间
                {
                    if ((System.Environment.TickCount - start) > sleep)
                    {
                        if (action != null)
                        {
                            action();
                        }
                        start = System.Environment.TickCount;
                    }
                    System.Threading.Thread.Sleep(30);
                }
                else
                {
                    if (action != null)
                    {
                        action();
                    }
                    start = System.Environment.TickCount;
                    System.Threading.Thread.Sleep(sleep);
                }
            }
        }
        /// <summary>
        /// 多线程管理类
        /// </summary>
        static Thread thManager = new Thread();
        /// <summary>
        /// 新建或打开指定ID的多线程
        /// </summary>
        /// <param name="threadID"></param>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static void CreateOrOpen(string threadID, Action action, int delay)
        {
            if (action == null)
            {
                return;
            }
            if (delay <= 0)
            {
                delay = 1000;
            }
            thManager.Create(threadID, action, delay);
        }
        /// <summary>
        /// 新建或打开指定ID的多线程
        /// </summary>
        /// <param name="threadID"></param>
        /// <param name="action"></param>
        public static void CreateOrOpen(string threadID, Action action)
        {
            CreateOrOpen(threadID, action, 1000);
        }
        /// <summary>
        /// 新建或找开一个新的多线程
        /// </summary>
        /// <param name="action"></param>
        public static string CreateOrOpen(Action action)
        {
            string guid = Num.CreateGUID();
            CreateOrOpen(guid, action);
            return guid;
        }
        /// <summary>
        /// 新建或找开一个新的多线程
        /// </summary>
        /// <param name="action"></param>
        public static string CreateOrOpen(Action action, int delay)
        {
            string guid = Num.CreateGUID();
            CreateOrOpen(guid, action,delay);
            return guid;
        }
        /// <summary>
        /// 关闭指定名称的线程
        /// </summary>
        /// <param name="threadID"></param>
        public static void Kill(string threadID)
        {
            if (threadID == null || threadID == "")
            {
                return;
            }
            thManager.Close(threadID);
        }
        /// <summary>
        /// 关闭所有线程
        /// </summary>
        public static void Kill()
        {
            thManager.Close();
        }
        /// <summary>
        /// 直接多线程运行
        /// </summary>
        /// <param name="action"></param>
        public static void Run(Action action)
        {
            new System.Threading.Thread(()=>action()) { IsBackground = true }.Start();
        }
    }
}
