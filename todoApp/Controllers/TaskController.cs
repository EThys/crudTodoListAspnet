using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApp.Models;


namespace todoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TodoDbContext _context;
        public TaskController(TodoDbContext context)
        {
            _context = context;
        }

        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public T? Data { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskItem>>>> GetTasks()
        {
            var tasks = await _context.TaskItems.ToListAsync();

            var response = new ApiResponse<IEnumerable<TaskItem>>
            {
                Success = true,
                Message = "Tâches récupérées avec succès.",
                Data = tasks
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TaskItem>>> GetTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)
            {
                return NotFound(new ApiResponse<TaskItem>
                {
                    Success = false,
                    Message = "Tâche non trouvée.",
                    Data = null
                });
            }

            var response = new ApiResponse<TaskItem>
            {
                Success = true,
                Message = "Tâche récupérée avec succès.",
                Data = task
            };

            return Ok(response);
        }


        [HttpPost]
        public async Task<ApiResponse<TaskItem>> PostTask(TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<TaskItem>
                {
                    Success = false,
                    Message = "Les données fournies ne sont pas valides.",
                    Data = null
                };
            }

            if (await _context.TaskItems.AnyAsync(t => t.Title == task.Title))
            {
                return new ApiResponse<TaskItem>
                {
                    Success = false,
                    Message = "Un élément avec ce titre existe déjà.",
                    Data = null
                };
            }

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            task.CreatedAt = currentDate; 
            _context.TaskItems.Add(task);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ApiResponse<TaskItem>
                {
                    Success = true,
                    Message = "Tâche ajoutée avec succès.",
                    Data = task
                };
            }

          
            return new ApiResponse<TaskItem>
            {
                Success = false,
                Message = "Une erreur est survenue lors de l'ajout de la tâche.",
                Data = null
            };
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<TaskItem>> PutTask(int id, TaskItem task)
        {

            ModelState.Clear();
            var existingTask = await _context.TaskItems.FindAsync(id);
            if (existingTask == null)
            {
                return new ApiResponse<TaskItem>
                {
                    Success = false,
                    Message = "Tâche non trouvée.",
                    Data = null
                };
            }


            existingTask.Title = task.Title;
            existingTask.Description = task.Description; 
            existingTask.IsCompleted = task.IsCompleted;

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ApiResponse<TaskItem>
                {
                    Success = true,
                    Message = "Tâche mise à jour avec succès.",
                    Data = existingTask
                };
            }

            return new ApiResponse<TaskItem>
            {
                Success = false,
                Message = "Une erreur est survenue lors de la mise à jour de la tâche.",
                Data = null
            };
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<TaskItem>> DeleteTask(int id)
        {
            var taskToDelete = await _context.TaskItems.FindAsync(id);

            if (taskToDelete == null)
            {
                return new ApiResponse<TaskItem>
                {
                    Success = false,
                    Message = "Tâche non trouvée.",
                    Data = null
                };
            }
            _context.TaskItems.Remove(taskToDelete);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ApiResponse<TaskItem>
                {
                    Success = true,
                    Message = "Tâche supprimée avec succès.",
                    Data = null
                };
            }

            return new ApiResponse<TaskItem>
            {
                Success = false,
                Message = "Une erreur est survenue lors de la suppression de la tâche.",
                Data = null
            };
        }
    }
}
