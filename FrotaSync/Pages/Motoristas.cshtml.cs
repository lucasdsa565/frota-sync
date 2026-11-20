using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrotaSync.Models;
using FrotaSync.Services;

namespace FrotaSync.Pages
{
    public class MotoristasModel : PageModel
    {
        private readonly CsvStorageService _service;
        public MotoristasModel(CsvStorageService service) => _service = service;

        [BindProperty] public Motorista NovoMotorista { get; set; } = new();
        public List<Motorista> Motoristas { get; set; } = new();

        public void OnGet()
        {
            Motoristas = _service.ListarMotoristas();
        }

        public IActionResult OnPost()
        {
            _service.SalvarMotorista(NovoMotorista);
            return RedirectToPage();
        }
    }
}