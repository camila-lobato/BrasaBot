# 🔥 BrasaBot

Sistema de gestão web para o **Brasa&Lenha Restaurante**, desenvolvido como Trabalho de Conclusão de Curso (TCC) do curso superior de Tecnologia em Análise e Desenvolvimento de Sistemas do IFRO Campus Ji-Paraná.

🌐 **Sistema online:** [https://brasabot-app-hbbqa9axddcebwey.spaincentral-01.azurewebsites.net](https://brasabot-app-hbbqa9axddcebwey.spaincentral-01.azurewebsites.net)

---

## 📋 Sobre o projeto

O BrasaBot informatiza e integra os processos operacionais e financeiros do Brasa&Lenha Restaurante, substituindo o controle manual em papel por uma solução digital que centraliza informações e reduz erros.

O sistema contempla:

- **Cadastros:** Clientes, Funcionários, Usuários, Fornecedores, Insumos, Produtos, Categorias, Serviços e Cargos
- **Operação:** Pedidos em Mesa, Delivery e Balcão
- **Financeiro:** Caixa, Despesas e Relatórios
- **Estoque:** Controle de entradas e saídas de insumos com baixa automática ao vender

---

## 🛠️ Tecnologias

| Camada | Tecnologia |
|---|---|
| Backend | C# + ASP.NET Core MVC |
| ORM | Entity Framework Core |
| Banco de dados | MySQL (Azure Database for MySQL) |
| Frontend | Razor Views, HTML5, CSS3 (Bootstrap 5), JavaScript |
| Autenticação | Cookie Authentication + BCrypt |
| Gráficos | Chart.js |
| PDF | html2pdf.js |
| Máscaras | jQuery Mask, jQuery MaskMoney |
| API externa | ViaCEP (preenchimento de endereço) |
| Hospedagem | Microsoft Azure (App Service + MySQL Flexible Server) |

---

## 🗺️ Estrutura do projeto
BrasaBot/
├── Controllers/ ← Controladores MVC (um por módulo)
├── Models/ ← Entidades do banco
├── ViewModels/ ← Models específicos para as Views
├── Repositories/ ← Camada de acesso a dados
│ └── Interfaces/
├── Services/ ← Regras de negócio
│ └── Interfaces/
├── Views/ ← Telas Razor
├── wwwroot/ ← CSS, JS, imagens, uploads
├── Migrations/ ← Histórico do banco (EF Core)
├── docs/ ← Documentação e diagramas
└── Program.cs

O projeto segue o padrão **MVC** com camadas de **Repository** e **Service** para separação de responsabilidades.

---

## 🏗️ Arquitetura
┌─────────────────────────┐
│ Browser / Navegador │
└────────────┬────────────┘
│ HTTP/HTTPS
▼
┌─────────────────────────┐
│ Controller │ ← recebe requisições
└────────────┬────────────┘
▼
┌─────────────────────────┐
│ Service │ ← regras de negócio
└────────────┬────────────┘
▼
┌─────────────────────────┐
│ Repository │ ← acesso a dados
└────────────┬────────────┘
▼
┌─────────────────────────┐
│ AppDbContext (EF Core) │
└────────────┬────────────┘
▼
┌─────────────────────────┐
│ MySQL │
└─────────────────────────┘

---

## 🚀 Como executar localmente

### Pré-requisitos

- .NET 8 SDK
- MySQL 8+
- Visual Studio 2022

### Passo a passo

1. Clone o repositório:
   git clone https://github.com/SEU-USUARIO/BrasaBot.git
   cd BrasaBot


2. Ajuste a connection string em appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=brasabot;user=root;password=SUA_SENHA;"
}

3. Aplique as migrations no terminal:
Update-Database

4. Execute o projeto (F5) e acesse:
https://localhost:XXXX

👤 Acesso ao sistema online
O sistema está publicado no Azure. Para acessar:

URL: https://brasabot-app-hbbqa9axddcebwey.spaincentral-01.azurewebsites.net

E-mail: admin@brasabot.com

Senha: (fornecida ao orientador por e-mail)

⚠️ Aviso: o plano gratuito do Azure "dorme" após 20 minutos sem acesso. A primeira visita pode demorar ~30 segundos para responder.
✅ Funcionalidades implementadas
Cadastros (RF1–RF15)
☑ Funcionários (com Cargo)
☑ Clientes
☑ Fornecedores
☑ Usuários (com nível de acesso)
☑ Insumos (com fornecedor)
☑ Produtos (com foto, categoria e insumos)
☑ Categorias
☑ Serviços
☑ Cargos
Autenticação e Perfil (RF16–RF18)
☑ Login com BCrypt + Cookie Authentication
☑ Dashboard com gráficos (Chart.js)
☑ Meu Perfil (editar, alterar senha, sair)
☑ Controle de acesso (Administrador / Básico)
Vendas (RF20–RF25)
☑ Gerenciar Pedidos (abas Mesa, Delivery, Balcão)
☑ Vender para Mesa (mapa de mesas com status)
☑ Vender para Delivery (cliente, endereço, taxa)
☑ Vender para Balcão (venda direta)
☑ Adicionar Produtos ao Pedido (categorias + carrinho)
☑ Fechar Conta (formas de pagamento, desconto, troco)
Financeiro (RF19, RF27, RF28)
☑ Gerenciar Caixa (abrir, fechar, inserir, retirar)
☑ Gerenciar Despesas (cadastrar, pagar)
☑ Emitir Relatórios (6 tipos: Vendas, Caixa, Despesas, Financeiro, Produtos, Estoque)
☑ Exportar relatórios em PDF
Estoque (RF26)
☑ Gerenciar Estoque (movimentações de entrada/saída)
☑ Alerta de estoque baixo
☑ Baixa automática de insumos ao vender
Melhorias de UX
☑ Máscaras de CPF, CNPJ, Telefone, CEP
☑ Máscara monetária em valores
☑ Preenchimento automático de endereço via ViaCEP
☑ Indicadores visuais de campos obrigatórios
☑ Histórico de relatórios emitidos (auditoria)

👩‍💻 Autora
Camila Lobato Moreira
Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
Instituto Federal de Educação, Ciência e Tecnologia de Rondônia — IFRO Campus Ji-Paraná

Orientador: Prof. Me. Jackson Henrique da Silva Bezerra

📄 Licença
Projeto acadêmico desenvolvido para fins educacionais.