using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TClone.Models
{
    public class Bookmark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookmarkId { get; set; }
        public string Username { get; set; }
        public int PostId { get; set; }
    }
}
