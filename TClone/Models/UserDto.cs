namespace TClone.Models
{
    public class UserDto
    {
        public int ContactId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public long Phone { get; set; }
        public bool Active { get; set; } = true;
    }
}
