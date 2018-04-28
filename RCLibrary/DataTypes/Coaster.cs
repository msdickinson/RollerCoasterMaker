using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{

    public class Coaster
    {
        public bool TracksStarted = false;
        public bool TracksFinshed = false;
        public bool TracksInFinshArea = false;
        public Track[] Tracks = new Track[50000];
        public int[] Chunks = new int[50000];
        //  public QuadTrees.QuadTreePoint<TrackQuadTree> tree = new QuadTrees.QuadTreePoint<TrackQuadTree>();
        //  public List<TrackQuadTree> items = new List<TrackQuadTree>();
        public int TrackCount;
        public int ChunkCount;
        public List<Track>[,,] Regions = new List<Track>[Globals.X_Regions, Globals.Y_Regions, Globals.Z_Regions];
        public int TrackCountBuild;

        public Track[] NewTracks = new Track[50000];
        public int[] NewChunks = new int[50000];
        public int NewTrackCount;
        public int NewChunkCount;

        public Coaster()
        {
            //for(int i = 0; i < Globals.X_Regions; i++)
            //{
            //    for (int j = 0; j < Globals.Y_Regions; j++)
            //    {
            //        for (int k = 0; k < Globals.Z_Regions; k++)
            //        {
            //            Regions[i, j, k] = new List<Track>();
            //        }
            //    }
            //}
        }

        public void Merge(bool startTracks = false)
        {
            //If Tracks were removed
            if (TrackCount != TrackCountBuild)
            {
                //Fix Chunks
                do
                {
                    Chunks[ChunkCount - 1] = Chunks[ChunkCount - 1] - 1;

                    if (Chunks[ChunkCount - 1] == 0)
                    {
                        ChunkCount = ChunkCount - 1;
                    }

                    TrackCount--;
                } while (TrackCount > TrackCountBuild && ChunkCount > 0);
            }

            //If Tracks were added
            for (int i = 0; i < NewTrackCount; i++)
            {
                NewTracks[i].Position = TrackCount;
                Tracks[TrackCount] = NewTracks[i];
                //   var item = new TrackQuadTree(Tracks[TrackCount]);
                //   items.Add(item);
                //  tree.Add(item);
                TrackCount++;
                if(!startTracks)
                {
                    int x = (int)(NewTracks[i].X / Globals.REGION_LENGTH);
                    int y = (int)(NewTracks[i].Y / Globals.REGION_LENGTH);
                    int z = (int)(NewTracks[i].Z / Globals.REGION_LENGTH);
                    if (Regions[x, y, z] == null)
                    {
                        Regions[x, y, z] = new List<Track>();
                    }
                    Regions[x, y, z].Add(NewTracks[i]);
                }
            
            }
            for (int i = 0; i < NewChunkCount; i++)
            {
                Chunks[ChunkCount] = NewChunks[i];
                ChunkCount++;
            }

            TrackCountBuild = TrackCount;
            NewTrackCount = 0;
            NewChunkCount = 0;
        }
        public void Reset()
        {
            TrackCountBuild = TrackCount;
            NewTrackCount = 0;
            NewChunkCount = 0;
        }
    }
}
