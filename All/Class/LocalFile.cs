using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Class
{
    public class LocalFile
    {
        /// <summary>
        /// 从文件加载数据
        /// </summary>
        public void Load(string fileName)
        {
            Dictionary<string, string> buff = SingleFileSave.SSFile.Text2Dictionary(FileIO.ReadFile(fileName));
        }
    }
}
