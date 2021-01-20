using System;
using Chat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data
{
    public class ChatDbContext : DbContext, IDisposable
    {
        public ChatDbContext(DbContextOptions dbContextOptions) : base(options: dbContextOptions) { }

        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Group>().Property(x => x.Name).HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Entity<Group>().Property(x => x.IsActive).HasDefaultValue(true).IsRequired();
            builder.Entity<Group>().Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Entity<Group>().Property(x => x.CreatedDate).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Entity<Group>().HasIndex(x => new { x.Name });

            builder.Entity<User>().Property(x => x.Name).HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Entity<User>().Property(x => x.IsActive).HasDefaultValue(true).IsRequired();
            builder.Entity<User>().Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Entity<User>().Property(x => x.CreatedDate).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Entity<User>().HasIndex(x => new { x.Name });

            builder.Entity<Conversation>().Property(x => x.Message).HasColumnType("varchar(1000)").HasMaxLength(1000).IsRequired();
            builder.Entity<Conversation>().Property(x => x.IsActive).HasDefaultValue(true).IsRequired();
            builder.Entity<Conversation>().Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Entity<Conversation>().Property(x => x.CreatedDate).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Entity<Conversation>().Property(x => x.SenderId).IsRequired();
            builder.Entity<Conversation>().HasIndex(x => new { x.SenderId, x.ReceiverId, x.GroupId });

            base.OnModelCreating(builder);
        }
    }
}