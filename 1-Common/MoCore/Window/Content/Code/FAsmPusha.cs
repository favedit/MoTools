using MO.Common.Collection;

namespace MO.Core.Window.Context.Asm.Code {

   public class FAsmPusha : FAsmOpCode {

      public override bool Parse(FAsmCommand cmd, FBytes memory, int offset) {
         // 01100000
         return true;
      }

   }

}
