using System.Threading.Tasks;
using BasicTodoList.API.Data;
using BasicTodoList.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BasicTodoList.API.Controllers
{
    /// <summary>
    /// Controller xử lý các thao tác CRUD cho danh sách công việc (TodoList)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] // Route mặc định: api/todo
    public class TodoController : ControllerBase
    {
        //// Danh sách tạm thời lưu trong bộ nhớ (thay cho database trong ví dụ đơn giản)
        //private static List<TodoItem> _todos = new List<TodoItem>();

        //// Biến đếm để tự động tăng ID
        //private static int _nextId = 1;

        private readonly TodoDbContext _context;

        public TodoController(TodoDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Lấy tất cả các mục công việc
        /// </summary>
        /// <returns>Danh sách các mục công việc</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _context.Todos.ToListAsync();

            // Trả về HTTP 200 OK với danh sách todos
            return Ok(todos);
        }

        /// <summary>
        /// Lấy một mục công việc theo ID
        /// </summary>
        /// <param name="id">ID của mục công việc cần lấy</param>
        /// <returns>Mục công việc tìm thấy hoặc NotFound nếu không tồn tại</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Tìm todo item có ID trùng khớp
            var todo = await _context.Todos.FindAsync(id);

            // Nếu không tìm thấy, trả về HTTP 404 Not Found
            if (todo == null) return NotFound();

            // Nếu tìm thấy, trả về HTTP 200 OK với todo item
            return Ok(todo);
        }

        /// <summary>
        /// Thêm một mục công việc mới
        /// </summary>
        /// <param name="todo">Đối tượng mục công việc mới</param>
        /// <returns>Thông tin mục công việc vừa tạo kèm link để xem chi tiết</returns>
        [HttpPost]
        public async Task<IActionResult> Create(TodoItem todo)
        {
            //// Gán ID mới (tự động tăng)
            //todo.Id = _nextId++;

            //// Thêm vào danh sách
            //_todos.Add(todo);

            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();

            // Trả về HTTP 201 Created với link để xem item vừa tạo
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        /// <summary>
        /// Cập nhật thông tin mục công việc
        /// </summary>
        /// <param name="id">ID của mục cần cập nhật</param>
        /// <param name="updatedTodo">Đối tượng mới với thông tin cập nhật</param>
        /// <returns>NoContent nếu thành công hoặc NotFound nếu không tìm thấy</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TodoItem updatedTodo)
        {
            var existingTodo = await _context.Todos.FindAsync(id);
            if (existingTodo == null) return NotFound();

            existingTodo.Task = updatedTodo.Task;
            existingTodo.IsCompleted = updatedTodo.IsCompleted;
            existingTodo.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            // Trả về HTTP 204 No Content (thành công nhưng không có nội dung trả về)
            return NoContent();
        }

        /// <summary>
        /// Xóa một mục công việc
        /// </summary>
        /// <param name="id">ID của mục cần xóa</param>
        /// <returns>NoContent nếu thành công hoặc NotFound nếu không tìm thấy</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
