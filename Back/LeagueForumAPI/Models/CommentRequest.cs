// Models/CommentRequest.cs
namespace LeagueForumAPI.Models
{
    public class CommentRequest
    {
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }
    }
}