# 🔥 BrasaBot

Sistema de gestão web para o **Brasa&Lenha Restaurante**, desenvolvido como Trabalho de Conclusão de Curso (TCC) do curso superior de Tecnologia em Análise e Desenvolvimento de Sistemas do IFRO Campus Ji-Paraná.

---

## 📋 Sobre o projeto

O BrasaBot tem como objetivo informatizar e integrar os processos operacionais e financeiros do Brasa&Lenha Restaurante, substituindo o controle manual em papel por uma solução digital que centraliza informações e reduz erros.

O sistema contempla os seguintes módulos:

- **Cadastros:** Clientes, Funcionários, Usuários, Fornecedores, Insumos, Produtos, Categorias e Serviços
- **Operação:** Pedidos em Mesa, Delivery e Balcão
- **Financeiro:** Caixa, Despesas e Relatórios
- **Estoque:** Controle de entradas e saídas de insumos

---

## 🛠️ Tecnologias

| Camada | Tecnologia |
|---|---|
| Backend | C# + ASP.NET Core MVC |
| ORM | Entity Framework Core |
| Banco de dados | MySQL |
| Frontend | Razor Views, HTML5, CSS3 (Bootstrap 5), JavaScript |
| Autenticação | Cookie Authentication + BCrypt |
| Gráficos | Chart.js |
| IDE | Visual Studio 2022 |

---

## 🗺️ Estrutura do projeto
BrasaBot/
├── Controllers/ ← Controladores MVC (um por entidade/módulo)
├── Models/ ← Entidades do banco de dados
├── ViewModels/ ← Models específicos para as Views
├── Repositories/ ← Camada de acesso a dados
│ └── Interfaces/ ← Contratos dos repositórios
├── Services/ ← Regras de negócio
│ └── Interfaces/ ← Contratos dos services
├── Views/ ← Telas Razor (por Controller)
├── wwwroot/ ← Arquivos estáticos (CSS, JS, imagens, uploads)
├── Migrations/ ← Histórico do banco via EF Core
├── docs/ ← Documentação e diagramas
└── Program.cs ← Configuração inicial da aplicação


O projeto segue o padrão **MVC (Model-View-Controller)** com camadas adicionais de **Repository** e **Service** para separação de responsabilidades.

---

## 🏗️ Arquitetura

```mermaid
graph TD
    A[Browser - Razor Views] -->|HTTP/HTTPS| B[Controller]
    B --> C[Service]
    C --> D[Repository]
    D --> E[AppDbContext - EF Core]
    E --> F[(MySQL)]

    🚀 Como executar o projeto
Pré-requisitos
.NET 8 SDK

MySQL 8+

Visual Studio 2022 (ou VS Code com extensão C#)

Passo a passo
Clone o repositório:

bash
git clone https://github.com/SEU-USUARIO/BrasaBot.git
cd BrasaBot
Ajuste a connection string em appsettings.json com seus dados do MySQL:

json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=brasabot;user=root;password=SUA_SENHA;"
}
Aplique as migrations para criar o banco de dados:

powershell
Update-Database
Cadastre um usuário administrador inicial:

Crie um Funcionário e um Usuário vinculado a ele pela própria interface, OU

Insira diretamente via SQL (script de exemplo em docs/seed.sql)

Execute o projeto (F5) e acesse no navegador:

text
https://localhost:XXXX
📚 Documentação
📄 Documento de Especificação do Projeto

🎨 Protótipo no Figma (link a definir)

📊 Diagramas UML

📅 Status do desenvolvimento
☑ Modelagem do banco de dados
☑ Cadastro de Funcionários
☑ Cadastro de Clientes
☑ Cadastro de Fornecedores
☑ Cadastro de Insumos
☑ Cadastro de Produtos
☑ Cadastro de Categorias
☑ Cadastro de Serviços
☑ Cadastro de Usuários
☑ Autenticação com BCrypt
☑ Dashboard com gráficos
☑ Tela de Meu Perfil
□ Gerenciamento de Caixa
□ Módulo de Pedidos (Mesa / Delivery / Balcão)
□ Controle de Estoque
□ Gerenciamento de Despesas
□ Emissão de Relatórios


👩‍💻 Autora
Camila Lobato Moreira
Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
Instituto Federal de Educação, Ciência e Tecnologia de Rondônia — IFRO Campus Ji-Paraná

Orientador: Prof. Me. Jackson Henrique da Silva Bezerra

📄 Licença
Projeto acadêmico desenvolvido para fins educacionais.