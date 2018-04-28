using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCLibrary.Support;
namespace RCLibrary
{
    public static class FixZ
    {
        public static List<float> angles = new List<float>() { 0, 180 };
        public static TaskResults Run(Coaster coaster)
        {
            return BuildToPitch.Run(coaster, angles);
        }
    }
}
