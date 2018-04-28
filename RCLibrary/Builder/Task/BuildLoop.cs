using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    class BuildLoop : BuilderTask
    {
        public TaskResults Run(Coaster coaster)
        {
            List<BuildAction> buildActions = new List<BuildAction>();

            for (int i = 0; i < 24; i++)
                buildActions.Add(new BuildAction(TrackType.Custom, .5f, Globals.STANDARD_ANGLE_CHANGE));
            for (int i = 0; i < 24; i++)
                buildActions.Add(new BuildAction(TrackType.Custom, -.5f, Globals.STANDARD_ANGLE_CHANGE));

            return Builder.BuildTracks(buildActions, coaster);

        }
    }
}
