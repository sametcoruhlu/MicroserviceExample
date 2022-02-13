using MicroserviceExample.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceExample.EntityFramework
{
    public class MicroserviceExampleContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public MicroserviceExampleContext(DbContextOptions<MicroserviceExampleContext> options) : base(options)
        {

        }
    }
}
