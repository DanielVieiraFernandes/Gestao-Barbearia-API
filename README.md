# 💈 Gestao-Barbearia-API

Uma API robusta e modular desenvolvida em C# (.NET) para o gerenciamento completo de barbearias, cobrindo agendamentos, cadastros de clientes e serviços, e gestão de dados essenciais para o negócio.

## 🚀 Tecnologias

Este projeto foi construído utilizando as seguintes tecnologias e padrões de arquitetura:

* **Linguagem:** C#
* **Framework:** .NET (Asp.Net Core)
* **Banco de Dados:** (A ser definido, possivelmente SQL Server ou PostgreSQL, gerenciado por EF Core)
* **Migrações de Banco de Dados:** DbUp
* **Padrão de Arquitetura:** DDD (Domain-Driven Design) com arquitetura em camadas.

## 📁 Estrutura do Projeto

O projeto segue uma arquitetura em camadas clara, separando as responsabilidades para facilitar a manutenção e escalabilidade.

| Pasta | Responsabilidade |
| :--- | :--- |
| `GestaoDeBarbearia.Api` | Ponto de entrada da aplicação. Contém os controllers e a configuração da API. |
| `GestaoDeBarbearia.Application` | Contém as regras de negócio de alto nível (casos de uso) e a orquestração de operações. |
| `GestaoDeBarbearia.Domain` | O núcleo do sistema. Contém as entidades, objetos de valor e interfaces de repositório. |
| `GestaoDeBarbearia.Infraestructure` | Implementações dos repositórios, serviços externos e configuração do Entity Framework Core. |
| `GestaoDeBarbearia.Communication` | Contém modelos de entrada (Requests) e saída (Responses) da API. |
| `GestaoDeBarbearia.DbUp` | Responsável pela execução de scripts de migração de banco de dados, garantindo a evolução do schema. |
| `GestaoDeBarbearia.Exception` | Classes e modelos para tratamento de erros padronizados e exceções personalizadas. |

## ⚙️ Configuração e Execução

### Pré-requisitos

Certifique-se de ter instalado em sua máquina:

1.  **SDK do .NET:** Versão 7.0 ou superior.
2.  **Visual Studio 2022** ou outra IDE de sua preferência (VS Code, JetBrains Rider).
3.  **Banco de Dados:** (Ex: SQL Server LocalDB ou contêiner Docker).

### Passos

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/DanielVieiraFernandes/Gestao-Barbearia-API.git](https://github.com/DanielVieiraFernandes/Gestao-Barbearia-API.git)
    cd Gestao-Barbearia-API
    ```

2.  **Restaure as dependências:**
    ```bash
    dotnet restore
    ```

3.  **Configure o Banco de Dados:**
    * Abra o arquivo de configuração (`appsettings.json` na pasta `GestaoDeBarbearia.Api`) e ajuste a string de conexão do banco de dados.

4.  **Execute as Migrações (DbUp):**
    O projeto `GestaoDeBarbearia.DbUp` é o responsável por criar o banco de dados e aplicar as migrações.
    É um projeto console que deve ser executado antes de iniciar a API.
    Nele é possível gerar as tabelas e popular com dados iniciais.

    ```bash
    dotnet run --project GestaoDeBarbearia.DbUp
    ```

5.  **Inicie a API:**
    ```bash
    dotnet run --project GestaoDeBarbearia.Api
    ```

A API estará acessível em `https://localhost:<porta_configurada>`.

## 📌 Endpoints Principais

A FAZER: Documentar os principais endpoints da API
