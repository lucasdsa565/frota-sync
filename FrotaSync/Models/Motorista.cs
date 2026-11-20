namespace FrotaSync.Models
{
    public class Motorista
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Cnh { get; set; } = string.Empty;
        public string CategoriaCnh { get; set; } = "B";
        public DateTime ValidadeCnh { get; set; } = DateTime.Today.AddYears(1);

        public bool ValidarValidadeCnh() => ValidadeCnh.Date >= DateTime.Today;

        public bool VerificarCompatibilidadeCategoria(string categoriaExigida)
        {
            var hierarquia = new Dictionary<string, int>
            {
                { "A", 1 }, { "B", 2 }, { "C", 3 }, { "D", 4 }, { "E", 5 }
            };

            if (!hierarquia.ContainsKey(CategoriaCnh) || !hierarquia.ContainsKey(categoriaExigida))
                return false;

            return hierarquia[CategoriaCnh] >= hierarquia[categoriaExigida];
        }

        public string ToCsv() => $"{Id};{Nome};{Cnh};{CategoriaCnh};{ValidadeCnh:yyyy-MM-dd}";

        public static Motorista FromCsv(string linha)
        {
            var d = linha.Split(';');
            return new Motorista
            {
                Id = Guid.Parse(d[0]),
                Nome = d[1],
                Cnh = d[2],
                CategoriaCnh = d[3],
                ValidadeCnh = DateTime.Parse(d[4])
            };
        }
    }
}