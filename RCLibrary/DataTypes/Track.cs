using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    public struct Track 
    {
        public int Position;
        public float X; 
        public float Y;
        public float Z;
        public float Yaw;
        public float Pitch;
        public TrackType TrackType;
    }

    //public class TrackQuadTree : QuadTrees.QTreePoint.IPointQuadStorable
    //{
    //    public TrackQuadTree(Track track)
    //    {
    //        this.point = new PointF(track.X, track.Y);
    //    }
    //    private PointF point;
    //    public PointF Point { get { return point; } }
    //}

}
