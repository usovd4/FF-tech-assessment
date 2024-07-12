using Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    internal class AppDB : DbContext
    {
        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Speciality> Specialities { get; set; } = null!;
        public DbSet<PersonSpecialityMap> People_Specialities_Map { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Config.DbConnectionString);
        }
    }


}
