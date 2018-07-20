using RCLibrary.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    class BuildFlaten : BuilderTask
    {
        public TaskResults Run(Coaster coaster)
        {
            TaskResults results = TaskResults.Fail;
            List<BuildAction> buildActions = new List<BuildAction>();

            if (coaster.LastTrack.Pitch == 0)
            {
                buildActions.Add(new BuildAction(TrackType.Custom));
                for (int i = 0; i < 3; i++)
                {
                    buildActions.Add(new BuildAction(TrackType.Custom));
                }
                results = Builder.BuildTracks(buildActions, coaster);
            }
            else
            {
                results = BuildToPitch.Run(coaster, new List<float>() { 0 });
            }

            return results;
        }
    }
}
