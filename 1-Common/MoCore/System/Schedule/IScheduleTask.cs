using System;
using System.Collections.Generic;
using System.Text;

namespace MObj.Windows.Schedule {

   public interface IScheduleTask {

      DateTime QueryNextRunTime(DateTime date);

   }

}
