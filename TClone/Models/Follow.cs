using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TClone.Models
{
    public class Follow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FollowId { get; set; }
        public string Username { get; set; }
        public string Follows { get; set; }
    }
}
