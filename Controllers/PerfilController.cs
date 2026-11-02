using System.Security.Claims;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BrasaBot.Controllers
{
    public class PerfilController : Controller
    {
        private readonly IPerfilService _perfilService;

        public PerfilController(IPerfilService perfilService)
        {
            _perfilService = perfilService;
        }

        private int ObterUsuarioId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim ?? "0");
        }

        // GET: /Perfil
        public async Task<IActionResult> Index()
        {
            var perfil = await _perfilService.ObterPerfilAsync(ObterUsuarioId());
            if (perfil == null)
                return NotFound();

            return View(perfil);
        }

        // GET: /Perfil/Edit
        public async Task<IActionResult> Edit()
        {
            var vm = await _perfilService.ObterParaEdicaoAsync(ObterUsuarioId());
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        // POST: /Perfil/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditarPerfilViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var sucesso = await _perfilService.EditarPerfilAsync(ObterUsuarioId(), vm);
            if (!sucesso)
                return NotFound();

            // Atualiza o nome no cookie para refletir imediatamente
            var identity = (ClaimsIdentity)User.Identity!;
            var nomeClaim = identity.FindFirst(ClaimTypes.Name);
            if (nomeClaim != null)
                identity.RemoveClaim(nomeClaim);
            identity.AddClaim(new Claim(ClaimTypes.Name, vm.Nome));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        // POST: /Perfil/AlterarSenha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErroSenha"] = "Verifique os dados e tente novamente.";
                return RedirectToAction(nameof(Index));
            }

            var sucesso = await _perfilService.AlterarSenhaAsync(
                ObterUsuarioId(), vm.SenhaAtual, vm.NovaSenha);

            if (!sucesso)
            {
                TempData["ErroSenha"] = "Senha atual incorreta.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Sucesso"] = "Senha alterada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Perfil/Sair
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}