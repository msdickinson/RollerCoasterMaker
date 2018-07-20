using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary.Support
{
    public static class BuildToYaw 
    {
        public static TaskResults Run(Coaster coaster, List<float> angles)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
            TaskResults results = TaskResults.Fail;

            buildActions.Clear();

            for (int i = 0; i < 15; i++)
            {
                float startYaw = coaster.LastTrack.Yaw;
                foreach (float angle in angles)
                {
                    results = Builder.BuildTracks(DetermineActions(angle, i, startYaw), coaster);
                    if (results == TaskResults.Successful && coaster.LastTrack.Yaw == angle)
                        return results;
                    else
                        coaster.Reset();
                }
            }
            return results;
        }

        private static List<BuildAction> DetermineActions(float goalYaw, int tracksRemoving, float startYaw)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
          
            for (int j = 0; j < tracksRemoving; j++)
            {
                buildActions.Add(new BuildAction(true));
            }

            float differnce = goalYaw - startYaw;
            if(differnce > 180)
            {
                differnce -= 360;
            }
            else if(differnce < -180)
            {
                differnce += 360;
            }
            TrackType direction = (differnce >= 0) ? TrackType.Left : TrackType.Right;
            BuildAction buildAction = new BuildAction(direction);
            differnce = Math.Abs(differnce);
            int tracks = (int)(differnce / Globals.STANDARD_ANGLE_CHANGE);

            for (int j = 0; j < tracks; j++)
            {
                buildActions.Add(buildAction);
            }

            return buildActions;
        }
    }
}
