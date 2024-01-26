using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication
{
    internal class TestData
    {
        public static void AddToDatabase()
        {
            using (var myDb = new Models.MyDbContext())
            {
                Models.Facility whiteboard = new Models.Facility { Name = "Whiteboard" };
                Models.Facility projector = new Models.Facility { Name = "Projector" };
                Models.Facility smartboard = new Models.Facility { Name = "Smartboard" };
                Models.Facility overheadProjector = new Models.Facility { Name = "Overhead projector" };
                Models.Facility surroundSpeakers = new Models.Facility { Name = "Surround speakers" };
                Models.Facility projectorScreen = new Models.Facility { Name = "Large Projector screen" };
                Models.Facility microphones = new Models.Facility { Name = "Microphones" };
                Models.Facility discoLightning = new Models.Facility { Name = "Disco lightning" };

                myDb.AddRange(whiteboard, projector, smartboard, overheadProjector, surroundSpeakers, projectorScreen, microphones, discoLightning);

                Models.Room room1 = new Models.Room { Name = "" };

                myDb.SaveChanges();
            }
        }
    }
}
