﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MO.Design2d.Frame.Common
{
   public enum EUiLineStyle : int
   {
      // 无效果
      None = 0,

      // 实线
      Solid = 1,

   }

   //============================================================
   // <T>线条样式工具类。</T>
   //============================================================
   public class RUiLineStyle
   {
      //============================================================
      // <T>根据从字符串获得枚举内容。</T>
      //
      // @param value 字符串
      // @return 枚举内容
      //============================================================
      public static EUiLineStyle Parse(string value) {
         switch (value) {
            case "Solid":
               return EUiLineStyle.Solid;
         }
         return EUiLineStyle.None;
      }
   }
}
