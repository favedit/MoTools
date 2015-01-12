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
   // <T>网络请求。</T>
   //============================================================
   public class FHttpRequest : FObject, IDump
   {
      // 网络链接
      protected FHttpConnection _connection;

      // 提交方式
      protected EHttpMethod _method;

      // 接收内容
      protected string _acceptContent = "*/*";

      // 接收语言
      protected string _acceptLanguage = "zh-CN";

      // 接收编码
      protected string _acceptEncoding = "utf-8";

      // 用户标志
      protected string _userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

      // 开始点
      protected int _startPoint;

      // 结束点
      protected int _endPoint;

      // 头信息集合
      protected FAttributes _heads = new FAttributes();

      // 参数集合
      protected FAttributes _parameters = new FAttributes();

      // 内容集合
      protected FAttributes _values;

      // 文件集合
      protected FObjects<FHttpUploadFile> _files;

      //============================================================
      // <T>构造网络请求。</T>
      //============================================================
      public FHttpRequest() {
         ResetHeads();
      }

      //============================================================
      // <T>构造网络请求。</T>
      //
      // @param connection 网络链接
      //============================================================
      public FHttpRequest(FHttpConnection connection) {
         _connection = connection;
         ResetHeads();
      }

      //============================================================
      // <T>重置头信息。</T>
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
      // <T>获得网络链接。</T>
      //============================================================
      public FHttpConnection Connection {
         get { return _connection; }
      }

      //============================================================
      // <T>获得或设置方法。</T>
      //============================================================
      public EHttpMethod Method {
         get { return _method; }
         set { _method = value; }
      }

      //============================================================
      // <T>获得或设置接收内容。</T>
      //============================================================
      public string AcceptContent {
         get { return _acceptContent; }
         set { _acceptContent = value; }
      }

      //============================================================
      // <T>获得或设置接收语言。</T>
      //============================================================
      public string AcceptLanguage {
         get { return _acceptLanguage; }
         set { _acceptLanguage = value; }
      }

      //============================================================
      // <T>获得或设置接收编码。</T>
      //============================================================
      public string AcceptEncoding {
         get { return _acceptEncoding; }
         set { _acceptEncoding = value; }
      }

      //============================================================
      // <T>获得头信息集合。</T>
      //============================================================
      public FAttributes Heads {
         get { return _heads; }
      }

      //============================================================
      // <T>判断是否含有参数。</T>
      //
      // @return 是否含有
      //============================================================
      public bool HasValue() {
         return (_values != null) ? !_values.IsEmpty() : false;
      }

      //============================================================
      // <T>获得参数集合。</T>
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
      // <T>判断是否含有文件。</T>
      //
      // @return 是否含有
      //============================================================
      public bool HasFile() {
         return (_files != null) ? !_files.IsEmpty() : false;
      }

      //============================================================
      // <T>获得文件集合。</T>
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
      // <T>发送数据。</T>
      //
      // @param request 网络请求
      //============================================================
      internal void Send(HttpWebRequest request) {
         // 修改模式
         bool hasData = HasValue() | HasFile();
         if (hasData) {
            _method = EHttpMethod.POST;
         }
         // 构造请求
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
         // 构建头集合
         if (!_heads.IsEmpty()) {
            int count = _heads.Count;
            for (int n = 0; n < count; n++) {
               request.Headers.Add(_heads.Name(n), _heads.Value(n));
            }
         }
         //............................................................
         // 写入内容
         if (hasData) {
            using (MemoryStream memoryStream = new MemoryStream()) {
               using (StreamWriter writer = new StreamWriter(memoryStream)) {
                  // 设置文件
                  if (HasFile()) {
                     string boundary = Guid.NewGuid().ToString().Replace("-", "");
                     request.ContentType = "multipart/form-data; boundary=" + boundary;
                     // 写入提交数据
                     string newLine = "\r\n";
                     if (HasValue()) {
                        foreach (IStringPair pair in _values) {
                           writer.Write("--" + boundary + newLine);
                           writer.Write("Content-Disposition: form-data; name=\"{0}\"{1}{1}", pair.Name, newLine);
                           writer.Write(pair.Value + newLine);
                        }
                     }
                     // 写入文件数据
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
                  // 写入输出流
                  writer.Flush();
                  request.ContentLength = memoryStream.Length;
                  using (Stream stream = request.GetRequestStream()) {
                     memoryStream.WriteTo(stream);
                  }
               }
            }
         }
         //............................................................
         // 写入范围
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

