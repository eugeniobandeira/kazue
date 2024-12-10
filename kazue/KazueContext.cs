using Microsoft.EntityFrameworkCore;

namespace kazue
{
    public class KazueContext : DbContext
    {
        public KazueContext(DbContextOptions<KazueContext> options) : base(options) { }

        public DbSet<ClienteEntidade> Fila { get; set; }
    }
}
