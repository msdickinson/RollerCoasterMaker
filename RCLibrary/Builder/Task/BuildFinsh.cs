using RCLibrary.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    class BuildFinsh : BuilderTask
    {
        public TaskResults Run(Coaster coaster)
        {
            TaskResults results = RCLibrary.Support.BuildToXY.Run(
                coaster,
                Globals.FINSH_AREA_X,
                Globals.FINSH_AREA_Y,
                Globals.FINSH_AREA_X_RANGE,
                Globals.FINSH_AREA_Y_RANGE);

            return results;
        }
    }
}
