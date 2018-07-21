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
            //Determine Left, Or Right to Try first
            TaskResults results = TaskResults.Fail;
            bool Left = true;
            float  yawGoal = Convert.ToSingle(Math.Atan2(
                                     (y - coaster.LastTrack.Y),
                                     (x - coaster.LastTrack.X)) * 180 / Math.PI);
            int totalAdjustments = (int)(yawGoal / Globals.STANDARD_ANGLE_CHANGE);

            if ((yawGoal % 15) > Globals.STANDARD_ANGLE_CHANGE / 2)
                totalAdjustments++;
            List<BuildAction> buildActions = new List<BuildAction>();
          

            yawGoal = totalAdjustments * Globals.STANDARD_ANGLE_CHANGE;
            Left = (Math.Abs(yawGoal - coaster.LastTrack.Yaw) < 180);
            for (int i = 0; i <= 3; i++)
            {
                //Try Level
                coaster.Reset();
                results = Runner(coaster, i, x, y, xRange, yRange, Left);
                if (results != TaskResults.Successful)
                {
                    coaster.Reset();
                    results = Runner(coaster, i, x, y, xRange, yRange, !Left);
                }

                //Try Going Up First
                if (results != TaskResults.Successful)
                {
                    coaster.Reset();
                    results = Runner(coaster, i, x, y, xRange, yRange, Left, 100);
                }
                if (results != TaskResults.Successful)
                {
                    coaster.Reset();
                    results = Runner(coaster, i, x, y, xRange, yRange, !Left, 100);
                }
                if (results != TaskResults.Successful)
                {
                    coaster.Reset();
                    results = Runner(coaster, i, x, y, xRange, yRange, Left, 100, true);
                }


                //Try Going Down First
                if (results != TaskResults.Successful)
                {
                    coaster.Reset();
                    results = Runner(coaster, i, x, y, xRange, yRange, Left, -100);
                }
                if (results != TaskResults.Successful)
                {
                    coaster.Reset();
                    results = Runner(coaster, i, x, y, xRange, yRange, !Left, -100);
                }
                if (results != TaskResults.Successful)
                {
                    coaster.Reset();
                    results = Runner(coaster, i, x, y, xRange, yRange, Left, -100, true);
                }


                //Reset Coaster
                if (results != TaskResults.Successful)
                {
                    coaster.Reset();
                }
            }
            
            return results;

        }

        private static TaskResults Runner(Coaster coaster, int RemoveTracks, float x, float y, float xRange, float yRange, bool TurnLeft, float ChangeZ = 0, bool changeYawAfterZChange = false, float ChangeZWithin = 50)
        {
            bool TurnedToAngleForceDirectionOnce = false;
            TaskResults results = TaskResults.Fail;
            List<BuildAction> buildActions = new List<BuildAction>();
            float yawGoal = Convert.ToSingle(Math.Atan2(
                                     (y - coaster.LastTrack.Y),
                                     (x - coaster.LastTrack.X)) * 180 / Math.PI);

  
            if (yawGoal < 0)
                yawGoal = yawGoal + 360;
            int totalAdjustments = (int)(yawGoal / Globals.STANDARD_ANGLE_CHANGE);

            if ((yawGoal % 15) > Globals.STANDARD_ANGLE_CHANGE / 2)
                totalAdjustments++;

            yawGoal = totalAdjustments * Globals.STANDARD_ANGLE_CHANGE;
            RemoveChunk removeChunk = new RemoveChunk();
            for (int j = 0; j < RemoveTracks * 5; j++)
            {
                removeChunk.Run(coaster);
            }
            results = Builder.BuildTracks(buildActions, coaster);
            if (results != TaskResults.Successful)
                return results;


            if (ChangeZ != 0)
            {
                results = BuildToZ.Run(coaster, coaster.LastTrack.Z + ChangeZ, 50);
                if (results != TaskResults.Successful)
                    return results;
            }

            if (changeYawAfterZChange)
            { 
                while (coaster.LastTrack.Yaw != yawGoal)
                {
                    if (TurnLeft)
                    {
                        buildActions.Add(new BuildAction(TrackType.Left));
                    }
                    else
                    {
                        buildActions.Add(new BuildAction(TrackType.Right));
                    }

                    results = Builder.BuildTracks(buildActions, coaster);

                    if (results != TaskResults.Successful)
                        return results;
                    buildActions.Clear();
                }
                TurnedToAngleForceDirectionOnce = true;
             }


            results = BuildToPitch.Run(coaster, new List<float>() { 0 });
            if (results != TaskResults.Successful)
                return results;


            while (!((coaster.LastTrack.X < x + (xRange / 2) && coaster.LastTrack.X > x - (xRange / 2)) && (coaster.LastTrack.Y < y + (yRange / 2) && coaster.LastTrack.Y > y - (yRange / 2))) && results == TaskResults.Successful)
            {
                yawGoal = Convert.ToSingle(Math.Atan2(
                          (y - coaster.LastTrack.Y),
                          (x - coaster.LastTrack.X)) * 180 / Math.PI);

                if (yawGoal < 0)
                    yawGoal = yawGoal + 360;

                //Get YawGoal To Nearest Angle Game Can Handle
                totalAdjustments = (int)(yawGoal / Globals.STANDARD_ANGLE_CHANGE);

                if ((yawGoal % 15) > Globals.STANDARD_ANGLE_CHANGE / 2)
                    totalAdjustments++;

                yawGoal = totalAdjustments * Globals.STANDARD_ANGLE_CHANGE;


             
                if (coaster.LastTrack.Yaw == yawGoal)
                {
                    TurnedToAngleForceDirectionOnce = true;
                    float lastDistance = Math.Abs((coaster.LastTrack.X - x) + (coaster.LastTrack.Y - y));
                    buildActions.Add(new BuildAction(TrackType.Stright));
                    results = Builder.BuildTracks(buildActions, coaster);

                    if (results != TaskResults.Successful)
                        return results;
                    buildActions.Clear();

                    float distance = Math.Abs((coaster.LastTrack.X - x) + (coaster.LastTrack.Y - y));

                    if (distance >= lastDistance)
                        return TaskResults.Fail;

                    lastDistance = distance;

                }
                else if(!TurnedToAngleForceDirectionOnce)
                {
                    if (TurnLeft)
                    {
                        buildActions.Add(new BuildAction(TrackType.Left));
                    }
                    else
                    {
                        buildActions.Add(new BuildAction(TrackType.Right));
                    }
                 
                    results = Builder.BuildTracks(buildActions, coaster);

                    if (results != TaskResults.Successful)
                        return results;
                    buildActions.Clear();
                }
                else
                {
                    TrackType type = new TrackType();

                    if (coaster.LastTrack.Yaw - yawGoal > 0)
                    {
                        if (Math.Abs(coaster.LastTrack.Yaw - yawGoal) < 180)
                            type = TrackType.Right;
                        else
                            type = TrackType.Left;

                    }
                    else
                    {
                        if (Math.Abs(yawGoal - coaster.LastTrack.Yaw) < 180)
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

            if (coaster.LastTrack.X < x + (xRange / 2) && coaster.LastTrack.X > x - (xRange / 2) && (coaster.LastTrack.Y < y + (yRange / 2) && coaster.LastTrack.Y > y - (yRange / 2)))
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
