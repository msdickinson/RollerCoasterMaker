using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary.Support
{
    public static  class  BuildToZ
    {

        public static TaskResults Run(Coaster coaster, float z, float withIn)
        {
            TaskResults results;

            //Go Left
            results = GoToZ(coaster, z, withIn, TrackType.Left);

            //Go Right
            if (results != TaskResults.Successful)
                results = GoToZ(coaster, z, withIn, TrackType.Right);

            return results;
        }
        private static TaskResults GoToZ(Coaster coaster, float z, float withIn, TrackType type)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
            TaskResults results = TaskResults.Successful;
            float pitchGoal = 0;

            if (z > coaster.Tracks[coaster.TrackCountBuild - 1].Z)
                pitchGoal = 90;
            else
                pitchGoal = 270;

            bool firstStrightTrack = true;
            float lastZ = 0;
            float lastDiffernce = 0;

            results = BuildToPitch.Run(coaster, new List<float>() { 0 });
            if(results != TaskResults.Successful)
                return results;

            while (!((coaster.Tracks[coaster.TrackCountBuild - 1].Z < z + (withIn / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].Z > z - (withIn / 2))) && results == TaskResults.Successful)
            {
                if (coaster.Tracks[coaster.TrackCountBuild - 1].Pitch == pitchGoal)
                {
                    buildActions.Add(new BuildAction(TrackType.Stright));
                    results = Builder.BuildTracks(buildActions, coaster);

                    if (results != TaskResults.Successful)
                        return results;

                    buildActions.Clear();

                    float differnce = Math.Abs(coaster.Tracks[coaster.TrackCountBuild - 1].Z - lastZ);
                    if (!firstStrightTrack)
                    {
                        //This Means You Passed The Goal Point, This could have been done by turning, Or After the Fact. But You Are now going the wrong way.
                        if (differnce > lastDiffernce)
                           return TaskResults.Fail;
                    }
                    else
                        firstStrightTrack = true;

                    lastZ = coaster.Tracks[coaster.TrackCountBuild - 1].Z;
                    lastDiffernce = differnce;
                }
                else
                {
                    buildActions.Add(new BuildAction(type));
                    results = Builder.BuildTracks(buildActions, coaster);
                    buildActions.Clear();
                }

            }
            if (coaster.Tracks[coaster.TrackCountBuild - 1].Z < z + (withIn / 2) && coaster.Tracks[coaster.TrackCountBuild - 1].Z > z - (withIn / 2))
                return TaskResults.Successful;
            else
                return TaskResults.Fail;

        }

    }
}
