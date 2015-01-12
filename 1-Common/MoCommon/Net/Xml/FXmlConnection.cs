using MO.Common.Content;
using MO.Common.Lang;
using MO.Common.Net.Http;
using System;

namespace MO.Common.Net.Xml
{
   //============================================================
   // <T>配置链接。</T>
   //============================================================
   public class FXmlConnection : FHttpConnection
   {
      // 输入节点
      protected FXmlNode _inputNode;

      // 输出节点
      protected FXmlNode _outputNode;

      //============================================================
      // <T>构造配置链接。</T>
      //============================================================
      public FXmlConnection() {
      }

      //============================================================
      // <T>配置链接。</T>
      //
      // @param url 网络地址
      //============================================================
      public FXmlConnection(String url)
         : base(url) {
      }

      //============================================================
      // <T>构造配置链接。</T>
      //
      // @param url 网络地址
      //============================================================
      public FXmlConnection(FHttpUrl url)
         : base(url) {
      }

      //============================================================
      // <T>获得输入节点。</T>
      //============================================================
      public FXmlNode InputNode{
         get{
            return _inputNode;
         }
      }

      //============================================================
      // <T>获得输出节点。</T>
      //============================================================
      public FXmlNode OutputNode {
         get {
            return _outputNode;
         }
      }

      //============================================================
      // <T>获取处理。</T>
      //============================================================
      public override void Fetch() {
         base.Fetch();
         string outputXml = _response.DataString;
         if (!RString.IsEmpty(outputXml)) {
            FXmlDocument xdocument = new FXmlDocument();
            xdocument.LoadXml(outputXml);
            _outputNode = xdocument.Root;
         }
      }
   }
}
