using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly AppDbContext _context;

        public RelatorioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RelatorioEmitido>> ObterHistoricoAsync(int limite = 10)
        {
            return await _context.RelatoriosEmitidos
                .Include(r => r.Usuario)
                    .ThenInclude(u => u.Funcionario)
                .OrderByDescending(r => r.DataEmissao)
                .Take(limite)
                .ToListAsync();
        }

        public async Task AdicionarHistoricoAsync(RelatorioEmitido relatorio)
        {
            await _context.RelatoriosEmitidos.AddAsync(relatorio);
            await _context.SaveChangesAsync();
        }

        public async Task<RelatorioVendasViewModel> ObterRelatorioVendasAsync(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            var query = _context.Pedidos
                .Include(p => p.Funcionario)
                .Include(p => p.Cliente)
                .Where(p => p.Status == "Fechado"
                        && p.DataCriacao >= dataInicial
                        && p.DataCriacao <= dataFinal.AddDays(1).AddSeconds(-1));

            if (filtro != "Todos")
                query = query.Where(p => p.Tipo == filtro);

            var pedidos = await query.OrderBy(p => p.DataCriacao).ToListAsync();

            // Para cada pedido, pega as formas de pagamento das movimentações
            var itens = new List<ItemRelatorioVendas>();
            foreach (var p in pedidos)
            {
                var movs = await _context.MovimentacoesCaixa
                    .Where(m => m.Observacao != null && m.Observacao.Contains($"Pedido #{p.Id}"))
                    .ToListAsync();

                var formas = string.Join(", ", movs.Select(m => m.FormaPagamento).Distinct());

                itens.Add(new ItemRelatorioVendas
                {
                    Data = p.DataCriacao,
                    Detalhes = $"Pedido #{p.Id}",
                    Funcionario = p.Funcionario?.Nome ?? "-",
                    FormaPagamento = string.IsNullOrEmpty(formas) ? "-" : formas,
                    TipoPedido = p.Tipo,
                    Valor = p.Total,
                    Desconto = p.Desconto,
                    Status = "Pago"
                });
            }

            var totalVendas = itens.Sum(i => i.Valor);
            var totalPedidos = itens.Count;

            return new RelatorioVendasViewModel
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                Filtro = filtro,
                TotalVendas = totalVendas,
                TotalPedidos = totalPedidos,
                TicketMedio = totalPedidos > 0 ? totalVendas / totalPedidos : 0,
                Itens = itens
            };
        }

        public async Task<RelatorioCaixaViewModel> ObterRelatorioCaixaAsync(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            var query = _context.MovimentacoesCaixa
                .Include(m => m.Caixa)
                .Where(m => m.DataHora >= dataInicial && m.DataHora <= dataFinal.AddDays(1).AddSeconds(-1));

            var movs = await query.OrderByDescending(m => m.DataHora).ToListAsync();

            var itens = movs.Select(m => new ItemRelatorioCaixa
            {
                Data = m.DataHora.Date,
                Hora = m.DataHora.ToString("HH:mm"),
                Detalhes = m.Observacao ?? "-",
                FormaPagamento = m.FormaPagamento,
                Valor = m.Valor,
                Desconto = 0,
                Tipo = m.Tipo
            }).ToList();

            var totalEntradas = itens.Where(i => i.Tipo == "Entrada").Sum(i => i.Valor);
            var totalSaidas = itens.Where(i => i.Tipo == "Saida").Sum(i => i.Valor);

            return new RelatorioCaixaViewModel
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                Filtro = filtro,
                TotalEntradas = totalEntradas,
                TotalSaidas = totalSaidas,
                SaldoFinal = totalEntradas - totalSaidas,
                Itens = itens
            };
        }

        public async Task<RelatorioDespesasViewModel> ObterRelatorioDespesasAsync(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            try
            {
                var query = _context.Despesas
                    .Include(d => d.Fornecedor)
                    .Where(d => d.DataCadastro.Date >= dataInicial.Date && d.DataCadastro.Date <= dataFinal.Date);

                var despesas = await query.OrderByDescending(d => d.Vencimento).ToListAsync();

                var itens = despesas.Select(d => new ItemRelatorioDespesas
                {
                    Data = d.Vencimento,
                    Categoria = d.Fornecedor?.Nome ?? "Sem fornecedor",
                    Funcionario = "-",
                    Vencimento = d.Vencimento,
                    Valor = d.Valor,
                    Status = d.Status ?? "A Pagar"
                }).ToList();

                return new RelatorioDespesasViewModel
                {
                    DataInicial = dataInicial,
                    DataFinal = dataFinal,
                    Filtro = filtro,
                    TotalPeriodo = itens.Sum(i => i.Valor),
                    TotalPago = itens.Where(i => i.Status == "Pago").Sum(i => i.Valor),
                    TotalPendente = itens.Where(i => i.Status == "A Pagar").Sum(i => i.Valor),
                    Itens = itens
                };
            }
            catch (Exception ex)
            {
                // Log para debug
                Console.WriteLine($">>> ERRO RELATÓRIO DESPESAS: {ex.Message}");
                Console.WriteLine($">>> STACK: {ex.StackTrace}");

                // Retorna vazio em caso de erro
                return new RelatorioDespesasViewModel
                {
                    DataInicial = dataInicial,
                    DataFinal = dataFinal,
                    Filtro = filtro,
                    Itens = new List<ItemRelatorioDespesas>()
                };
            }
        }

        public async Task<RelatorioFinanceiroViewModel> ObterRelatorioFinanceiroAsync(DateTime dataInicial, DateTime dataFinal)
        {
            // Pedidos fechados no período (faturamento)
            var pedidos = await _context.Pedidos
                .Where(p => p.Status == "Fechado"
                        && p.DataCriacao.Date >= dataInicial.Date
                        && p.DataCriacao.Date <= dataFinal.Date)
                .ToListAsync();

            var faturamento = pedidos.Sum(p => p.Total);
            var totalPedidos = pedidos.Count;
            var ticketMedio = totalPedidos > 0 ? faturamento / totalPedidos : 0;

            // Despesas pagas no período (por DataCadastro)
            var despesasPagas = await _context.Despesas
                .Where(d => d.Status == "Pago"
                        && d.DataCadastro.Date >= dataInicial.Date
                        && d.DataCadastro.Date <= dataFinal.Date)
                .SumAsync(d => d.Valor);

            return new RelatorioFinanceiroViewModel
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                FaturamentoBruto = faturamento,
                TotalDespesas = despesasPagas,
                LucroBruto = faturamento - despesasPagas,
                TotalPedidos = totalPedidos,
                TicketMedio = ticketMedio
            };
        }
        public async Task<RelatorioProdutosViewModel> ObterRelatorioProdutosAsync(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            // Busca itens de pedidos fechados no período
            var itens = await _context.ItensPedido
                .Include(i => i.Produto)
                    .ThenInclude(p => p!.Categoria)
                .Include(i => i.Pedido)
                .Where(i => i.Pedido!.Status == "Fechado"
                        && i.Pedido.DataCriacao.Date >= dataInicial.Date
                        && i.Pedido.DataCriacao.Date <= dataFinal.Date)
                .ToListAsync();

            // Agrupa por produto
            var agrupado = itens
                .GroupBy(i => new { i.ProdutoId, i.Produto!.Nome, Categoria = i.Produto.Categoria?.Nome ?? "-" })
                .Select(g => new ItemRelatorioProdutos
                {
                    Id = g.Key.ProdutoId,
                    Produto = g.Key.Nome,
                    Categoria = g.Key.Categoria,
                    QuantidadeVendida = g.Sum(i => i.Quantidade),
                    ReceitaTotal = g.Sum(i => i.SubTotal)
                })
                .ToList();

            // Aplica filtro de ordenação
            if (filtro == "Mais Vendidos")
                agrupado = agrupado.OrderByDescending(p => p.QuantidadeVendida).ToList();
            else if (filtro == "Menos Vendidos")
                agrupado = agrupado.OrderBy(p => p.QuantidadeVendida).ToList();
            else
                agrupado = agrupado.OrderBy(p => p.Produto).ToList();

            return new RelatorioProdutosViewModel
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                Filtro = filtro,
                Itens = agrupado
            };
        }

        public async Task<RelatorioEstoqueViewModel> ObterRelatorioEstoqueAsync(string filtro)
        {
            var insumos = await _context.Insumos
                .Where(i => i.Ativo)
                .OrderBy(i => i.Nome)
                .ToListAsync();

            var itens = insumos.Select(i => new ItemRelatorioEstoque
            {
                Insumo = i.Nome,
                Unidade = i.UnidadeMedida,
                EstoqueAtual = i.EstoqueAtualQtd,
                EstoqueMinimo = i.EstoqueMinimo,
                Status = i.EstoqueAtualQtd <= i.EstoqueMinimo ? "Baixo" : "OK"
            }).ToList();

            if (filtro == "Estoque Baixo")
                itens = itens.Where(i => i.Status == "Baixo").ToList();

            return new RelatorioEstoqueViewModel
            {
                Filtro = filtro,
                Itens = itens
            };
        }
    }
}