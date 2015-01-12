using MO.Common.Net.Xml;

namespace MpServiceTest
{
   class Program
   {
      static void Main(string[] args) {
         FXmlConnection connection = new FXmlConnection(@"http://10.127.3.12:8040/content.data.form.ws?action=list");
         connection.Fetch();
         System.Console.WriteLine(connection.OutputNode.Xml);
      }
   }
}
