using MO.Common.Lang;
using MO.Common.Net.Sockets;
using System;
using System.Net;

namespace MO.Common.Net.Http
{
   //============================================================
   // <T>网络链接。</T>
   //============================================================
   public class FHttpConnection : FObject, IDisposable
   {
      // 网络地址
      protected FHttpUrl _url = new FHttpUrl();

      // 使用代理
      protected bool _useProxy = false;

      // 网络请求
      protected FHttpRequest _request;

      // 网络应答
      protected FHttpResponse _response;

      //============================================================
      // <T>构造网络链接。</T>
      //============================================================
      public FHttpConnection() {
         Construct();
      }

      //============================================================
      // <T>构造网络链接。</T>
      //
      // @param url 网络地址
      //============================================================
      public FHttpConnection(String url) {
         Construct();
         _url = new FHttpUrl(url);
      }

      //============================================================
      // <T>构造网络链接。</T>
      //
      // @param url 网络地址
      //============================================================
      public FHttpConnection(FHttpUrl url) {
         Construct();
         _url = url;
      }

      //============================================================
      // <T>构造处理。</T>
      //============================================================
      protected void Construct() {
         _request = new FHttpRequest(this);
         _response = new FHttpResponse(this);
      }

      //============================================================
      // <T>获得网络地址。</T>
      //============================================================
      public FHttpUrl Url {
         get { return _url; }
      }

      //============================================================
      // <T>获得或设置使用代理。</T>
      //============================================================
      public bool UseProxy {
         get { return _useProxy; }
         set { _useProxy = value; }
      }

      //============================================================
      // <T>获得网络请求。</T>
      //============================================================
      public FHttpRequest Request {
         get { return _request; }
      }

      //============================================================
      // <T>获得网络应答。</T>
      //============================================================
      public FHttpResponse Response {
         get { return _response; }
      }

      //============================================================
      // <T>获取处理。</T>
      //============================================================
      public virtual void Fetch() {
         string url = _url.ToString();
         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
         _request.Send(request);
         using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
            _response.Receive(response);
         }
      }

      //============================================================
      // <T>释放处理。</T>
      //============================================================
      public void Dispose() {
      }
   }
}
