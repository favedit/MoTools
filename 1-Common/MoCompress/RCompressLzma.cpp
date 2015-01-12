#include <MoLzma.h>
#include "MoCompress.h"

using namespace MoCompress;

typedef unsigned char uint8_t;
typedef unsigned short uint16_t;
typedef unsigned int uint32_t;
typedef unsigned long long uint64_t;
typedef char int8_t;
typedef short int16_t;
typedef int int32_t;
typedef long long int64_t;

static void *SzAlloc(void *p, size_t size) { p = p; return malloc(size); }
static void SzFree(void *p, void *address) { p = p; free(address); }
static ISzAlloc g_Alloc = { SzAlloc, SzFree };

//============================================================
#pragma pack(1)
struct SLzmaContent{
   uint8_t  propties[5];
   uint64_t length;
   uint8_t  data[1];
};
#pragma pack()

//============================================================
// <T>ѹ���������ݵ�������ݡ�</T>
//============================================================
int RCompressLzma::Compress(array<System::Byte>^ output, int outputOffset, int outputLength, array<System::Byte>^ input, int inputOffset, int inputLength){
   int result = 0;
   size_t targetLength = outputLength;
   SLzmaContent* pContent = (SLzmaContent*)malloc(targetLength);
   // �����������
   char* pInput = (char*)malloc(inputLength);
   pin_ptr<System::Byte> inputPtr = &input[inputOffset];
   memcpy(pInput, (void*)inputPtr, inputLength);
   // ����ѹ������
   size_t propLen = 5;
   CLzmaEncProps props;
   LzmaEncProps_Init(&props);
   props.level = 9;
   props.dictSize = (1 << 22);
   props.lc = 3;
   props.lp = 0;
   props.pb = 2;
   props.fb = 273;
   props.numThreads = 2;
   props.writeEndMark = 1;
   // ѹ������
   SRes lzmaResult = LzmaEncode(pContent->data, &targetLength, (const unsigned char*)pInput, inputLength, &props, pContent->propties, &propLen, 1, NULL, &g_Alloc, &g_Alloc);
   if(SZ_OK == lzmaResult){
      targetLength += 5 + 8;
      pContent->length = inputLength;
      result = targetLength;
   }else{
      result = -lzmaResult;
   }
   // �������
   if(targetLength < (size_t)outputLength){
      pin_ptr<System::Byte> outputPtr = &output[outputOffset];
      memcpy((void*)outputPtr, (void*)pContent, targetLength);
   }else{
      result = 0;
   }
   // �ͷ��ڴ�
   free(pInput);
   free(pContent);
   return result;
}

//============================================================
// <T>��ѹ���������ݵ�������ݡ�</T>
//============================================================
int RCompressLzma::Uncompress(array<System::Byte>^ output, int outputOffset, int outputLength, array<System::Byte>^ input, int inputOffset, int inputLength){
   int result = 0;
   // �����������
   char* pInput = (char*)malloc(inputLength);
   pin_ptr<System::Byte> inputPtr = &input[inputOffset];
   memcpy(pInput, (void*)inputPtr, inputLength);
   SLzmaContent* pContent = (SLzmaContent*)pInput;
   // �ж���������С
   if(pContent->length < (size_t)outputLength){
      unsigned char* pOutput = (unsigned char*)malloc(pContent->length);
      // ��ѹ������
      size_t sourceLength = pContent->length;
      size_t targetLength;
      ELzmaStatus stat;
      SRes lzmaResult = LzmaDecode(pOutput, &targetLength, pContent->data, &sourceLength, pContent->propties, 5, LZMA_FINISH_END, &stat, &g_Alloc);
      if(SZ_OK == lzmaResult){
         // �������
         pin_ptr<System::Byte> outputPtr = &output[outputOffset];
         memcpy((void*)outputPtr, (void*)pOutput, targetLength);
         result = targetLength;
      }else{
         result = -lzmaResult;
      }
      free(pOutput);
   }else{
      result = 0;
   }
   // �ͷ��ڴ�
   free(pInput);
   return result;
}
