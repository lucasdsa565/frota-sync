using FrotaSync.Models;

namespace FrotaSync.Services
{
    public class CsvStorageService
    {
        private readonly string _veiculosPath = Path.Combine("Data", "veiculos.csv");
        private readonly string _motoristasPath = Path.Combine("Data", "motoristas.csv");
        private readonly string _alocacoesPath = Path.Combine("Data", "alocacoes.csv");
        private readonly string _manutencoesPath = Path.Combine("Data", "manutencoes.csv");

        public CsvStorageService()
        {
            Directory.CreateDirectory("Data");
            GarantirArquivo(_veiculosPath, "Id;Placa;Modelo;Ano;Quilometragem;CategoriaExigida;Status");
            GarantirArquivo(_motoristasPath, "Id;Nome;Cnh;CategoriaCnh;ValidadeCnh");
            GarantirArquivo(_alocacoesPath, "Id;VeiculoId;MotoristaId;DataHoraInicio;DataHoraFim;StatusAlocacao");
            GarantirArquivo(_manutencoesPath, "Id;VeiculoId;DescricaoServico;ValorEstimado;DataSolicitacao;StatusManutencao");
        }

        private void GarantirArquivo(string caminho, string cabecalho)
        {
            if (!File.Exists(caminho) || new FileInfo(caminho).Length == 0)
                File.WriteAllText(caminho, cabecalho + Environment.NewLine);
        }

        // VEÍCULOS
        public List<Veiculo> ListarVeiculos() =>
            File.ReadAllLines(_veiculosPath).Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)).Select(Veiculo.FromCsv).ToList();

        public void SalvarVeiculo(Veiculo veiculo) =>
            File.AppendAllText(_veiculosPath, veiculo.ToCsv() + Environment.NewLine);

        private void ReescreverVeiculos(List<Veiculo> lista)
        {
            var linhas = new List<string> { "Id;Placa;Modelo;Ano;Quilometragem;CategoriaExigida;Status" };
            linhas.AddRange(lista.Select(v => v.ToCsv()));
            File.WriteAllLines(_veiculosPath, linhas);
        }

        // MOTORISTAS
        public List<Motorista> ListarMotoristas() =>
            File.ReadAllLines(_motoristasPath).Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)).Select(Motorista.FromCsv).ToList();

        public void SalvarMotorista(Motorista motorista) =>
            File.AppendAllText(_motoristasPath, motorista.ToCsv() + Environment.NewLine);

        // ALOCAÇÕES (com RN01, RN02, RN03, RN04 e Encerramento)
        public List<Alocacao> ListarAlocacoes() =>
            File.ReadAllLines(_alocacoesPath).Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)).Select(Alocacao.FromCsv).ToList();

        private void ReescreverAlocacoes(List<Alocacao> lista)
        {
            var linhas = new List<string> { "Id;VeiculoId;MotoristaId;DataHoraInicio;DataHoraFim;StatusAlocacao" };
            linhas.AddRange(lista.Select(a => a.ToCsv()));
            File.WriteAllLines(_alocacoesPath, linhas);
        }

        public (bool Sucesso, string Mensagem) RegistrarAlocacao(Guid veiculoId, Guid motoristaId)
        {
            var veiculos = ListarVeiculos();
            var veiculo = veiculos.FirstOrDefault(v => v.Id == veiculoId);
            var motorista = ListarMotoristas().FirstOrDefault(m => m.Id == motoristaId);

            if (veiculo == null || motorista == null)
                return (false, "Veículo ou motorista não encontrado.");

            if (!veiculo.EstaDisponivel())
                return (false, $"Operação não permitida. O veículo {veiculo.Placa} encontra-se indisponível ({veiculo.Status}).");

            if (!motorista.ValidarValidadeCnh())
                return (false, $"Alerta de CNH Vencida: {motorista.Nome} está com a CNH fora da validade.");

            if (!motorista.VerificarCompatibilidadeCategoria(veiculo.CategoriaExigida))
                return (false, $"Erro: A categoria da CNH do motorista {motorista.Nome} ({motorista.CategoriaCnh}) é incompatível. Exigida: {veiculo.CategoriaExigida}.");

            var alocacao = new Alocacao { VeiculoId = veiculoId, MotoristaId = motoristaId };
            File.AppendAllText(_alocacoesPath, alocacao.ToCsv() + Environment.NewLine);

            veiculo.AlterarStatus("EM_USO");
            ReescreverVeiculos(veiculos);

            return (true, "Alocação realizada com sucesso! Status do veículo atualizado para EM_USO.");
        }

        public (bool Sucesso, string Mensagem) EncerrarAlocacao(Guid alocacaoId, int novaQuilometragem)
        {
            var alocacoes = ListarAlocacoes();
            var alocacao = alocacoes.FirstOrDefault(a => a.Id == alocacaoId);
            if (alocacao == null || alocacao.StatusAlocacao != "ATIVA")
                return (false, "Alocação não encontrada ou já encerrada.");

            var veiculos = ListarVeiculos();
            var veiculo = veiculos.FirstOrDefault(v => v.Id == alocacao.VeiculoId);
            if (veiculo != null)
            {
                if (novaQuilometragem < veiculo.Quilometragem)
                    return (false, $"A nova quilometragem ({novaQuilometragem} km) não pode ser menor que a atual ({veiculo.Quilometragem} km).");

                veiculo.AtualizarQuilometragem(novaQuilometragem);
                veiculo.AlterarStatus("DISPONIVEL");
                ReescreverVeiculos(veiculos);
            }

            alocacao.EncerrarAlocacao();
            ReescreverAlocacoes(alocacoes);

            return (true, "Alocação encerrada, quilometragem atualizada e veículo devolvido à frota.");
        }

        // MANUTENÇÕES (com Conclusão)
        public List<Manutencao> ListarManutencoes() =>
            File.ReadAllLines(_manutencoesPath).Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)).Select(Manutencao.FromCsv).ToList();

        private void ReescreverManutencoes(List<Manutencao> lista)
        {
            var linhas = new List<string> { "Id;VeiculoId;DescricaoServico;ValorEstimado;DataSolicitacao;StatusManutencao" };
            linhas.AddRange(lista.Select(m => m.ToCsv()));
            File.WriteAllLines(_manutencoesPath, linhas);
        }

        public void RegistrarManutencao(Manutencao manutencao)
        {
            File.AppendAllText(_manutencoesPath, manutencao.ToCsv() + Environment.NewLine);

            var veiculos = ListarVeiculos();
            var veiculo = veiculos.FirstOrDefault(v => v.Id == manutencao.VeiculoId);
            if (veiculo != null)
            {
                veiculo.AlterarStatus("EM_MANUTENCAO");
                ReescreverVeiculos(veiculos);
            }
        }

        public void ConcluirManutencao(Guid manutencaoId)
        {
            var manutencoes = ListarManutencoes();
            var manutencao = manutencoes.FirstOrDefault(m => m.Id == manutencaoId);
            if (manutencao == null || manutencao.StatusManutencao == "CONCLUIDA") return;

            manutencao.ConcluirManutencao();
            ReescreverManutencoes(manutencoes);

            var veiculos = ListarVeiculos();
            var veiculo = veiculos.FirstOrDefault(v => v.Id == manutencao.VeiculoId);
            if (veiculo != null)
            {
                veiculo.AlterarStatus("DISPONIVEL");
                ReescreverVeiculos(veiculos);
            }
        }
    }
}