using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    class RemoveChunk : BuilderTask
    {
        public TaskResults Run(Coaster coaster)
        {
            if (coaster.ChunkCount == 1)
                return TaskResults.RemoveStartingTracks;

            List<BuildAction> buildActions = new List<BuildAction>();

            for(int i = 0; i < coaster.Chunks[coaster.ChunkCount - 1]; i++)
            {
                buildActions.Add(new BuildAction(true));
            }
            return Builder.BuildTracks(buildActions, coaster);
        }
    }
}
