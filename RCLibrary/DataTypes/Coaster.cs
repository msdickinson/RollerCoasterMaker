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
        //public float[] X = new float[Globals.MAX_TRACKS];
        //public float[] Y = new float[Globals.MAX_TRACKS];
        //public float[] Z = new float[Globals.MAX_TRACKS];
        //public float[] Yaw = new float[Globals.MAX_TRACKS];
        //public float[] Pitch = new float[Globals.MAX_TRACKS];

        public float[] Data = new float[Globals.MAX_TRACKS * 7];
        public bool LastBuildSucessful = false;
        public bool TracksStarted = false;
        public bool TracksFinshed = false;
        public bool TracksInFinshArea = false;
        public Track[] Tracks = new Track[Globals.MAX_TRACKS];
        public int[] Chunks = new int[Globals.MAX_TRACKS];
        public int TrackCount;
        public int ChunkCount;
        public List<Track>[,,] Regions = new List<Track>[Globals.X_Regions, Globals.Y_Regions, Globals.Z_Regions];
        public int TrackCountBuild;

        public Track[] NewTracks = new Track[Globals.MAX_TRACKS];
        public int[] NewChunks = new int[Globals.MAX_TRACKS];
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
            LastBuildSucessful = true;
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

                Data[TrackCount * 5] = NewTracks[i].X;
                Data[TrackCount * 5 + 1] = NewTracks[i].Y;
                Data[TrackCount * 5 + 2] = NewTracks[i].Z;
                Data[TrackCount * 5 + 3] = NewTracks[i].Yaw;
                Data[TrackCount * 5 + 4] = NewTracks[i].Pitch;

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
            LastBuildSucessful = false;
        }

        public CoasterUpdate GetLastCoasterUpdate()
        { 
            if(LastBuildSucessful)
            {

            }
            LastBuildSucessful = true;
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
