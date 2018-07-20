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

            results = GoToZ(coaster, z, withIn);

            return results;
        }
        private static TaskResults GoToZ(Coaster coaster, float z, float withIn)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
            TaskResults results = TaskResults.Successful;
            float pitchGoal = 0;
            float lastDistance = Math.Abs(coaster.LastTrack.Z - z);
            if (z > coaster.LastTrack.Z)
            {
                pitchGoal = 90;
            }
            else
            {
                pitchGoal = 270;
            }

            results = BuildToPitch.Run(coaster, new List<float>() { pitchGoal });
            if(results != TaskResults.Successful)
                return results;

            while (!((coaster.LastTrack.Z < z + (withIn / 2) && coaster.LastTrack.Z > z - (withIn / 2))) && results == TaskResults.Successful)
            {
                buildActions.Add(new BuildAction(TrackType.Stright));
                results = Builder.BuildTracks(buildActions, coaster);
                buildActions.Clear();

                float distance = Math.Abs(coaster.LastTrack.Z - z);

                if (distance >= lastDistance)
                    return TaskResults.Fail;

                lastDistance = distance;
            }
            if (coaster.LastTrack.Z < z + (withIn / 2) && coaster.LastTrack.Z > z - (withIn / 2))
                return TaskResults.Successful;
            else
                return TaskResults.Fail;

        }

    }
}
