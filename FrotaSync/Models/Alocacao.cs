namespace FrotaSync.Models
{
    public class Alocacao
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VeiculoId { get; set; }
        public Guid MotoristaId { get; set; }
        public DateTime DataHoraInicio { get; set; } = DateTime.Now;
        public DateTime? DataHoraFim { get; set; }
        public string StatusAlocacao { get; set; } = "ATIVA";

        public void EncerrarAlocacao()
        {
            StatusAlocacao = "CONCLUIDA";
            DataHoraFim = DateTime.Now;
        }

        public string ToCsv() => $"{Id};{VeiculoId};{MotoristaId};{DataHoraInicio:O};{(DataHoraFim.HasValue ? DataHoraFim.Value.ToString("O") : "")};{StatusAlocacao}";

        public static Alocacao FromCsv(string linha)
        {
            var d = linha.Split(';');
            return new Alocacao
            {
                Id = Guid.Parse(d[0]),
                VeiculoId = Guid.Parse(d[1]),
                MotoristaId = Guid.Parse(d[2]),
                DataHoraInicio = DateTime.Parse(d[3]),
                DataHoraFim = string.IsNullOrWhiteSpace(d[4]) ? null : DateTime.Parse(d[4]),
                StatusAlocacao = d[5]
            };
        }
    }
}