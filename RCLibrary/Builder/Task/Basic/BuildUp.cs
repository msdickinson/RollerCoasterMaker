using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    class BuildUp : BuilderTask
    {
        public TaskResults Run(Coaster coaster)
        {
            List<Command> commands = new List<Command>();
            List<BuildAction> buildActions = new List<BuildAction>();

            buildActions.Add(new BuildAction(TrackType.Up));
            buildActions.Add(new BuildAction(TrackType.Up));
            buildActions.Add(new BuildAction(TrackType.Up));

            return Builder.BuildTracks(buildActions, coaster);
        }
    }
}
