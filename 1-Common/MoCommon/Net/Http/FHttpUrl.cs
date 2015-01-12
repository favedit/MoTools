using System;
using MO.Common.Lang;

namespace MO.Common.Net.Http
{
   //============================================================
   // <T>�����ַ��</T>
   //============================================================
   public class FHttpUrl
   {
      // Э��
      protected string _protocol;

      // ����
      protected string _host;

      // �˿�
      protected int _port = 80;

      // ·��
      protected string _path;

      // ����
      protected string _value;

      //============================================================
      // <T>���������ַ��</T>
      //============================================================
      public FHttpUrl() {
      }

      //============================================================
      // <T>���������ַ��</T>
      //
      // @param url �����ַ
      //============================================================
      public FHttpUrl(string url) {
         Parse(url);
      }

      //============================================================
      // <T>��û�����Э�顣</T>
      //============================================================
      public string Protocol {
         get { return _protocol; }
         set { _protocol = value; }
      }

      //============================================================
      // <T>��û�����������</T>
      //============================================================
      public string Host {
         get { return _host; }
         set { _host = value; }
      }

      //============================================================
      // <T>��û����ö˿ڡ�</T>
      //============================================================
      public int Port {
         get { return _port; }
         set { _port = value; }
      }

      //============================================================
      // <T>��û�����·����</T>
      //============================================================
      public string Path {
         get { return _path; }
         set { _path = value; }
      }

      //============================================================
      // <T>����ַ�����</T>
      //============================================================
      public override string ToString() {
         return _value;
      }

      //============================================================
      // <T>�ֽ����ݡ�</T>
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
