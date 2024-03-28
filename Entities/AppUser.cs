namespace API.Entities
{
    public class AppUser
    {
        // columns of table in data base
        public int Id { get; set; } // primary key
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
