using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrotaSync.Models;
using FrotaSync.Services;

namespace FrotaSync.Pages
{
    public class VeiculosModel : PageModel
    {
        private readonly CsvStorageService _service;
        public VeiculosModel(CsvStorageService service) => _service = service;

        [BindProperty] public Veiculo NovoVeiculo { get; set; } = new();
        public List<Veiculo> Veiculos { get; set; } = new();

        public void OnGet()
        {
            Veiculos = _service.ListarVeiculos();
        }

        public IActionResult OnPost()
        {
            _service.SalvarVeiculo(NovoVeiculo);
            return RedirectToPage();
        }
    }
}