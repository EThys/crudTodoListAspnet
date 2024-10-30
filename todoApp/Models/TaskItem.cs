using System.ComponentModel.DataAnnotations;

namespace todoApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire.")]
        [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "La description est obligatoire.")]
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La date d'échéance est obligatoire.")]
        public DateTime DueDate { get; set; }
        public DateOnly CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}
