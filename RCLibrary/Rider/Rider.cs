//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace RCLibrary.Rider
//{
//    public class Rider
//    {
//        //Rider Varibles
//        public bool IsActive = false;
//        private float CurrentSpeed = 0;
//        private float totalMoved = 0;

//        public int currentTrack = 0;

//        public void RideCoaster()
//        {
//            IsActive = true;
//        }
//        public void StopCoaster()
//        {
//            totalMoved = 0;

//            IsActive = false;
//        }
//        public void PauseCoaster()
//        {
//            IsActive = false;
//        }

//        //Contine Ride
//        public void contineRide(List<Track> myTracks, TimeSpan elapsedTime)
//        {
//            //Find Current Track (For Cart)
//            currentTrack = Math.Abs((int)((totalMoved) / Globals.TRACK_LENGTH));

//            if (currentTrack >= myTracks.Count)
//            {
//                currentTrack = myTracks.Count - 1;
//            }

//            float distanceOnCurrenttrack = (totalMoved % Globals.TRACK_LENGTH);

//            //Find Total Moved
//            float speed = (float)GetSpeed(myTracks, currentTrack);
//            float numberOfFrames = ((float)elapsedTime.Milliseconds / (float)20);
//            totalMoved += (float)GetDistance(speed) * (float)numberOfFrames;

//            //Stop Coaster If Its at the end
//            if (currentTrack >= myTracks.Count - 2)
//            {
//                IsActive = false;
//                currentTrack = 1;
//                return;
//            }

//            CurrentSpeed = speed;


//            //Cart
//            Orientation CartOrienation = GetOrientation(myTracks, distanceOnCurrenttrack, currentTrack);
//            cart.Orientation = CartOrienation;

//            Vector3 cartLocation = GetNewLocation(CartOrienation, myTracks, distanceOnCurrenttrack, currentTrack, Globals.CART_HEIGHT);
//            cart.Location = cartLocation;
//        }

//        //Move Cart
//        public void GotoTrack(List<Track> myTracks, int trackNumber)
//        {

//            //Cart
//            Orientation CartOrienation = GetOrientation(myTracks, 0, trackNumber);
//            cart.Orientation = CartOrienation;

//            Vector3 cartLocation = GetNewLocation(CartOrienation, myTracks, 0, trackNumber, Globals.CART_HEIGHT);
//            cart.Location = cartLocation;

//            totalMoved = trackNumber * Globals.TRACK_LENGTH;

//        }

//        //Support fuctions
//        public float GetSpeed(List<Track> myTracks, int currentTrack)
//        {
//            float highestPoint = 0;



//            //Highest Been Point
//            for (int i = 0; i < currentTrack + 1; i++)
//            {
//                if (highestPoint < myTracks[i].Position.Y)
//                {
//                    highestPoint = myTracks[i].Position.Y;
//                }
//            }

//            float totalEnergy = highestPoint / 3 * Globals.GRAVITY + (1.0f / 2.0f * (float)Math.Pow(Globals.CART_MIN_SPEED, 2));
//            float speed = 2 * (totalEnergy - myTracks[currentTrack].Position.Y / 3 * Globals.GRAVITY);
//            speed = (float)Math.Sqrt(speed);


//            speed = speed * (1.0f / 3.0f);

//            return speed;
//        }
//        public float GetDistance(float speed)
//        {
//            if (speed < Globals.CART_MIN_SPEED)
//            {
//                return (Globals.CART_MIN_SPEED * .10f);
//            }
//            else
//            {
//                return (speed * .20f);
//            }

//        }
//        private Vector3 GetNewLocation(Orientation orientation, List<Track> tracks, float distanceOnCurrenttrack, int trackNum, float heightOffset)
//        {
//            float stopPointX = 0;
//            float stopPointY = 0;
//            float stopPointZ = (float)Math.Cos(MathHelper.ToRadians(tracks[trackNum].Orientation.Pitch));



//            stopPointX = tracks[trackNum].Position.X + (tracks[trackNum + 1].Position.X - tracks[trackNum].Position.X) * distanceOnCurrenttrack / Globals.TRACK_LENGTH
//                           - ((float)Math.Sin(MathHelper.ToRadians(orientation.Pitch)) * (float)Math.Sin(MathHelper.ToRadians(orientation.Yaw)) * heightOffset);
//            stopPointY = tracks[trackNum].Position.Y + (tracks[trackNum + 1].Position.Y - tracks[trackNum].Position.Y) * distanceOnCurrenttrack / Globals.TRACK_LENGTH
//                           + (float)Math.Cos(MathHelper.ToRadians(orientation.Pitch)) * heightOffset;
//            stopPointZ = tracks[trackNum].Position.Z + (tracks[trackNum + 1].Position.Z - tracks[trackNum].Position.Z) * distanceOnCurrenttrack / Globals.TRACK_LENGTH
//                           - ((float)Math.Sin(MathHelper.ToRadians(orientation.Pitch)) * (float)Math.Cos(MathHelper.ToRadians(orientation.Yaw)) * heightOffset);
//            return (new Vector3(stopPointX, stopPointY, stopPointZ));
//        }
//        public Orientation GetOrientation(List<Track> myTracks, float distanceOnCurrenttrack, int trackNum)
//        {
//            Orientation orientation = myTracks[trackNum].Orientation.Clone();

//            //Pitch
//            if ((myTracks[trackNum].Orientation.Pitch - myTracks[trackNum + 1].Orientation.Pitch) == -7.5
//                || (myTracks[trackNum].Orientation.Pitch - myTracks[trackNum + 1].Orientation.Pitch) == 352.5)
//            {
//                orientation.Pitch += (distanceOnCurrenttrack / Globals.TRACK_LENGTH) * 7.5f;
//            }
//            else if ((myTracks[trackNum].Orientation.Pitch - myTracks[trackNum + 1].Orientation.Pitch) == 7.5
//                 || (myTracks[trackNum].Orientation.Pitch - myTracks[trackNum + 1].Orientation.Pitch) == -352.5)
//            {
//                orientation.Pitch += (distanceOnCurrenttrack / Globals.TRACK_LENGTH) * -7.5f;
//            }

//            //Yaw
//            if ((myTracks[trackNum].Orientation.Yaw - myTracks[trackNum + 1].Orientation.Yaw) == -7.5
//                || (myTracks[trackNum].Orientation.Yaw - myTracks[trackNum + 1].Orientation.Yaw) == 352.5)
//            {
//                orientation.Yaw += (distanceOnCurrenttrack / Globals.TRACK_LENGTH) * 7.5f;
//            }
//            else if ((myTracks[trackNum].Orientation.Yaw - myTracks[trackNum + 1].Orientation.Yaw) == 7.5
//                || (myTracks[trackNum].Orientation.Yaw - myTracks[trackNum + 1].Orientation.Yaw) == -352.5)
//            {
//                orientation.Yaw += (distanceOnCurrenttrack / Globals.TRACK_LENGTH) * -7.5f;
//            }


//            return orientation;

//        }
//        public double GetCartTrackPercent(List<Track> tracks)
//        {
//            double totalTracks = tracks.Count - 3;

//            int tracknumber = Math.Abs((int)((totalMoved) / Globals.TRACK_LENGTH));

//            if (totalTracks < 1) totalTracks = 1;
//            if (tracknumber < 1) tracknumber = 1;
//            if (tracknumber > tracks.Count - 2) tracknumber = tracks.Count - 2;

//            double percent = tracknumber / totalTracks;

//            if (percent < .018)
//            {
//                percent = 0;
//            }
//            return percent;
//        }
//    }
//}
