using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    class BuildStright : BuilderTask
    {
        public TaskResults Run(Coaster coaster)
        {
            List<Command> commands = new List<Command>();
            List<BuildAction> buildActions = new List<BuildAction>();

            for(int i = 0; i < 3; i++)
            {
                buildActions.Add(new BuildAction(TrackType.Stright));
            }

            return Builder.BuildTracks(buildActions, coaster);
        }
    }
}
