using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var usuarios = await _usuarioService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(usuarios);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new UsuarioViewModel
            {
                Funcionarios = await ObterFuncionariosSelectList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Funcionarios = await ObterFuncionariosSelectList();
                return View(vm);
            }

            try
            {
                var caminhoFoto = await _usuarioService.SalvarImagemAsync(vm.ArquivoFoto);

                var usuario = new Usuario
                {
                    Email = vm.Email,
                    Senha = vm.Senha, // TODO: hash depois, na etapa de Login
                    NivelAcesso = vm.NivelAcesso,
                    Foto = caminhoFoto,
                    FuncionarioId = vm.FuncionarioId,
                    Ativo = true
                };

                var sucesso = await _usuarioService.CadastrarAsync(usuario);

                if (!sucesso)
                {
                    ModelState.AddModelError("", "Já existe um usuário com este e-mail ou este funcionário já possui um usuário vinculado.");
                    vm.Funcionarios = await ObterFuncionariosSelectList();
                    return View(vm);
                }

                TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("ArquivoFoto", ex.Message);
                vm.Funcionarios = await ObterFuncionariosSelectList();
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            var vm = new UsuarioViewModel
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Senha = usuario.Senha,
                NivelAcesso = usuario.NivelAcesso,
                FotoAtual = usuario.Foto,
                FuncionarioId = usuario.FuncionarioId,
                Ativo = usuario.Ativo,
                Funcionarios = await ObterFuncionariosSelectList(usuario.Id, usuario.FuncionarioId)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Funcionarios = await ObterFuncionariosSelectList(vm.Id, vm.FuncionarioId);
                return View(vm);
            }

            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            try
            {
                if (vm.ArquivoFoto != null && vm.ArquivoFoto.Length > 0)
                {
                    var novaFoto = await _usuarioService.SalvarImagemAsync(vm.ArquivoFoto);
                    if (novaFoto != null)
                        usuario.Foto = novaFoto;
                }

                usuario.Email = vm.Email;
                usuario.Senha = vm.Senha;
                usuario.NivelAcesso = vm.NivelAcesso;
                usuario.FuncionarioId = vm.FuncionarioId;
                usuario.Ativo = vm.Ativo;

                var sucesso = await _usuarioService.AtualizarAsync(usuario);

                if (!sucesso)
                {
                    ModelState.AddModelError("", "Este funcionário já possui outro usuário vinculado.");
                    vm.Funcionarios = await ObterFuncionariosSelectList(vm.Id, vm.FuncionarioId);
                    vm.FotoAtual = usuario.Foto;
                    return View(vm);
                }

                TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("ArquivoFoto", ex.Message);
                vm.Funcionarios = await ObterFuncionariosSelectList(vm.Id, vm.FuncionarioId);
                vm.FotoAtual = usuario.Foto;
                return View(vm);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _usuarioService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // Combobox de funcionários — lista apenas os que ainda não têm usuário,
        // mais o funcionário atual (quando estiver editando)
        private async Task<IEnumerable<SelectListItem>> ObterFuncionariosSelectList(int? ignorarUsuarioId = null, int? funcionarioAtualId = null)
        {
            var funcionarios = await _usuarioService.ObterFuncionariosSemUsuarioAsync(ignorarUsuarioId);

            var lista = funcionarios.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Nome
            }).ToList();

            // Se for edição, garante que o funcionário atual apareça na lista
            if (funcionarioAtualId.HasValue && !lista.Any(i => i.Value == funcionarioAtualId.Value.ToString()))
            {
                var usuario = ignorarUsuarioId.HasValue
                    ? await _usuarioService.ObterPorIdAsync(ignorarUsuarioId.Value)
                    : null;

                if (usuario != null)
                {
                    lista.Insert(0, new SelectListItem
                    {
                        Value = usuario.Funcionario.Id.ToString(),
                        Text = usuario.Funcionario.Nome,
                        Selected = true
                    });
                }
            }

            return lista;
        }
    }
}