using System.ComponentModel.DataAnnotations;

namespace ClaySysEventMa.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
        [Required]
        public int EventId { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
