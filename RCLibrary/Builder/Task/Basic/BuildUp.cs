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
            TaskResults result = TaskResults.Fail; 
            bool loopDetected = true;
            for(int i = coaster.TrackCount - 42; i < coaster.TrackCount; i++)
            {
                if(coaster.Tracks[i].TrackType != TrackType.Up)
                {
                    loopDetected = false;
                }
            }
            if (loopDetected)
            {
                for (int i = 0; i < 42; i++)
                {
                    buildActions.Add(new BuildAction(true));
                }

                result =  Builder.BuildTracks(buildActions, coaster);
                if (result != TaskResults.Fail)
                {
                    return (new BuildLoop()).Run(coaster);
                }
                else
                {
                    return result;
                }
            }
            else
            {
                buildActions.Add(new BuildAction(TrackType.Up));
                buildActions.Add(new BuildAction(TrackType.Up));
                buildActions.Add(new BuildAction(TrackType.Up));

                return Builder.BuildTracks(buildActions, coaster);
            }

        }
    }
}
