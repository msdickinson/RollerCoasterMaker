using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary.Support
{
    class BuildToX 
    {
        public static TaskResults Run(Coaster coaster, float x, float withIn)
        {
            TaskResults results;

            //Go Left
            results = GoToX(coaster, x, withIn, TrackType.Left);
             
            //Go Right
            if(results != TaskResults.Successful)
                results = GoToX(coaster, x, withIn, TrackType.Right);
 
             return results;
        }
        private static TaskResults GoToX(Coaster coaster, float x, float withIn, TrackType type)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
            TaskResults results = TaskResults.Successful;
            float yawGoal = 0;

            if (x > coaster.LastTrack.X)
                yawGoal = 0;
            else
                yawGoal = 180;

            bool firstStrightTrack = true;
            float lastX = 0;
            float lastDiffernce = 0;

            results = BuildToPitch.Run(coaster, new List<float>() { 0 });
            if(results != TaskResults.Successful)
                return results;

            while (!((coaster.LastTrack.X < x + (withIn / 2) && coaster.LastTrack.X > x - (withIn / 2))) && results == TaskResults.Successful)
            {
                if (coaster.LastTrack.Yaw == yawGoal)
                {

                    buildActions.Add(new BuildAction(TrackType.Stright));
                    results = Builder.BuildTracks(buildActions, coaster);
                    if (results != TaskResults.Successful)
                        return results;
                    buildActions.Clear();

                    float differnce = Math.Abs(coaster.LastTrack.X - lastX);
                    if (!firstStrightTrack)
                    {
                        //This Means You Passed The Goal Point, This could have been done by turning, Or After the Fact. But You Are now going the wrong way.
                        if (differnce > lastDiffernce)
                            return TaskResults.Fail;
                    }
                    else
                        firstStrightTrack = true;

                    lastX = coaster.LastTrack.X;
                    lastDiffernce = differnce;
                }
                else
                {
                    buildActions.Add(new BuildAction(type));
                    results = Builder.BuildTracks(buildActions, coaster);
                    buildActions.Clear();
                }

            }
            if (coaster.LastTrack.X < x + (withIn / 2) && coaster.LastTrack.X > x - (withIn / 2))
                return TaskResults.Successful;
            else
                return TaskResults.Fail;

        }
    }
}
