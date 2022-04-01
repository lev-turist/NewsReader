using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace NewsReader.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<News> News { get; set; }
    }
}
