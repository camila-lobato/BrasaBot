using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;

        public ProdutoController(IProdutoService produtoService, ICategoriaService categoriaService)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var produtos = await _produtoService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(produtos);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ProdutoViewModel
            {
                Categorias = await ObterCategoriasSelectList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categorias = await ObterCategoriasSelectList();
                return View(vm);
            }

            try
            {
                var caminhoFoto = await _produtoService.SalvarImagemAsync(vm.ArquivoFoto);

                var produto = new Produto
                {
                    Nome = vm.Nome,
                    Valor = vm.Valor,
                    Foto = caminhoFoto ?? "/img/produto-padrao.png",
                    CategoriaId = vm.CategoriaId,
                    Ativo = true
                };

                await _produtoService.CadastrarAsync(produto);

                TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("ArquivoFoto", ex.Message);
                vm.Categorias = await ObterCategoriasSelectList();
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
                return NotFound();

            var vm = new ProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Valor = produto.Valor,
                FotoAtual = produto.Foto,
                CategoriaId = produto.CategoriaId,
                Ativo = produto.Ativo,
                Categorias = await ObterCategoriasSelectList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProdutoViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Categorias = await ObterCategoriasSelectList();
                return View(vm);
            }

            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
                return NotFound();

            try
            {
                // Se enviou nova imagem, salva e substitui
                if (vm.ArquivoFoto != null && vm.ArquivoFoto.Length > 0)
                {
                    var novaFoto = await _produtoService.SalvarImagemAsync(vm.ArquivoFoto);
                    if (novaFoto != null)
                        produto.Foto = novaFoto;
                }

                produto.Nome = vm.Nome;
                produto.Valor = vm.Valor;
                produto.CategoriaId = vm.CategoriaId;
                produto.Ativo = vm.Ativo;

                await _produtoService.AtualizarAsync(produto);

                TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("ArquivoFoto", ex.Message);
                vm.Categorias = await ObterCategoriasSelectList();
                vm.FotoAtual = produto.Foto;
                return View(vm);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
                return NotFound();

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _produtoService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> ObterCategoriasSelectList()
        {
            var categorias = await _categoriaService.ObterTodasAsync();
            return categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            });
        }
    }
}