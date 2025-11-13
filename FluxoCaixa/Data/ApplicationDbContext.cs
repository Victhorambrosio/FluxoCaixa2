using FluxoCaixa.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContaFinanceira> ContasFinanceiras { get; set; }
        public DbSet<TipoConta> TipoConta { get; set; }
        public DbSet<Conta> Conta { get; set; }
        public DbSet<Fluxo> Fluxo { get; set; }
    }
}
