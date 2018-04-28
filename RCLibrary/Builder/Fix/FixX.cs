using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCLibrary.Support;
namespace RCLibrary
{
    public static class FixX
    {
        public static TaskResults Run(Coaster coaster)
        {
            List<float> angles = new List<float>() { 90, 270, MathHelper.KeepBetween360Degrees(Builder.lastRuleIssueTrack.Yaw + 180) };
            return BuildToYaw.Run(coaster, angles);
        }
    }
}
