using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Events.Web.Controllers
{
    public class MyFollowController : BaseController
    {
        // GET: MyFollow
        public ActionResult Index()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var allEvents = this.db.Events
                .OrderBy(e => e.StartDateTime)
                .Select(Models.EventViewModel.ViewModel);

            var otherUsersImFollowingIds = this.db.MyFollows
                .Where(e => e.UserId == currentUserId)
                .Select(e => e.FollowedUserId)
                .ToArray();

            IDictionary<string, IEnumerable<Models.EventViewModel>> groupedEvent = new Dictionary<string, IEnumerable<Models.EventViewModel>>();

            foreach (string id_ in otherUsersImFollowingIds)
            {
                groupedEvent[id_] = allEvents.Where(e => e.AuthorId.Equals(id_));
            }

            return View(new Models.MyFollowViewModel()
            {
                MyFollowedEvents = groupedEvent
            });
        }


        public ActionResult Unfollow(string id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            Data.MyFollow follow = this.db.MyFollows.FirstOrDefault(x => x.UserId == currentUserId && x.FollowedUserId == id);

            this.db.MyFollows.Remove(follow);
            this.db.SaveChanges();
            return this.RedirectToAction("Index", "MyFollow", "MyFollow/Index");

        }


        //public ActionResult FollowUserById(string id)
        //{
        //    var currentUserId = this.User.Identity.GetUserId();
        //    Data.MyFollow myFollows = new Data.MyFollow
        //    {
        //        UserId = currentUserId,
        //        FollowedUserId = id
        //    };
        //    this.db.MyFollows.Add(myFollows);
        //    this.db.SaveChanges();
        //    return this.RedirectToAction("My", "Events", "Events/My");
        //}
    }

}