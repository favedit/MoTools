using MO.Common.Collection;
using MO.Common.Lang;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace MO.Common.Net.Http
{
   //============================================================
   // <T>网络请求。</T>
   //============================================================
   public class FHttpResponse : FObject, IDump
   {
      // 网络链接
      protected FHttpConnection _connection;

      // 编码
      protected string _encoding = "utf-8";

      // 头信息集合
      protected FAttributes _heads = new FAttributes();

      // 参数集合
      protected FAttributes _parameters = new FAttributes();

      // 数据
      protected FBytes _data = new FBytes();

      // 数据字符串
      protected string _dataString;

      protected string _protocol;

      protected int _status;

      protected string _statusNote;

      //============================================================
      // <T>构造网络请求。</T>
      //============================================================
      public FHttpResponse() {
      }

      //============================================================
      // <T>构造网络请求。</T>
      //
      // @param connection 网络链接
      //============================================================
      public FHttpResponse(FHttpConnection connection) {
         _connection = connection;
      }

      //============================================================
      // <T>获得网络链接。</T>
      //============================================================
      public FHttpConnection Connection {
         get { return _connection; }
      }

      //============================================================
      // <T>获得头信息集合。</T>
      //============================================================
      public FAttributes Heads {
         get { return _heads; }
      }

      //============================================================
      // <T>获得参数集合。</T>
      //============================================================
      public FAttributes Parameters {
         get { return _parameters; }
      }

      //============================================================
      // <T>获得数据。</T>
      //============================================================
      public FBytes Data {
         get { return _data; }
      }

      //============================================================
      // <T>获得数据字符串。</T>
      //============================================================
      public String DataString {
         get {
            if (_dataString == null) {
               Encoding encoding = Encoding.GetEncoding(_encoding);
               _dataString = encoding.GetString(_data.Memory, 0, _data.Length);
            }
            return _dataString; 
         }
      }

      //============================================================
      // <T>接收处理。</T>
      //
      // @param response 网络应答
      //============================================================
      internal void Receive(HttpWebResponse response) {
         // 获取头信息
         if(response.Headers.Count > 0){
            foreach(string name in response.Headers){
               string value = response.Headers[name];
               _heads.Set(name, value);
               // 获得编码
               if (name == "Content-Type") {
                  int find = value.IndexOf("charset=");
                  if (find != -1) {
                     _encoding = value.Substring(find + 8);
                  }
               }
            }
         }
         // 获得返回数据
         int contentLength = (int)response.ContentLength;
         using (MemoryStream memoryStream = new MemoryStream()) {
            byte[] buffer = new byte[0x100];
            Stream stream = response.GetResponseStream();
            while(true){
               int readed = stream.Read(buffer, 0, buffer.Length);
               if(readed <= 0){
                  break;
               }
               _data.Append(buffer, 0, readed);
            }
         }
      }


      #region IDump members

      public FDumpInfo Dump() {
         return Dump(new FDumpInfo(this));
      }

      public virtual FDumpInfo Dump(FDumpInfo info) {
         RDump.StartDump(info);
         info.Append(" Protocol:", _protocol);
         info.Append(" Status:", _status.ToString());
         info.Append(' ');
         info.AppendLine(_statusNote);
         info.AppendLine(RDump.LINE_L2);
         int count = _heads.Count;
         for (int n = 0; n < count; n++) {
            info.Append(_heads.Name(n));
            info.Append(": ");
            info.AppendLine(_heads.Value(n));
         }
         info.AppendLine(RDump.LINE_L2);
         if (_data == null) {
            info.AppendLine("[null]");
         } else {
            info.AppendLine(Encoding.UTF8.GetString(_data.ToArray()));
         }
         info.AppendLine(RDump.LINE_L2);
         return info;
      }

      #endregion
   }

}
