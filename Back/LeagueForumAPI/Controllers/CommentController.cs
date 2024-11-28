// Controllers/CommentController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeagueForumAPI.Data;
using System.Linq;
using System.Threading.Tasks;
using LeagueForumAPI.Models;


namespace LeagueForumAPI.Controllers
{
    [Route("api/comments/{postId:int}")]

    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получение всех комментариев для конкретного поста
        [HttpGet]
        public async Task<IActionResult> GetComments(int postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.Author)
                .ToListAsync();

            return Ok(comments);
        }

        // Добавление нового комментария для поста
        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, [FromBody] CommentRequest commentRequest)
        {
            // Проверяем, что commentRequest содержит контент
            if (string.IsNullOrEmpty(commentRequest.Content))
            {
                return BadRequest("Комментарий не может быть пустым.");
            }

            // Ищем пост, к которому добавляется комментарий
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound("Пост не найден.");
            }

            // Проверяем, что передан authorId
            if (commentRequest.AuthorId == 0)
            {
                return BadRequest("AuthorId не может быть пустым.");
            }

            // Ищем автора комментария
            var author = await _context.Users.FindAsync(commentRequest.AuthorId);
            if (author == null)
            {
                return NotFound("Автор не найден.");
            }

            // Создаем новый комментарий
            var comment = new Comment
            {
                PostId = postId,
                Content = commentRequest.Content,
                AuthorId = commentRequest.AuthorId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Возвращаем комментарий с данными об авторе
            var commentWithAuthor = new
            {
                comment.Id,
                comment.Content,
                comment.PostId,
                Author = author.Name // Возвращаем имя автора
            };

            return CreatedAtAction(nameof(GetComments), new { postId = postId }, commentWithAuthor);
        }

    }

}