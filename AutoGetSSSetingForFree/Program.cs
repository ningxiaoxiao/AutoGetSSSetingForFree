using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace AutoGetSSSetingForFree
{
    class Program
    {
        static void Main(string[] args)
        {
            //下载图片

            var req = WebRequest.Create("http://get.shadowsocks8.cc/images/server03.png");
            var res = req.GetResponse();
            var result = new StreamReader(res.GetResponseStream(), Encoding.UTF8);



        }
    }
}
