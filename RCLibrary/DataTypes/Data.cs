using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    public class Data
    {
        public Data(int size = 1000)
        {
            this.TrackType = new TrackType[size];
            X = new float[size];
            Y = new float[size];
            Z = new float[size];

            Yaw = new float[size];
            Pitch = new float[size];
        }
        public TrackType[] TrackType;

        public float[] X;
        public float[] Y;
        public float[] Z;

        public float[] Yaw;
        public float[] Pitch;

    }
}
