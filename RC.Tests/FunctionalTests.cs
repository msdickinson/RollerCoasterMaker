using System.Collections.Generic;
using NUnit.Framework;
using RCLibrary;

namespace NUnitTest
{
    [TestFixture]
    class GameTests
    {
        [Test]
        public void Reset()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            Assert.AreEqual(64, game.coaster.TrackCount);
        }


        [Test]
        public void DetectMinX()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            for(int i = 0; i < 30; i++)
            {
                game.BuildStright();
            }
            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MinX);
        }

        [Test]
        public void DetectMinY()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();

            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MinY);
        }

        [Test]
        public void DetectMinZ()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildDown();
            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MinZ);
        }

        [Test]
        public void DetectMaxX()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildRight();
            game.BuildRight();
            game.BuildRight();
            game.BuildRight();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();

            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MaxX);
        }
        [Test]
        public void DetectMaxY()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            for (int i = 0; i < 45; i++)
            {
                game.BuildStright();
            }

            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MaxY);
        }

        [Test]
        public void DetectCollision()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();

            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();


            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.Collison);
        }
        [Test]
        public void FixMinX()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            for (int i = 0; i < 30; i++)
            {
                game.BuildStright();
            }
            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MinX);
            Assert.AreEqual(game.builder.lastBuildActionFail, false);
        }

        [Test]
        public void FixMinY()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();

            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MinY);
            Assert.AreEqual(game.builder.lastBuildActionFail, false);
        }

        [Test]
        public void FixMinZ()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildUp();
            game.BuildUp();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildDown();
            game.BuildDown();
            game.BuildDown();
            game.BuildDown();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();

            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MinZ);
            Assert.AreEqual(game.builder.lastBuildActionFail, false);
        }

        [Test]
        public void FixMaxX()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildRight();
            game.BuildRight();
            game.BuildRight();
            game.BuildRight();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();

            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MaxX);
            Assert.AreEqual(game.builder.lastBuildActionFail, false);
        }

        [Test]
        public void FixMaxY()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            for (int i = 0; i < 45; i++)
            {
                game.BuildStright();
            }

            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.MaxY);
            Assert.AreEqual(game.builder.lastBuildActionFail, false);

        }
        [Test]
        public void FixTrackCollison()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            for (int i = 0; i < 6; i++)
            {
                game.BuildStright();
            }
            for (int i = 0; i < 15; i++)
            {
                game.BuildLeft();
            }
            Assert.AreEqual(game.builder.initialTaskResults, TaskResults.Collison);
            Assert.AreEqual(game.builder.lastBuildActionFail, false);
        }

        [Test]
        public void BuildStright()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildStright();
            Assert.AreEqual(67, game.coaster.TrackCount);
        }
        [Test]
        public void BuildLeft()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildLeft();
            Assert.AreEqual(67, game.coaster.TrackCount);
        }
        [Test]
        public void BuildRight()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildRight();
            Assert.AreEqual(67, game.coaster.TrackCount);
        }
        [Test]
        public void BuildUp()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildUp();

            Assert.AreEqual(67, game.coaster.TrackCount);
        }
        [Test]
        public void BuildDown()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildUp();
            game.BuildDown();

            Assert.AreEqual(70, game.coaster.TrackCount);
        }

        [Test]
        public void RemoveChunk()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildStright();
            game.Back();
            Assert.AreEqual(64, game.coaster.TrackCount);
        }
        [Test]
        public void BuildLoop()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            for (int i = 0; i < 10; i++)
            {
                game.BuildStright();
            }
            game.BuildLoop();

            Assert.AreEqual(false, game.builder.lastBuildActionFail);
        }
        [Test]
        public void BuildUpward()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            game.BuildUpWard();

            Assert.AreEqual(false, game.builder.lastBuildActionFail);
        }
        [Test]
        public void BuildDownward()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            for (int i = 0; i < 4; i++)
            {
                game.BuildUp();
            }

            for (int i = 0; i < 10; i++)
            {
                game.BuildStright();
            }
            game.BuildUpWard();

            Assert.AreEqual(false, game.builder.lastBuildActionFail);
        }
        [Test]
        public void BuildFlaten()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           
            for (int i = 0; i < 4; i++)
            {
                game.BuildUp();
            }

            for (int i = 0; i < 10; i++)
            {
                game.BuildStright();
            }
            game.BuildFlaten();

            Assert.AreEqual(false, game.builder.lastBuildActionFail);
        }
        //[Test]
        //public void BuildToFinshArea()
        //{
        //    throw (new NotImplementedException());
        //}
        //[Test]
        //public void BuildInFinshArea()
        //{
        //    throw (new NotImplementedException());
        //}



        [Test]
        public void BuildToPitch()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildToPitch(new List<float>() { 90 });

            Assert.AreEqual(90, game.coaster.Tracks[game.coaster.TrackCount - 1].Pitch);
        }
        //[Test]
        //public void BuildToX()
        //{
        //    RollerCoasterMaker game = new RollerCoasterMaker();
        //   

        //    float x = 500;
        //    float withinX = 100;

        //    game.BuildToY(x, withinX);

        //    Assert.IsTrue(game.coaster.Tracks[game.coaster.TrackCount - 1].X >= x - withinX && game.coaster.Tracks[game.coaster.TrackCount - 1].X <= x + withinX);
        //}
        //[Test]
        //public void BuildToXY()
        //{
        //    RollerCoasterMaker game = new RollerCoasterMaker();
        //   

        //    float x = 500;
        //    float y = 500;
        //    float withinX = 100;
        //    float withinY = 100;

        //    game.BuildToXY(x, y, withinX, withinY);

        //    Assert.IsTrue(game.coaster.Tracks[game.coaster.TrackCount - 1].X >= x - withinX && game.coaster.Tracks[game.coaster.TrackCount - 1].X <= x + withinX &&
        //                  game.coaster.Tracks[game.coaster.TrackCount - 1].Y >= y - withinY && game.coaster.Tracks[game.coaster.TrackCount - 1].Y <= y + withinY);
        //}
        //[Test]
        //public void BuildToXYZ()
        //{
        //    RollerCoasterMaker game = new RollerCoasterMaker();
        //   

        //    float x = 500;
        //    float y = 500;
        //    float z = 500;
        //    float withinX = 100;
        //    float withinY = 100;
        //    float withinZ = 100;
        //    game.BuildToXYZ(x, y, z, withinX, withinY, withinZ);

        //    Assert.IsTrue(game.coaster.Tracks[game.coaster.TrackCount - 1].X >= x - withinX && game.coaster.Tracks[game.coaster.TrackCount - 1].X <= x + withinX &&
        //                  game.coaster.Tracks[game.coaster.TrackCount - 1].Y >= y - withinY && game.coaster.Tracks[game.coaster.TrackCount - 1].Y <= y + withinY && 
        //                  game.coaster.Tracks[game.coaster.TrackCount - 1].Z >= z - withinZ && game.coaster.Tracks[game.coaster.TrackCount - 1].Z <= z + withinZ);
        //}
        //[Test]
        //public void BuildToY()
        //{
        //    RollerCoasterMaker game = new RollerCoasterMaker();
        //   

        //    float y = 500;
        //    float withinY = 100;

        //    game.BuildToY(y, withinY);

        //    Assert.IsTrue(game.coaster.Tracks[game.coaster.TrackCount - 1].Y >= y - withinY && game.coaster.Tracks[game.coaster.TrackCount - 1].Y <= y + withinY);
        //}
        [Test]
        public void BuildToYaw()
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
           

            game.BuildToYaw(new List<float>() { 180 });

            Assert.AreEqual(180, game.coaster.Tracks[game.coaster.TrackCount - 1].Yaw);
        }
        //[Test]
        //public void BuildToZ()
        //{
        //    RollerCoasterMaker game = new RollerCoasterMaker();
        //   

        //    float z = 500;
        //    float withinZ = 100;

        //    game.BuildToZ(500, 100);

        //    Assert.IsTrue(game.coaster.Tracks[game.coaster.TrackCount - 1].Z >= z - withinZ && game.coaster.Tracks[game.coaster.TrackCount - 1].Z <= z + withinZ);
        //}
        //[Test]
        //public void RemoveCustomTrack()
        //{
        //    //Remove Custom Chunk By doing Back
            
        //    //Have a fix function attempt to remove a custom track, and have it NOT remove it.
        //    //OR
        //    //Have a fix function attempt to remove a custom track, and remove the entire chunk
        //    //OR
        //    //Always Force Back to remove the last chunk
        //    //OR
        //    //Make sure there is a FIX Custom at the end of Build Process

        //    throw (new NotImplementedException());
        //}
        //[Test]
        //public void AutoDetectFinshArea()
        //{
        //    //Remove Custom Chunk By doing Back

        //    //Have a fix function attempt to remove a custom track, and have it NOT remove it.
        //    //OR
        //    //Have a fix function attempt to remove a custom track, and remove the entire chunk
        //    //OR
        //    //Always Force Back to remove the last chunk
        //    //OR
        //    //Make sure there is a FIX Custom at the end of Build Process

        //    throw (new NotImplementedException());
        //}
        //[Test]
        //public void AutoLoop()
        //{
        //    throw (new NotImplementedException());
        //}
    }
}
