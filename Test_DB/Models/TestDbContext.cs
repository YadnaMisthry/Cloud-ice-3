using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Test_DB.Models;

public partial class TestDbContext : DbContext
{
    private List<Comment> comments;

    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    private DbSet<Comment> comments1;

    public virtual DbSet<Comment> GetComments()
    {
        return comments1;
    }

    public virtual void SetComments(DbSet<Comment> value)
    {
        comments1 = value;
    }

    private DbSet<Like> likes;

    public virtual DbSet<Like> GetLikes()
    {
        return likes;
    }

    public virtual void SetLikes(DbSet<Like> value)
    {
        likes = value;
    }

    private DbSet<Post> posts;

    public virtual DbSet<Post> GetPosts()
    {
        return posts;
    }

    public virtual void SetPosts(DbSet<Post> value)
    {
        posts = value;
    }

    private DbSet<User> users;

    public virtual DbSet<User> GetUsers()
    {
        return users;
    }

    public virtual void SetUsers(DbSet<User> value)
    {
        users = value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:testdb1.database.windows.net,1433;Initial Catalog=testDB;Persist Security Info=False;User ID=yadna;Password=ScupsPilies85@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC07D89737FF");

            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Comments__PostId__3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Comments__UserId__403A8C7D");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Likes__3214EC07C0E9A130");

            entity.HasOne(d => d.Post).WithMany(p => p.Likes)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Likes__PostId__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Likes__UserId__44FF419A");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Posts__3214EC079A6BC214");

            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Posts__UserId__3B75D760");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC074A3D20A6");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E492EE57D3").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A1E5014E").IsUnique();

            entity.Property(e => e.Bio)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            // Sample data for Users
            entity.HasData(
                new User
                {
                    Id = 1,
                    Username = "john_doe",
                    Email = "john@example.com",
                    PasswordHash = "hashed_password_1", // Replace with actual hashes
                    Bio = "This is John."
                },
                new User
                {
                    Id = 2,
                    Username = "jane_smith",
                    Email = "jane@example.com",
                    PasswordHash = "hashed_password_2", // Replace with actual hashes
                    Bio = "This is Jane."
                }
            );
        });

        modelBuilder.Entity<Comment>(builder =>
        {
            builder.HasData(
                new Comment
                {
                    Id = 1,
                    Content = "This is the first comment.",
                    CreatedAt = DateTime.Now,
                    PostId = 1,
                    UserId = 1
                },
                new Comment
                {
                    Id = 2,
                    Content = "Hello, welcome to my page!",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    PostId = 2,
                    UserId = 2
                }
            );
        });

        modelBuilder.Entity<Like>(builder =>
        {
            builder.HasData(
                new Like
                {
                    Id = 1,
                    PostId = 1,
                    UserId = 1
                },
                new Like
                {
                    Id = 2,
                    PostId = 2,
                    UserId = 2
                }
            );
        });

        modelBuilder.Entity<Post>(builder =>
        {
            builder.HasData(
                new Post
                {
                    Id = 1,
                    Content = "This is the first post.",
                    CreatedAt = DateTime.Now,
                    UserId = 1
                },
                new Post
                {
                    Id = 2,
                    Content = "Hello, welcome to my page!",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UserId = 2
                }
            );
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public List<Comment> Comments { get => comments; set => comments = value; }
    public List<Like> Likes { get; set; }
    public List<Post> Posts { get; set; }
    public List<User> Users { get; set; }
}