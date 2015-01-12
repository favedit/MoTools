#include "MoCompressPicture.h"
#include "MoCompress.h"

using namespace MoCompress;

//============================================================
// <T>ѹ��ͼƬ��ɫ���ݡ�</T>
//============================================================
int RCompressPicture::Compress(array<System::Byte>^ output, int outputOffset, int outputLength, array<System::Byte>^ input, int inputOffset, int inputLength){
   int result = 0;
   size_t dstDataLen = inputLength + 1024;
   TByte* pCont = (TByte*)malloc(dstDataLen);
   // �����������
   TByte* pInput = (TByte*)malloc(inputLength);
   pin_ptr<System::Byte> inputPtr = &input[inputOffset];
   memcpy(pInput, (void*)inputPtr, inputLength);
   // ȡ�ÿ������
   TInt width  = *(TInt*)pInput[0];
   TInt height = *(TInt*)pInput[4];
   // ��ȥ��͸ߵ�8���ֽڡ�
   TInt colorDataLength = inputLength - 8;
   TByte* pColorDate = pInput + 8;
   // ������
   FComOctree* pTree = new FComOctree();
   FComColor color;
   for(TInt n = 0; n < colorDataLength; n += 4){
      color.red   = pColorDate[n];
      color.green = pColorDate[n + 1];
      color.blue  = pColorDate[n + 2];
      color.alpha = pColorDate[n + 3];
      pTree->AddColor(color);
   }
   TInt numberColorCount = pTree->GetColorCount();
   return result; 
}

//============================================================
// <T>��ѹͼƬ��ɫ���ݡ�</T>
//============================================================
int RCompressPicture::Uncompress(array<System::Byte>^ output, int outputOffset, int outputLength, array<System::Byte>^ input, int inputOffset, int inputLength){
   int result = 0;
   return result;
}
