using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using MO.Common.Content;
using MO.Common.Lang;
using System.ComponentModel;

namespace MObj.Net.Mail {

   public class FSmtpClient {

      public const string TAG_SERVER = "Server";

      public const string PTY_SEND = "Send";

      public const string TAG_HOST = "Host";

      public const string TAG_PORT = "Port";

      public const string PTY_LOGINID = "LoginId";

      public const string PTY_LOGINPW = "LoginPw";

      private ILogger _logger = RLogger.Find(typeof(FSmtpClient));

      private SmtpClient _nativeSmtp;

      // ��������ַ��
      private string _host;

      // �˿ڡ�
      private int _port;

      // �������˺š�
      private string _loginId;

      // ���������롣
      private string _loginPw;
      //============================================================
      // <T>���졣<T>
      //============================================================
      public FSmtpClient() {
         _nativeSmtp = new SmtpClient();
         _nativeSmtp.SendCompleted += OnSendCompleted;
      }

      //============================================================
      // <T>��ȡ��õ��˿ڡ�</T>
      //
      // @param value �˿ڡ�
      // @return �˿ڡ�
      //============================================================
      public string Host {
         get { return _host; }
         set {
            _host = value;
            _nativeSmtp.Host = value;
         }
      }

      //============================================================
      // <T>��ȡ��õ��˿ڡ�</T>
      //
      // @param value �˿ڡ�
      // @return �˿ڡ�
      //============================================================
      public int Port {
         get { return _port; }
         set {
            _port = value;
            _nativeSmtp.Port = value;
         }
      }

      //============================================================
      // <T>��ȡ��õ��������˺š�</T>
      //
      // @param value �������˺š�
      // @return �������˺š�
      //============================================================
      public string LoginId {
         get { return _loginId; }
         set { _loginId = value; }
      }

      //============================================================
      // <T>��ȡ��õ����������롣</T>
      //
      // @param value ���������롣
      // @return ���������롣
      //============================================================
      public string LoginPw {
         get { return _loginPw; }
         set { _loginPw = value; }
      }

      //============================================================
      // <T>�õ����ͷ�ʽ��</T>
      //
      // @param value ���ͷ�ʽ��
      // @return ���ͷ�ʽ��
      //============================================================
      public SmtpClient NativeSmtp {
         get { return _nativeSmtp; }
      }

      //============================================================
      // <T>������</T>
      //============================================================
      public void Send(FMail mail) {
         //_nativeSmtp.EnableSsl = true;
         _nativeSmtp.Timeout = 9999;
         _nativeSmtp.UseDefaultCredentials = false;
         if (!RString.IsEmpty(_loginId) && !RString.IsEmpty(_loginPw)) {
            _nativeSmtp.Credentials = new NetworkCredential(_loginId, _loginPw);
         }
         _nativeSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
         _nativeSmtp.Send(mail._nativeMail);
      }

      //============================================================
      // <T>���������</T>
      //============================================================
      protected void OnSendCompleted(object sender, AsyncCompletedEventArgs e) {
         _logger.Debug(this, "OnSendCompleted", "Send mail success.");

      }

   }

}
