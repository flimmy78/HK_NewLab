using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
namespace All.Communicate
{
    /// <summary>
    /// 自动读取数据
    /// </summary>
    public class AutoReadAndWrite
    {
        /// <summary>
        /// 设置文本示例
        /// </summary>
        public const string ExampleXmlSet =
"<?xml version=\"1.0\" encoding=\"GB2312\" ?>" +
"<AllCommunication>" +
"  <!--此节点下所有数据都为通讯部件-->" +
"  <Commun Class=\"All.Communicate.Udp\" LocalPort=\"3000\" Text=\"8#固定风机通讯端口\">" +
"     <Meter Class=\"All.Meter.SSRead\" Text=\"8#固定风机\"  FlushTick=\"100\" TimeOut=\"1000\" String=\"7\" Float=\"1\">" +
"     	<Read>" +
"			<Value1>" +
"				<Text>工位[0->5]条码,故障码</Text>" +
"				<Data>string</Data>" +
"				<Start>0</Start>" +
"				<End>6</End>" +
"				<Index>[51->53],84,[86->88]</Index>" +
"			</Value1>" +
"			<Value2>" +
"				<Text>8#通讯</Text>" +
"				<Data>float</Data>" +
"				<Start>0</Start>" +
"				<End>0</End>" +
"				<Index>7</Index>" +
"			</Value2>" +
"		</Read>" +
"	</Meter>" +
" </Commun>" +
"</AllCommunication>";
        List<Thread> AllThread = new List<Thread>();
        /// <summary>
        /// 所有读取数据
        /// </summary>
        public Data.AllData Datas
        { get; set; }
        /// <summary>
        /// 所有通讯类
        /// </summary>
        public Data.CommunicateStyleCollection Communicates
        { get; set; }
        bool exit = false;
        public AutoReadAndWrite()
        {
            this.Datas = new Data.AllData();
            this.Communicates = new Data.CommunicateStyleCollection();
        }
        /// <summary>
        /// 加载默认位置文件,从中反射类
        /// </summary>
        public void Load()
        {
            this.Load("\\Data\\Meter.xml");
        }
        /// <summary>
        /// 加载所有读取类
        /// </summary>
        /// <returns></returns>
        public void Load(string fileName)
        {
            LoadFromXml(fileName);
            InitData();
            StartRead();
        }
        /// <summary>
        /// 关闭所有读取数据
        /// </summary>
        public void Close()
        {
            if (AllThread == null || AllThread.Count <= 0)
            {
                return;
            }
            exit = true;
            new Thread(() =>
            {
                Thread.CurrentThread.Join(1000);//等待1s后,强制关闭所有线程
                AllThread.ForEach(t =>
                {
                    if (t != null || t.ThreadState != ThreadState.Stopped)
                    {
                        t.Abort();
                    }
                });
            })
            {
                IsBackground = true
            }.Start();
        }
        ~AutoReadAndWrite()
        {
            this.Close();
        }
        /// <summary>
        /// 开始自动读取数据
        /// </summary>
        private void StartRead()
        {
            if (Communicates == null || Communicates.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < Communicates.Count; i++)
            {
                int index = i;
                Thread tmp = new Thread(() => Loop(index));
                AllThread.Add(tmp);
            }
            for (int i = 0; i < AllThread.Count; i++)
            {
                AllThread[i].IsBackground = true;
                AllThread[i].Start();
            }
        }
        /// <summary>
        /// 循环读取数据
        /// </summary>
        /// <param name="index"></param>
        private void Loop(int index)
        {
            if (Communicates[index].Meters == null || Communicates[index].Meters.Count <= 0)
            {
                return;
            }
            Dictionary<int, object> value = new Dictionary<int, object>();
            int[] start = new int[Communicates[index].Meters.Count];
            for (int i = 0; i < Communicates[index].Meters.Count; i++)
            {
                Communicates[index].Meters[i].Value.Init(Communicates[index].Meters[i].Value.InitParm);

                start[i] = Environment.TickCount;
            }
            while (!exit)
            {
                Thread.Sleep(25);
                for (int i = 0; i < Communicates[index].Meters.Count; i++)
                {
                    //没东西可读的设备,或没有打开的端口
                    if (!Communicates[index].Value.IsOpen ||
                        Communicates[index].Meters[i].Reads == null ||
                        Communicates[index].Meters[i].Reads.Count <= 0)
                    {
                        continue;
                    }
                    //刷新时间还没到
                    if ((Environment.TickCount - start[i]) < Communicates[index].Meters[i].Value.FlushTick)
                    {
                        continue;
                    }
                    start[i] = Environment.TickCount;
                    for (int j = 0; j < Communicates[index].Meters[i].Reads.Count; j++)
                    {
                        if (Communicates[index].Meters[i].Read(j, out value))
                        {
                            Datas.SetValue(Communicates[index].Meters[i].Reads[j].ReadType, value);
                        }
                    }
                }
            }
            for (int i = 0; i < Communicates[index].Meters.Count; i++)
            {
                Communicates[index].Meters[i].Value.Close();
            }
            Communicates[index].Value.Close();
        }
        /// <summary>
        /// 初始化各类型数据到总数据列表
        /// </summary>
        /// <returns></returns>
        private void InitData()
        {
            if (Communicates == null || Communicates.Count <= 0)
            {
                All.Class.Error.Add("没有通讯类须要读取");
                return;
            }
            for (int i = 0; i < Communicates.Count; i++)
            {
                if (Communicates[i].Meters == null || Communicates[i].Meters.Count <= 0)//父通讯类下没有设备,则继续
                {
                    All.Class.Error.Add(string.Format("{0},没有通讯设备", Communicates[i].Value.Text));
                    continue;
                }
                for (int j = 0; j < Communicates[i].Meters.Count; j++)
                {
                    if (Communicates[i].Meters[j].Reads == null)//有通讯设备,但不读取数据,则继续
                    {
                        continue;
                    }
                    for (int k = 0; k < Communicates[i].Meters[j].Reads.Count; k++)
                    {
                        Communicates[i].Meters[j].Reads[k].Analysis();
                        Datas.AddRange(Communicates[i].Meters[j].Reads[k].ReadType,
                            Communicates[i].Meters[j].Reads[k].Datas);
                    }
                }
            }
        }
        /// <summary>
        /// 从XML文件读取设置
        /// </summary>
        private void LoadFromXml(string fileName)
        {
            XmlNode tmpXml = Class.XmlHelp.GetXmlNode(string.Format("{0}\\{1}", Class.FileIO.NowPath,fileName));
            if (tmpXml == null)
            {
                All.Class.Error.Add("加载错误", "从MeterConnect.xml加载文件读取数据失败,不能读取数据");
                return;
            }
            Data.CommunicateStyle tmpCommunicateStyle;
            Data.MeterStyle tmpMeterStyle;
            All.Communicate.Data.ReadDetial tmpReadDetial;
            foreach (XmlNode tmpConnectNode in tmpXml.ChildNodes)//取所有Connect
            {
                if (tmpConnectNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                //反射通讯类
                tmpCommunicateStyle = new Data.CommunicateStyle();
                tmpCommunicateStyle.Value = All.Communicate.Communicate.Parse(Class.XmlHelp.GetAttribute(tmpConnectNode));
                if (tmpCommunicateStyle.Value == null)
                {
                    continue;
                }
                foreach (XmlNode tmpMeterNode in tmpConnectNode.ChildNodes)//获取所有设备
                {
                    if (tmpMeterNode.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }
                    //反射设备类
                    tmpMeterStyle = new Data.MeterStyle();
                    tmpMeterStyle.Value = All.Meter.Meter.Parse(Class.XmlHelp.GetAttribute(tmpMeterNode));
                    if (tmpMeterStyle.Value == null)//解析不出设备类型
                    {
                        continue;
                    }
                    tmpMeterStyle.Value.Parent = tmpCommunicateStyle.Value;
                    foreach (XmlNode tmpReadAndWriteNode in tmpMeterNode.ChildNodes)
                    {
                        if (tmpReadAndWriteNode.NodeType != XmlNodeType.Element)//不是数据标签则继续
                        {
                            continue;
                        }
                        if (tmpReadAndWriteNode.LocalName.ToUpper() == "WRITE")//常写标签,暂时没有
                        { 
                        }
                        if (tmpReadAndWriteNode.LocalName.ToUpper() == "READ")//获取所有读取节点
                        {
                            foreach (XmlNode tmpValue in tmpReadAndWriteNode.ChildNodes)
                            {
                                if (tmpValue.NodeType != XmlNodeType.Element)//排除其他辅助标签
                                {
                                    continue;
                                }
                                tmpReadDetial = new All.Communicate.Data.ReadDetial(Class.XmlHelp.GetInner(tmpValue));
                                tmpReadDetial.Parent = tmpMeterStyle;
                                tmpMeterStyle.Reads.Add(tmpReadDetial);
                            }
                        }
                    }
                    tmpCommunicateStyle.Meters.Add(tmpMeterStyle);
                }
                Communicates.Add(tmpCommunicateStyle);
            }
        }
    }
}
