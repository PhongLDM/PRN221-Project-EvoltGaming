using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EvoltingStore.Entity
{
    public partial class EvoltingStoreContext : DbContext
    {
        public EvoltingStoreContext()
        {
        }

        public EvoltingStoreContext(DbContextOptions<EvoltingStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameRequirement> GameRequirements { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=(local); database=EvoltingStore;user=sa; password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("commentId");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.GameId).HasColumnName("gameId");

                entity.Property(e => e.PostTime)
                    .HasColumnType("datetime")
                    .HasColumnName("postTime");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameId).HasColumnName("gameId");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .HasColumnType("text")
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.OfficialLink)
                    .HasColumnType("text")
                    .HasColumnName("officialLink");

                entity.Property(e => e.PirateLink)
                    .HasColumnType("text")
                    .HasColumnName("pirateLink");

                entity.Property(e => e.Platform)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("platform");

                entity.Property(e => e.Publisher)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("publisher");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("date")
                    .HasColumnName("releaseDate");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");

                entity.HasMany(d => d.Genres)
                    .WithMany(p => p.Games)
                    .UsingEntity<Dictionary<string, object>>(
                        "GameGenre",
                        l => l.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_GameGenre_Genre"),
                        r => r.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_GameGenre_Game"),
                        j =>
                        {
                            j.HasKey("GameId", "GenreId");

                            j.ToTable("GameGenre");

                            j.IndexerProperty<int>("GameId").HasColumnName("gameId");

                            j.IndexerProperty<int>("GenreId").HasColumnName("genreId");
                        });

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Games)
                    .UsingEntity<Dictionary<string, object>>(
                        "Favourite",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Favourite_User"),
                        r => r.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Favourite_Game"),
                        j =>
                        {
                            j.HasKey("GameId", "UserId");

                            j.ToTable("Favourite");

                            j.IndexerProperty<int>("GameId").HasColumnName("gameId");

                            j.IndexerProperty<int>("UserId").HasColumnName("userId");
                        });
            });

            modelBuilder.Entity<GameRequirement>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.Type });

                entity.ToTable("GameRequirement");

                entity.Property(e => e.GameId).HasColumnName("gameId");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.DirectX).HasColumnName("directX");

                entity.Property(e => e.Graphic)
                    .HasColumnType("text")
                    .HasColumnName("graphic");

                entity.Property(e => e.Memory).HasColumnName("memory");

                entity.Property(e => e.Os)
                    .HasColumnType("text")
                    .HasColumnName("os");

                entity.Property(e => e.Other)
                    .HasColumnType("text")
                    .HasColumnName("other");

                entity.Property(e => e.Processor)
                    .HasColumnType("text")
                    .HasColumnName("processor");

                entity.Property(e => e.Storage).HasColumnName("storage");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameRequirements)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameRequirement_Game");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.GenreId).HasColumnName("genreId");

                entity.Property(e => e.GenreName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("genreName");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Username, "Unique_username")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserDetail");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("userId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.Image)
                    .HasColumnType("text")
                    .HasColumnName("image");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserDetail)
                    .HasForeignKey<UserDetail>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDetail_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
