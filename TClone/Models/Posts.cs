using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TClone.Models
{
    public class Posts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        public string Post { get; set; }
        public string Username { get; set; }
        public string View { get; set; }

    }
}
