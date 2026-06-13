using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BagStore.Models
{
    public class BagContext : DbContext
    {
        public BagContext(DbContextOptions<BagContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bag> Bags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bag>().HasData(

                new Bag
                {
                    BagId = 1,
                    BagName = "Classic Leather Bag",
                    Brand = "Zara",
                    Price = 45.99m,
                    Color = "Black",
                    UserId = 1
                },

                new Bag
                {
                    BagId = 2,
                    BagName = "Mini Crossbody Bag",
                    Brand = "Mango",
                    Price = 29.99m,
                    Color = "Beige",
                    UserId = 1
                },

                new Bag
                {
                    BagId = 3,
                    BagName = "Travel Backpack",
                    Brand = "Nike",
                    Price = 65.00m,
                    Color = "Gray",
                    UserId = 2
                },

                new Bag
                {
                    BagId = 4,
                    BagName = "Elegant Handbag",
                    Brand = "Guess",
                    Price = 75.50m,
                    Color = "Brown",
                    UserId = 3
                }

            );

            modelBuilder.Entity<User>().HasData(

                new User
                {
                    UserId = 1,
                    FullName = "Sara Ahmad",
                    Email = "sara@gmail.com",
                    Password = "123"
                },

                new User
                {
                    UserId = 2,
                    FullName = "Rawan Ali",
                    Email = "rawan@gmail.com",
                    Password = "456"
                },

                new User
                {
                    UserId = 3,
                    FullName = "Noor Omar",
                    Email = "noor@gmail.com",
                    Password = "789"
                }

            );
        }
    }
}
