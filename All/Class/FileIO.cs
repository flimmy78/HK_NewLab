using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using All.Data;
namespace All.Class
{
    public static class FileIO
    {
        /// <summary>
        /// 获取当前程序工作目录
        /// </summary>
        public static string NowPath
        {
            get
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
        }
        static object lockObject = new object();
        /// <summary>
        /// 检测文件夹的路径是否存在，不存在则新建
        /// </summary>
        /// <param name="directory"></param>
        public static void CheckDirectory(string directory)
        {
            string[] dir = directory.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (dir.Length == 1)
            {
                return;
            }
            else
            {
                string tmpDir = "";
                for (int i = 0; i < dir.Length - 1; i++)
                {
                    tmpDir = string.Format("{0}{1}\\", tmpDir, dir[i]);
                }
                CheckDirectory(tmpDir);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }
        /// <summary>
        /// 检测文件的路径是否存在，不存在则新建
        /// </summary>
        /// <param name="file"></param>
        public static void CheckFileDirectory(string file)
        {
            CheckDirectory(file.Substring(0, file.LastIndexOf('\\')));
        }
        /// <summary>
        /// 读取文本字节
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] Read(string fileName)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate))
            {
                byte[] buff = new byte[fs.Length];
                fs.Read(buff, 0, buff.Length);
                fs.Flush();
                return buff;
            }
        }
        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFile(string fileName, Encoding encod)
        {
            byte[] buff = Read(fileName);
            if (buff != null && buff.Length > 3//这个是UTF8的头文件
                && buff[0] == 0xEF
                && buff[1] == 0xBB
                && buff[2] == 0xBF)
            {
                return Encoding.UTF8.GetString(buff, 3, buff.Length - 3);
            }
            return encod.GetString(buff);
        }
        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFile(string fileName)
        {
            return ReadFile(fileName, Encoding.UTF8);
        }
        /// <summary>
        /// 将数据写入文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buff"></param>
        /// <param name="fm"></param>
        public static void Write(string fileName, byte[] buff, FileMode fileMode)
        {
            lock (lockObject)
            {
                using (FileStream fs = new FileStream(fileName, fileMode))
                {
                    fs.Write(buff, 0, buff.Length);
                    fs.Flush();
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 将数据写入文件 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buff"></param>
        public static void Write(string fileName, byte[] buff)
        {
            Write(fileName, buff, FileMode.Create);
        }
        /// <summary>
        /// 将数据写入指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public static void Write(string fileName, string value, FileMode fm)
        {
            Write(fileName, Encoding.UTF8.GetBytes(value), fm);
        }
        /// <summary>
        /// 将数据写入指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public static void Write(string fileName, string value)
        {
            Write(fileName, value, FileMode.Create);
        }
        /// <summary>
        /// 将指定文字写入到文件,并换行
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public static void WriteLine(string fileName, string value)
        {
            Write(fileName, string.Format("{0}\r\n", value));
        }
        /// <summary>
        /// 将指定文字写入到文件,并换行,且添加日期
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public static void WriteDate(string fileName, string value)
        {
            Write(fileName, string.Format("{0:yyyy-MM-dd HH:mm:ss}->{1}\r\n", DateTime.Now, value));
        }
        /// <summary>
        /// 获取指定文件的版本,Exe,Dll等执行文件取版本号,Txt,Mdb等数据文件取Hash值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileCode(string fileName, string NullValue)
        {
            string result = NullValue;
            if (System.IO.File.Exists(fileName))
            {
                lock (lockObject)
                {
                    System.Diagnostics.FileVersionInfo fi = System.Diagnostics.FileVersionInfo.GetVersionInfo(fileName);
                    result = fi.FileVersion;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取文件Hash值,如果文件正在使用中,则会报错
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="NullValue"></param>
        /// <returns></returns>
        public static string GetFileHash(string fileName, string NullValue)
        {
            string result = NullValue;
            if (System.IO.File.Exists(fileName))
            {
                lock (lockObject)
                {
                    try
                    {
                        using (System.Security.Cryptography.HashAlgorithm ah = System.Security.Cryptography.HashAlgorithm.Create())
                        {
                            using (FileStream fs = new FileStream(fileName, FileMode.Open))
                            {
                                result = All.Class.Num.Hex2Str(ah.ComputeHash(fs));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        All.Class.Error.Add(e);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取文件的MD5值.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="NullValue"></param>
        /// <returns></returns>
        public static string GetFileMD5(string fileName, string NullValue)
        {
            string result = NullValue;
            if (System.IO.File.Exists(fileName))
            {
                lock (lockObject)
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open))
                    {
                        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                        result = All.Class.Num.Hex2Str(md5.ComputeHash(fs));
                    }
                }
            }
            return result;
        }
        /// <summary>   
        /// 取得一个文本文件的编码方式。如果无法在文件头部找到有效的前导符，Encoding.Default将被返回。   
        /// </summary>   
        /// <param name="fileName">文件名。</param>   
        /// <returns></returns>   
        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }
        /// <summary>   
        /// 取得一个文本文件流的编码方式。   
        /// </summary>   
        /// <param name="stream">文本文件流。</param>   
        /// <returns></returns>   
        public static Encoding GetEncoding(FileStream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }
        /// <summary>   
        /// 取得一个文本文件的编码方式。   
        /// </summary>   
        /// <param name="fileName">文件名。</param>   
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>   
        /// <returns></returns>   
        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Encoding targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            return targetEncoding;
        }
        /// <summary>   
        /// 取得一个文本文件流的编码方式。   
        /// </summary>   
        /// <param name="stream">文本文件流。</param>   
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>   
        /// <returns></returns>   
        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节   
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置   
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }
                //根据文件流的前4个字节判断Encoding   
                //Unicode {0xFF, 0xFE};   
                //BE-Unicode {0xFE, 0xFF};   
                //UTF8 = {0xEF, 0xBB, 0xBF};   
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe   
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode   
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8   
                {
                    targetEncoding = Encoding.UTF8;
                }
                //恢复Seek位置         
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }
        /// <summary>   
        /// 通过给定的文件流，判断文件的编码类型   
        /// </summary>   
        /// <param name="fs">文件流</param>   
        /// <returns>文件的编码类型</returns>   
        public static System.Text.Encoding GetEncoding(Stream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM   
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            byte[] ss = r.ReadBytes(4);
            if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            else
            {
                if (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)
                {
                    reVal = Encoding.UTF8;
                }
                else
                {
                    int i;
                    int.TryParse(fs.Length.ToString(), out i);
                    ss = r.ReadBytes(i);

                    if (IsUTF8Bytes(ss))
                        reVal = Encoding.UTF8;
                }
            }
            r.Close();
            return reVal;

        }

        /// <summary>   
        /// 判断是否是不带 BOM 的 UTF8 格式   
        /// </summary>   
        /// <param name="data"></param>   
        /// <returns></returns>   
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;　 //计算当前正分析的字符应还有的字节数   
            byte curByte; //当前分析的字节.   
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前   
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　   
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1   
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式!");
            }
            return true;
        }
    }
}
