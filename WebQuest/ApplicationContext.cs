using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ELEKSUNI;

namespace WebQuest
{
    public class ApplicationContext : DbContext
    {
        public DbSet<QuestState> Quests { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=QuestsDB;Username=postgres;Password=samehaun");
        }
    }

}
