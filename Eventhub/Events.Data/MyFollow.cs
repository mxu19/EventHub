namespace Events.Data
{
    public class MyFollow
    {
        public MyFollow()
        {
        }
        public int Id { get; set; }

        public string UserId { get; set; }

        public string FollowedUserId { get; set; }
    }
}
