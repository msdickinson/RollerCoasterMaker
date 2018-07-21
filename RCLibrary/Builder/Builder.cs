using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    public enum BuildFix
    {
        None,
        Collison,
        MaxX,
        MaxY,
        MinX,
        MinY,
        MinZ,
    }
    public enum TaskResults
    {
        Successful,
        Angle,
        Collison,
        MaxX,
        MaxY,
        MinX,
        MinY,
        MinZ,
        MaxZ,
        OutOfBounds,
        SupportFunctionFailed,
        RemoveStartingTracks,
        Fail,
        RemoveCustomTrack
    }
    public class Builder
    {
        Coaster builderCoaster = new Coaster();
        public static Track lastRuleIssueTrack;
        public void StartTracks(Coaster coaster)
        {
            List<BuildAction> buildActions = new List<BuildAction>();
            //Stright
            for (int i = 0; i < (22); i++)
            {
                buildActions.Add(new BuildAction(TrackType.Stright));
            }

            //Left
            for (int i = 0; i < (4 * 3); i++)
            {
                buildActions.Add(new BuildAction(TrackType.Left));
            }

            //Stright  5 
            for (int i = 0; i < (11); i++)
            {
                buildActions.Add(new BuildAction(TrackType.Stright));
            }

            BuildTracks(buildActions, coaster);
            coaster.NewChunks[coaster.NewChunkCount] = (coaster.NewTrackCount);
            coaster.NewChunkCount++;
            coaster.Merge(true);
            coaster.TracksStarted = true;
        }
        public BuilderTask lastBuildTask;
        public bool lastBuildActionFail = false;
        public TaskResults initialTaskResults;
        public string lastTaskName = "";
        public bool ProcessBuildAction(Coaster coaster, BuilderTask task)
        {
            if (coaster.TracksStarted == false || (lastBuildActionFail && task.GetType() == lastBuildTask.GetType()))
                return false;

            TaskResults results = task.Run(coaster);
            initialTaskResults = results;
            initialTaskResults = results;
            lastBuildTask = task;
            
            return ProcessAfterBuildAttempt(coaster, results);
        }

        public bool ProcessAfterBuildAttempt(Coaster coaster,TaskResults results, string lastTaskName = "")
        {
            this.lastTaskName = lastTaskName;

            if(results != TaskResults.Successful)
            {
                coaster.Reset();
            }
            switch (results)
            {
                case TaskResults.MaxX:
                    results = FixX.Run(coaster);
                    break;
                case TaskResults.MaxY:
                    results = FixY.Run(coaster);
                    break;
                case TaskResults.MinX:
                    results = FixX.Run(coaster);
                    break;
                case TaskResults.MinY:
                    results = FixY.Run(coaster);
                    break;
                case TaskResults.MinZ:
                    results = FixZ.Run(coaster);
                    break;
                case TaskResults.Collison:
                    results = FixTrackCollison.Run(coaster);
                    break;
            }


            if (results == TaskResults.Successful)
            {
                //Chunk anything that has not already been Chunked
                int totalNewTracksChunked = 0;
                for (int i = 0; i < coaster.NewChunkCount; i++)
                {
                    totalNewTracksChunked += coaster.NewChunks[i];
                }

                int tracksWihtNoChunk = coaster.NewTrackCount - totalNewTracksChunked;
                if (tracksWihtNoChunk > 0)
                {
                    coaster.NewChunks[coaster.NewChunkCount] = tracksWihtNoChunk;
                    coaster.NewChunkCount++;
                }

                lastBuildActionFail = false;

                coaster.Merge();
                return true;
            }
            else
            {
                lastBuildActionFail = true;
                coaster.Reset();
                return false;
            }
        }
       public static TaskResults BuildTracks(List<BuildAction> buildActions, Coaster coaster, bool removeChunk = false)
       {
            //Check If Coater Finshed
            TaskResults result = TaskResults.Successful;

            foreach(BuildAction buildAction in buildActions)
            {
                if(buildAction.RemoveTrack == false)
                {
                    result = BuildTrack(coaster, buildAction);
                }
                else
                {
                    result = RemoveTrack(coaster, removeChunk);
                }
               
                if (result != TaskResults.Successful)
                    break;
            }

            return result;
        }
        public static TaskResults BuildTrack(Coaster coaster, BuildAction action)
        {
            //Check If Coater Finshed
            float yaw = 0;
            float pitch = 0;
            float x = 0;
            float y = 0;
            float z = 0;
            Track lastTrack = new Track();
            //Determine Starting Position
            if (coaster.TrackCountBuild == 0 && coaster.NewTrackCount == 0)
            {
                yaw = Globals.START_YAW;
                pitch = Globals.START_PITCH;
                x = Globals.START_X;
                y = Globals.START_Y;
                z = Globals.START_Z;

            }
            else
            {
                
                if (coaster.NewTrackCount > 0)
                {
                    lastTrack = coaster.NewTracks[coaster.NewTrackCount - 1];
                }
                else
                {
                    lastTrack = coaster.LastTrack;
                }

                yaw = lastTrack.Yaw;
                pitch = lastTrack.Pitch;
                x = lastTrack.X;
                y = lastTrack.Y;
                z = lastTrack.Z;

                //Determine Yaw And Pitch
                switch (action.TrackType)
                {
                    case TrackType.Stright:
                        break;
                    case TrackType.Left:
                        yaw = yaw + Globals.STANDARD_ANGLE_CHANGE;
                        break;
                    case TrackType.Right:
                        yaw = yaw - Globals.STANDARD_ANGLE_CHANGE;
                        break;
                    case TrackType.Up:
                        pitch = pitch + Globals.STANDARD_ANGLE_CHANGE;
                        break;
                    case TrackType.Down:
                        pitch = pitch - Globals.STANDARD_ANGLE_CHANGE;
                        break;
                    case TrackType.Custom:
                        yaw = yaw + action.YawOffset;
                        pitch = pitch + action.PitchOffset;
                        break;
                }

                //IF X out of 360
                if(yaw < 0)
                    yaw += 360;

                if (yaw >= 360)
                    yaw += -360;

                if (pitch < 0)
                    pitch += 360;

                if (pitch >= 360)
                    pitch += -360;

                //IF Y out of 360


                //Determine X, Y, And Z
                x = lastTrack.X + 
                    (float)(Math.Cos(MathHelper.ToRadians(lastTrack.Yaw)) * Math.Cos(MathHelper.ToRadians(lastTrack.Pitch)) * Globals.HALF_TRACK_LENGTH) +
                    (float)(Math.Cos(MathHelper.ToRadians(yaw)) * Math.Cos(MathHelper.ToRadians(pitch)) * Globals.HALF_TRACK_LENGTH) ;
                y = lastTrack.Y + 
                    (float)(Math.Sin(MathHelper.ToRadians(lastTrack.Yaw)) * Math.Cos(MathHelper.ToRadians(lastTrack.Pitch)) * Globals.HALF_TRACK_LENGTH) +
                    (float)(Math.Sin(MathHelper.ToRadians(yaw)) * Math.Cos(MathHelper.ToRadians(pitch)) * Globals.HALF_TRACK_LENGTH);
                z = lastTrack.Z + 
                    (float)(Math.Sin(MathHelper.ToRadians(lastTrack.Pitch)) * Globals.HALF_TRACK_LENGTH) + 
                    (float)(Math.Sin(MathHelper.ToRadians(pitch)) * Globals.HALF_TRACK_LENGTH);

            }


            //Check Rules
            TaskResults result = CheckRules(coaster, x, y, z, yaw, pitch, action.TrackType);

            //Add Track
            if (TaskResults.Successful == result)
            {
                coaster.NewTracks[coaster.NewTrackCount] = new Track() { X = x, Y = y, Z = z, Pitch = pitch, Yaw = yaw, TrackType = action.TrackType };
              //  coaster.NewTracksQTree[coaster.NewTrackCount] = new TrackQuadTree(coaster.NewTracks[coaster.NewTrackCount]);
              //  coaster.QTree.Add(coaster.NewTracksQTree[coaster.NewTrackCount]);
                coaster.NewTrackCount++;
            }
 
            return result;
        }
        public static TaskResults RemoveTrack(Coaster coaster, bool removeChunk)
        {
            if (coaster.NewTrackCount == 0 && coaster.TrackCountBuild == 45)
            {
                return TaskResults.OutOfBounds;
            }
            else
            {
                if (coaster.NewTrackCount > 0)
                {
                    coaster.NewTrackCount--;
                }
                else if (removeChunk == true || coaster.LastTrack.TrackType != TrackType.Custom)
                {
                    coaster.TrackCountBuild--;
                }
                else
                {
                    return TaskResults.RemoveCustomTrack;
                }
            }
            return TaskResults.Successful;
        }
        private static TaskResults CheckRules(Coaster coaster, float x, float y, float z, float yaw, float pitch, TrackType trackType)
        {
            TaskResults result = TaskResults.Successful;

            if (coaster.TracksStarted == false || coaster.TracksInFinshArea == true)
                return result;

            if (!Rules.AngleCheck(yaw, pitch, trackType))
                result = TaskResults.Angle;

            if (!Rules.MaxX(x))
                result = TaskResults.MaxX;

            else if (!Rules.MaxY(y))
                result = TaskResults.MaxY;

            else if (!Rules.MinX(x))
                result = TaskResults.MinX;

            else if (!Rules.MinY(y))
                result = TaskResults.MinY;

            else if (!Rules.MinZ(yaw, pitch, z))
                result = TaskResults.MinZ;

            else if (!Rules.MaxZ(z))
                result = TaskResults.MaxZ;
            
            else if (!Rules.CollisonX(coaster, x, y, z))
                result = TaskResults.Collison;

            if (result != TaskResults.Successful)
                lastRuleIssueTrack = new Track() { X = x, Y = y, Z = z, Yaw = yaw, Pitch = pitch, TrackType = trackType };

            return result;
        }

    }
}
