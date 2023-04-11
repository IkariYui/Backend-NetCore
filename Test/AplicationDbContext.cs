using Microsoft.EntityFrameworkCore;
using Test.models;

namespace Test
{
    public class AplicationDbContext: DbContext
    {
        public DbSet<Comentario> Comentario { get; set; }

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }
    }
}
