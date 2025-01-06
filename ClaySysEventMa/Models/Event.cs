using System.ComponentModel.DataAnnotations;

namespace ClaySysEventMa.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Location { get; set; }

        public string ImageBase64 { get; set; }  
    }
}
