using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCLibrary.Support;
namespace RCLibrary
{
    public static class FixTrackCollison 
    {
        public static TaskResults Run(Coaster coaster)
        {
            TaskResults taskResults = TaskResults.Successful;
            taskResults = BuildToYaw.Run(
                coaster,
                new List<float>()
                {
                    MathHelper.KeepBetween360Degrees(coaster.Tracks[Rules.lastCollsionIndex].Yaw),
                    MathHelper.KeepBetween360Degrees(coaster.Tracks[Rules.lastCollsionIndex].Yaw + 180),
                    MathHelper.KeepBetween360Degrees(Builder.lastRuleIssueTrack.Yaw + 180)
                });
            return taskResults;
            //if(taskResults == TaskResults.Successful)
            //{
            //    return taskResults;
            //}
            ////Go To XYZ
            //float x = coaster.LastTrack.X;
            //float y = coaster.LastTrack.Y;
            //float z = coaster.LastTrack.Z;

            ////Determine
            //x = x + (float)(Math.Cos(MathHelper.ToRadians(coaster.LastTrack.Yaw)) * Math.Cos(MathHelper.ToRadians(coaster.LastTrack.Pitch)) * Globals.TRACK_LENGTH * 10);
            //y = y + (float)(Math.Sin(MathHelper.ToRadians(coaster.LastTrack.Yaw)) * Math.Cos(MathHelper.ToRadians(coaster.LastTrack.Pitch)) * Globals.TRACK_LENGTH * 10);
            //z = z + (float)(Math.Sin(MathHelper.ToRadians(coaster.LastTrack.Pitch)) * Globals.TRACK_LENGTH * 10);
            ////Check if there in bonunds and that there is no track at that location

            ////Try To Build To them

            ////If Fail return fail.

        }
          
    }
}
