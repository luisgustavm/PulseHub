# PulseHub
# 🚀 PulseHub

> Uma API backend robusta construída com **.NET 10** seguindo princípios de **Clean Architecture** e **CQRS**

[![.NET](https://img.shields.io/badge/.NET-10.0-blue)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![GitHub](https://img.shields.io/badge/GitHub-PulseHub-black)](https://github.com/ProfCristianoDePaula/PulseHub)

---

## 📋 Sumário

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#technologias)
- [Funcionalidades](#funcionalidades)
- [Requisitos](#requisitos)
- [Configuração](#configuração)
- [Como Usar](#como-usar)
- [Endpoints](#endpoints)
- [Arquitetura](#arquitetura)
- [Contribuindo](#contribuindo)
- [Licença](#licença)

---

## 📌 Sobre o Projeto

**PulseHub** é uma API RESTful completa para gerenciamento de e-commerce, fornecendo endpoints para:
- ✅ Gerenciamento de **Produtos**
- ✅ Gerenciamento de **Categorias**
- ✅ Gerenciamento de **Usuários**
- ✅ Gerenciamento de **Pedidos**

A arquitetura segue as melhores práticas de desenvolvimento, com separação clara de responsabilidades e código limpo e testável.

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Descrição |
|-----------|--------|-----------|
| **.NET** | 10.0 | Framework principal |
| **ASP.NET Core** | 10.0 | Framework web |
| **Entity Framework Core** | Latest | ORM para acesso a dados |
| **SQL Server** | LocalDB | Banco de dados |
| **Scalar** | Latest | UI moderna para API docs |
| **OpenAPI** | 7.0 | Documentação da API |

---

## ✨ Funcionalidades

### 🛍️ Produtos
- ✅ Listar produtos com paginação e filtros
- ✅ Buscar produto por ID
- ✅ Criar novo produto
- ✅ Filtrar por categoria
- ✅ Filtro de status (ativo/inativo)
- ✅ Busca por termo

### 🏷️ Categorias
- ✅ Listar todas as categorias
- ✅ Filtro de categorias ativas
- ✅ Criar nova categoria

### 👥 Usuários
- ✅ Listar usuários com paginação
- ✅ Criar novo usuário
- ✅ Busca por termo
- ✅ Filtro por status

### 📦 Pedidos
- ⏳ Em desenvolvimento

---

## 📋 Requisitos

Antes de começar, certifique-se de ter instalado:

- **Visual Studio 2026** (ou VSCode + .NET CLI)
- **.NET 10 SDK** ou posterior
- **SQL Server LocalDB** (incluído com Visual Studio)
- **Git**

### Links de Download

| Software | Link |
|----------|------|
| .NET 10 SDK | https://dotnet.microsoft.com/download/dotnet/10.0 |
| Visual Studio | https://visualstudio.microsoft.com/vs/community/ |
| SQL Server LocalDB | https://learn.microsoft.com/pt-br/sql/database-engine/ |

---

## ⚙️ Configuração

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/ProfCristianoDePaula/PulseHub.git
cd PulseHub
```

### 2️⃣ Restaurar Dependências

```bash
dotnet restore
```

### 3️⃣ Configurar Banco de Dados

O arquivo `PulseHub.API/appsettings.json` contém a string de conexão padrão:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PulseHubDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**Se usando SQL Server diferente:**
- Atualize a string `DefaultConnection` em `appsettings.json`

### 4️⃣ Executar Migrations

```bash
cd PulseHub.API
dotnet ef database update
```

### 5️⃣ Iniciar a API

```bash
dotnet run
```

A API estará disponível em:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`

---

## 🚀 Como Usar

### 📚 Documentação Interativa (Swagger)

Após iniciar a API, acesse:
```
https://localhost:5001/scalar/v1
```

Aqui você pode:
- Visualizar todos os endpoints
- Testar requisições diretamente
- Ver modelos de requisição/resposta

### 🧪 Testando com cURL

```bash
# Listar produtos
curl -X GET "https://localhost:5001/api/v1/products?page=1&pageSize=20"

# Criar categoria
curl -X POST "https://localhost:5001/api/v1/categories" \
  -H "Content-Type: application/json" \
  -d '{"name":"Eletrônicos","description":"Produtos eletrônicos"}'
```

---

## 📡 Endpoints

### 🛍️ Produtos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/v1/products` | Listar produtos (com filtros) |
| `GET` | `/api/v1/products/{id}` | Obter produto por ID |
| `POST` | `/api/v1/products` | Criar novo produto |

**Query Parameters (GET /api/v1/products):**
- `page` (int, default: 1) - Número da página
- `pageSize` (int, default: 20) - Itens por página
- `categoryId` (guid, optional) - Filtrar por categoria
- `isActive` (bool, optional) - Filtrar por status
- `searchTerm` (string, optional) - Buscar por termo

**Exemplo de Requisição POST:**
```json
{
  "name": "Notebook Dell",
  "description": "Notebook de alta performance",
  "price": 3500.00,
  "categoryId": "550e8400-e29b-41d4-a716-446655440000",
  "isActive": true
}
```

---

### 🏷️ Categorias

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/v1/categories` | Listar categorias |
| `POST` | `/api/v1/categories` | Criar nova categoria |

**Query Parameters (GET /api/v1/categories):**
- `onlyActive` (bool, default: true) - Apenas categorias ativas

**Exemplo de Requisição POST:**
```json
{
  "name": "Eletrônicos",
  "description": "Produtos eletrônicos em geral"
}
```

---

### 👥 Usuários

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/v1/users` | Listar usuários |
| `POST` | `/api/v1/users` | Criar novo usuário |

**Query Parameters (GET /api/v1/users):**
- `page` (int, default: 1) - Número da página
- `pageSize` (int, default: 20) - Itens por página
- `searchTerm` (string, optional) - Buscar por termo
- `isActive` (bool, optional) - Filtrar por status

**Exemplo de Requisição POST:**
```json
{
  "name": "João Silva",
  "email": "joao@example.com",
  "isActive": true
}
```

---

## 📊 Estrutura de Resposta

### ✅ Sucesso (200-201)

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Produto Exemplo",
  "description": "Descrição do produto",
  "price": 99.99,
  "categoryId": "550e8400-e29b-41d4-a716-446655440000",
  "isActive": true,
  "createdAt": "2024-01-15T10:30:00Z"
}
```

### ❌ Erro (400-404-500)

```json
{
  "error": "Mensagem descritiva do erro"
}
```

---

## 🏗️ Arquitetura

### 📁 Estrutura de Pastas

```
PulseHub/
├── PulseHub.API/
│   ├── Controllers/          # Endpoints da API
│   ├── Program.cs            # Configuração da aplicação
│   ├── appsettings.json      # Configurações
│   └── appsettings.Development.json
│
├── PulseHub.Application/
│   ├── Commands/             # Operações de escrita (CUD)
│   ├── Queries/              # Operações de leitura (R)
│   ├── DTOs/                 # Data Transfer Objects
│   └── Common/               # Classes compartilhadas
│
├── PulseHub.Domain/
│   ├── Entities/             # Entidades de domínio
│   └── Interfaces/           # Contratos
│
├── PulseHub.Infrastructure/
│   ├── Persistence/          # EF Core DbContext
│   ├── Repositories/         # Padrão Repository
│   └── Services/             # Serviços diversos
│
└── README.md                 # Este arquivo
```

### 🏛️ Padrões de Arquitetura

#### **Clean Architecture**
- Separação clara de responsabilidades
- Dependências apontam para dentro (regra de dependência)
- Independente de frameworks, bancos de dados e detalhes de UI

#### **CQRS (Command Query Responsibility Segregation)**
- **Commands**: Operações que modificam dados (Create, Update, Delete)
- **Queries**: Operações que consultam dados (Read)
- **Handlers**: Processam commands e queries

#### **Dependency Injection**
Configuração em `Program.cs`:
```csharp
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

---

## 🔒 Segurança

### CORS (Cross-Origin Resource Sharing)
Atualmente configurado para aceitar todas as origens:
```csharp
policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
```

⚠️ **Para Produção:** Restrinja os domínios permitidos!

```csharp
policy.WithOrigins("https://seu-dominio.com")
      .AllowAnyMethod()
      .AllowAnyHeader();
```

---

## 🐛 Problemas Comuns

### ❌ Erro: "Database does not exist"
```bash
# Solução
dotnet ef database update
```

### ❌ Erro: "SQL Server LocalDB not found"
Instale SQL Server LocalDB ou atualize a string de conexão.

### ❌ Erro: ".NET 10 SDK not found"
Baixe em: https://dotnet.microsoft.com/download/dotnet/10.0

### ❌ Erro: "CORS policy: No 'Access-Control-Allow-Origin'"
Atualize a configuração CORS em `Program.cs` para seu domínio.

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Siga os passos:

1. **Fork** o projeto
2. Crie uma **branch** para sua feature (`git checkout -b feature/MinhaFeature`)
3. **Commit** suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. **Push** para a branch (`git push origin feature/MinhaFeature`)
5. Abra um **Pull Request**

---

## 📝 Licença

Este projeto está licenciado sob a **MIT License**.

```
MIT License

Copyright (c) 2024 Prof. Cristiano De Paula

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software...
```

Veja o arquivo [LICENSE](LICENSE) para detalhes completos.

---

## 👨‍💻 Autor

**Prof. Cristiano De Paula**

- GitHub: [@ProfCristianoDePaula](https://github.com/ProfCristianoDePaula)
- Repositório: [PulseHub](https://github.com/ProfCristianoDePaula/PulseHub)

---

## 🗺️ Roadmap

- [ ] Autenticação e Autorização (JWT)
- [ ] Gerenciamento de Pedidos (completo)
- [ ] Processamento de Pagamentos
- [ ] Sistema de Notificações
- [ ] Testes Unitários (xUnit)
- [ ] Testes de Integração
- [ ] Docker & Docker Compose
- [ ] CI/CD Pipeline (GitHub Actions)
- [ ] Logging centralizado
- [ ] Rate Limiting

---

## 📞 Suporte

Encontrou um problema? Abra uma [Issue](https://github.com/ProfCristianoDePaula/PulseHub/issues)!

Tem uma pergunta? Consulte:
- [Documentação .NET](https://learn.microsoft.com/dotnet/)
- [Documentação ASP.NET Core](https://learn.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)

---

## 📚 Recursos Úteis

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [RESTful API Design Best Practices](https://restfulapi.net/)
- [OpenAPI Specification](https://spec.openapis.org/)

---

<div align="center">

**⭐ Se este projeto foi útil, considere dar uma estrela! ⭐**

Desenvolvido com ❤️ por Prof. Cristiano De Paula

</div>
