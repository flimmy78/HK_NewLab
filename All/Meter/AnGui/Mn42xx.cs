using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class Mn42xx : AnGui.AnGui
    {
        Dictionary<string, string> initParm;
        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            value = new List<T>();
            return false;
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                try
                {
                    if (!parm.ContainsKey("Code"))
                    {
                        All.Class.Error.Add("数据写入参数中不包含命令", Environment.StackTrace);
                        return false;
                    }
                    //bool goon = false;
                    //if (parm.ContainsKey("Goon"))
                    //{
                    //    goon = parm["Goon"].ToBool();
                    //}
                    //int group = 1;
                    //if (parm.ContainsKey("Group"))
                    //{
                    //    group = parm["Group"].ToInt();
                    //}
                    string sendValue = "";
                    string readValue="";
                    switch (parm["Code"].ToUpper())
                    {
                        case "START":
                            sendValue = "#G";
                            break;
                        case "STOP":
                            sendValue = "#U";
                            break;
                        case "SETTING":
                            throw new Exception("此处须添加对应方法");
                    }
                    if (WriteAndRead<string, string>(sendValue, 3, out readValue))
                    {
                        if (readValue.Trim().ToUpper() != "Y")
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                    return false;
                }
                return result;
            }
        }
    }
}
