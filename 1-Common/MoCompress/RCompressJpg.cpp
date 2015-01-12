#include <iomanip>
#include <MoJpeg.h>
#include "MoCompress.h"

using namespace MoCompress;

//============================================================
// <T>ѹ��ͼƬ��ɫ���ݡ�</T>
// <P>channelCd=1:RGB��ɫ</P>
// <P>channelCd=2:Alpha��ɫΪ��ɫͼ</P>
// <P>channelCd=3:Alpha��ɫΪ��ɫͼ</P>
//============================================================
int RCompressJpg::Compress(array<System::Byte>^ output, int outputOffset, int outputLength, array<System::Byte>^ input, int inputOffset, int inputLength, int width, int height, int quality, int channelCd){
   // �����������
   unsigned char* pInput = (unsigned char*)malloc(inputLength);
   pin_ptr<System::Byte> inputPtr = &input[inputOffset];
   memcpy(pInput, (void*)inputPtr, inputLength);
   // ��������ʼ����ѹ������ͬʱ�ƶ�������Ϣ������
   struct jpeg_compress_struct cinfo;
   // ���ô�����
   struct jpeg_error_mgr jerr;
   cinfo.err = jpeg_std_error(&jerr);
   jpeg_create_compress(&cinfo);
   cinfo.progressive_mode = false;
   cinfo.image_width = width;
   cinfo.image_height = height;
   if(3 == channelCd){
      cinfo.input_components = 1;
      cinfo.in_color_space = JCS_GRAYSCALE;
   }else{
      cinfo.input_components = 3;
      cinfo.in_color_space = JCS_RGB;
   }
   // �趨�������
   unsigned char* pWrite = NULL;
   unsigned long writeLength = 0;
   jpeg_mem_dest(&cinfo, &pWrite, &writeLength);
   // ����Ĭ�ϲ���
   jpeg_set_defaults(&cinfo);
   // ����ѹ��Ʒ��
   jpeg_set_quality(&cinfo, quality, TRUE);
   //jpeg_quality_scaling(quality);
   //jpeg_set_linear_quality(&cinfo, 16, TRUE);
   // ��ʼѹ������
   jpeg_start_compress(&cinfo, TRUE);
   // ѹ��ȫ������
   int widthStride = 0;
   if(0 == channelCd){
      widthStride = 3 * width;
   }else{
      widthStride = 4 * width;
   }
   unsigned char* pLine = (unsigned char*)malloc(widthStride);
   JSAMPROW rowPtr = pLine;
   while(cinfo.next_scanline < cinfo.image_height){
      int linePosition = 0;
      int inputPosition = widthStride * cinfo.next_scanline;
      for(int n = 0; n < width; n++){
         if(channelCd == 0){
            pLine[linePosition    ] = pInput[inputPosition + 2];
            pLine[linePosition + 1] = pInput[inputPosition + 1];
            pLine[linePosition + 2] = pInput[inputPosition    ];
            linePosition += 3;
            inputPosition += 3;
         }else if(channelCd == 1){
            pLine[linePosition    ] = pInput[inputPosition + 2];
            pLine[linePosition + 1] = pInput[inputPosition + 1];
            pLine[linePosition + 2] = pInput[inputPosition    ];
            linePosition += 3;
            inputPosition += 4;
         }else if(channelCd == 2){
            pLine[linePosition    ] = pInput[inputPosition + 3];
            pLine[linePosition + 1] = pInput[inputPosition + 3];
            pLine[linePosition + 2] = pInput[inputPosition + 3];
            linePosition += 3;
            inputPosition += 4;
         }else if(channelCd == 3){
            pLine[linePosition    ] = pInput[inputPosition + 3];
            linePosition += 1;
            inputPosition += 4;
         }
      }
      jpeg_write_scanlines(&cinfo, &rowPtr, 1);
   }
   free(pLine);
   // ѹ��ȫ�����
   jpeg_finish_compress(&cinfo);
   // ���ѹ������
   if(writeLength > 0){
      pin_ptr<System::Byte> outputPtr = &output[outputOffset];
      memcpy((void*)outputPtr, (void*)pWrite, writeLength);
   }
   // �ͷ�������Դ
   jpeg_destroy_compress(&cinfo);
   free(pInput);
   return writeLength; 
}

//============================================================
// <T>��ѹͼƬ��ɫ���ݡ�</T>
//============================================================
int RCompressJpg::Uncompress(array<System::Byte>^ output, int outputOffset, int outputLength, array<System::Byte>^ input, int inputOffset, int inputLength){
   int result = 0;
   return result;
}
