using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    public class RollerCoasterMaker
    {
        public Coaster coaster = new Coaster();
        public Builder builder = new Builder();

        List<Command> commands = new List<Command>();
        List<BuildAction> buildStright = new List<BuildAction>() { new BuildAction(TrackType.Stright), new BuildAction(TrackType.Stright), new BuildAction(TrackType.Stright) };

        public RollerCoasterMaker()
        {
            Reset();
        }

        public void Reset()
        {
            coaster = new Coaster();
            builder.StartTracks(coaster);

            Stopwatch sw = new Stopwatch();
            sw.Start();
        }
        public void Log()
        {
            Console.WriteLine(coaster.Tracks);
        }

        public void BuildStright()
        {
            builder.ProcessBuildAction(coaster, new BuildStright());
        }
        public void BuildLeft()
        {
            builder.ProcessBuildAction(coaster, new BuildLeft());
        }
        public void BuildRight()
        {
            builder.ProcessBuildAction(coaster, new BuildRight());
        }
        public void BuildDown()
        {
            builder.ProcessBuildAction(coaster, new BuildDown());
        }
        public void BuildUp()
        {
            builder.ProcessBuildAction(coaster, new BuildUp());
        }
        public void Back()
        {
            builder.ProcessBuildAction(coaster, new RemoveChunk());
        }
        public void BuildLoop()
        {
            builder.ProcessBuildAction(coaster, new BuildLoop());
        }
        public void BuildDownward()
        {
            builder.ProcessBuildAction(coaster, new BuildDownward());
        }
        public void BuildUpward()
        {
            builder.ProcessBuildAction(coaster, new BuildUpward());
        }
        public void BuildFlaten()
        {
            builder.ProcessBuildAction(coaster, new BuildFlaten());
        }
        public void BuildFinsh()
        {
            builder.ProcessBuildAction(coaster, new BuildFinsh());
        }
        public void BuildToX(float x, float withInX)
        {
            if (coaster.TracksStarted == false)
                return;

            TaskResults results = RCLibrary.Support.BuildToX.Run(coaster, x, withInX);
            builder.ProcessAfterBuildAttempt(coaster, results);
        }
        public void BuildToY(float y, float withInY)
        {
            if (coaster.TracksStarted == false)
                return;

            TaskResults results = RCLibrary.Support.BuildToY.Run(coaster, y, withInY);
            builder.ProcessAfterBuildAttempt(coaster, results);
        }
        public void BuildToZ(float z, float withInZ)
        {
            if (coaster.TracksStarted == false)
                return;

            TaskResults results = RCLibrary.Support.BuildToZ.Run(coaster, z, withInZ);
            builder.ProcessAfterBuildAttempt(coaster, results);
        }
        public void BuildToXY(float x, float y, float withInX, float withInY)
        {
            if (coaster.TracksStarted == false)
                return;

            TaskResults results = RCLibrary.Support.BuildToXY.Run(coaster, x, y, withInX, withInY);
            builder.ProcessAfterBuildAttempt(coaster, results);
        }
        public void BuildToXYZ(float x, float y, float z, float withInX, float withInY, float withInZ)
        {
            if (coaster.TracksStarted == false)
                return;

            TaskResults results = RCLibrary.Support.BuildToXYZ.Run(coaster, x, y, z, withInX, withInY, withInZ);
            builder.ProcessAfterBuildAttempt(coaster, results);
        }
        public void BuildToYaw(List<float> yaws)
        {
            if (coaster.TracksStarted == false )
                return;

            TaskResults results = RCLibrary.Support.BuildToYaw.Run(coaster, yaws);
            builder.ProcessAfterBuildAttempt(coaster, results);
        }
        public void BuildToPitch(List<float> pitch)
        {
            if (coaster.TracksStarted == false)
                return;

            TaskResults results = RCLibrary.Support.BuildToPitch.Run(coaster, pitch);
            builder.ProcessAfterBuildAttempt(coaster, results);
        }
    }
}
