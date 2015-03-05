using System.Collections.Generic;
using MO.Common.Content;
using MO.Common.IO;
using MO.Common.Lang;
using MO.Content3d.Common;
using MO.Content3d.Resource.Theme;
using MO.Common.Collection;

namespace MO.Content3d.Resource.Material
{
   //============================================================
   // <T>材质定义。</T>
   //============================================================
   public class FDrMaterial : FDrMaterialInfo, IComparer<FDrMaterial>
   {
      // 日志输出接口
      private static ILogger _logger = RLogger.Find(typeof(FDrMaterial));

      // 主题
      protected FDrTheme _theme;

      // 材质组
      protected FDrMaterialGroup _group;

      // 主题名称
      protected string _themeName;

      //============================================================
      // <T>构造材质定义。</T>
      //============================================================
      public FDrMaterial() {
      }

      //============================================================
      // <T>获得或设置主题。</T>
      //============================================================
      public FDrTheme Theme {
         get { return _theme; }
         set { 
            _theme = value;
            _themeName = value.Name;
         }
      }
      
      //============================================================
      // <T>获得或设置材质组。</T>
      //============================================================
      public FDrMaterialGroup Group{
         get { return _group; }
         set { _group = value; }
      }

      //============================================================
      // <T>获得主题代码。</T>
      //============================================================
      public string ThemeCode {
         get { return RDrUtil.FormatPathToCode(_themeName); }
      }

      //============================================================
      // <T>获得或设置主题名称。</T>
      //============================================================
      public string ThemeName {
         get { return _themeName; }
         set { _themeName = value; }
      }
      
      //============================================================
      // <T>比较材质顺序。</T>
      //============================================================
      public int Compare(FDrMaterial target, FDrMaterial source) {
         return target._themeName.CompareTo(source._themeName);
      }

      //============================================================
      // <T>接收数据。</T>
      //============================================================
      public void Assign(FDrMaterial material) {
         // 存储属性
         _themeName = material.ThemeName;
         // 存储设置
         AssignInfo(material);
      }
      
      //============================================================
      // <T>加载配置信息。</T>
      //
      // @param config 配置信息
      //============================================================
      public void LoadConfig(FXmlNode xconfig) {
         // 读取主题
         _theme = RContent3dManager.ThemeConsole.Find(xconfig.Nvl("theme_name"));
         if (_theme == null) {
            _theme = RContent3dManager.ThemeConsole.DefaultTheme;
         }
         _themeName = _theme.Name;
         // 读取属性
         LoadConfigInfo(xconfig);
      }

      //============================================================
      // <T>存储配置信息。</T>
      //
      // @param xconfig 配置信息
      //============================================================
      public void SaveConfig(FXmlNode xconfig) {
         // 存储主题
         xconfig.Set("theme_name", _themeName);
         // 存储属性
         SaveConfigInfo(xconfig);
      }

