using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace First_Project.Models;

public partial class ModelContext : DbContext
{

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AboutUs> AboutUs { get; set; }
    public virtual DbSet<ContactUs> ContactUs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<HomePage> HomePages { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeItem> RecipeItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Order> Orders{ get; set; }
    public virtual DbSet<MenuItem> MenuItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
