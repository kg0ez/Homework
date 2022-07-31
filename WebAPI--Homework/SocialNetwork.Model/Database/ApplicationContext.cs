using System;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Model.DatabaseModels;

namespace SocialNetwork.Model.Database
{
	public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; } = null!;
    }
}

