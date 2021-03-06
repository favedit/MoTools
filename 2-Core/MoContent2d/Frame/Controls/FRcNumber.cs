﻿using MO.Common.Content;
using MO.Common.IO;
using MO.Common.Lang;
using MO.Content2d.Frame.Common;
using MO.Content2d.Theme;
using MO.Design2d.Face.Console;

namespace MO.Content2d.Frame.Controls
{
   //============================================================
   // <T>控件声明。</T>
   //============================================================
   public class FRcNumber : FRcControl
   {
      // 文本对齐方式
      protected ERcTextAlign _textAlignCd;

      // 字体
      protected FRcFont _font = new FRcFont();

      // 字体属性
      protected FTplThemeStyleProperty _propertyFont;

      // 文本内容
      protected string _text;

      //============================================================
      // <T>构造控件。</T>
      //============================================================
      public FRcNumber(FRcFrameConsole console = null)
         : base(console) {
         _typeName = REnum.ToString<ERcComponent>(ERcComponent.Number);
      }

      //============================================================
      // <T>获得或设置文本对齐方式。</T>
      //============================================================
      public ERcTextAlign TextAlignCd {
         get { return _textAlignCd; }
         set { _textAlignCd = value; }
      }

      //============================================================
      // <T>获得或设置字体。</T>
      //============================================================
      public FRcFont Font {
         get { return _font; }
         set { _font = value; }
      }

      //============================================================
      // <T>获得或设置文本内容。</T>
      //============================================================
      public string Text {
         get { return _text; }
         set { _text = value; }
      }

      //============================================================
      // <T>加载样式属性。</T>
      //============================================================
      public override void LoadStyleProperty() {
         // 加载父样式属性
         base.LoadStyleProperty();
         // 加载样式信息
         FTplThemeStyle style = RContent2dManager.ThemeConsole.FindActiveStyle("frame.number");
         if (style != null) {
            _propertyBorderInner = style.FindProperty("border_inner");
            _propertyBorderOuter = style.FindProperty("border_outer");
            _propertyBackColor = style.FindProperty("back_color");
            _propertyFont = style.FindProperty("font");
         }
      }

      //============================================================
      // <T>加载样式内容。</T>
      //============================================================
      public override void LoadStyleValue() {
         // 加载父样式内容
         base.LoadStyleValue();
         // 加载字体属性
         if (_propertyFont != null) {
            _font.LoadStyleProperty(_propertyFont);
         }
      }

      //============================================================
      // <T>加载设置信息。</T>
      //
      // @param xconfig 设置信息
      //============================================================
      public override void OnLoadConfig(FXmlNode xconfig) {
         base.OnLoadConfig(xconfig);
         if (xconfig.Contains("text_align_cd")) {
            _textAlignCd = (ERcTextAlign)REnum.ToValue(typeof(ERcTextAlign), xconfig.Get("text_align_cd"));
         }
         _font.LoadConfig(xconfig, "font");
         _text = xconfig.Get("text", _text);
      }

      //============================================================
      // <T>存储设置信息。</T>
      //
      // @param xconfig 设置信息
      //============================================================
      public override void OnSaveConfig(FXmlNode xconfig) {
         // 保存处理
         base.OnSaveConfig(xconfig);
         // 保存属性
         if (_textAlignCd != ERcTextAlign.None) {
            xconfig.Set("text_align_cd", _textAlignCd.ToString());
         }
         if (_propertyFont == null || (_propertyFont != null && !_font.EqualsStyleProperty(_propertyFont))) {
            _font.SaveConfig(xconfig, "font");
         }
         xconfig.SetNvl("text", _text);
      }

      //============================================================
      // <T>序列化内容到输出流。</T>
      //
      // @param output 输出流
      //============================================================
      public override void OnSerialize(IOutput output) {
         base.OnSerialize(output);
         output.WriteUint8((byte)_textAlignCd);
         _font.Serialize(output);
         output.WriteWideString(RString.Nvl(_text, _label));
      }

      //============================================================
      // <T>释放资源。</T>
      //============================================================
      public override void OnDispose() {
         base.OnDispose();
      }
   }
}
