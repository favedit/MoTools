using System.IO;

namespace MO.Common.Net.Http
{
   //============================================================
   // <T>上传文件信息。</T>
   //============================================================
   public class FHttpUploadFile
   {
      // 名称
      protected string _name;

      // 文件名称
      protected string _fileName;

      // 数据
      protected byte[] _data;

      //============================================================
      // <T>构造上传文件信息。</T>
      //
      // @param name 名称
      // @param fileName 文件名称
      //============================================================
      public FHttpUploadFile(string name, string fileName) {
         _name = name;
         _fileName = fileName;
         using (FileStream stream = new FileStream(fileName, FileMode.Open)) {
            _data = new byte[stream.Length];
            stream.Read(_data, 0, _data.Length);
         }
      }

      //============================================================
      // <T>构造上传文件信息。</T>
      //
      // @param name 名称
      // @param fileName 文件名称
      // @param data 数据
      //============================================================
      public FHttpUploadFile(string name, string fileName, byte[] data) {
         _name = name;
         _fileName = fileName;
         _data = data;
      }

      //============================================================
      // <T>获得或设置名称。</T>
      //============================================================
      public string Name {
         get { return _name; }
         set { _name = value; }
      }

      //============================================================
      // <T>获得或设置文件名称。</T>
      //============================================================
      public string FileName {
         get { return _fileName; }
         set { _fileName = value; }
      }

      //============================================================
      // <T>获得或设置数据。</T>
      //============================================================
      public byte[] Data {
         get { return _data; }
         set { _data = value; }
      }
   }
}
