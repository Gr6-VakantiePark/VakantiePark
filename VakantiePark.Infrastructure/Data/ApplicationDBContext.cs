using Microsoft.EntityFrameworkCore;

namespace StockFlow.Infrastructure.Data
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
    {
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSeeding((context, _) =>
            {
                //context.Use{NAME}Seeder();
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
