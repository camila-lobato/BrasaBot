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
        public DbSet<RelatorioEmitido> RelatoriosEmitidos { get; set; }

        public DbSet<Cargo> Cargos { get; set; }



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

            // Insumo → Fornecedor
            modelBuilder.Entity<Insumo>()
                .HasOne(i => i.Fornecedor)
                .WithMany()
                .HasForeignKey(i => i.FornecedorId)
                .OnDelete(DeleteBehavior.SetNull);

            // MovimentacaoCaixa
            modelBuilder.Entity<MovimentacaoCaixa>()
                .HasOne(m => m.Pedido)
                .WithMany()
                .HasForeignKey(m => m.PedidoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Cargo)
                .WithMany()
                .HasForeignKey(f => f.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caixa>()
       .Property(c => c.SaldoInicial).HasPrecision(10, 2);
            modelBuilder.Entity<Caixa>()
                .Property(c => c.TotalEntradas).HasPrecision(10, 2);
            modelBuilder.Entity<Caixa>()
                .Property(c => c.TotalSaidas).HasPrecision(10, 2);
            modelBuilder.Entity<Caixa>()
                .Property(c => c.Total).HasPrecision(10, 2);

            modelBuilder.Entity<MovimentacaoCaixa>()
                .Property(m => m.Valor).HasPrecision(10, 2);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Valor).HasPrecision(10, 2);

            modelBuilder.Entity<Servico>()
                .Property(s => s.Valor).HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total).HasPrecision(10, 2);
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Desconto).HasPrecision(10, 2);
            modelBuilder.Entity<Pedido>()
                .Property(p => p.TaxaEntrega).HasPrecision(10, 2);

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.ValorUnitario).HasPrecision(10, 2);
            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.SubTotal).HasPrecision(10, 2);

            modelBuilder.Entity<Despesa>()
                .Property(d => d.Valor).HasPrecision(10, 2);

            modelBuilder.Entity<Insumo>()
                .Property(i => i.EstoqueAtualQtd).HasPrecision(10, 2);
            modelBuilder.Entity<Insumo>()
                .Property(i => i.EstoqueMinimo).HasPrecision(10, 2);

            modelBuilder.Entity<ProdutoInsumo>()
                .Property(pi => pi.Quantidade).HasPrecision(10, 2);

            modelBuilder.Entity<MovimentacaoEstoque>()
                .Property(m => m.Quantidade).HasPrecision(10, 2);

            modelBuilder.Entity<RelatorioEmitido>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}