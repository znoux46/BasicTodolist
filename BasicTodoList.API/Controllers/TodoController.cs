using BasicTodoList.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicTodoList.API.Controllers
{
    /// <summary>
    /// Controller xử lý các thao tác CRUD cho danh sách công việc (TodoList)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] // Route mặc định: api/todo
    public class TodoController : ControllerBase
    {
        // Danh sách tạm thời lưu trong bộ nhớ (thay cho database trong ví dụ đơn giản)
        private static List<TodoItem> _todos = new List<TodoItem>();

        // Biến đếm để tự động tăng ID
        private static int _nextId = 1;

        /// <summary>
        /// Lấy tất cả các mục công việc
        /// </summary>
        /// <returns>Danh sách các mục công việc</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            // Trả về HTTP 200 OK với danh sách todos
            return Ok(_todos);
        }

        /// <summary>
        /// Lấy một mục công việc theo ID
        /// </summary>
        /// <param name="id">ID của mục công việc cần lấy</param>
        /// <returns>Mục công việc tìm thấy hoặc NotFound nếu không tồn tại</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Tìm todo item có ID trùng khớp
            var todo = _todos.FirstOrDefault(t => t.Id == id);

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
        public IActionResult Create(TodoItem todo)
        {
            // Gán ID mới (tự động tăng)
            todo.Id = _nextId++;

            // Thêm vào danh sách
            _todos.Add(todo);

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
        public IActionResult Update(int id, TodoItem updatedTodo)
        {
            // Tìm todo item cần cập nhật
            var existingTodo = _todos.FirstOrDefault(t => t.Id == id);

            // Nếu không tìm thấy, trả về HTTP 404 Not Found
            if (existingTodo == null) return NotFound();

            // Cập nhật thông tin
            existingTodo.Task = updatedTodo.Task;
            existingTodo.IsCompleted = updatedTodo.IsCompleted;

            // Trả về HTTP 204 No Content (thành công nhưng không có nội dung trả về)
            return NoContent();
        }

        /// <summary>
        /// Xóa một mục công việc
        /// </summary>
        /// <param name="id">ID của mục cần xóa</param>
        /// <returns>NoContent nếu thành công hoặc NotFound nếu không tìm thấy</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Tìm todo item cần xóa
            var todo = _todos.FirstOrDefault(t => t.Id == id);

            // Nếu không tìm thấy, trả về HTTP 404 Not Found
            if (todo == null) return NotFound();

            // Xóa khỏi danh sách
            _todos.Remove(todo);

            // Trả về HTTP 204 No Content
            return NoContent();
        }
    }
}
