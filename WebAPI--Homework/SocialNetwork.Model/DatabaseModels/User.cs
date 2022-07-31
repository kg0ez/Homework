namespace SocialNetwork.Model.DatabaseModels
{
	public class User
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
		public List<Post> Posts { get; set; }
		public string RefreshToken { get; set; }
		public DateTime TokenCreated { get; set; }
		public DateTime TokenExpires { get; set; }
	}
}

