using MO.Common.Lang;
using MO.Common.Net.Sockets;
using System;
using System.Net;

namespace MO.Common.Net.Http
{
   //============================================================
   // <T>�������ӡ�</T>
   //============================================================
   public class FHttpConnection : FObject, IDisposable
   {
      // �����ַ
      protected FHttpUrl _url = new FHttpUrl();

      // ʹ�ô���
      protected bool _useProxy = false;

      // ��������
      protected FHttpRequest _request;

      // ����Ӧ��
      protected FHttpResponse _response;

      //============================================================
      // <T>�����������ӡ�</T>
      //============================================================
      public FHttpConnection() {
         Construct();
      }

      //============================================================
      // <T>�����������ӡ�</T>
      //
      // @param url �����ַ
      //============================================================
      public FHttpConnection(String url) {
         Construct();
         _url = new FHttpUrl(url);
      }

      //============================================================
      // <T>�����������ӡ�</T>
      //
      // @param url �����ַ
      //============================================================
      public FHttpConnection(FHttpUrl url) {
         Construct();
         _url = url;
      }

      //============================================================
      // <T>���촦��</T>
      //============================================================
      protected void Construct() {
         _request = new FHttpRequest(this);
         _response = new FHttpResponse(this);
      }

      //============================================================
      // <T>��������ַ��</T>
      //============================================================
      public FHttpUrl Url {
         get { return _url; }
      }

      //============================================================
      // <T>��û�����ʹ�ô���</T>
      //============================================================
      public bool UseProxy {
         get { return _useProxy; }
         set { _useProxy = value; }
      }

      //============================================================
      // <T>�����������</T>
      //============================================================
      public FHttpRequest Request {
         get { return _request; }
      }

      //============================================================
      // <T>�������Ӧ��</T>
      //============================================================
      public FHttpResponse Response {
         get { return _response; }
      }

      //============================================================
      // <T>��ȡ����</T>
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
      // <T>�ͷŴ���</T>
      //============================================================
      public void Dispose() {
      }
   }
}
