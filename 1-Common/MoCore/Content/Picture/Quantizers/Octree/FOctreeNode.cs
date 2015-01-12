﻿using System;
using System.Drawing;
using MO.Common.Lang;

namespace MO.Core.Content.Picture.Quantizers.Octree
{
   //============================================================
   // <T>八叉树节点。</T>
   //============================================================
   public class FOctreeNode
   {
      private static readonly Byte[] Mask = new Byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

      protected int _red;

      protected int _green;

      protected int _blue;

      protected int _pixelCount;

      protected int _paletteIndex;

      protected FOctreeNode[] _nodes = new FOctreeNode[8];

      //============================================================
      // <T>构造八叉树节点。</T>
      //============================================================
      public FOctreeNode(FOctreeQuantizer quantizer, int level) {
         if(level < 7) {
            quantizer.AddLevelNode(level, this);
         }
      }

      //============================================================
      // <T>判断是否是颜色节点。</T>
      //============================================================
      public Boolean IsLeaf {
         get { return _pixelCount > 0; }
      }

      //============================================================
      // <T>获得使用节点集合。</T>
      //============================================================
      public void FilterLeaf(FOctreeNodes leafs) {
         if(_pixelCount > 0) {
            leafs.Push(this);
         }
         for(int index = 0; index < 8; index++) {
            FOctreeNode node = _nodes[index];
            if(null != node) {
               node.FilterLeaf(leafs);
            }
         }
      }

      //============================================================
      // <T>获得使用节点集合。</T>
      //============================================================
      public int NodeCount() {
         int count = 0;
         for(int index = 0; index < 8; index++) {
            FOctreeNode node = _nodes[index];
            if(null != node) {
               count++;
            }
         }
         return count;
      }

      //============================================================
      // <T>获得使用节点集合。</T>
      //============================================================
      public int LeafCount() {
         int count = 0;
         if(_pixelCount > 0) {
            count++;
         }
         for(int index = 0; index < 8; index++) {
            FOctreeNode node = _nodes[index];
            if(null != node) {
               count += node.LeafCount();
            }
         }
         return count;
      }

      //============================================================
      // <T>获得颜色。</T>
      //============================================================
      public Color Color {
         get {
            Color result;
            if(IsLeaf) {
               if(_pixelCount == 1) {
                  result = Color.FromArgb(255, _red, _green, _blue);
               } else {
                  result = Color.FromArgb(255, _red / _pixelCount, _green / _pixelCount, _blue / _pixelCount);
               }
            } else {
               throw new InvalidOperationException("Cannot retrieve a color for other node than leaf.");
            }
            return result;
         }
      }

      //============================================================
      // <T>获得使用节点像素数。</T>
      //============================================================
      public int ActiveNodesPixelCount {
         get {
            int result = _pixelCount;
            for(Int32 index = 0; index < 8; index++) {
               FOctreeNode node = _nodes[index];
               if(null != node) {
                  result += node._pixelCount;
               }
            }
            return result;
         }
      }

      //============================================================
      // <T>增加颜色。</T>
      //============================================================
      public void AddColor(Color color, int level, FOctreeQuantizer quantizer) {
         if(level == 8) {
            _red += color.R;
            _green += color.G;
            _blue += color.B;
            _pixelCount++;
         } else if(level <  8) {
            int index = GetColorIndexAtLevel(color, level);
            FOctreeNode node = _nodes[index];
            if(null == node) {
               node = new FOctreeNode(quantizer, level);
               _nodes[index] = node;
            }
            node.AddColor(color, level + 1, quantizer);
         }
      }

      //============================================================
      // <T>获得调色板索引。</T>
      //============================================================
      public Int32 GetPaletteIndex(Color color, Int32 level) {
         int result = 0;
         if(_pixelCount > 0) {
            result = _paletteIndex;
         } else {
            int index = GetColorIndexAtLevel(color, level);
            result = _nodes[index].GetPaletteIndex(color, level + 1);
         }
         return result;
      }

      //============================================================
      // <T>移除子节点。</T>
      //============================================================
      public int RemoveLeaves() {
         if(_pixelCount > 0) {
            throw new FFatalException();
         }
         int count = 0;
         for(int n = 0; n < 8; n++) {
            FOctreeNode node = _nodes[n];
            if(null != node) {
               // 计算颜色总和
               _red += node._red;
               _green += node._green;
               _blue += node._blue;
               // 计算使用数
               _pixelCount += node._pixelCount;
               // 移除子节点
               _nodes[n] = null;
               count++;
            }
         }
         return (count > 0) ? count - 1 : 0;
      }

      //============================================================
      // <T>获得颜色级别。</T>
      //============================================================
      private static Int32 GetColorIndexAtLevel(Color color, int level) {
         bool r = ((color.R & Mask[level]) == Mask[level]);
         bool g = ((color.G & Mask[level]) == Mask[level]);
         bool b = ((color.B & Mask[level]) == Mask[level]);
         return (r ? 4 : 0) | (g ? 2 : 0) | (b ? 1 : 0);
      }

      //============================================================
      // <T>设置调色板索引。</T>
      //============================================================
      internal void SetPaletteIndex(Int32 index) {
         _paletteIndex = index;
      }

      //============================================================
      // <T>获得字符串信息。</T>
      //============================================================
      public override string ToString() {
         return "Node: leaf=" + LeafCount() + ", node=" + NodeCount() + " pixel=" + _pixelCount;
      }
   }
}