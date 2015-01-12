using System;
using MO.Common.Lang;

namespace MO.Common.Net.Http
{
   //============================================================
   // <T>网络地址。</T>
   //============================================================
   public class FHttpUrl
   {
      // 协议
      protected string _protocol;

      // 主机
      protected string _host;

      // 端口
      protected int _port = 80;

      // 路径
      protected string _path;

      // 内容
      protected string _value;

      //============================================================
      // <T>构造网络地址。</T>
      //============================================================
      public FHttpUrl() {
      }

      //============================================================
      // <T>构造网络地址。</T>
      //
      // @param url 网络地址
      //============================================================
      public FHttpUrl(string url) {
         Parse(url);
      }

      //============================================================
      // <T>获得或设置协议。</T>
      //============================================================
      public string Protocol {
         get { return _protocol; }
         set { _protocol = value; }
      }

      //============================================================
      // <T>获得或设置主机。</T>
      //============================================================
      public string Host {
         get { return _host; }
         set { _host = value; }
      }

      //============================================================
      // <T>获得或设置端口。</T>
      //============================================================
      public int Port {
         get { return _port; }
         set { _port = value; }
      }

      //============================================================
      // <T>获得或设置路径。</T>
      //============================================================
      public string Path {
         get { return _path; }
         set { _path = value; }
      }

      //============================================================
      // <T>获得字符串。</T>
      //============================================================
      public override string ToString() {
         return _value;
      }

      //============================================================
      // <T>分解内容。</T>
      //============================================================
      protected void Parse(string url) {
         _value = url;
         int start = 0;
         // Protocol
         int find = url.IndexOf("://", start);
         if (find != -1) {
            _protocol = url.Substring(start, find - start);
            start = find + 3;
         }
         // Host
         find = url.IndexOf("/", start);
         if (find != -1) {
            _host = url.Substring(start, find - start);
            start = find;
            // Port
            find = _host.IndexOf(':');
            if (find != -1) {
               _port = RInt.Parse(_host.Substring(find + 1), _port);
               _host = _host.Substring(0, find);
            }
            // Path
            find = url.IndexOf("/", start + 1);
            _path = url.Substring(Math.Max(find, start));
         } else {
            _host = url.Substring(start);
            // Port
            find = _host.IndexOf(':');
            if (find != -1) {
               _port = RInt.Parse(_host.Substring(find + 1), _port);
               _host = _host.Substring(0, find);
            }
            // Path
            _path = "/";
         }
      }
   }
}
