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
            //float x = coaster.Tracks[coaster.TrackCount - 1].X;
            //float y = coaster.Tracks[coaster.TrackCount - 1].Y;
            //float z = coaster.Tracks[coaster.TrackCount - 1].Z;

            ////Determine
            //x = x + (float)(Math.Cos(MathHelper.ToRadians(coaster.Tracks[coaster.TrackCount - 1].Yaw)) * Math.Cos(MathHelper.ToRadians(coaster.Tracks[coaster.TrackCount - 1].Pitch)) * Globals.TRACK_LENGTH * 10);
            //y = y + (float)(Math.Sin(MathHelper.ToRadians(coaster.Tracks[coaster.TrackCount - 1].Yaw)) * Math.Cos(MathHelper.ToRadians(coaster.Tracks[coaster.TrackCount - 1].Pitch)) * Globals.TRACK_LENGTH * 10);
            //z = z + (float)(Math.Sin(MathHelper.ToRadians(coaster.Tracks[coaster.TrackCount - 1].Pitch)) * Globals.TRACK_LENGTH * 10);
            ////Check if there in bonunds and that there is no track at that location

            ////Try To Build To them

            ////If Fail return fail.

        }
          
    }
}
