namespace FrotaSync.Models
{
    public class Manutencao
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VeiculoId { get; set; }
        public string DescricaoServico { get; set; } = string.Empty;
        public decimal ValorEstimado { get; set; }
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;
        public string StatusManutencao { get; set; } = "EM_ANDAMENTO";

        public void ConcluirManutencao() => StatusManutencao = "CONCLUIDA";

        public string ToCsv() => $"{Id};{VeiculoId};{DescricaoServico};{ValorEstimado};{DataSolicitacao:O};{StatusManutencao}";

        public static Manutencao FromCsv(string linha)
        {
            var d = linha.Split(';');
            return new Manutencao
            {
                Id = Guid.Parse(d[0]),
                VeiculoId = Guid.Parse(d[1]),
                DescricaoServico = d[2],
                ValorEstimado = decimal.Parse(d[3]),
                DataSolicitacao = DateTime.Parse(d[4]),
                StatusManutencao = d[5]
            };
        }
    }
}