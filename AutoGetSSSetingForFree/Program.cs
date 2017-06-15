using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using LitJson2;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace AutoGetSSSetingForFree
{
    class Program
    {


        //从免费SS服务器 得到配置 更新到配置中 00:00 12:00各更新一次  最好晚半个小时



        private static string ConfigPath = @"C:\Users\Administrator\Desktop\gui-config.json";
        private static string SSPath = @"C:\Users\Administrator\Desktop\Shadowsocks.exe";


        static void Main(string[] args)
        {
            //下载图片

            KillProcess("Shadowsocks");

            var req = WebRequest.Create("http://get.shadowsocks8.cc/images/server03.png");
            var res = req.GetResponse();
            var s = res.GetResponseStream();
            //todo s有可能为空
            var image = new QRCodeBitmapImage(new System.Drawing.Bitmap(s));
            var qr = new QRCodeDecoder();
            var result = qr.decode(image).Remove(0, 5);

            result = Encoding.UTF8.GetString(Convert.FromBase64String(result));
            //rc4-md5:91447392@106.186.116.88:443


            var configstrs = result.Split('@');





            var configjs = new JsonData();
            configjs["server"] = configstrs[1].Split(':')[0];
            configjs["server_port"] = configstrs[1].Split(':')[1];
            configjs["method"] = configstrs[0].Split(':')[0];
            configjs["password"] = configstrs[0].Split(':')[1];
            //----
            configjs["remarks"] = "";
            configjs["auth"] = false;
            configjs["timeout"] = 5;

            var fs = File.Open(ConfigPath, FileMode.Open);

            var str = new StreamReader(fs).ReadToEnd();

            var js = JsonMapper.ToObject(str);


            var config = js["configs"];
            config.Clear();
            config.Add(configjs);
            
            fs.SetLength(0);
            var sw=new StreamWriter(fs);
            
            sw.Write(js.ToJson());
            sw.Flush();
            sw.Close();
            fs.Close();

            Process.Start(SSPath);
        }
        private static void KillProcess(string processName)
        {
            //得到所有打开的进程     
            try
            {
                //获得需要杀死的进程名    
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                {
                    //立即杀死进程  
                    thisproc.Kill();
                }
            }
            catch (Exception exc)
            {
                throw new Exception("", exc);
            }
        }
    }
}
