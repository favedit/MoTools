using MO.Common.Collection;

namespace MO.Core.Window.Context.Asm.Code {

   public class FAsmPushf : FAsmOpCode {

      public override bool Parse(FAsmCommand cmd, FBytes memory, int offset) {
         // 10011100
         return true;
      }

   }

}
