using System;
using System.Collections.Generic;
using System.Text;

namespace MO.Core.Window.Api {

   [Flags]
   public enum ECmf : uint {
      NORMAL = 0x00000000,
      DEFAULTONLY = 0x00000001,
      VERBSONLY = 0x00000002,
      EXPLORE = 0x00000004,
      NOVERBS = 0x00000008,
      CANRENAME = 0x00000010,
      NODEFAULT = 0x00000020,
      INCLUDESTATIC = 0x00000040,
      EXTENDEDVERBS = 0x00000100,
      RESERVED = 0xffff0000
   }

}
