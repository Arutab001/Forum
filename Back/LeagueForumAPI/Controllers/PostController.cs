// Controllers/PostController.cs

using System.Security.Claims;
using LeagueForumAPI.Data;
using LeagueForumAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeagueForumAPI.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получение всех постов
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts.Include(p => p.Author).ToListAsync();
            return Ok(posts);
        }

        // Получение поста по его ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Author) // Загружаем информацию о авторе
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound(new { message = "Пост не найден" });
            }

            return Ok(post);
        }

        // Получение постов по ID темы
        [HttpGet("thread/{threadId:int}/posts")]
        public async Task<IActionResult> GetPostsByThreadId(int threadId)
        {
            var posts = await _context.Posts
                .Where(p => p.ThreadId == threadId)
                .Include(p => p.Author) // Загружаем информацию о пользователе (авторе)
                .ToListAsync();

            return Ok(posts);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            // Проверяем, что данные заполнены
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Заголовок и контент не могут быть пустыми.");
            }

            // Создаем новый объект Post
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                ThreadId = request.ThreadId,
                AuthorId = request.AuthorId // Привязываем пользователя
            };

            // Добавляем пост в базу данных
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Загружаем пост с его автором
            var postWithAuthor = await _context.Posts
                .Where(p => p.Id == post.Id)
                .Include(p => p.Author)  // Включаем информацию о пользователе (авторе)
                .FirstOrDefaultAsync();

            // Если пост или автор не найдены, возвращаем ошибку
            if (postWithAuthor == null || postWithAuthor.Author == null)
            {
                return NotFound("Пост или автор не найдены.");
            }

            // Возвращаем созданный пост с автором
            return CreatedAtAction(nameof(GetPosts), new { id = postWithAuthor.Id }, postWithAuthor);
        }

    }
}
