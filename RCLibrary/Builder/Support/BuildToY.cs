using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary.Support
{
    class BuildToY 
    {
        public static TaskResults Run(Coaster coaster, float y, float withIn)
        {
            TaskResults results;

            //Go Left
            results = GoToY(coaster, y, withIn, TrackType.Left);

            //Go Right
            if (results != TaskResults.Successful)
                results = GoToY(coaster, y, withIn, TrackType.Right);

            return results;
        }
        private static TaskResults GoToY(Coaster coaster, float y, float withIn, TrackType type)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
            TaskResults results = TaskResults.Successful;
            float yawGoal = 0;

            if (y > coaster.LastTrack.Y)
                yawGoal = 0;
            else
                yawGoal = 180;

            bool firstStrightTrack = true;
            float lastY = 0;
            float lastDiffernce = 0;
            results = BuildToPitch.Run(coaster, new List<float>() { 0 });

            if (results != TaskResults.Successful)
                return results;

            while (!((coaster.LastTrack.Y < y + (withIn / 2) && coaster.LastTrack.Y > y - (withIn / 2))) && results == TaskResults.Successful)
            {
                if (coaster.LastTrack.Yaw == yawGoal)
                {

                    buildActions.Add(new BuildAction(TrackType.Stright));
                    results = Builder.BuildTracks(buildActions, coaster);

                    if (results != TaskResults.Successful)
                        return results;

                    buildActions.Clear();

                    float differnce = Math.Abs(coaster.LastTrack.Y - lastY);
                    if (!firstStrightTrack)
                    {
                        //This Means You Passed The Goal Point, This could have been done by turning, Or After the Fact. But You Are now going the wrong way.
                        if (differnce > lastDiffernce)
                            return TaskResults.Fail;
                    }
                    else
                        firstStrightTrack = true;

                    lastY = coaster.LastTrack.Y;
                    lastDiffernce = differnce;
                }
                else
                {

                    buildActions.Add(new BuildAction(type));
                    results = Builder.BuildTracks(buildActions, coaster);
                    buildActions.Clear();
                }

            }
            if (coaster.LastTrack.Y < y + (withIn / 2) && coaster.LastTrack.Y > y - (withIn / 2))
                return TaskResults.Successful;
            else
                return TaskResults.Fail;

        }
    }
}
