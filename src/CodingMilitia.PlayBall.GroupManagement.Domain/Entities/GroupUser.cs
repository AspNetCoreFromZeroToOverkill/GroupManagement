namespace CodingMilitia.PlayBall.GroupManagement.Domain.Entities
{
    public class GroupUser
    {
        //public long GroupId { get; set; }
        public Group Group { get; set; }
        //public string UserId { get; set; }
        public User User { get; set; }
        public GroupUserRole Role { get; set; }
    }
}