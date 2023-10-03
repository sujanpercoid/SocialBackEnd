namespace TClone.Models
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Post { get; set; }
        public string Username { get; set; }
        public string View { get; set; }
        public int TotalLikes { get; set; }
        public int LikeId { get; set; }
    }
}
