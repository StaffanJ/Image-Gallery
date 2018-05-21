using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace JavaScript.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Url { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}