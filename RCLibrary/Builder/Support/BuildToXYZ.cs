using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary.Support
{
    class BuildToXYZ 
    {
        public static TaskResults Run(Coaster coaster, float x, float y, float z, float xRange, float yRange, float zRange)
        {
            TaskResults results;
            results = GoToXYZ(coaster, x, y, z,xRange, yRange, zRange);
            return results;
        }
        private static TaskResults GoToXYZ(Coaster coaster, float x, float y, float z, float xRange, float yRange, float zRange)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
            TaskResults results = TaskResults.Successful;

            bool firstStrightTrack = true;
            float last = 0;
            float lastDiffernce = 0;
            float yawGoal = 0;
            float pitchGoal = 0;

            while (!
                (coaster.Tracks[coaster.TrackCountBuild - 1].X < x + (xRange / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].X > x - (xRange / 2))
                && (coaster.Tracks[coaster.TrackCountBuild - 1].Y < y + (yRange / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].Y > y - (yRange / 2))
                && (coaster.Tracks[coaster.TrackCountBuild - 1].Z <= (z + (zRange / 2)) && coaster.Tracks[coaster.TrackCountBuild - 1].Z >= (z - (zRange / 2)))
                && results == TaskResults.Successful)
            {
                //Determine Best Yaw
                yawGoal = Convert.ToSingle(Math.Atan2((double)(coaster.Tracks[coaster.TrackCountBuild - 1].Y),
                                                      (double)(x - coaster.Tracks[coaster.TrackCountBuild - 1].X)) * 180 / Math.PI);

                if (yawGoal < 0)
                    yawGoal = yawGoal + 360;

                //Get YawGoal To Nearest Angle Game Can Handle
                int totalAdjustments = (int)(yawGoal / Globals.STANDARD_ANGLE_CHANGE);

                if ((yawGoal % 15) > Globals.STANDARD_ANGLE_CHANGE / 2)
                    totalAdjustments++;

                yawGoal = totalAdjustments * Globals.STANDARD_ANGLE_CHANGE;

                //If Z to High, Z To Low
                if (coaster.Tracks[coaster.TrackCountBuild - 1].Z <= (z + (zRange / 2)) && coaster.Tracks[coaster.TrackCountBuild - 1].Z >= (z - (zRange / 2)))
                    pitchGoal = 0;
                else if ((z - coaster.Tracks[coaster.TrackCountBuild - 1].Z) > 0)
                    pitchGoal = 90;
                else
                    pitchGoal = 270;

                //Determine Best Yaw
                if (coaster.Tracks[coaster.TrackCountBuild - 1].Yaw == yawGoal && coaster.Tracks[coaster.TrackCountBuild - 1].Pitch == pitchGoal)
                {

                    buildActions.Add(new BuildAction(TrackType.Stright));
                    results = Builder.BuildTracks(buildActions, coaster);
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
                    int yawDirection = 0;
                    int pitchDirection = 0;
                    if (!(coaster.Tracks[coaster.TrackCountBuild - 1].Yaw == yawGoal))
                    {
                        if (coaster.Tracks[coaster.TrackCountBuild - 1].Yaw - yawGoal > 0)
                        {
                            if (Math.Abs(coaster.Tracks[coaster.TrackCountBuild - 1].Yaw - yawGoal) < 180)
                                yawDirection = -1; //Right
                            else
                                yawDirection = 1; //Left

                        }
                        else
                        {
                            if (Math.Abs(yawGoal - coaster.Tracks[coaster.TrackCountBuild - 1].Yaw) < 180)
                                yawDirection = 1; //Left
                            else
                                yawDirection = -1; //Right

                        }
                    }
                    //
                    if (!(coaster.Tracks[coaster.TrackCountBuild - 1].Pitch == pitchGoal))
                    {
                        if (coaster.Tracks[coaster.TrackCountBuild - 1].Pitch - pitchGoal > 0)
                        {
                            if ((coaster.Tracks[coaster.TrackCountBuild - 1].Pitch - pitchGoal > 360 - coaster.Tracks[coaster.TrackCountBuild - 1].Pitch))
                                pitchDirection = 1; //Up
                            else
                                pitchDirection = -1; //Down

                        }
                        else
                        {
                            if ((pitchGoal - coaster.Tracks[coaster.TrackCountBuild - 1].Pitch > 360 - pitchGoal))
                                pitchDirection = -1; //Down
                            else
                                pitchDirection = 1; //Up
                        }
                    }

                    buildActions.Add(new BuildAction(TrackType.Custom, Globals.STANDARD_ANGLE_CHANGE * yawDirection, Globals.STANDARD_ANGLE_CHANGE * pitchDirection));
                    results = Builder.BuildTracks(buildActions, coaster);
                    buildActions.Clear();
                }

            }

            if ((coaster.Tracks[coaster.TrackCountBuild - 1].X < x + (xRange / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].X > x - (xRange / 2)) 
                && (coaster.Tracks[coaster.TrackCountBuild - 1].Y < y + (yRange / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].Y > y - (yRange / 2)) 
                && (coaster.Tracks[coaster.TrackCountBuild - 1].Z <= (z + (zRange / 2)) && coaster.Tracks[coaster.TrackCountBuild - 1].Z >= (z - (zRange / 2))))
                return TaskResults.Successful;
            else
                return TaskResults.Fail;
        }
    }
}
