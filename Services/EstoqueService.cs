using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;

namespace BrasaBot.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IInsumoRepository _insumoRepository;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;

        public EstoqueService(
            IInsumoRepository insumoRepository,
            IMovimentacaoEstoqueRepository movimentacaoRepository)
        {
            _insumoRepository = insumoRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<EstoqueIndexViewModel> ObterIndexAsync()
        {
            var insumos = await _insumoRepository.ObterTodosAsync();
            var movimentacoes = await _movimentacaoRepository.ObterUltimasAsync(50);

            var insumosBaixos = insumos
                .Where(i => i.EstoqueAtualQtd <= i.EstoqueMinimo)
                .ToList();

            // Calcula "estoque anterior" para cada insumo (baseado na última movimentação)
            var insumosComAnterior = new List<InsumoComEstoqueAnterior>();
            foreach (var insumo in insumos)
            {
                var ultimaMov = await _movimentacaoRepository.ObterUltimaPorInsumoAsync(insumo.Id);

                decimal anterior = insumo.EstoqueAtualQtd;
                if (ultimaMov != null)
                {
                    anterior = ultimaMov.Tipo == "Entrada"
                        ? insumo.EstoqueAtualQtd - ultimaMov.Quantidade
                        : insumo.EstoqueAtualQtd + ultimaMov.Quantidade;
                }

                insumosComAnterior.Add(new InsumoComEstoqueAnterior
                {
                    Insumo = insumo,
                    EstoqueAnterior = anterior
                });
            }

            return new EstoqueIndexViewModel
            {
                Insumos = insumosComAnterior,
                InsumosBaixos = insumosBaixos,
                Movimentacoes = movimentacoes
            };
        }

        public async Task<bool> RegistrarMovimentacaoAsync(int insumoId, string tipo, decimal quantidade, string? observacao, int funcionarioId)
        {
            var insumo = await _insumoRepository.ObterPorIdAsync(insumoId);
            if (insumo == null)
                return false;

            // Validações
            if (tipo != "Entrada" && tipo != "Saida")
                return false;

            if (quantidade <= 0)
                return false;

            if (tipo == "Saida" && insumo.EstoqueAtualQtd < quantidade)
                return false;  // Não pode ficar negativo

            // Atualiza o estoque do insumo
            if (tipo == "Entrada")
                insumo.EstoqueAtualQtd += quantidade;
            else
                insumo.EstoqueAtualQtd -= quantidade;

            await _insumoRepository.AtualizarAsync(insumo);

            // Registra a movimentação
            var movimentacao = new MovimentacaoEstoque
            {
                DataHora = DateTime.Now,
                Tipo = tipo,
                Quantidade = quantidade,
                Observacao = observacao,
                InsumoId = insumoId,
                FuncionarioId = funcionarioId
            };

            await _movimentacaoRepository.AdicionarAsync(movimentacao);
            return true;
        }
    }
}