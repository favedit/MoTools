using MO.Common.Collection;
using MO.Common.Lang;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace MO.Common.Net.Http
{
   //============================================================
   // <T>��������</T>
   //============================================================
   public class FHttpResponse : FObject, IDump
   {
      // ��������
      protected FHttpConnection _connection;

      // ����
      protected string _encoding = "utf-8";

      // ͷ��Ϣ����
      protected FAttributes _heads = new FAttributes();

      // ��������
      protected FAttributes _parameters = new FAttributes();

      // ����
      protected FBytes _data = new FBytes();

      // �����ַ���
      protected string _dataString;

      protected string _protocol;

      protected int _status;

      protected string _statusNote;

      //============================================================
      // <T>������������</T>
      //============================================================
      public FHttpResponse() {
      }

      //============================================================
      // <T>������������</T>
      //
      // @param connection ��������
      //============================================================
      public FHttpResponse(FHttpConnection connection) {
         _connection = connection;
      }

      //============================================================
      // <T>����������ӡ�</T>
      //============================================================
      public FHttpConnection Connection {
         get { return _connection; }
      }

      //============================================================
      // <T>���ͷ��Ϣ���ϡ�</T>
      //============================================================
      public FAttributes Heads {
         get { return _heads; }
      }

      //============================================================
      // <T>��ò������ϡ�</T>
      //============================================================
      public FAttributes Parameters {
         get { return _parameters; }
      }

      //============================================================
      // <T>������ݡ�</T>
      //============================================================
      public FBytes Data {
         get { return _data; }
      }

      //============================================================
      // <T>��������ַ�����</T>
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
      // <T>���մ���</T>
      //
      // @param response ����Ӧ��
      //============================================================
      internal void Receive(HttpWebResponse response) {
         // ��ȡͷ��Ϣ
         if(response.Headers.Count > 0){
            foreach(string name in response.Headers){
               string value = response.Headers[name];
               _heads.Set(name, value);
               // ��ñ���
               if (name == "Content-Type") {
                  int find = value.IndexOf("charset=");
                  if (find != -1) {
                     _encoding = value.Substring(find + 8);
                  }
               }
            }
         }
         // ��÷�������
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
