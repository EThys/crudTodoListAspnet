using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using todoApp.Models;

namespace todoApp
{
    public class TodoDbContext:DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
        public DbSet<TaskItem> TaskItems { get; set; }

    }
}
