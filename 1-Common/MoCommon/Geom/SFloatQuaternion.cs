using System;
using MO.Common.Content;
using MO.Common.IO;
using MO.Common.Lang;

namespace MO.Common.Geom
{
   public class SFloatQuaternion
   {
      public float X = 0.0f;

      public float Y = 0.0f;

      public float Z = 0.0f;

      public float W = 1.0f;

      //============================================================
      public SFloatQuaternion() {
      }

      //============================================================
      public SFloatQuaternion(float x, float y, float z, float w) {
         X = x;
         Y = y;
         Z = z;
         W = w;
      }

      //============================================================
      public void Clear() {
         X = 0.0f;
         Y = 0.0f;
         Z = 0.0f;
         W = 1.0f;
      }

      //============================================================
      public void Assign(SFloatQuaternion value) {
         X = value.X;
         Y = value.Y;
         Z = value.Z;
         W = value.W;
      }

      //============================================================
      public void Set(float x, float y, float z, float w) {
         X = x;
         Y = y;
         Z = z;
         W = w;
      }
      
      //============================================================
      // <T>加载配置信息。</T>
      //
      // @param xconfig 配置信息
      //============================================================
      public void LoadConfig(FXmlNode xconfig) {
         X = xconfig.GetFloat("x");
         Y = xconfig.GetFloat("y");
         Z = xconfig.GetFloat("z");
         W = xconfig.GetFloat("w");
      }

      //============================================================
      // <T>存储配置信息。</T>
      //
      // @param xconfig 配置信息
      //============================================================
      public void SaveConfig(FXmlNode xconfig) {
         xconfig.Set("x", X);
         xconfig.Set("y", Y);
         xconfig.Set("z", Z);
         xconfig.Set("w", W);
      }

      //============================================================
      public void Unserialize(IInput input) {
         X = input.ReadFloat();
         Y = input.ReadFloat();
         Z = input.ReadFloat();
         W = input.ReadFloat();
      }

      //============================================================
      public void Serialize(IOutput output) {
         output.WriteFloat(X);
         output.WriteFloat(Y);
         output.WriteFloat(Z);
         output.WriteFloat(W);
      }
      
      //============================================================
      public override string ToString() {
         return X + "," + Y + "," + Z + "," + W;
      }
   }
}
