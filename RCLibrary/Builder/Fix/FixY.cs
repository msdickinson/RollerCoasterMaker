using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCLibrary.Support;
namespace RCLibrary
{
    public static class FixY
    {
        public static TaskResults Run(Coaster coaster)
        {
            List<float> angles = new List<float>() { 0, 180, MathHelper.KeepBetween360Degrees(Builder.lastRuleIssueTrack.Yaw + 180) };
            return BuildToYaw.Run(coaster, angles);
        }
    }
}
