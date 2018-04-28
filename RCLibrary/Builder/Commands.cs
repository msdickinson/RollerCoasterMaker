namespace RCLibrary
{
    public class BuildAction
    {
        public bool RemoveTrack = false;
        public TrackType TrackType;
        public float YawOffset = 0;
        public float PitchOffset = 0;

        public BuildAction(TrackType trackType, float yawOffset = 0, float pitchOffset = 0)
        {
            TrackType = trackType;
            YawOffset = yawOffset;
            PitchOffset = pitchOffset;
        }

        public BuildAction(bool removeTrack)
        {
            RemoveTrack = removeTrack;
        }
    }
}