using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using System.Windows;
using System.Text.RegularExpressions;

namespace jupyter.util
{
    internal abstract class MyDataClass // 抽象类，不支持实例化
    {
        public static string Instance { get; set; }
        public static string Connected_Ins { get; set; }

        [Serializable]
        public class Serial_PublicDataClass
        {
            private List<string> _元氏表;
            public List<string> 元氏表
            {
                get
                {
                    return _元氏表;
                }
                set
                {
                    _元氏表 = value;
                }
            }
            /// <summary>
            /// 储存反序列化时候的溢出数据
            /// </summary>
            [JsonExtensionData]
            public Dictionary<string, JsonElement> ExtensionData { get; set; }
        }

        public static object? SerialReader_json(string file)
        {
            string jsonString = File.ReadAllText(file);
            var obj = JsonSerializer.Deserialize<Serial_PublicDataClass>(jsonString, App.jsonSerializerOptions);
            return obj;
        }
        async public static Task SerialWriter_json(string file, object sp)
        {
            using FileStream createStream = File.Create(file);
            await JsonSerializer.SerializeAsync(createStream, sp, App.jsonSerializerOptions);
            await createStream.DisposeAsync();
        }

        public static bool RemoteFileExists(string fileUrl)
        {
            bool result = false;//下载结果

            WebResponse response = null;
            try
            {
                WebRequest req = WebRequest.Create(fileUrl);

                response = req.GetResponse();

                result = response == null ? false : true;

            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }
        public static int Text_Length(string Text)

        {

            int len = 0;

            for (int i = 0; i < Text.Length; i++)

            {

                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));

                if (byte_len.Length > 1)

                    len += 2; //如果长度大于1，是中文，占两个字节，+2

                else

                    len += 1;  //如果长度等于1，是英文，占一个字节，+1

            }

            return len;

        }

        public static int GetSingleLength(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
            return Regex.Replace(input, @"[^\x00-\xff]", "aa").Length;//计算得到该字符串对应单字节字符串的长度
        }
    }
}
