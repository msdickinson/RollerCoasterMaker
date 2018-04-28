using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    class BuildLeft : BuilderTask
    {
        public TaskResults Run(Coaster coaster)
        {
            List<BuildAction> buildActions = new List<BuildAction>();

            buildActions.Add(new BuildAction(TrackType.Left));
            buildActions.Add(new BuildAction(TrackType.Left));
            buildActions.Add(new BuildAction(TrackType.Left));

            return Builder.BuildTracks(buildActions, coaster);
        }
    }
}
