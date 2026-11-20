# frota-sync — Sistema de Gestão de Frotas

Aplicação web desenvolvida em **C# / ASP.NET Core Razor Pages** para o controle operacional de veículos, motoristas, alocações e manutenções da **LogiTrans Logistics** (Projeto de Bloco — Engenharia de Software / Instituto Infnet).

## Funcionalidades Principais
- **Gestão de Veículos e Motoristas:** Cadastro completo com controle de status (`DISPONIVEL`, `EM_USO`, `EM_MANUTENCAO`), validade e categoria da CNH.
- **Alocação com Validação de Regras de Negócio:** Verificação automática de disponibilidade do veículo, validade da CNH e compatibilidade entre a categoria exigida pelo veículo e a habilitação do motorista.
- **Devolução e Quilometragem:** Encerramento de alocações ativas com atualização da quilometragem final e liberação automática do veículo.
- **Controle de Manutenções:** Abertura e conclusão de ordens de serviço com atualização do estado da frota.
- **Persistência em CSV:** Armazenamento estruturado em arquivos `.csv` (`veiculos.csv`, `motoristas.csv`, `alocacoes.csv` e `manutencoes.csv`).

## Tecnologias Utilizadas
- C# (.NET / ASP.NET Core Razor Pages)
- HTML5, CSS3 & Bootstrap 5
- Persistência local via manipulação de arquivos CSV (`System.IO`)
