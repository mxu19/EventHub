using System;
using System.Linq;
using Events.Web.Models;
using Microsoft.AspNet.Identity;

namespace Events.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var events = this.db.Events
                .OrderBy(e => e.StartDateTime)
                .Where(e => e.IsPublic)
                .Select(EventViewModel.ViewModel);

            var otherEventsImGoingIds = new int[] {};

            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);
            var otherEvents = events.Where(e => otherEventsImGoingIds.Contains(e.Id));

            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents,
                OtherEvents = otherEvents
            });
        }

        public ActionResult AddEventById(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            Data.MyCalendar myCalendars = new Data.MyCalendar
            {
                UserId = currentUserId,
                EventId = id
            };
            this.db.MyCalendars.Add(myCalendars);
            this.db.SaveChanges();
            return this.RedirectToAction("My", "Events", "Events/My");
        }

        public ActionResult FollowUserById(string id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            Data.MyFollow myFollows = new Data.MyFollow
            {
                UserId = currentUserId,
                FollowedUserId = id
            };
            this.db.MyFollows.Add(myFollows);
            this.db.SaveChanges();
            return this.RedirectToAction("Index", "MyFollow", "MyFollow/Index");
        }

        public ActionResult EventDetailsById(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventDetails = this.db.Events
                .Where(e => e.Id == id)
                .Where(e => e.IsPublic || isAdmin || (e.AuthorId != null && e.AuthorId == currentUserId))
                .Select(EventDetailsViewModel.ViewModel)
                .FirstOrDefault();

            var isOwner = (eventDetails != null && eventDetails.AuthorId != null &&
                           eventDetails.AuthorId == currentUserId);
            this.ViewBag.CanEdit = isOwner || isAdmin;

            return this.PartialView("_EventDetails", eventDetails);
        }
    }
}