      //============================================================
      public void ExportConfig(FXmlNode xconfig) {
         // 存储属性
         xconfig.Set("effect_code", RString.Nvl(_effectName, "automatic"));
         //xconfig.Set("transform_code", _transformName);
         //xconfig.Set("option_light", _optionLight);
         //xconfig.Set("option_merge", _optionMerge);
         //xconfig.Set("option_sort", _optionSort);
         //xconfig.Set("sort_level", _sortLevel);
         //xconfig.Set("option_alpha", _optionAlpha);
         //xconfig.Set("option_depth", _optionDepth);
         //xconfig.Set("option_compare", _optionCompare);
         xconfig.Set("option_double", _optionDouble);
         xconfig.Set("option_shadow", _optionShadow);
         xconfig.Set("option_shadow_self", _optionShadowSelf);
         //xconfig.Set("option_dynamic", _optionDynamic);
         //xconfig.Set("option_transmittance", _optionTransmittance);
         //xconfig.Set("option_opacity", _optionOpacity);
         // 存储纹理
         FXmlNode xcoord = xconfig.CreateNode("Coord");
         xcoord.Set("rate_width", _coordRateWidth);
         xcoord.Set("rate_height", _coordRateHeight);
         // 存储颜色
         FXmlNode xcolor = xconfig.CreateNode("Color");
         xcolor.Set("min", _colorMin);
         xcolor.Set("max", _colorMax);
         xcolor.Set("rate", _colorRate);
         xcolor.Set("merge", _colorMerge);
         // 读取透明信息
         FXmlNode xalpha = xconfig.CreateNode("Alpha");
         xalpha.Set("base", _alphaBase);
         xalpha.Set("rate", _alphaRate);
         //xalpha.Set("level", _alphaLevel);
         //xalpha.Set("merge", _alphaMerge);
         // 存储环境光
         FXmlNode xambient = xconfig.CreateNode("Ambient");
         _ambientColor.SaveConfigPower(xambient);
         xambient.Set("shadow", _ambientShadow);
         // 存储散射光
         FXmlNode xdiffuse = xconfig.CreateNode("Diffuse");
         _diffuseColor.SaveConfigPower(xdiffuse);
         xdiffuse.Set("shadow", _diffuseShadow);
         // 存储视角散射光
         FXmlNode xdiffuseview = xconfig.CreateNode("DiffuseView");
         _diffuseViewColor.SaveConfigPower(xdiffuseview);
         xdiffuseview.Set("shadow", _diffuseViewShadow);
         //// 存储高光
         FXmlNode xspecular = xconfig.CreateNode("Specular");
         _specularColor.SaveConfigPower(xspecular);
         xspecular.Set("base", _specularBase);
         xspecular.Set("level", _specularRate);
         //xspecular.Set("average", _specularAverage);
         xspecular.Set("shadow", _specularShadow);
         // 存储视角高光
         FXmlNode xspecularview = xconfig.CreateNode("SpecularView");
         _specularViewColor.SaveConfigPower(xspecularview);
         xspecularview.Set("base", _specularViewBase);
         xspecularview.Set("level", _specularViewRate);
         //xspecularview.Set("average", _specularViewAverage);
         xspecularview.Set("shadow", _specularViewShadow);
         // 存储反射
         FXmlNode xreflect = xconfig.CreateNode("Reflect");
         _reflectColor.SaveConfigPower(xreflect);
         xreflect.Set("merge", _reflectMerge);
         // 存储前折射
         FXmlNode xrefractFront = xconfig.CreateNode("RefractFront");
         _refractFrontColor.SaveConfigPower(xrefractFront);
         // 存储后折射
         FXmlNode xrefractBack = xconfig.CreateNode("RefractBack");
         _refractBackColor.SaveConfigPower(xrefractBack);
         // 存储不发光度
         FXmlNode xopacity = xconfig.CreateNode("Opacity");
         _opacityColorColor.SaveConfigPower(xopacity);
         xopacity.Set("rate", _opacityRate);
         xopacity.Set("alpha", _opacityAlpha);
         xopacity.Set("depth", _opacityDepth);
         xopacity.Set("transmittance", _opacityTransmittance);
         // 存储自发光
         FXmlNode xemissive = xconfig.CreateNode("Emissive");
         _emissiveColor.SaveConfigPower(xemissive);
      }

      //============================================================
      // <T>序列化内部数据到输出流。</T>
      //
      // @param output 输出流
      //============================================================
      public void LoadGroup(FDrMaterialGroup group) {
         // 存储属性
         _effectName = group.EffectName;
         _transformName = group.TransformName;
         // 存储设置
         _optionLight = group.OptionLight;
         _optionMerge = group.OptionMerge;
         _optionSort = group.OptionSort;
         _sortLevel = group.SortLevel;
         _optionAlpha = group.OptionAlpha;
         _optionDepth = group.OptionDepth;
         _optionCompare = group.OptionCompare;
         _optionDouble = group.OptionDouble;
         _optionShadow = group.OptionShadow;
         _optionShadowSelf = group.OptionShadowSelf;
         _optionDynamic = group.OptionDynamic;
         _optionTransmittance = group.OptionTransmittance;
         _optionOpacity = group.OptionOpacity;
      }

      //============================================================
      // <T>序列化内部数据到输出流。</T>
      //
      // @param output 输出流
      //============================================================
      public void Serialize(IOutput output) {
         // 存储属性
         output.WriteString(ThemeCode);
         output.WriteUTFString(_group.Label);
         // 存储设置
         SerializeInfo(output);
      }

      //============================================================
      // <T>序列化内部数据到输出流。</T>
      //
      // @param output 输出流
      //============================================================
      public void SerializeAll(IOutput output) {
         // 存储属性
         output.WriteString(_group.Code);
         //output.WriteUint32(0); // _group.CodeNumber
         //output.WriteInt32(0); // timeout
         //output.WriteString(_group.Code);
         //output.WriteUTFString(_group.Label);
         //output.WriteString(ThemeCode);
         // 存储设置
         SerializeInfo(output);
         // 存储贴图
         FVector<FDrMaterialTexture> textures = _group.Textures;
         output.WriteUint8((byte)textures.Count);
         foreach (FDrMaterialTexture texture in textures) {
            texture.Serialize(output);
         }
      }
   }
}
