using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication.Models
{
    internal class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Size should be an enum with different sizes
        public int Size { get; set; }
        public int Seats { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ICollection<Facility> Facilities {  get; set; }
    }
}
