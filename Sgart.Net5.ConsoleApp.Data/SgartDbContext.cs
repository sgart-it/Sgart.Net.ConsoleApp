using Microsoft.EntityFrameworkCore;

namespace Sgart.Net5.ConsoleApp.Data
{
    public class SgartDbContext : DbContext
    {
        public SgartDbContext(DbContextOptions options) : base(options)
        {
        }

        // aggiungere tutte le entite/tabelle
        public DbSet<BO.Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // regitro tutte le configurazioni presenti nell'assembly
            // vedi cartella Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SgartDbContext).Assembly);
            
            // aggiungere eventuali indici o vincoli di univocità

            //var eUser = modelBuilder.Entity<User>();
            //eUser.HasIndex(x => x.LoginName).IsUnique(false);
        }
    }
}
