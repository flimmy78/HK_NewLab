using System;
using System.Collections.Generic;
using System.Text;

namespace All.Communicate.Data
{
    public class ReadDetial
    {
        #region//属性
        /// <summary>
        /// 解析后的读取内容值0:M0,1:M1,2:M2,4:M4这种东西
        /// </summary>
        public Dictionary<int, string> Datas
        { get; set; }
        /// <summary>
        /// 读取内容的原始buff,即<Index>1,2,3,4</Index>,<Text>M0,M1,M2</Text>这种东西
        /// </summary>
        public Dictionary<string, string> Values
        { get; set; }
        /// <summary>
        /// 包含此读取块的设备
        /// </summary>
        public MeterStyle Parent
        { get; set; }
        All.Class.TypeUse.TypeList readType = All.Class.TypeUse.TypeList.UnKnow;
        /// <summary>
        /// 当前读取类型
        /// </summary>
        public All.Class.TypeUse.TypeList ReadType
        {
            get { return readType; }
        }
        #endregion
        #region//构造函数
        /// <summary>
        /// 读取数据buff
        /// </summary>
        public ReadDetial()
            : this(new Dictionary<string, string>())
        {
        }
        /// <summary>
        /// 读取数据buff
        /// </summary>
        /// <param name="value"></param>
        public ReadDetial(Dictionary<string, string> value)
        {
            this.Values = value;
            this.Datas = new Dictionary<int, string>();
        }
        #endregion
        /// <summary>
        /// 从初始化值中解析须要的数据类型,以及读取值
        /// </summary>
        public void Analysis()
        {
            if (Parent == null || Values == null || !Values.ContainsKey("Data"))
            {
                return;
            }
            if (!Values.ContainsKey("Text") || !Values.ContainsKey("Index"))
            {
                All.Class.Error.Add(string.Format("{0}.{1},没有数据类型或数据序号", Parent.Value.Parent.Text, Parent.Value.Text));
            }
            readType = All.Class.TypeUse.GetType(Values["Data"]);
            this.Datas = GetIndexFromSet(Values["Index"], Values["Text"]);
        }
        /// <summary>
        /// 从字符串中,解析出下标,可以解析1,2,3,4这种以','号分隔,或者'数据名[0->9]'这种格式的数据名
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Dictionary<int, string> GetIndexFromSet(string index, string text)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<int> indexs = new List<int>();
            List<string> texts = new List<string>();

            string[] tmpBuff = index.Split(',');
            string[] tmp;

            if (tmpBuff == null || tmpBuff.Length == 0)
            {
                All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列为空");
                All.Class.Error.Add("数据列值", index);
                return result;
            }
            int start = 0, end = 0;
            for (int i = 0; i < tmpBuff.Length; i++)
            {
                tmp = tmpBuff[i].Split(new string[] { "->", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
                switch (tmp.Length)
                {
                    case 0:
                        break;
                    case 1:
                        indexs.Add(tmp[0].ToInt());
                        break;
                    case 2:
                        if (!All.Class.Check.isFix(tmp[0], Class.Check.RegularList.整数) ||
                            !All.Class.Check.isFix(tmp[1], Class.Check.RegularList.整数))
                        {
                            All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号中数值不能转化为整数");
                            All.Class.Error.Add("数据列值", index);
                        }
                        start = tmp[0].ToInt();
                        end = tmp[1].ToInt();
                        if (start > end)
                        {
                            All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列起始值大于结果值");
                            All.Class.Error.Add("数据列值", index);
                            return result;
                        }
                        for (int j = start; j <= end; j++)
                        {
                            indexs.Add(j);
                        }
                        break;
                }
            }
            tmpBuff = text.Split(',');
            if (tmpBuff == null || tmpBuff.Length == 0)
            {
                All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中名称列为空");
                All.Class.Error.Add("数据名值", text);
                return result;
            }
            string[] temp;
            int[] tempIndex;
            for (int i = 0; i < tmpBuff.Length; i++)
            {
                tmp = tmpBuff[i].Split(new string[] { "->", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp.Length == 0 || tmp.Length > 4)//最多为这种类型  ***[xx->xx]***
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet标志字符过多出现,只能有'->','['或']'");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                temp = new string[] { "", "", "", "" };
                tempIndex = new int[] { tmpBuff[i].IndexOf("->"), tmpBuff[i].IndexOf("["), tmpBuff[i].IndexOf("]") };
                if ((tempIndex[1] >= 0 && tempIndex[2] < 0) ||
                    (tempIndex[1] < 0 && tempIndex[2] >= 0))
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet单独出现了标志字符'['或']'");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                if ((tempIndex[1] >= 0 || tempIndex[2] >= 0) &&
                    (tempIndex[1] > tempIndex[0] ||
                    tempIndex[0] > tempIndex[2]))
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet出现错误标志符,必须为 ***[xx->xx]***这种");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                if (tempIndex[0] < 0)//没有->
                {
                    texts.Add(tmp[0]);
                    continue;
                }
                if (tempIndex[1] < 0 && tempIndex[2] < 0)//有->,没有[,]
                {
                    if (tmp.Length >= 3)
                    {
                        All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet出现不对等数据");
                        All.Class.Error.Add("数据名值", text);
                        continue;
                    }
                    temp[1] = tmp[0];//把数据放入temp格式中
                    temp[2] = tmp[1];
                }
                if (tempIndex[1] >= 0 && tempIndex[2] >= 0)//都有 '->','['和']'三个符号
                {
                    temp[0] = tmpBuff[i].Substring(0, tempIndex[1]);
                    temp[1] = tmpBuff[i].Substring(tempIndex[1] + 1, tempIndex[0] - tempIndex[1] - 1);
                    temp[2] = tmpBuff[i].Substring(tempIndex[0] + 2, tempIndex[2] - tempIndex[0] - 2);
                    temp[3] = tmpBuff[i].Substring(tempIndex[2] + 1);
                }
                if (!All.Class.Check.isFix(temp[1], Class.Check.RegularList.整数) ||
                    !All.Class.Check.isFix(temp[2], Class.Check.RegularList.整数))
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号中数值不能转化为整数");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                start = temp[1].ToInt();
                end = temp[2].ToInt();
                if (start > end)
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列起始值大于结果值");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                for (int j = start; j <= end; j++)
                {
                    texts.Add(string.Format("{0}{1}{2}", temp[0], j, temp[3]));
                }
            }
            if (indexs.Count == texts.Count)
            {
                for (int i = 0; i < indexs.Count; i++)
                {
                    if (result.ContainsKey(indexs[i]))
                    {
                        All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列出现重复的数据列值");
                        All.Class.Error.Add("数据列值", index);
                        continue;
                    }
                    result.Add(indexs[i], texts[i]);
                }
            }
            else
            {
                All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列和名称列数量不等");
                All.Class.Error.Add("数据列值", index);
                All.Class.Error.Add("数据名值", text);
            }
            return result;
        }
    }
}
