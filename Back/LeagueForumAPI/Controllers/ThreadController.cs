using LeagueForumAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thread = LeagueForumAPI.Data.Thread;

namespace LeagueForumAPI.Controllers
{
    [Route("api/threads")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ThreadController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получение всех тем
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thread>>> GetThreads()
        {
            try
            {
                var threads = await _context.Threads.ToListAsync();
                if (threads == null || !threads.Any())
                {
                    return NoContent();  // или просто возвращаем пустой список
                }

                return Ok(threads);
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        // Получение конкретной темы по ID
        [HttpGet("thread/{id:int}")]
        public async Task<ActionResult<Thread>> GetThreadById(int id)
        {
            try
            {
                var thread = await _context.Threads
                    .Include(t => t.Posts) // Загружаем связанные посты
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (thread == null)
                {
                    return NotFound(new { message = "Тема не найдена" });
                }

                return Ok(thread); // Возвращаем тему с вложенными постами
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
        
        // Создание новой темы
        [HttpPost]
        public async Task<IActionResult> CreateThread([FromBody] Thread thread)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Threads.Add(thread);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetThreadById), new { id = thread.Id }, thread);
        }

        // Обновление темы
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateThread(int id, [FromBody] Thread updatedThread)
        {
            var thread = await _context.Threads.FindAsync(id);

            if (thread == null)
                return NotFound("Тема не найдена");

            thread.Title = updatedThread.Title;
            thread.Description = updatedThread.Description;

            _context.Threads.Update(thread);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Удаление темы
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThread(int id)
        {
            var thread = await _context.Threads.FindAsync(id);

            if (thread == null)
                return NotFound("Тема не найдена");

            _context.Threads.Remove(thread);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
