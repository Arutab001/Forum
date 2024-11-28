namespace LeagueForumAPI.Models;

public class CreatePostRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int ThreadId { get; set; }
    public int AuthorId { get; set; }
}
