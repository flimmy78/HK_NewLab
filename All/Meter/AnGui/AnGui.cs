using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter.AnGui
{
    public  abstract class AnGui:Meter
    {
        public enum Projects : int
        {
            空白 = 0,
            接地,
            绝缘,
            耐压,
            泄漏,
            功率,
            启动,
            直耐
        }
        public class ReadValue : ICloneable
        {
            /// <summary>
            /// 读取值
            /// </summary>
            public class StepValue:ICloneable
            {
                /// <summary>
                /// 测试项
                /// </summary>
                public Projects Project
                { get; set; }
                /// <summary>
                /// 输出值
                /// </summary>
                public float OutValue
                { get; set; }
                /// <summary>
                /// 测试结果
                /// </summary>
                public float Value
                { get; set; }
                /// <summary>
                /// 结果二号
                /// </summary>
                public float ValueTwo
                { get; set; }
                /// <summary>
                /// 备用结果
                /// </summary>
                public float ValueOther
                { get; set; }
                /// <summary>
                /// 测试时间
                /// </summary>
                public float Time
                { get; set; }
                /// <summary>
                /// 是否动态
                /// </summary>
                public bool Active
                { get; set; }
                /// <summary>
                /// 测试结果
                /// </summary>
                public bool Result
                { get; set; }
                public object Clone()
                {
                    StepValue result = new StepValue();
                    result.Project = this.Project;
                    result.OutValue = this.OutValue;
                    result.Value = this.Value;
                    result.ValueTwo = this.ValueTwo;
                    result.ValueOther = this.ValueOther;
                    result.Time = this.Time;
                    result.Active = this.Active;
                    result.Result = this.Result;
                    return result;
                }
            }
            /// <summary>
            /// 测试步骤数据
            /// </summary>
            public List<StepValue> Value
            { get; set; }
            /// <summary>
            /// 测试总结果
            /// </summary>
            public bool Result
            { get; set; }
            /// <summary>
            /// 测试是否已结束
            /// </summary>
            public bool Over
            { get; set; }
            public ReadValue()
            {
                this.Value = new List<StepValue>();
                Result = true;
                Over = false;
            }
            public object Clone()
            {
                ReadValue result = new ReadValue();
                result.Result = this.Result;
                result.Over = this.Over;
                if (Value != null && Value.Count > 0)
                {
                    Value.ForEach(value =>
                        {
                            this.Value.Add((StepValue)value.Clone());
                        });
                }
                return result;
            }
        }
        public class SendValue
        {
            /// <summary>
            /// 设置值,部分项目没有时,可以不填或随便填
            /// </summary>
            public class StepValue
            {
                /// <summary>
                /// 测试项目
                /// </summary>
                public Projects Project
                { get; set; }
                /// <summary>
                /// 输出值
                /// </summary>
                public float OutValue
                { get; set; }
                /// <summary>
                /// 下限值
                /// </summary>
                public float Down
                { get; set; }
                /// <summary>
                /// 上限值
                /// </summary>
                public float Up
                { get; set; }
                /// <summary>
                /// 测试时间
                /// </summary>
                public float Time
                { get; set; }
                /// <summary>
                /// 动态,部分项目没有
                /// </summary>
                public bool Active
                { get; set; }
                /// <summary>
                /// 频率,部分项目没有
                /// </summary>
                public int Hz
                { get; set; }
                /// <summary>
                /// 缓降,部分项目没有
                /// </summary>
                public int SlowDown
                { get; set; }
                /// <summary>
                /// 缓升,部分项目没有
                /// </summary>
                public int SlowUp
                { get; set; }
                public StepValue()
                {
                    this.Project = Projects.接地;
                    OutValue = 0;
                    Down = 0;
                    Up = 0;
                    Time = 0;
                    Active = false;
                    this.Hz = 50;
                    this.SlowDown = 0;
                    this.SlowUp = 0;
                }
            }
            /// <summary>
            /// 设置值
            /// </summary>
            public List<StepValue> Value
            { get; set; }
            /// <summary>
            /// 不合格是否继续
            /// </summary>
            public bool Goon
            { get; set; }
            /// <summary>
            /// 组号
            /// </summary>
            public int Group
            { get; set; }
            /// <summary>
            /// 频率
            /// </summary>
            public int Hz
            { get; set; }
            public SendValue()
            {
                Value = new List<StepValue>();
                Goon = true;
                Group = 1;
                Hz = 50;
            }
        }
        /// <summary>
        /// 连接测试
        /// </summary>
        /// <returns></returns>
        public override bool Test()
        {
            return Stop();
        }
        /// <summary>
        /// 启动测试
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            Dictionary<string, string> buff = new Dictionary<string, string>();
            buff.Add("Code", "Start");
            return WriteInternal<bool>(null, buff);
        }
        /// <summary>
        /// 停止测试
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            Dictionary<string, string> buff = new Dictionary<string, string>();
            buff.Add("Code", "Stop");
            return WriteInternal<bool>(null, buff);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reading"></param>
        /// <returns></returns>
        public virtual bool Reading(out ReadValue reading)
        {
            Dictionary<string, string> parm = new Dictionary<string, string>();
            List<float> value = new List<float>();
            parm.Add("Code", "Reading");
            bool result = Read<float>(out value, parm);
            reading = new ReadValue();
            int index = 0;
            //[Project,OutValue,Value,ValueTwo,ValueOther,Time,Active,Result]*N+Result,Over,.....固定格式
            if (result && value != null && value.Count >= 2)
            {
                ReadValue.StepValue tmp;
                for (int i = 0; i < 8 && index < value.Count - 2; i++, index = index + 8)
                {
                    if (value[index] == 0 && value[index + 1] == 0 && value[index + 2] == 0 && value[index + 3] == 0
                        && value[index + 4] == 0 && value[index + 5] == 0 && value[index + 6] == 0 && value[index + 7] == 0)
                    {
                        continue;
                    }
                    tmp = new ReadValue.StepValue();
                    tmp.Project = (Projects)(((int)(float)(object)value[index]) % Enum.GetNames(typeof(Projects)).Length);
                    tmp.OutValue = (float)(object)value[index + 1];
                    tmp.Value = (float)(object)value[index + 2];
                    tmp.ValueTwo = (float)(object)value[index + 3];
                    tmp.ValueOther = (float)(object)value[index + 4];
                    tmp.Time = (float)(object)value[index + 5];
                    tmp.Active = (1 == (int)(float)(object)value[index + 6]);
                    tmp.Result = (1 == (int)(float)(object)value[index + 7]);
                    reading.Value.Add(tmp);
                }
                reading.Over = (1 == (int)(float)(object)value[index]);
                reading.Result = (1 == (int)(float)(object)value[index + 1]);
            }
            return result;
        }
        /// <summary>
        /// 写入设置
        /// </summary>
        /// <param name="buff">包含必须参数,如Goon,NG是否继续,Group,组号等</param>
        /// <returns></returns>
        public virtual bool Setting(SendValue setting)
        {
            if (setting == null || setting.Value == null || setting.Value.Count <= 0)
            {
                return false;
            }
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("Goon", setting.Goon.ToString());
            parm.Add("Group", setting.Group.ToString());
            parm.Add("Code","Setting");//Start=>开始,Stop=>停止,Setting=>设置
            parm.Add("Hz", setting.Hz.ToString());
            List<float> value = new List<float>();
            for (int i = 0; i < setting.Value.Count; i++)
            {
                value.Add((int)setting.Value[i].Project);
                value.Add(setting.Value[i].OutValue);
                value.Add(setting.Value[i].Down);
                value.Add(setting.Value[i].Up);
                value.Add(setting.Value[i].Time);
                value.Add(setting.Value[i].Active ? 1 : 0);
                value.Add(setting.Value[i].Hz);
                value.Add(setting.Value[i].SlowUp);
                value.Add(setting.Value[i].SlowDown);
            }
            return WriteInternal<float>(value, parm);
        }
    }
}
