using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrizeDrawWeb.Models.Home
{
    public class EventViewModel
    {
        public double RowHeight { get; set; }
        public double CellWidth { get; set; }
        public RsvpViewModel[][] Rsvps { get; set; }
    }
    public class RsvpViewModel
    {
        public string Display { get; set; }
        public string RsvpInfo { get; set; }
        public string Colour { get; set; }
        public string ImageUrl { get; set; }
    }
}
