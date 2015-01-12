using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using MO.Common.Content;
using MO.Common.Lang;
using System.Net.Mime;
using MO.Common.IO;

namespace MObj.Net.Mail {

   public class FMail {

      // SmtpClient���͵ĵ����ʼ���
      internal MailMessage _nativeMail = new MailMessage();

      //============================================================
      // <T>���졣<T>
      //============================================================
      public FMail() {
      }

      //============================================================
      // <T>��ȡ��õ������ˡ�</T>
      //
      // @param value �����ˡ�
      // @return �����ˡ�
      //============================================================
      public string From {
         get { return _nativeMail.From.Address; }
         set {
            if (value != null) {
               _nativeMail.From = new MailAddress(value.Trim());
            }
         }
      }

      //============================================================
      // <T>��ȡ��õ������ˡ�</T>
      //
      // @param value �����ˡ�
      // @return �����ˡ�
      //============================================================
      public string To {
         get { return _nativeMail.To.ToString(); }
         set {
            if (value != null) {
               foreach (string id in value.Split('\n')) {
                  string mailid = id.Trim();
                  if (!RString.IsEmpty(mailid)) {
                     _nativeMail.To.Add(new MailAddress(mailid));
                  }
               }
            }
         }
      }

      //============================================================
      // <T>��ȡ��õ���ǩ��</T>
      //
      // @param value ��ǩ��
      // @return ��ǩ��
      //============================================================
      public string Subject {
         get { return _nativeMail.Subject; }
         set { _nativeMail.Subject = value; }
      }

      //============================================================
      // <T>��ȡ��õ��ʼ����ݡ�</T>
      //
      // @param value �ʼ����ݡ�
      // @return �ʼ����ݡ�
      //============================================================
      public string Body {
         get { return _nativeMail.Body; }
         set { _nativeMail.Body = value; }
      }

      public bool HasAttach { 
         get{
            return (_nativeMail.Attachments.Count > 0);
         }
      }

      //============================================================
      // <T>����ʼ�������<T>
      //============================================================
      public void AddFile(string filename) {
         FFileInfo fi = new FFileInfo(filename);
         Attachment attach = new Attachment(filename);
         attach.Name = fi.Name;
         attach.NameEncoding = Encoding.ASCII;
         attach.TransferEncoding = TransferEncoding.Base64;
         _nativeMail.Attachments.Add(attach);
      }

      //============================================================
      // <T>����ʼ�������<T>
      //============================================================
      public void AddMemoryFile(string filename) {
         byte[] bytes = File.ReadAllBytes(filename);
         FFileInfo fi = new FFileInfo(filename);
         ContentType context = new ContentType();
         context.Name = fi.Name;
         context.MediaType = MediaTypeNames.Application.Octet;
         Attachment attach = new Attachment(new MemoryStream(bytes), context);
         attach.Name = fi.Name;
         attach.NameEncoding = Encoding.ASCII;
         attach.TransferEncoding = TransferEncoding.Base64;
         _nativeMail.Attachments.Add(attach);
      }

      //============================================================
      // <T>����ʼ�������<T>
      //============================================================
      public void AddMemory(string name, byte[] data) {
         ContentType context = new ContentType();
         context.Name = name;
         context.MediaType = MediaTypeNames.Application.Octet;
         Attachment attach = new Attachment(new MemoryStream(data), context);
         attach.Name = name;
         attach.NameEncoding = Encoding.ASCII;
         attach.TransferEncoding = TransferEncoding.Base64;
         _nativeMail.Attachments.Add(attach);
      }

      //============================================================
      // <T>����ʼ�������<T>
      //============================================================
      public void AddFile(string name, string filename) {
         Attachment attach = new Attachment(filename);
         attach.Name = name;
         attach.NameEncoding = Encoding.ASCII;
         attach.TransferEncoding = TransferEncoding.Base64;
         _nativeMail.Attachments.Add(attach);
      }

   }

}
