using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrizeDrawWeb.Services.Meetup
{
    public class MeetupService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MeetupService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Rsvp[]> GetRsvpsAsync(string groupId, int eventId)
        {
            var client = _httpClientFactory.CreateClient("meetup");

            // https://www.meetup.com/meetup_api/docs/:urlname/events/:event_id/rsvps/#list
            var response = await client.GetAsync($"/{groupId}/events/{eventId}/rsvps?response=yes"); // get "yes" responses for event
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var rsvps = JsonConvert.DeserializeAnonymousType(content,
                new[] {
                        new {
                            response = "",
                            member = new {
                                name = "",
                                id = 123,
                                photo = new
                                {
                                    highres_link = "",
                                    photo_link = ""
                                }
                            }
                        }
                }
                );

            return rsvps.Select(x => new Rsvp(
                name: x.member.name,
                attendeeId: x.member.id,
                imageUri: x.member.photo?.highres_link ?? x.member.photo?.photo_link
            )).ToArray();
        }
    }

    public class Rsvp
    {
        public Rsvp(string name, int attendeeId, string imageUri)
        {
            Name = name;
            AttendeeId = attendeeId;
            ImageUri = imageUri;
        }

        public string Name { get; }
        public int AttendeeId { get; }
        public string ImageUri { get; }
    }
}
