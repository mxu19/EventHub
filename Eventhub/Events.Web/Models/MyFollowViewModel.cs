namespace Events.Web.Models
{
    using System.Collections.Generic;

    public class MyFollowViewModel
    {
        public IDictionary<string, IEnumerable<EventViewModel>> MyFollowedEvents { get; set; }
    }
}