using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class SSLockClient:Meter
    {
        string clientGuid = All.Class.Num.CreateGUID();
        string threadGuid = All.Class.Num.CreateGUID();
        public enum SetStatueList : int
        {
            无动作 = 0,
            删除请求=0,
            请求测试,
            开始测试
        }
        public enum GetStatueList : int
        {
            无状态 = 0,
            等待执行,
            允许执行,
        }
        /// <summary>
        /// 主机已执行互锁切换完毕,等待分站切换通道,如果没有通道切换,则直接发送[开始测试]状态
        /// </summary>
        public event EventHandler NeedClientSwitch;
        /// <summary>
        /// 主机已执行互锁动作完毕,等待分站关闭通道
        /// </summary>
        public event EventHandler NeedClientReset;

        GetStatueList currentGetStatue = GetStatueList.无状态;
        SetStatueList currentSetStatue = SetStatueList.无动作;
        Dictionary<string, string> initParm = new Dictionary<string, string>();

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            this.TimeOut = 200;
            this.FlushTick = 100;
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
            All.Class.Thread.CreateOrOpen(threadGuid, UpdateStatue, 500);
        }
        /// <summary>
        /// 定时发送数据给主机来查询状态
        /// </summary>
        private void UpdateStatue()
        {
            Loop();
        }
        private bool Loop()
        {
            try
            {
                lock (lockObject)
                {
                    Dictionary<string, string> buff = new Dictionary<string, string>();
                    buff.Add("Code", string.Format("{0}", (int)currentSetStatue));
                    buff.Add("GUID", clientGuid);
                    SSMeter.SSReadMeter.ReadDataStyle rd = new SSMeter.SSReadMeter.ReadDataStyle(Class.TypeUse.TypeList.Int, buff);
                    byte[] readBuff;
                    if (WriteAndRead<byte[], byte[]>(rd.GetBytes(), 7, out readBuff))
                    {
                        SSMeter.SSReadMeter.ReadResultStyle rs = SSMeter.SSReadMeter.ReadResultStyle.Parse(readBuff);
                        if (rs == null || rs.Random != rd.Random || !rs.Result)
                        {
                            All.Class.Error.Add("SSLockClient.Loop无校验或校验不通过");
                            return false;
                        }
                        List<int> result = (List<int>)rs.Value;
                        if (result[0] < 0 || result[0] >= Enum.GetNames(typeof(GetStatueList)).Length)
                        {
                            All.Class.Error.Add("SSLockClient.Loop无效的命令");
                            return false;
                        }
                        switch ((SSLockMain.ReturnStatueList)result[0])
                        {
                            case SSLockMain.ReturnStatueList.无请求:
                                if (currentGetStatue != GetStatueList.无状态 && NeedClientReset != null)
                                {
                                    NeedClientReset(this,new EventArgs());
                                }
                                currentGetStatue = GetStatueList.无状态;
                                break;
                            case SSLockMain.ReturnStatueList.允许:
                                if (currentGetStatue != GetStatueList.允许执行 && NeedClientSwitch!=null)
                                {
                                    NeedClientSwitch(this,new EventArgs());
                                }
                                currentGetStatue = GetStatueList.允许执行;
                                break;
                            case SSLockMain.ReturnStatueList.等待:
                                currentGetStatue = GetStatueList.等待执行;
                                break;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                All.Class.Error.Add(e);
                return false;
            }
            return true;
        }
        ~SSLockClient()
        {
            All.Class.Thread.Kill(threadGuid);
            this.Close();
        }
        public override void Close()
        {
            if (!WriteInternal<int>((int)SetStatueList.删除请求, 0) && !WriteInternal<int>((int)SetStatueList.删除请求, 0))
            {
                All.Class.Error.Add("SSLockClient:删除远程连接请求失败");
            }
            base.Close();
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            value = new List<T>();
            switch (All.Class.TypeUse.GetType<T>())
            {
                case Class.TypeUse.TypeList.Boolean:
                case Class.TypeUse.TypeList.DateTime:
                case Class.TypeUse.TypeList.String:
                case Class.TypeUse.TypeList.Double:
                case Class.TypeUse.TypeList.Float:
                    All.Class.Error.Add("SSLockClient,无法读取当前指定类型数据");
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
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            try
            {
                lock (lockObject)
                {
                    if (value == null || value.Count <= 0)
                    {
                        All.Class.Error.Add("SSLockClient,当前不包含写入的数据");
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
                            All.Class.Error.Add(string.Format("SSLockClient,不支持当前的数据类型,{0}", typeof(T)));
                            return false;
                    }
                    if (tmpValue < 0 || tmpValue >= Enum.GetNames(typeof(SetStatueList)).Length)
                    {
                        All.Class.Error.Add(string.Format("SSLockClient,不包含当前可写入的执行命令,{0}", value[0]));
                        return false;
                    }
                    currentSetStatue = (SetStatueList)tmpValue;
                    return Loop();
                }
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
