using MO.Common.Collection;
using MO.Common.Lang;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace MO.Common.Net.Http
{
   //============================================================
   // <T>��������</T>
   //============================================================
   public class FHttpRequest : FObject, IDump
   {
      // ��������
      protected FHttpConnection _connection;

      // �ύ��ʽ
      protected EHttpMethod _method;

      // ��������
      protected string _acceptContent = "*/*";

      // ��������
      protected string _acceptLanguage = "zh-CN";

      // ���ձ���
      protected string _acceptEncoding = "utf-8";

      // �û���־
      protected string _userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

      // ��ʼ��
      protected int _startPoint;

      // ������
      protected int _endPoint;

      // ͷ��Ϣ����
      protected FAttributes _heads = new FAttributes();

      // ��������
      protected FAttributes _parameters = new FAttributes();

      // ���ݼ���
      protected FAttributes _values;

      // �ļ�����
      protected FObjects<FHttpUploadFile> _files;

      //============================================================
      // <T>������������</T>
      //============================================================
      public FHttpRequest() {
         ResetHeads();
      }

      //============================================================
      // <T>������������</T>
      //
      // @param connection ��������
      //============================================================
      public FHttpRequest(FHttpConnection connection) {
         _connection = connection;
         ResetHeads();
      }

      //============================================================
      // <T>����ͷ��Ϣ��</T>
      //============================================================
      protected void ResetHeads() {
         _heads.Clear();
         _heads[EHttpHead.AcceptLanguage] = _acceptLanguage;
         //_heads[EHttpHead.AcceptEncoding] = "gzip, deflate";
         _heads[EHttpHead.AcceptEncoding] = "deflate";
         _heads[EHttpHead.CacheControl] = "no-cache";
         _heads[EHttpHead.Pragma] = "no-cache";
      }

      //============================================================
      // <T>����������ӡ�</T>
      //============================================================
      public FHttpConnection Connection {
         get { return _connection; }
      }

      //============================================================
      // <T>��û����÷�����</T>
      //============================================================
      public EHttpMethod Method {
         get { return _method; }
         set { _method = value; }
      }

      //============================================================
      // <T>��û����ý������ݡ�</T>
      //============================================================
      public string AcceptContent {
         get { return _acceptContent; }
         set { _acceptContent = value; }
      }

      //============================================================
      // <T>��û����ý������ԡ�</T>
      //============================================================
      public string AcceptLanguage {
         get { return _acceptLanguage; }
         set { _acceptLanguage = value; }
      }

      //============================================================
      // <T>��û����ý��ձ��롣</T>
      //============================================================
      public string AcceptEncoding {
         get { return _acceptEncoding; }
         set { _acceptEncoding = value; }
      }

      //============================================================
      // <T>���ͷ��Ϣ���ϡ�</T>
      //============================================================
      public FAttributes Heads {
         get { return _heads; }
      }

      //============================================================
      // <T>�ж��Ƿ��в�����</T>
      //
      // @return �Ƿ���
      //============================================================
      public bool HasValue() {
         return (_values != null) ? !_values.IsEmpty() : false;
      }

      //============================================================
      // <T>��ò������ϡ�</T>
      //============================================================
      public FAttributes Values {
         get {
            if (_values == null) {
               _values = new FAttributes();
            }
            return _values; 
         }
      }

      //============================================================
      // <T>�ж��Ƿ����ļ���</T>
      //
      // @return �Ƿ���
      //============================================================
      public bool HasFile() {
         return (_files != null) ? !_files.IsEmpty() : false;
      }

      //============================================================
      // <T>����ļ����ϡ�</T>
      //============================================================
      public FObjects<FHttpUploadFile> Files {
         get {
            if (_files == null) {
               _files = new FObjects<FHttpUploadFile>();
            }
            return _files; 
         }
      }

      //============================================================
      // <T>�������ݡ�</T>
      //
      // @param request ��������
      //============================================================
      internal void Send(HttpWebRequest request) {
         // �޸�ģʽ
         bool hasData = HasValue() | HasFile();
         if (hasData) {
            _method = EHttpMethod.POST;
         }
         // ��������
         if (_method == EHttpMethod.POST) {
            request.Method = "POST";
         } else {
            request.Method = "GET";
         }
         request.AllowAutoRedirect = true;
         request.UserAgent = _userAgent;
         request.Accept = _acceptContent;
         request.KeepAlive = false;
         request.CookieContainer = new CookieContainer();
         //............................................................
         // ����ͷ����
         if (!_heads.IsEmpty()) {
            int count = _heads.Count;
            for (int n = 0; n < count; n++) {
               request.Headers.Add(_heads.Name(n), _heads.Value(n));
            }
         }
         //............................................................
         // д������
         if (hasData) {
            using (MemoryStream memoryStream = new MemoryStream()) {
               using (StreamWriter writer = new StreamWriter(memoryStream)) {
                  // �����ļ�
                  if (HasFile()) {
                     string boundary = Guid.NewGuid().ToString().Replace("-", "");
                     request.ContentType = "multipart/form-data; boundary=" + boundary;
                     // д���ύ����
                     string newLine = "\r\n";
                     if (HasValue()) {
                        foreach (IStringPair pair in _values) {
                           writer.Write("--" + boundary + newLine);
                           writer.Write("Content-Disposition: form-data; name=\"{0}\"{1}{1}", pair.Name, newLine);
                           writer.Write(pair.Value + newLine);
                        }
                     }
                     // д���ļ�����
                     foreach (FHttpUploadFile file in _files) {
                        writer.Write("--" + boundary + newLine);
                        writer.Write("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.FileName, newLine);
                        writer.Write("Content-Type: application/octet-stream" + newLine + newLine);
                        writer.Flush();
                        memoryStream.Write(file.Data, 0, file.Data.Length);
                        writer.Write(newLine);
                        writer.Write("--" + boundary + newLine);
                     }
                  } else {
                     request.ContentType = "application/x-www-form-urlencoded";
                     if (HasValue()) {
                        StringBuilder pack = new StringBuilder();
                        foreach (IStringPair pair in _values) {
                           pack.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(pair.Name), HttpUtility.UrlEncode(pair.Value));
                        }
                        if (pack.Length > 0) {
                           pack.Length--;
                        }
                        writer.Write(pack.ToString());
                     }
                  }
                  // д�������
                  writer.Flush();
                  request.ContentLength = memoryStream.Length;
                  using (Stream stream = request.GetRequestStream()) {
                     memoryStream.WriteTo(stream);
                  }
               }
            }
         }
         //............................................................
         // д�뷶Χ
         if ((_startPoint != 0) && (_endPoint != 0)){
            request.AddRange(_startPoint, _endPoint);
         } else if ((_startPoint != 0) && (_endPoint == 0)) {
            request.AddRange(_startPoint);
         }
      }

      #region IDump members

      public FDumpInfo Dump() {
         return Dump(new FDumpInfo(this));
      }

      public virtual FDumpInfo Dump(FDumpInfo info) {
         //RDump.StartDump(info);
         //FBytes bytes = Build();
         //info.Append("Length:");
         //info.AppendLine(bytes.Length);
         //info.AppendLine(RDump.LINE_L2);
         //string value = Encoding.ASCII.GetString(bytes.Memory, 0, bytes.Length);
         //char[] chs = value.ToCharArray();
         //int count = Math.Min(chs.Length, 2048);
         //for (int n = 0; n < count; n++) {
         //   char ch = chs[n];
         //   if (ch >= ' ') {
         //      info.Append(ch);
         //   } else if (ch == '\n' || ch == '\r') {
         //      info.Append(ch);
         //   } else {
         //      info.Append('.');
         //   }
         //}
         //info.AppendLine();
         //info.AppendLine(RDump.LINE_L2);
         return info;
      }
      #endregion

   }

}

