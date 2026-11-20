namespace FrotaSync.Models
{
    public class Veiculo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Ano { get; set; }
        public int Quilometragem { get; set; }
        public string CategoriaExigida { get; set; } = "B";
        public string Status { get; set; } = "DISPONIVEL";

        public void AlterarStatus(string novoStatus) => Status = novoStatus;
        public void AtualizarQuilometragem(int novaKm) => Quilometragem = novaKm;
        public bool EstaDisponivel() => Status == "DISPONIVEL";

        public string ToCsv() => $"{Id};{Placa};{Modelo};{Ano};{Quilometragem};{CategoriaExigida};{Status}";

        public static Veiculo FromCsv(string linha)
        {
            var d = linha.Split(';');
            return new Veiculo
            {
                Id = Guid.Parse(d[0]),
                Placa = d[1],
                Modelo = d[2],
                Ano = int.Parse(d[3]),
                Quilometragem = int.Parse(d[4]),
                CategoriaExigida = d[5],
                Status = d[6]
            };
        }
    }
}