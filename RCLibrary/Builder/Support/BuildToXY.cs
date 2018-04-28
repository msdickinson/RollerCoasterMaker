using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary.Support
{
    public class BuildToXY 
    {
        public static TaskResults Run(Coaster coaster, float x, float y, float xRange, float yRange)
        {
            TaskResults results = TaskResults.Successful;
            List<BuildAction> buildActions = new List<BuildAction>();
            bool firstStrightTrack = true;
            float last = 0;
            float lastDiffernce = 0;
            float yawGoal = 0;
            results = BuildToPitch.Run(coaster, new List<float>() { 0 });
            if (results != TaskResults.Successful)
                return results;

            while (!((coaster.Tracks[coaster.TrackCountBuild - 1].X < x + (xRange / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].X > x - (xRange / 2)) && (coaster.Tracks[coaster.TrackCountBuild - 1].Y < y + (yRange / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].Y > y - (yRange / 2))) && results == TaskResults.Successful)
            {
                //Determine Best Yaw
                yawGoal = Convert.ToSingle(Math.Atan2(
                    (double)(y - coaster.Tracks[coaster.TrackCountBuild - 1].Y),
                    (double)(x - coaster.Tracks[coaster.TrackCountBuild - 1].X)) * 180 / Math.PI);

                if (yawGoal < 0)
                    yawGoal = yawGoal + 360;

                //Get YawGoal To Nearest Angle Game Can Handle
                int totalAdjustments = (int)(yawGoal / Globals.STANDARD_ANGLE_CHANGE);

                if ((yawGoal % 15) > Globals.STANDARD_ANGLE_CHANGE / 2)
                    totalAdjustments++;

                yawGoal = totalAdjustments * Globals.STANDARD_ANGLE_CHANGE;

                if (coaster.Tracks[coaster.TrackCountBuild - 1].Yaw == yawGoal)
                {
                    buildActions.Add(new BuildAction(TrackType.Stright));
                    results = Builder.BuildTracks(buildActions, coaster);

                    if (results != TaskResults.Successful)
                        return results;
                    buildActions.Clear();

                    float differnce = Math.Abs((coaster.Tracks[coaster.TrackCountBuild - 1].X - x) + (coaster.Tracks[coaster.TrackCountBuild - 1].Y - y));
                    if (!firstStrightTrack)
                    {
                        //This Means You Passed The Goal Point, This could have been done by turning, Or After the Fact. But You Are now going the wrong way.
                        if (differnce > lastDiffernce)
                            return TaskResults.Fail;
                    }
                    else
                        firstStrightTrack = true;

                    last = coaster.Tracks[coaster.TrackCountBuild - 1].X + coaster.Tracks[coaster.TrackCountBuild - 1].Y;
                    lastDiffernce = differnce;

                }
                else
                {
                    TrackType type = new TrackType();

                    if (coaster.Tracks[coaster.TrackCountBuild - 1].Yaw - yawGoal > 0)
                    {
                        if (Math.Abs(coaster.Tracks[coaster.TrackCountBuild - 1].Yaw - yawGoal) < 180)
                            type = TrackType.Right;
                        else
                            type = TrackType.Left;

                    }
                    else
                    {
                        if (Math.Abs(yawGoal - coaster.Tracks[coaster.TrackCountBuild - 1].Yaw) < 180)
                            type = TrackType.Left;
                        else
                            type = TrackType.Right;

                    }


                    buildActions.Add(new BuildAction(type));
                    results = Builder.BuildTracks(buildActions, coaster);

                    if (results != TaskResults.Successful)
                        return results;
                    buildActions.Clear();
                }

            }
            if (coaster.Tracks[coaster.TrackCountBuild - 1].X < x + (xRange / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].X > x - (xRange / 2) && (coaster.Tracks[coaster.TrackCountBuild - 1].Y < y + (yRange / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].Y > y - (yRange / 2)))
            {
                return TaskResults.Successful;
            }
            else
            {
                return TaskResults.Fail;
            }

        }
    }
}
