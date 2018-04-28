using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    static class Rules
    {
        public static bool MaxX(float x)
        {
            if (x > Globals.BUILD_AREA_SIZE_X)
                return false;
            else
                return true;
        }

        public static bool MaxY(float y)
        {
            if (y > Globals.BUILD_AREA_SIZE_Y)
                return false;
            else
                return true;
        }

        public static bool MinX(float x)
        {
            if (x < 0)
                return false;
            else
                return true;
        }
        public static bool MinY(float y)
        {
            if (y < 0)
                return false;
            else
                return true;
        }

        public static bool MinZ(float yaw, float pitch, float z)
        {
            if ((pitch > 90 && pitch < 270) && (z < (0 + Globals.CART_HEIGHT * -1 * Math.Cos(MathHelper.ToRadians(pitch)))))
                return false;
            else if (z < 0)
                return false;
            else
                return true;
        }
        public static bool AngleCheck(float yaw, float pitch, TrackType trackType)
        {
            if (trackType == TrackType.Custom)
                return true;

            if (yaw % Globals.STANDARD_ANGLE_CHANGE != 0.0f)
                return false;
            if (pitch % Globals.STANDARD_ANGLE_CHANGE != 0.0f)
                return false;

            return true;
        }
        public static bool CollisonGroupCheck(Coaster coaster)
        {
            float minX = coaster.NewTracks[0].X;
            float minY = coaster.NewTracks[0].Y;
            float maxX = coaster.NewTracks[0].X;
            float maxY = coaster.NewTracks[0].Y;
            float totalX = 0;
            float totalY = 0;

            for (int i = 0; i < coaster.NewTrackCount; i++)
            {
                totalX += coaster.NewTracks[i].X;
                totalY += coaster.NewTracks[i].Y;
                if (minX > coaster.NewTracks[i].X)
                {
                    minX = coaster.NewTracks[i].X;
                }
                if (minY > coaster.NewTracks[i].Y)
                {
                    minY = coaster.NewTracks[i].Y;
                }
                if (maxX < coaster.NewTracks[i].X)
                {
                    maxX = coaster.NewTracks[i].X;
                }
                if (maxY < coaster.NewTracks[i].Y)
                {
                    maxY = coaster.NewTracks[i].Y;
                }
            }
            float midX = totalX / coaster.NewTrackCount;
            float midY = totalY / coaster.NewTrackCount;

            float length = (float)(Math.Sqrt(Math.Pow(maxX - minX, 2) + Math.Pow(maxY - minY, 2)));
            List<Track> tracks = new List<Track>();

            int count = 0;
            float deltaXSquared = 0;
            float deltaYSquared = 0;
            float sumRadiiSquared = 0;
            for (int i = 0; i < coaster.TrackCountBuild; i++)
            {
                if (count > coaster.TrackCountBuild - 2)
                {

                }
                else
                {
                    deltaXSquared = midX - coaster.Tracks[i].X; // calc. delta X
                    deltaXSquared *= deltaXSquared; // square delta X
                    deltaYSquared = midY - coaster.Tracks[i].Y; // calc. delta Y
                    deltaYSquared *= deltaYSquared; // square delta Y

                    // Calculate the sum of the radii, then square it
                    sumRadiiSquared = Globals.TRACK_LENGTH + length;
                    sumRadiiSquared *= sumRadiiSquared;

                    if (deltaXSquared + deltaYSquared <= sumRadiiSquared)
                    {
                        tracks.Add(coaster.Tracks[i]);
                    }
                }
            }

            //
            if (tracks.Count == 0)
                return true;

            for (int x = 0; x < tracks.Count; x++)
            {
                count = 0;
                for (int i = 0; i < coaster.TrackCountBuild; i++)
                {
                    count++;
                    if (count > coaster.TrackCountBuild - 2)
                    {

                    }
                    else
                    {
                        deltaXSquared = coaster.NewTracks[x].X - coaster.Tracks[i].X; // calc. delta X
                        deltaXSquared *= deltaXSquared; // square delta X
                        deltaYSquared = coaster.NewTracks[x].Y - coaster.Tracks[i].Y; // calc. delta Y
                        deltaYSquared *= deltaYSquared; // square delta Y

                        // Calculate the sum of the radii, then square it
                        sumRadiiSquared = Globals.TRACK_LENGTH;
                        sumRadiiSquared *= sumRadiiSquared;

                        if (deltaXSquared + deltaYSquared <= sumRadiiSquared)
                        {
                            //  return false;
                        }
                    }
                }

            }
            return true;
            //count = 0;
            //for (int i = 0; i < coaster.NewTrackCount; i++)
            //{
            //    count++;
            //    if (count > coaster.NewTrackCount - 2)
            //    {
            //        //      sw.Stop();
            //        return true;
            //    }

            //    deltaXSquared = (x - coaster.NewTracks[i].X); // calc. delta X
            //    deltaXSquared *= deltaXSquared; // square delta X
            //    deltaYSquared = y - coaster.NewTracks[i].Y; // calc. delta Y
            //    deltaYSquared *= deltaYSquared; // square delta Y

            //    // Calculate the sum of the radii, then square it
            //    sumRadiiSquared = Globals.TRACK_LENGTH;
            //    sumRadiiSquared *= sumRadiiSquared;

            //    if (deltaXSquared + deltaYSquared <= sumRadiiSquared)
            //    {
            //        //     sw.Stop();
            //        return false;
            //    }
            //}
            ////  sw.Stop();
            //return true;
        }
        public static int[] found = new int[50000];
        public static int foundIndex = 0;
        public static bool Collison(Coaster coaster, float x, float y, float z, int totalTracks = 1)
        {
            float raidus = Globals.TRACK_LENGTH * totalTracks;
            int count = 0;
            foundIndex = 0;
            float j = 0;
            float q = 0;
            float d = 0;
            for (int i = 0; i < coaster.TrackCountBuild; i++)
            {
                count++;
                if (count > coaster.TrackCountBuild - totalTracks - 1)
                {
                    return true;
                }

                j = x - coaster.Tracks[i].X;
                q = y - coaster.Tracks[i].Y;
                d = z - coaster.Tracks[i].Z;
                if (((j * j) + (q * q) + (d * d)) <= raidus * raidus)
                {
                    found[foundIndex] = i;
                    // foundIndex++;
                    return false;
                }

            }
            return true;

        }
        public static int lastCollsionIndex = 0;
        public static bool CollisonX(Coaster coaster, float xA, float yA, float zA, int totalTracks = 1)
        {
            //IQNORE 3 + All "Back Tracks"
            int trackMaxPosition = coaster.TrackCountBuild - 3;
            Track t1 = coaster.Tracks[coaster.TrackCountBuild - 1];
            Track t2 = coaster.Tracks[coaster.TrackCountBuild - 2];
            Track t3 = coaster.Tracks[coaster.TrackCountBuild - 3];
            int xT = (int)(xA / Globals.REGION_LENGTH);
            int yT = (int)(yA / Globals.REGION_LENGTH);
            int zT = (int)(zA / Globals.REGION_LENGTH);

            float j = 0;
            float q = 0;
            float g = 0;
            for (int x = xT - 1; x < xT + 1; x++)
            {
                for (int y = yT - 1; y < yT + 1; y++)
                {
                    for (int z = zT - 1; z < zT + 1; z++)
                    {
                        if (x >= 0 && x < Globals.X_Regions &&
                            y >= 0 && y < Globals.Y_Regions &&
                            z >= 0 && z < Globals.Z_Regions &&
                            coaster.Regions[x, y, z] != null)
                        {
                            foreach (Track t in coaster.Regions[x, y, z])
                            {
                                j = xA - t.X;
                                q = yA - t.Y;
                                g = zA - t.Z;
                                if (((j * j) + (q * q) + (g * g)) <= Globals.TRACK_HALF_LENGH_SQUARED 
                                    && (t.Position < trackMaxPosition))
                                {
                                    lastCollsionIndex = t.Position;
                                    return false;
                                }

                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
