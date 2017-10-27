using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class SSLockMain:Meter
    {
        string threadGuid = All.Class.Num.CreateGUID();
        /// <summary>
        /// 返回给分站的状态
        /// </summary>
        public enum ReturnStatueList : int
        {
            无请求 = 0,
            测试完成 = 0,
            等待,
            允许
        }
        /// <summary>
        /// 主机当前状态
        /// </summary>
        public enum GetStatueList : int
        {
            无请求 = 0,
            等待主机执行互锁前切换动作,
            等待执行互锁动作
        }
        /// <summary>
        /// 主机可能的操作
        /// </summary>
        public enum SetStatueList : int
        {
            互锁切换执行完毕,
            互锁操作执行完毕
        }
        /// <summary>
        /// 分站排队完成,等待主机执行互锁前的动作,如果没有动作,请直接写入[互锁切换执行完毕]
        /// </summary>
        public event EventHandler NeedMainSwitch;
        /// <summary>
        /// 分站切换完成,等待主机执行互锁动作,如安检等
        /// </summary>
        public event EventHandler NeedMainStart;
        /// <summary>
        /// 检测完成,等待主机关闭所有动作
        /// </summary>
        public event EventHandler NeedMainReset;

        GetStatueList currentGetStatue = GetStatueList.无请求;
        /// <summary>
        /// 排队序列
        /// </summary>
        public Class.Queue<string> Queue
        { get; set; }
        /// <summary>
        /// 管理类当前处理后的结果,用于直接返回分站请求
        /// </summary>
        Dictionary<string, ReturnStatueList> allClientReturnStatue = new Dictionary<string, ReturnStatueList>();
        /// <summary>
        /// 当前分站发送过来的请求
        /// </summary>
        Dictionary<string, SSLockClient.SetStatueList> allClientRequestStatue = new Dictionary<string, SSLockClient.SetStatueList>();
        /// <summary>
        /// 当前正在执行的第一个分站
        /// </summary>
        string currentGUID = "";

        Dictionary<string, string> initParm = new Dictionary<string, string>();

        public SSLockMain()
        {
            Queue = new Class.Queue<string>(1);
        }
        ~SSLockMain()
        {
            All.Class.Thread.Kill(threadGuid);
            this.Close();
        }
        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            base.Init(initParm);
            if (this.Parent == null)
            {
                All.Class.Error.Add("SSMeter初始化前,必须先初始化设置Parent通讯类");
                return;
            }
            if (this.Parent.Meters.Count > 0)
            {
                All.Class.Error.Add("SSMeter必须独占一个通讯类,如果有多组通讯类,请分开处理");
                return;
            }
            this.Parent.GetArgs += Parent_GetArgs;
            All.Class.Thread.CreateOrOpen(threadGuid, Loop, 100);
            All.Class.Log.Add("SSLockMain.初始化成功,开始执行监听操作");
        }
        void Parent_GetArgs(object sender, Communicate.Base.Base.ReciveArgs reciveArgs)
        {
            lock (lockObject)
            {
                Communicate.Communicate parent = (Communicate.Communicate)sender;
                int len = parent.DataRecive;
                byte[] buff;
                if (len > 0 && this.Read<byte[]>(len, out buff))
                {
                    SSMeter.SSReadMeter.ReadDataStyle rs = SSMeter.SSReadMeter.ReadDataStyle.Parse(buff);
                    SSMeter.SSReadMeter.ReadResultStyle rr = new SSMeter.SSReadMeter.ReadResultStyle(Class.TypeUse.TypeList.Int, rs.Random, true, null);
                    if (rs == null)
                    {
                        All.Class.Error.Add("SSLockClient.Loop无校验或校验不通过");
                        return;
                    } 
                    if (!rs.Symbol.ContainsKey("GUID"))
                    {
                        All.Class.Error.Add("SSLockMain接收的字符串没有GUID识别码");
                        return;
                    }
                    if (!Queue.Contain(rs.Symbol["GUID"]))
                    {
                        allClientReturnStatue.Add(rs.Symbol["GUID"], ReturnStatueList.无请求);
                        allClientRequestStatue.Add(rs.Symbol["GUID"], SSLockClient.SetStatueList.无动作);
                        Queue.Init(0, rs.Symbol["GUID"]);
                    }
                    if (!rs.Symbol.ContainsKey("Code"))
                    {
                        All.Class.Error.Add("SSLockMain接收的字符串没有Code命令码");
                        return;
                    }

                    allClientRequestStatue[rs.Symbol["GUID"]] = (SSLockClient.SetStatueList)rs.Symbol["Code"].ToInt();

                    rr.Result = true;
                    rr.Value = new List<int>() { (int)allClientReturnStatue[rs.Symbol["GUID"]] };
                    Dictionary<string, string> parm = new Dictionary<string, string>();
                    parm.Add("RemotIP", reciveArgs.RemotIP);
                    parm.Add("RemotPort", reciveArgs.RemotPort.ToString());
                    parent.Send<byte[]>(rr.GetBytes<int>(), parm);
                }
            }
        }
        /// <summary>
        /// 处理各种请求
        /// </summary>
        private void Loop()
        {
            lock (lockObject)
            {
                //检测分站状态的合法性
                allClientRequestStatue.Keys.ToList().ForEach(key =>
                    {
                        switch (allClientRequestStatue[key])
                        {
                            case SSLockClient.SetStatueList.开始测试:
                                if (Queue.GetCount(0) > 0 && Queue[0] != key)//如果分站不是第一个的话,状态不能为开始测试,
                                {
                                    allClientRequestStatue[key] = SSLockClient.SetStatueList.删除请求;//错误状态的请求,直接删除
                                }
                                break;
                        }
                    });
                //确定返回分站的状态
                allClientRequestStatue.Keys.ToList().ForEach(key =>
                    {
                        switch (allClientRequestStatue[key])
                        {
                            case SSLockClient.SetStatueList.无动作:
                                if (Queue.GetCurrent(key) != key)//排在后面的可以先删除掉
                                {
                                    Queue.Remove(key);
                                }
                                allClientReturnStatue[key] = ReturnStatueList.无请求;
                                break;
                            case SSLockClient.SetStatueList.请求测试:
                                Queue.Request(key);
                                if (allClientReturnStatue[key] == ReturnStatueList.无请求)
                                {
                                    allClientReturnStatue[key] = ReturnStatueList.等待;
                                }
                                break;
                            case SSLockClient.SetStatueList.开始测试:
                                break;
                        }
                    });
                //确定主机应该如何动作
                if (Queue.GetCount(0) > 0 )
                {
                    currentGUID = Queue[0];
                    if ((int)allClientRequestStatue[currentGUID] == (int)SSLockClient.SetStatueList.无动作)
                    {
                        ResetMain();
                        return;
                    }
                    switch (currentGetStatue)//主机当前状态
                    {
                        case GetStatueList.无请求:
                            switch (allClientRequestStatue[currentGUID])//分机请求状态
                            {
                                case SSLockClient.SetStatueList.请求测试:
                                    if ((int)currentGetStatue != (int)GetStatueList.等待主机执行互锁前切换动作)
                                    {
                                        All.Class.Log.Add("SSLockMain.状态->等待主机执行互锁前切换动作");
                                    }
                                    currentGetStatue = GetStatueList.等待主机执行互锁前切换动作;
                                    if (NeedMainSwitch != null)
                                    {
                                        NeedMainSwitch(this,new EventArgs());
                                    }
                                    break;
                                default:
                                    All.Class.Error.Add(string.Format("动作不能跳跃式完成,当前主机状态只能接收[请求测试]状态,实际接收[{0}]状态,请检查客户端发送状态逻辑", allClientRequestStatue[currentGUID]));
                                    break;
                            }
                            break;
                        case GetStatueList.等待主机执行互锁前切换动作:
                            switch (allClientRequestStatue[currentGUID])
                            {
                                case SSLockClient.SetStatueList.开始测试:
                                    if ((int)currentGetStatue != (int)GetStatueList.等待执行互锁动作)
                                    {
                                        All.Class.Log.Add("SSLockMain.状态->等待执行互锁动作");
                                    }
                                    currentGetStatue = GetStatueList.等待执行互锁动作;
                                    if (NeedMainStart != null)
                                    {
                                        NeedMainStart(this,new EventArgs());
                                    }
                                    break;
                                case SSLockClient.SetStatueList.请求测试:
                                    break;
                                default:
                                    All.Class.Error.Add(string.Format("动作不能跳跃式完成,当前主机状态只能接收[开始测试]状态,实际接收[{0}]状态,请检查客户端发送状态逻辑",allClientRequestStatue[currentGUID]));
                                    break;
                            }
                            break;
                        case GetStatueList.等待执行互锁动作:
                            break;
                    }
                }
                else
                {
                    ResetMain();
                }
            }
        }
        /// <summary>
        /// 主机操作复位
        /// </summary>
        private void ResetMain()
        {
            if (currentGUID != "")
            {
                Queue.Remove(currentGUID);
                currentGUID = "";
                if (currentGetStatue != GetStatueList.无请求 && NeedMainReset != null)
                {
                    NeedMainReset(this, new EventArgs());
                }
                currentGetStatue = GetStatueList.无请求;
            }
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="statue"></param>
        /// <returns></returns>
        public bool Write(SetStatueList statue)
        {
            List<int> value = new List<int>();
            value.Add((int)statue);
            return WriteInternal<int>(value, null);
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            Loop();
            value = new List<T>();
            switch (All.Class.TypeUse.GetType<T>())
            {
                case Class.TypeUse.TypeList.Boolean:
                case Class.TypeUse.TypeList.DateTime:
                case Class.TypeUse.TypeList.String:
                case Class.TypeUse.TypeList.Double:
                case Class.TypeUse.TypeList.Float:
                    All.Class.Error.Add("SSLockMain,无法读取当前指定类型数据");
                    return false;
                case Class.TypeUse.TypeList.Byte:
                    value.Add((T)(object)(byte)(int)currentGetStatue);
                    break;
                case Class.TypeUse.TypeList.Int:
                    value.Add((T)(object)(int)currentGetStatue);
                    break;
                case Class.TypeUse.TypeList.Long:
                    value.Add((T)(object)(long)(int)currentGetStatue);
                    break;
                case Class.TypeUse.TypeList.UShort:
                    value.Add((T)(object)(ushort)(int)currentGetStatue);
                    break;
            }
            return true;
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            try
            {
                if (value == null || value.Count <= 0)
                {
                    All.Class.Error.Add("SSLockMain,当前不包含写入的数据");
                    return false;
                }
                int tmpValue = 0;
                switch (All.Class.TypeUse.GetType<T>())
                {
                    case Class.TypeUse.TypeList.Int:
                        tmpValue = (int)(object)value[0];
                        break;
                    case Class.TypeUse.TypeList.Long:
                        tmpValue = (int)(long)(object)value[0];
                        break;
                    case Class.TypeUse.TypeList.Byte:
                        tmpValue = (int)(byte)(object)value[0];
                        break;
                    case Class.TypeUse.TypeList.UShort:
                        tmpValue = (int)(ushort)(object)value[0];
                        break;
                    case Class.TypeUse.TypeList.String:
                        tmpValue = Enum.GetNames(typeof(SetStatueList)).ToList().IndexOf((string)(object)value[0]);
                        return false;
                    default:
                        All.Class.Error.Add(string.Format("SSLockMain,不支持当前的数据类型,{0}", typeof(T)));
                        return false;
                }
                if (tmpValue < 0 || tmpValue >= Enum.GetNames(typeof(SetStatueList)).Length)
                {
                    All.Class.Error.Add(string.Format("SSLockMain,不包含当前可写入的执行命令,{0}", value[0]));
                    return false;
                }
                All.Class.Log.Add(string.Format("SSLockMain,开始执行指定命令,{0}", (SetStatueList)tmpValue));
                if (Queue[0] == currentGUID)
                {
                    switch ((SetStatueList)tmpValue)
                    {
                        case SetStatueList.互锁切换执行完毕:
                            allClientReturnStatue[currentGUID] = ReturnStatueList.允许;
                            break;
                        case SetStatueList.互锁操作执行完毕:
                            allClientReturnStatue[currentGUID] = ReturnStatueList.测试完成;
                            break;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                All.Class.Error.Add(e);
                return false;
            }
        }
        public override bool Test()
        {
            return base.Test();
        }
    }
}
