using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace First_Project.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeItem> RecipeItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##MVC;PASSWORD=Test321;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##MVC")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C008477");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CATEGORYNAME");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008468");

            entity.ToTable("PROFILE");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.Bio)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("BIO");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.User).WithOne(p => p.Profile)
                .HasForeignKey<Profile>(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008469");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Recipeid).HasName("SYS_C008482");

            entity.ToTable("RECIPE");

            entity.Property(e => e.Recipeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RECIPEID");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Instructions)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("INSTRUCTIONS");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("SYS_C008484");

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008483");
        });

        modelBuilder.Entity<RecipeItem>(entity =>
        {
            entity.HasKey(e => e.Recipeitemid).HasName("SYS_C008487");

            entity.ToTable("RECIPE_ITEM");

            entity.Property(e => e.Recipeitemid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RECIPEITEMID");
            entity.Property(e => e.Ingredient)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("INGREDIENT");
            entity.Property(e => e.Quantity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("QUANTITY");
            entity.Property(e => e.Recipeid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RECIPEID");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("UNIT");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeItems)
                .HasForeignKey(d => d.Recipeid)
                .HasConstraintName("SYS_C008488");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008460");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008472");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Dateposted)
                .HasPrecision(6)
                .HasColumnName("DATEPOSTED");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008473");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008465");

            entity.ToTable("USERS");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("SYS_C008466");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
