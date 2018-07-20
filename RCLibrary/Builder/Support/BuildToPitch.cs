using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary.Support
{
    public static class BuildToPitch 
    {
        public static TaskResults Run(Coaster coaster, List<float> angles)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
            TaskResults results = TaskResults.Fail;

            buildActions.Clear();

            for (int i = 0; i < 15; i++)
            {
                float startPitch = coaster.LastTrack.Pitch;
                foreach (float angle in angles)
                {
                    results = Builder.BuildTracks(DetermineActions(angle, i, startPitch), coaster);
                    if (results == TaskResults.Successful)
                        return results;
                    else
                        coaster.Reset();
                }
            }
            return results;
        }

        private static List<BuildAction> DetermineActions(float goalPitch, int tracksRemoving, float startPitch)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
          
            for (int j = 0; j < tracksRemoving; j++)
            {
                buildActions.Add(new BuildAction(true));
            }

            float differnce = goalPitch - startPitch;
            if(differnce >= 180)
            {
                differnce -= 360;
            }
            else if(differnce < -180)
            {
                differnce += 360;
            }
            TrackType direction = (differnce >= 0) ? TrackType.Up : TrackType.Down;
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
