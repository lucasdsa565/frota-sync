using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrotaSync.Models;
using FrotaSync.Services;

namespace FrotaSync.Pages
{
    public class ManutencoesModel : PageModel
    {
        private readonly CsvStorageService _service;
        public ManutencoesModel(CsvStorageService service) => _service = service;

        [BindProperty] public Manutencao NovaManutencao { get; set; } = new();
        public List<Veiculo> Veiculos { get; set; } = new();
        public List<Manutencao> Manutencoes { get; set; } = new();

        public void OnGet()
        {
            Veiculos = _service.ListarVeiculos();
            Manutencoes = _service.ListarManutencoes();
        }

        public IActionResult OnPost()
        {
            _service.RegistrarManutencao(NovaManutencao);
            return RedirectToPage();
        }

        public IActionResult OnPostConcluir(Guid manutencaoId)
        {
            _service.ConcluirManutencao(manutencaoId);
            return RedirectToPage();
        }
    }
}