using System.IO;
using System.Text;
using MO.Common.Lang;

namespace MO.Common.IO
{
   //============================================================
   // <T>�м����ļ���</T>
   //============================================================
   public class FLineFile : FStrings
   {
      //============================================================
      // <T>�����м����ļ���</T>
      //============================================================
      public FLineFile() {
      }

      //============================================================
      // <T>�����м����ļ���</T>
      //
      // @param fileName �ļ�����
      //============================================================
      public FLineFile(string fileName) {
         LoadFile(fileName);
      }

      //============================================================
      // <T>����ָ���ļ���</T>
      //
      // @param fileName �ļ�����
      //============================================================
      public void LoadFile(string fileName) {
         string[] lines = File.ReadAllLines(fileName);
         Append(lines);
      }

      //============================================================
      // <T>����Ϊָ���ļ���</T>
      //
      // @param fileName �ļ�����
      //============================================================
      public void SaveFile(string fileName) {
         using (FileStream stream = File.Open(fileName, FileMode.Create, FileAccess.Write)) {
            using (StreamWriter writer = new StreamWriter(stream, Encoding.Default)) {
               foreach (string line in this) {
                  writer.WriteLine(line);
               }
            }
         }
      }
   }
}
