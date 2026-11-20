using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrotaSync.Models;
using FrotaSync.Services;

namespace FrotaSync.Pages
{
    public class AlocacoesModel : PageModel
    {
        private readonly CsvStorageService _service;
        public AlocacoesModel(CsvStorageService service) => _service = service;

        [BindProperty] public Guid VeiculoId { get; set; }
        [BindProperty] public Guid MotoristaId { get; set; }

        public List<Veiculo> Veiculos { get; set; } = new();
        public List<Motorista> Motoristas { get; set; } = new();
        public List<Alocacao> Alocacoes { get; set; } = new();
        public string? MensagemErro { get; set; }
        public string? MensagemSucesso { get; set; }

        public void OnGet() => CarregarDados();

        public IActionResult OnPost()
        {
            var resultado = _service.RegistrarAlocacao(VeiculoId, MotoristaId);
            if (!resultado.Sucesso)
                MensagemErro = resultado.Mensagem;
            else
                MensagemSucesso = resultado.Mensagem;

            CarregarDados();
            return Page();
        }

        public IActionResult OnPostEncerrar(Guid alocacaoId, int novaKm)
        {
            var resultado = _service.EncerrarAlocacao(alocacaoId, novaKm);
            if (!resultado.Sucesso)
                MensagemErro = resultado.Mensagem;
            else
                MensagemSucesso = resultado.Mensagem;

            CarregarDados();
            return Page();
        }

        private void CarregarDados()
        {
            Veiculos = _service.ListarVeiculos();
            Motoristas = _service.ListarMotoristas();
            Alocacoes = _service.ListarAlocacoes();
        }
    }
}