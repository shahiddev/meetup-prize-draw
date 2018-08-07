using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrizeDrawWeb.Models;
using PrizeDrawWeb.Models.Home;
using PrizeDrawWeb.Services.Meetup;

using static System.Math;

namespace PrizeDrawWeb.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index(string meetupUrl)
        {
            meetupUrl = (meetupUrl ?? "").Trim(); // handle null input, trim any spaces that were accidentally copied :-)
           
            (string groupId, string eventId) = MeetupUrlParser.Parse(meetupUrl);
            if (groupId ==null || eventId == null)
            {
                 TempData["Message"] = "Couldn't parse URL :-(";
                return View();
            }
            else
            {
                 return RedirectToRoute("PrizeDraw", new { groupId, eventId });

            }
           
        }
        [HttpGet("prize-draws/{groupId}/events/{eventId}", Name = "PrizeDraw")]
        public async Task<IActionResult> Event([FromServices] MeetupService meetup, string groupId, int eventId)
        {
            var colours = new[]
            {
                "red", "yellow", "blue", "greenyellow", "azure", "purple", "maroon", "lightseagreen", "lightsalmon", "darkolivegreen"
            };

            var rsvps = await meetup.GetRsvpsAsync(groupId, eventId);

            var random = new Random();
            var rsvpModelsTemp = rsvps.Select(r => new RsvpViewModel
            {
                Display = r.Name,
                RsvpInfo = $"Name: {r.Name}; Id: {r.AttendeeId}",
                Colour = colours[random.Next(colours.Length)],
                ImageUrl = r.ImageUri
            }).ToArray();

            var rowCount = Ceiling(Sqrt(rsvpModelsTemp.Length));
            var columnCount = (rowCount * (rowCount - 1) >= rsvpModelsTemp.Length) ? rowCount - 1 : rowCount;

            var rsvpModels = rsvpModelsTemp
                .Select((r, i) => new { group = Floor(i / columnCount), Rsvp = r }) // add group number to each item
                .GroupBy(g => g.group, g => g.Rsvp) // group into rows
                .Select(r => r.ToArray()) // turn each row into an array of rsvps
                .ToArray(); // turn the rows into an array

            var viewModel = new EventViewModel
            {
                Rsvps = rsvpModels,
                RowHeight = 100.0 / rowCount,
                CellWidth = 100.0 / columnCount
            };
            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
