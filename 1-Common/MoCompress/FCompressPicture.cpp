#include "MoCompressPicture.h"

namespace MoCompress {

const TByte FComOctreeNode::Mask[FComOctreeNode::MaxNumLevel] = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
const FComColor backgroundColor;

void ConvertAlpha(FComColor& outColor, const FComColor& backgroundColor) {
   if(outColor.alpha < 255){
      double colorFactor = outColor.alpha / 255.0f;
      double backgroundFactor = (255 - outColor.alpha) / 255.0f;
      outColor.red   = (int)(outColor.red * colorFactor + backgroundColor.red * backgroundFactor);
      outColor.green = (int)(outColor.green * colorFactor + backgroundColor.green * backgroundFactor);
      outColor.blue  = (int)(outColor.blue * colorFactor + backgroundColor.blue * backgroundFactor);
      outColor.alpha = 255;
   }
}

//============================================================
// <T>�˲����ڵ㹹�졣</T>
//
// @param level 
// @param parent
//============================================================
FComOctreeNode::FComOctreeNode(TInt level, FComOctree* parent){
   red   = 0;
   green = 0;
   blue  = 0;
   pixelCount = 0;
   paletteIndex = 0;
   for(TInt n = 0; n < MaxNumLevel; n++){
      nodes[n] = NULL;
   }
   // ��ӵ��������Ĳ�������
   if(level < FComOctree::MaxNumList){
      parent->AddLevelNode(level, this);
   }
}

//============================================================
// <T>�˲����ڵ�����</T>
//============================================================
FComOctreeNode::~FComOctreeNode(){
   for(TInt n = 0; n < MaxNumLevel; n++){
      if(nodes[n]){
         delete nodes[n];
         nodes[n] = NULL;
      }
   }
}

//============================================================
// <T>ȡ��һ���ڵ����ɫ��</T>
//============================================================
FComColor FComOctreeNode::Color(){
   FComColor color;
   if(IsLeaf()){
      if(1 == pixelCount){
         color.red   = red;
         color.green = green;
         color.blue  = blue;
         color.alpha = 255;
         return color;
      }else{
         color.red   = red    / pixelCount;
         color.green = green  / pixelCount;
         color.blue  = blue   / pixelCount;
         color.alpha = 255;
         return color;
      }
   }
   return color;
}

//============================================================
// <T>ͳ�ƽڵ㼰�ӽڵ��������������</T>
//============================================================
TInt FComOctreeNode::ActiveNodesPixelCount(){
   TInt result = pixelCount;
   for(TInt n = 0; n < MaxNumChild; n++){
      if(nodes[n]){
         result += nodes[n]->pixelCount;
      }
   }
   return result;
}

//============================================================
// <T>ȡ������Ҷ�ӽڵ�ļ��ϡ�</T>
//
// @param outNodes ����Ľڵ㼯�������� 
//============================================================
void FComOctreeNode::ActiveNodes(FComOctreeNodeList& outNodes){
   for(TInt n = 0; n < MaxNumChild; n++){
      if(nodes[n]){
         if(IsLeaf()){
            outNodes.push_back(nodes[n]);
         }else{
            nodes[n]->ActiveNodes(outNodes);
         }
      }
   }
}

//============================================================
// <T>�ڵ�����һ����ɫ��</T>
//
// @param color   �������ɫ
// @param level   �㼶
// @param parent  ������
//============================================================
void FComOctreeNode::AddColor(FComColor& color, TInt level, FComOctree* parent){
   if(MaxNumLevel == level){
      red   += color.red;
      green += color.green;
      blue  += color.blue;
      pixelCount++;
   }else if(level < MaxNumLevel){
      TInt index = GetColorIndexAtLevel(color, level);
      if(NULL == nodes[index]){
         nodes[index] = new FComOctreeNode(level, parent);
      }
      // ��������һ�㼶�����ɫ
      nodes[index]->AddColor(color, level + 1, parent);
   }
}

//============================================================
// <T>ȡ���ƶ���ɫ�ĵ�ɫ��������</T>
//
// @param color �ƶ���ɫ
// @param level �㼶
//============================================================
TInt FComOctreeNode::GetPaletteIndex(FComColor& color, TInt level) {
   TInt result = -1;
   if(IsLeaf()){
      result = paletteIndex;
   }else{
      TInt index = GetColorIndexAtLevel(color, level);
      if(nodes[index]){
         result = nodes[index]->GetPaletteIndex(color, level + 1);
      }   
   }
   return result;
}

//============================================================
// <T>�Ƴ�����Ҷ�ӽڵ㡣</T>
//============================================================
TInt FComOctreeNode::RemoveLeaves(){
   TInt result = 0;
   for(TInt n = 0; n < MaxNumChild; n++){
      if(nodes[n]){
         red   += nodes[n]->red;
         green += nodes[n]->green;
         blue  += nodes[n]->blue;
         pixelCount += nodes[n]->pixelCount;
         delete nodes[n];
         nodes[n] = NULL;
         result++;
      }
   }
   return result - 1;
}

//============================================================
// <T>����ƶ���ɫ���ƶ��㼶�Ĵ��������</T>
//
// @param color �ƶ���ɫ
// @param level �㼶
//============================================================
TInt FComOctreeNode::GetColorIndexAtLevel(FComColor& color, TInt level){
   return   ((color.red    & Mask[level]) == Mask[level] ? 4 : 0) |
            ((color.green  & Mask[level]) == Mask[level] ? 2 : 0) |
            ((color.blue   & Mask[level]) == Mask[level] ? 1 : 0);
}

//============================================================
// <T>���õ�ɫ��������š�</T>
//============================================================
void FComOctreeNode::SetPaletteIndex(TInt index){
   paletteIndex = index;
}

//============================================================
// <T>�˲������캯����</T>
//============================================================
FComOctree::FComOctree(){
   _root = new FComOctreeNode(0, this);
}

//============================================================
// <T>���������һ����ɫ��</T>
// @param color �ƶ���ɫ
//============================================================
void FComOctree::AddColor(FComColor& color){
   // ת��Ϊ͸����������ɫ��
   ConvertAlpha(color, backgroundColor);
   _root->AddColor(color, 0, this);
}


//============================================================
// <T>ȡ�����д��ڵ���ɫ��������</T>
//============================================================
TInt FComOctree::GetColorCount(){
   FComOctreeNodeList nodes;
   Leaves(nodes);
   return nodes.size();
}

//============================================================
// <T>��������ɫʹ��Ȩ������ѡ���ϴ�Ȩ�صĵ�ɫ�����ϡ�</T>
//
// @param outPalettes   ����ĵ�ɫ�弯��  
// @param count         ѡȡ�ĵ�ɫ������
//============================================================
void FComOctree::MakePalette(FComColorVector& outPalettes, TInt count){
   FComOctreeNodeList nodes;
   Leaves(nodes);
   TInt leafCount = nodes.size();
   TInt paletteIndex = 0;
   FComOctreeNodeList* pLevel;
   list<FComOctreeNode*>::iterator iterator;
   for(TInt level = MaxNumList - 1; level >= 0; level--){
      pLevel = &_levels[level];
      if(pLevel->size() > 0){ 
         FComOctree::QsortOctreeNodesInList(*pLevel);
         iterator = pLevel->begin();
         while(iterator != pLevel->end()){
            leafCount -= (*iterator)->RemoveLeaves();
            if(leafCount <= count){
               break;
            }
            iterator++;
         }
         if(leafCount <= count){
            break;
         }
      }
   }
   nodes.clear();
   Leaves(nodes);
   iterator = nodes.begin();
   while(iterator != nodes.end()){
      outPalettes.push_back((*iterator)->Color());
      (*iterator)->SetPaletteIndex(paletteIndex++);
      iterator++;
   }
   if(nodes.size() == 0){
      // TODO : debug
   }
}

//============================================================
// <T>�ҳ���ɫ��������</T>
//============================================================
TInt FComOctree::FindPaletteIndex(FComColor& color){
   // ת��Ϊ͸����������ɫ��
   ConvertAlpha(color, backgroundColor);
   return _root->GetPaletteIndex(color, 0);
}

//============================================================
// <T>����������ڵ��Ҷ�ӽڵ㡣</T>
//============================================================
void FComOctree::Clear(){
   for(TInt n = 0; n < MaxNumList; n++){
       _levels[n].clear();
   }
   delete _root;
   _root = new FComOctreeNode(0, this);
}

//============================================================
// <T>�õ�����Ҷ�ӽڵ�ļ��ϡ�</T>
//
// @param outNodes  Ҷ�ӽڵ�ļ��ϡ�
//============================================================
void FComOctree::Leaves(FComOctreeNodeList& outNodes){
   _root->ActiveNodes(outNodes);
}

//============================================================
// <T>��ӽڵ㵽�������С�</T>
//============================================================
void FComOctree::AddLevelNode(TInt level, FComOctreeNode* node){
   _levels[level].push_back(node);
}

//============================================================
// <T>���ڵ㼯����������</T>
//
//  @param list ��������ļ��ϡ�
//============================================================
void FComOctree::QsortOctreeNodesInList(FComOctreeNodeList& list){
   list.sort(FComOctree::CompareOctreeNode);
}

//============================================================
// <T>��������ڵ�ȽϺ�����</T>
//
// @param one        ����ȽϽڵ�һ
// @param otherOne   ����ȽϽڵ��
//============================================================
TBool FComOctree::CompareOctreeNode(FComOctreeNode* one, FComOctreeNode* otherOne){
   return one->ActiveNodesPixelCount() > otherOne->ActiveNodesPixelCount();
}

}  // end namespace
