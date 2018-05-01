using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    public class CoasterUpdate
    {
        public bool TracksStarted = false;
        public bool TracksFinshed = false;
        public bool TracksInFinshArea = false;
        public int TrackCount = 0;
        public int ChunkCount = 0;
        public int LastChunkCount = 0;
        public int RemovedTracksCount = 0;
        public int NewTracksCount = 0;
        public Track[] NewTracks = null;
    }

    public class Coaster
    {
        public bool TracksStarted = false;
        public bool TracksFinshed = false;
        public bool TracksInFinshArea = false;
        public Track[] Tracks = new Track[1000];
        public int[] Chunks = new int[1000];
        public int TrackCount;
        public int ChunkCount;
        public List<Track>[,,] Regions = new List<Track>[Globals.X_Regions, Globals.Y_Regions, Globals.Z_Regions];
        public int TrackCountBuild;

        public Track[] NewTracks = new Track[1000];
        public int[] NewChunks = new int[1000];
        public int NewTrackCount;
        public int NewChunkCount;

        public int LastRemovedTracks = 0;
        public int LastNewTracks = 0;
        public Track[] lastSetOfNewTracks = null;


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
            LastRemovedTracks = TrackCount - TrackCountBuild;

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
            lastSetOfNewTracks = new Track[NewTrackCount];
            LastNewTracks = NewTrackCount;
            //If Tracks were added
            for (int i = 0; i < NewTrackCount; i++)
            {
                lastSetOfNewTracks[i] = NewTracks[i];

                NewTracks[i].Position = TrackCount;
                Tracks[TrackCount] = NewTracks[i];
                //   var item = new TrackQuadTree(Tracks[TrackCount]);
                //   items.Add(item);
                //  tree.Add(item);
                TrackCount++;
                if (!startTracks)
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

        public CoasterUpdate GetLastCoasterUpdate()
        {
            CoasterUpdate coasterChange = new CoasterUpdate();
            coasterChange.TracksStarted = TracksStarted;
            coasterChange.TracksFinshed = TracksFinshed;
            coasterChange.TrackCount = TrackCount;
            coasterChange.ChunkCount = ChunkCount;
            coasterChange.LastChunkCount = Chunks[ChunkCount - 1];
            coasterChange.RemovedTracksCount = LastRemovedTracks;
            coasterChange.NewTracksCount = LastNewTracks;
            coasterChange.NewTracks = lastSetOfNewTracks;

            return coasterChange;

        }
    }
}
