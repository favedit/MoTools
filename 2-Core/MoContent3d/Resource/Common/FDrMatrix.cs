using MO.Common.Content;
using MO.Common.Geom;
using MO.Common.IO;
using MO.Common.Lang;

namespace MO.Content3d.Resource.Common
{
   //============================================================
   public class FDrMatrix : FObject
   {
      // 移动信息
      protected SFloatPoint3 _translation = new SFloatPoint3();

      // 四元数旋转信息
      protected SFloatQuaternion _quaternion = new SFloatQuaternion();

      // 欧拉角旋转信息
      protected SFloatVector3 _euler = new SFloatVector3();

      // 缩放信息
      protected SFloatVector3 _scale = new SFloatVector3(1.0f, 1.0f, 1.0f);

      //============================================================
      // 移动信息
      public SFloatPoint3 Translation {
         get { return _translation; }
      }

      //============================================================
      // 缩放信息
      public SFloatVector3 Euler {
         get { return _euler; }
      }

      //============================================================
      // 缩放信息
      public SFloatVector3 Scale{ 
         get{return _scale;}
      }

      //============================================================
      // <T>判断数据是否相等。</T>
      //
      // @param value 数据
      // @return 是否相等
      //============================================================
      public bool EqualsData(FDrMatrix matrix) {
         if (!_translation.EqualsData(matrix._translation)) {
            return false;
         }
         if (!_quaternion.EqualsData(matrix._quaternion)) {
            return false;
         }
         if (!_euler.EqualsData(matrix._euler)) {
            return false;
         }
         if (!_scale.EqualsData(matrix._scale)) {
            return false;
         }
         return true;
      }

      //============================================================
      // <T>加载配置信息。</T>
      //
      // @param config 配置信息
      //============================================================
      public void LoadConfig(FXmlNode config) {
         _translation.LoadConfig(config.Find("Translation"));
         _euler.LoadConfig(config.Find("Euler"));
         _scale.LoadConfig(config.Find("Scale"));
      }

      //============================================================
      // <T>加载简要配置信息。</T>
      //
      // @param config 配置信息
      //============================================================
      public void LoadSimpleConfig(FXmlNode config) {
         _translation.X = config.GetFloat("tx");
         _translation.Y = config.GetFloat("ty");
         _translation.Z = config.GetFloat("tz");
         _euler.X = config.GetFloat("rx");
         _euler.Y = config.GetFloat("ry");
         _euler.Z = config.GetFloat("rz");
         _scale.X = config.GetFloat("sx");
         _scale.Y = config.GetFloat("sy");
         _scale.Z = config.GetFloat("sz");
      }

      //============================================================
      // <T>加载简要配置信息。</T>
      //
      // @param config 配置信息
      //============================================================
      public void LoadSimpleAngleConfig(FXmlNode config) {
         _translation.X = config.GetFloat("tx");
         _translation.Y = config.GetFloat("ty");
         _translation.Z = config.GetFloat("tz");
         _quaternion.X = config.GetFloat("qx", 0.0f);
         _quaternion.Y = config.GetFloat("qy", 0.0f);
         _quaternion.Z = config.GetFloat("qz", 0.0f);
         _quaternion.W = config.GetFloat("qw", 1.0f);
         _euler.X = config.GetFloat("rx") * RFloat.DegreeRate;
         _euler.Y = config.GetFloat("ry") * RFloat.DegreeRate;
         _euler.Z = config.GetFloat("rz") * RFloat.DegreeRate;
         _scale.X = config.GetFloat("sx");
         _scale.Y = config.GetFloat("sy");
         _scale.Z = config.GetFloat("sz");
      }

      //============================================================
      public void Unserialize(IInput input) {
         _translation.X = input.ReadFloat();
         _translation.Y = input.ReadFloat();
         _translation.Z = input.ReadFloat();
         _euler.X = input.ReadFloat();
         _euler.Y = input.ReadFloat();
         _euler.Z = input.ReadFloat();
         _scale.X = input.ReadFloat();
         _scale.Y = input.ReadFloat();
         _scale.Z = input.ReadFloat();
      }

      //============================================================
      public void UnserializeAngle(IInput input) {
         _translation.X = input.ReadFloat();
         _translation.Y = input.ReadFloat();
         _translation.Z = input.ReadFloat();
         _euler.X = input.ReadFloat() * RFloat.DegreeRate;
         _euler.Y = input.ReadFloat() * RFloat.DegreeRate;
         _euler.Z = input.ReadFloat() * RFloat.DegreeRate;
         _quaternion.X = input.ReadFloat();
         _quaternion.Y = input.ReadFloat();
         _quaternion.Z = input.ReadFloat();
         _quaternion.W = input.ReadFloat();
         _scale.X = input.ReadFloat();
         _scale.Y = input.ReadFloat();
         _scale.Z = input.ReadFloat();
      }

      //============================================================
      public void Serialize(IOutput output) {
         _translation.Serialize(output);
         _euler.Serialize(output);
         _scale.Serialize(output);
      }

      //============================================================
      public void SerializeQuaternion(IOutput output) {
         _translation.Serialize(output);
         _quaternion.Serialize(output);
         _scale.Serialize(output);
      }
   }
}
