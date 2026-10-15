using BrasaBot.Models;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<ProdutoInsumo> ProdutoInsumos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Caixa> Caixas { get; set; }
        public DbSet<MovimentacaoCaixa> MovimentacoesCaixa { get; set; }
        public DbSet<Despesa> Despesas { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProdutoInsumo>()
                .HasKey(pi => new { pi.ProdutoId, pi.InsumoId });

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Funcionario)
                .WithOne(f => f.Usuario)
                .HasForeignKey<Usuario>(u => u.FuncionarioId);

            modelBuilder.Entity<ProdutoInsumo>()
                .HasOne(pi => pi.Produto)
                .WithMany(p => p.ProdutoInsumos)
                .HasForeignKey(pi => pi.ProdutoId);

            modelBuilder.Entity<ProdutoInsumo>()
                .HasOne(pi => pi.Insumo)
                .WithMany(i => i.ProdutoInsumos)
                .HasForeignKey(pi => pi.InsumoId);

            // Insumo → Fornecedor (opcional, sem cascade delete)
            modelBuilder.Entity<Insumo>()
                .HasOne(i => i.Fornecedor)
                .WithMany()
                .HasForeignKey(i => i.FornecedorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}